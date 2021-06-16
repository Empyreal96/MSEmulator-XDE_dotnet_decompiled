using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Security;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020000EC RID: 236
	internal sealed class ScriptDebugger : Debugger, IDisposable
	{
		// Token: 0x06000D03 RID: 3331 RVA: 0x00046864 File Offset: 0x00044A64
		internal ScriptDebugger(ExecutionContext context)
		{
			this._context = context;
			this._inBreakpoint = false;
			this._idToBreakpoint = new Dictionary<int, Breakpoint>();
			this._pendingBreakpoints = new List<LineBreakpoint>();
			this._boundBreakpoints = new Dictionary<string, Tuple<WeakReference, List<LineBreakpoint>>>(StringComparer.OrdinalIgnoreCase);
			this._commandBreakpoints = new List<CommandBreakpoint>();
			this._variableBreakpoints = new Dictionary<string, List<VariableBreakpoint>>(StringComparer.OrdinalIgnoreCase);
			this._steppingMode = ScriptDebugger.SteppingMode.None;
			this._callStack = new ScriptDebugger.CallStackList();
			this._runningJobs = new Dictionary<Guid, PSJobStartEventArgs>();
			this._activeDebuggers = new ConcurrentStack<Debugger>();
			this._debuggerStopEventArgs = new ConcurrentStack<DebuggerStopEventArgs>();
			this._syncObject = new object();
			this._syncActiveDebuggerStopObject = new object();
			this._runningRunspaces = new Dictionary<Guid, PSMonitorRunspaceInfo>();
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0004695C File Offset: 0x00044B5C
		public override bool InBreakpoint
		{
			get
			{
				if (this._inBreakpoint)
				{
					return this._inBreakpoint;
				}
				Debugger debugger;
				return this._activeDebuggers.TryPeek(out debugger) && debugger.InBreakpoint;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0004698F File Offset: 0x00044B8F
		internal override bool IsPushed
		{
			get
			{
				return this._activeDebuggers.Count > 0;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0004699F File Offset: 0x00044B9F
		internal override bool IsPendingDebugStopEvent
		{
			get
			{
				return this._preserveDebugStopEvent != null && !this._preserveDebugStopEvent.IsSet;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x000469B9 File Offset: 0x00044BB9
		internal override bool IsDebuggerSteppingEnabled
		{
			get
			{
				return this._context._debuggingMode == 1 && this._currentDebuggerAction == DebuggerResumeAction.StepInto && this._steppingMode != ScriptDebugger.SteppingMode.None;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x000469E0 File Offset: 0x00044BE0
		private bool IsLocalSession
		{
			get
			{
				if (this._isLocalSession == null)
				{
					this._isLocalSession = new bool?(this._context.InternalHost.ExternalHost == null || !(this._context.InternalHost.ExternalHost is ServerRemoteHost));
				}
				return this._isLocalSession.Value;
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00046A40 File Offset: 0x00044C40
		internal void ResetDebugger()
		{
			this.SetDebugMode(DebugModes.None);
			this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
			this._steppingMode = ScriptDebugger.SteppingMode.None;
			this._inBreakpoint = false;
			this._idToBreakpoint.Clear();
			this._pendingBreakpoints.Clear();
			this._boundBreakpoints.Clear();
			this._commandBreakpoints.Clear();
			this._variableBreakpoints.Clear();
			ScriptDebugger._emptyBreakpointList.Clear();
			this._callStack.Clear();
			this._overOrOutFrame = null;
			this._commandProcessor = new DebuggerCommandProcessor();
			this._currentInvocationInfo = null;
			this._inBreakpoint = false;
			this._psDebuggerCommand = null;
			this.savedIgnoreScriptDebug = false;
			this._isLocalSession = null;
			this._nestedDebuggerStop = false;
			this._writeWFErrorOnce = false;
			this._debuggerStopEventArgs.Clear();
			this._lastActiveDebuggerAction = DebuggerResumeAction.Continue;
			this._currentDebuggerAction = DebuggerResumeAction.Continue;
			this._previousDebuggerAction = DebuggerResumeAction.Continue;
			this._nestedRunningFrame = null;
			this._nestedDebuggerStop = false;
			this._processingOutputCount = 0;
			this._preserveUnhandledDebugStopEvent = false;
			this.ClearRunningJobList();
			this.ClearRunningRunspaceList();
			this._activeDebuggers.Clear();
			this.ReleaseSavedDebugStop();
			this.SetDebugMode(DebugModes.Default);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00046B60 File Offset: 0x00044D60
		private void EnterScriptFunction(FunctionContext functionContext)
		{
			InvocationInfo invocationInfo = (InvocationInfo)functionContext._localsTuple.GetAutomaticVariable(AutomaticVariable.MyInvocation);
			ScriptDebugger.CallStackInfo item = new ScriptDebugger.CallStackInfo
			{
				InvocationInfo = invocationInfo,
				File = functionContext._file,
				DebuggerStepThrough = functionContext._debuggerStepThrough,
				FunctionContext = functionContext,
				IsFrameHidden = functionContext._debuggerHidden
			};
			this._callStack.Add(item);
			if (this._context._debuggingMode > 0)
			{
				ExternalScriptInfo externalScriptInfo = invocationInfo.MyCommand as ExternalScriptInfo;
				if (externalScriptInfo != null)
				{
					this.RegisterScriptFile(externalScriptInfo);
				}
				bool flag = this.CheckCommand(invocationInfo);
				this.SetupBreakpoints(functionContext);
				if (functionContext._debuggerStepThrough && this._overOrOutFrame == null && this._steppingMode == ScriptDebugger.SteppingMode.StepIn)
				{
					this.ResumeExecution(DebuggerResumeAction.StepOut);
				}
				if (flag)
				{
					this.OnSequencePointHit(functionContext);
				}
				if (this._context.PSDebugTraceLevel > 1 && !functionContext._debuggerStepThrough && !functionContext._debuggerHidden)
				{
					this.TraceScriptFunctionEntry(functionContext);
				}
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00046C74 File Offset: 0x00044E74
		private void SetupBreakpoints(FunctionContext functionContext)
		{
			Tuple<List<LineBreakpoint>, BitArray> value = this._mapScriptToBreakpoints.GetValue(functionContext._sequencePoints, (IScriptExtent[] _) => Tuple.Create<List<LineBreakpoint>, BitArray>(new List<LineBreakpoint>(), new BitArray(functionContext._sequencePoints.Length)));
			functionContext._boundBreakpoints = value.Item1;
			functionContext._breakPoints = value.Item2;
			this.SetPendingBreakpoints(functionContext);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00046CE0 File Offset: 0x00044EE0
		internal void ExitScriptFunction()
		{
			if (this._callStack.Last() == this._overOrOutFrame)
			{
				this._overOrOutFrame = null;
			}
			this._callStack.RemoveAt(this._callStack.Count - 1);
			if (this._callStack.Count == 0)
			{
				this._steppingMode = ScriptDebugger.SteppingMode.None;
				this._currentDebuggerAction = DebuggerResumeAction.Continue;
				this._previousDebuggerAction = DebuggerResumeAction.Continue;
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00046D41 File Offset: 0x00044F41
		internal void RegisterScriptFile(ExternalScriptInfo scriptCommandInfo)
		{
			this.RegisterScriptFile(scriptCommandInfo.Path, scriptCommandInfo.ScriptContents);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00046D58 File Offset: 0x00044F58
		internal void RegisterScriptFile(string path, string scriptContents)
		{
			Tuple<WeakReference, List<LineBreakpoint>> tuple;
			if (!this._boundBreakpoints.TryGetValue(path, out tuple))
			{
				this._boundBreakpoints.Add(path, Tuple.Create<WeakReference, List<LineBreakpoint>>(new WeakReference(scriptContents), new List<LineBreakpoint>()));
				return;
			}
			string text;
			tuple.Item1.TryGetTarget(out text);
			if (text == null || !text.Equals(scriptContents, StringComparison.Ordinal))
			{
				this.UnbindBoundBreakpoints(tuple.Item2);
				this._boundBreakpoints[path] = Tuple.Create<WeakReference, List<LineBreakpoint>>(new WeakReference(scriptContents), new List<LineBreakpoint>());
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00046DD5 File Offset: 0x00044FD5
		internal void AddBreakpointCommon(Breakpoint breakpoint)
		{
			if (this._context._debuggingMode == 0)
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
			}
			this._idToBreakpoint[breakpoint.Id] = breakpoint;
			this.OnBreakpointUpdated(new BreakpointUpdatedEventArgs(breakpoint, BreakpointUpdateType.Set, this._idToBreakpoint.Count));
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00046E15 File Offset: 0x00045015
		private Breakpoint AddCommandBreakpoint(CommandBreakpoint breakpoint)
		{
			this.AddBreakpointCommon(breakpoint);
			this._commandBreakpoints.Add(breakpoint);
			return breakpoint;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00046E2C File Offset: 0x0004502C
		internal Breakpoint NewCommandBreakpoint(string path, string command, ScriptBlock action)
		{
			WildcardPattern command2 = new WildcardPattern(command, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			return this.AddCommandBreakpoint(new CommandBreakpoint(path, command2, command, action));
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00046E50 File Offset: 0x00045050
		internal Breakpoint NewCommandBreakpoint(string command, ScriptBlock action)
		{
			WildcardPattern command2 = new WildcardPattern(command, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			return this.AddCommandBreakpoint(new CommandBreakpoint(null, command2, command, action));
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00046E74 File Offset: 0x00045074
		private Breakpoint AddLineBreakpoint(LineBreakpoint breakpoint)
		{
			this.AddBreakpointCommon(breakpoint);
			this._pendingBreakpoints.Add(breakpoint);
			return breakpoint;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00046E8C File Offset: 0x0004508C
		private void AddNewBreakpoint(Breakpoint breakpoint)
		{
			LineBreakpoint lineBreakpoint = breakpoint as LineBreakpoint;
			if (lineBreakpoint != null)
			{
				this.AddLineBreakpoint(lineBreakpoint);
				return;
			}
			CommandBreakpoint commandBreakpoint = breakpoint as CommandBreakpoint;
			if (commandBreakpoint != null)
			{
				this.AddCommandBreakpoint(commandBreakpoint);
				return;
			}
			VariableBreakpoint variableBreakpoint = breakpoint as VariableBreakpoint;
			if (variableBreakpoint != null)
			{
				this.AddVariableBreakpoint(variableBreakpoint);
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00046ED1 File Offset: 0x000450D1
		internal Breakpoint NewLineBreakpoint(string path, int line, ScriptBlock action)
		{
			return this.AddLineBreakpoint(new LineBreakpoint(path, line, action));
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00046EE1 File Offset: 0x000450E1
		internal Breakpoint NewStatementBreakpoint(string path, int line, int column, ScriptBlock action)
		{
			return this.AddLineBreakpoint(new LineBreakpoint(path, line, column, action));
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00046EF4 File Offset: 0x000450F4
		internal VariableBreakpoint AddVariableBreakpoint(VariableBreakpoint breakpoint)
		{
			this.AddBreakpointCommon(breakpoint);
			List<VariableBreakpoint> list;
			if (!this._variableBreakpoints.TryGetValue(breakpoint.Variable, out list))
			{
				list = new List<VariableBreakpoint>();
				this._variableBreakpoints.Add(breakpoint.Variable, list);
			}
			list.Add(breakpoint);
			return breakpoint;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00046F3D File Offset: 0x0004513D
		internal Breakpoint NewVariableBreakpoint(string path, string variableName, VariableAccessMode accessMode, ScriptBlock action)
		{
			return this.AddVariableBreakpoint(new VariableBreakpoint(path, variableName, accessMode, action));
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00046F4F File Offset: 0x0004514F
		internal Breakpoint NewVariableBreakpoint(string variableName, VariableAccessMode accessMode, ScriptBlock action)
		{
			return this.AddVariableBreakpoint(new VariableBreakpoint(null, variableName, accessMode, action));
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00046F60 File Offset: 0x00045160
		private void OnBreakpointUpdated(BreakpointUpdatedEventArgs e)
		{
			base.RaiseBreakpointUpdatedEvent(e);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00046F6C File Offset: 0x0004516C
		internal void RemoveBreakpoint(Breakpoint breakpoint)
		{
			if (this._idToBreakpoint.ContainsKey(breakpoint.Id))
			{
				this._idToBreakpoint.Remove(breakpoint.Id);
			}
			breakpoint.RemoveSelf(this);
			if (this._idToBreakpoint.Count == 0)
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
			}
			this.OnBreakpointUpdated(new BreakpointUpdatedEventArgs(breakpoint, BreakpointUpdateType.Removed, this._idToBreakpoint.Count));
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00046FD1 File Offset: 0x000451D1
		internal void RemoveVariableBreakpoint(VariableBreakpoint breakpoint)
		{
			this._variableBreakpoints[breakpoint.Variable].Remove(breakpoint);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00046FEB File Offset: 0x000451EB
		internal void RemoveCommandBreakpoint(CommandBreakpoint breakpoint)
		{
			this._commandBreakpoints.Remove(breakpoint);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00046FFC File Offset: 0x000451FC
		internal void RemoveLineBreakpoint(LineBreakpoint breakpoint)
		{
			this._pendingBreakpoints.Remove(breakpoint);
			Tuple<WeakReference, List<LineBreakpoint>> tuple;
			if (this._boundBreakpoints.TryGetValue(breakpoint.Script, out tuple))
			{
				tuple.Item2.Remove(breakpoint);
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00047058 File Offset: 0x00045258
		internal bool CheckCommand(InvocationInfo invocationInfo)
		{
			FunctionContext functionContext = this._callStack.LastFunctionContext();
			if (functionContext != null && functionContext._debuggerHidden)
			{
				return false;
			}
			List<Breakpoint> list = (from bp in this._commandBreakpoints
			where bp.Enabled && bp.Trigger(invocationInfo)
			select bp).ToList<Breakpoint>();
			bool result = true;
			if (list.Any<Breakpoint>())
			{
				list = this.TriggerBreakpoints(list);
				if (list.Any<Breakpoint>())
				{
					InvocationInfo invocationInfo2 = (functionContext != null) ? new InvocationInfo(invocationInfo.MyCommand, functionContext.CurrentPosition) : null;
					this.OnDebuggerStop(invocationInfo2, list);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000470F0 File Offset: 0x000452F0
		internal void CheckVariableRead(string variableName)
		{
			List<VariableBreakpoint> variableBreakpointsToTrigger = this.GetVariableBreakpointsToTrigger(variableName, true);
			if (variableBreakpointsToTrigger != null && variableBreakpointsToTrigger.Any<VariableBreakpoint>())
			{
				this.TriggerVariableBreakpoints(variableBreakpointsToTrigger);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00047118 File Offset: 0x00045318
		internal void CheckVariableWrite(string variableName)
		{
			List<VariableBreakpoint> variableBreakpointsToTrigger = this.GetVariableBreakpointsToTrigger(variableName, false);
			if (variableBreakpointsToTrigger != null && variableBreakpointsToTrigger.Any<VariableBreakpoint>())
			{
				this.TriggerVariableBreakpoints(variableBreakpointsToTrigger);
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0004716C File Offset: 0x0004536C
		private List<VariableBreakpoint> GetVariableBreakpointsToTrigger(string variableName, bool read)
		{
			FunctionContext functionContext = this._callStack.LastFunctionContext();
			if (functionContext != null && functionContext._debuggerHidden)
			{
				return null;
			}
			List<VariableBreakpoint> result;
			try
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
				List<VariableBreakpoint> list;
				if (!this._variableBreakpoints.TryGetValue(variableName, out list) && variableName.Equals("_", StringComparison.OrdinalIgnoreCase))
				{
					this._variableBreakpoints.TryGetValue("PSItem", out list);
				}
				if (list == null)
				{
					result = null;
				}
				else
				{
					ScriptDebugger.CallStackInfo callStackInfo = this._callStack.Last();
					string currentScriptFile = (callStackInfo != null) ? callStackInfo.File : null;
					result = (from bp in list
					where bp.Trigger(currentScriptFile, read)
					select bp).ToList<VariableBreakpoint>();
				}
			}
			finally
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
			}
			return result;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00047244 File Offset: 0x00045444
		internal void TriggerVariableBreakpoints(List<VariableBreakpoint> breakpoints)
		{
			FunctionContext functionContext = this._callStack.LastFunctionContext();
			InvocationInfo invocationInfo = (functionContext != null) ? new InvocationInfo(null, functionContext.CurrentPosition, this._context) : null;
			this.OnDebuggerStop(invocationInfo, breakpoints.ToList<Breakpoint>());
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00047284 File Offset: 0x00045484
		internal Breakpoint GetBreakpoint(int id)
		{
			Breakpoint result;
			this._idToBreakpoint.TryGetValue(id, out result);
			return result;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000472A9 File Offset: 0x000454A9
		internal List<Breakpoint> GetBreakpoints()
		{
			return (from bp in this._idToBreakpoint.Values
			orderby bp.Id
			select bp).ToList<Breakpoint>();
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000472E0 File Offset: 0x000454E0
		internal List<LineBreakpoint> GetBoundBreakpoints(IScriptExtent[] sequencePoints)
		{
			Tuple<List<LineBreakpoint>, BitArray> tuple;
			if (this._mapScriptToBreakpoints.TryGetValue(sequencePoints, out tuple))
			{
				return tuple.Item1;
			}
			return null;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00047308 File Offset: 0x00045508
		private List<Breakpoint> TriggerBreakpoints(List<Breakpoint> breakpoints)
		{
			List<Breakpoint> list = new List<Breakpoint>();
			try
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.InScriptStop);
				foreach (Breakpoint breakpoint in breakpoints)
				{
					if (breakpoint.Enabled)
					{
						try
						{
							this._inBreakpoint = true;
							if (breakpoint.Trigger() == Breakpoint.BreakpointAction.Break)
							{
								list.Add(breakpoint);
							}
						}
						finally
						{
							this._inBreakpoint = false;
						}
					}
				}
			}
			finally
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
			}
			return list;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000473A8 File Offset: 0x000455A8
		internal void EnableBreakpoint(Breakpoint bp)
		{
			bp.SetEnabled(true);
			this.OnBreakpointUpdated(new BreakpointUpdatedEventArgs(bp, BreakpointUpdateType.Enabled, this._idToBreakpoint.Count));
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000473C9 File Offset: 0x000455C9
		internal void DisableBreakpoint(Breakpoint bp)
		{
			bp.SetEnabled(false);
			this.OnBreakpointUpdated(new BreakpointUpdatedEventArgs(bp, BreakpointUpdateType.Disabled, this._idToBreakpoint.Count));
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00047410 File Offset: 0x00045610
		internal void OnSequencePointHit(FunctionContext functionContext)
		{
			if (this._context.ShouldTraceStatement && !this._callStack.Last().IsFrameHidden && !functionContext._debuggerStepThrough)
			{
				this.TraceLine(functionContext.CurrentPosition);
			}
			if (this._nestedDebuggerStop)
			{
				this._nestedDebuggerStop = false;
				this._currentDebuggerAction = DebuggerResumeAction.Continue;
				this.ResumeExecution(DebuggerResumeAction.Stop);
			}
			if (!this._wfStartEventSubscribed && this.IsJobDebuggingMode())
			{
				this.SubscribeToEngineWFJobStartEvent();
			}
			this.UpdateBreakpoints(functionContext);
			if (this._steppingMode == ScriptDebugger.SteppingMode.StepIn && (this._overOrOutFrame == null || this._callStack.Last() == this._overOrOutFrame))
			{
				if (!this._callStack.Last().IsFrameHidden)
				{
					this._overOrOutFrame = null;
					this.StopOnSequencePoint(functionContext, ScriptDebugger._emptyBreakpointList);
					return;
				}
				if (this._overOrOutFrame == null)
				{
					this.ResumeExecution(DebuggerResumeAction.StepOut);
					return;
				}
			}
			else if (functionContext._breakPoints[functionContext._currentSequencePointIndex])
			{
				List<Breakpoint> list = (from breakpoint in functionContext._boundBreakpoints
				where breakpoint.SequencePointIndex == functionContext._currentSequencePointIndex && breakpoint.Enabled
				select breakpoint).ToList<Breakpoint>();
				list = this.TriggerBreakpoints(list);
				if (list.Any<Breakpoint>())
				{
					this.StopOnSequencePoint(functionContext, list);
				}
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00047568 File Offset: 0x00045768
		private void UpdateBreakpoints(FunctionContext functionContext)
		{
			if (functionContext._breakPoints == null)
			{
				this.SetupBreakpoints(functionContext);
				return;
			}
			if (string.IsNullOrEmpty(functionContext._file))
			{
				return;
			}
			bool flag = false;
			foreach (LineBreakpoint lineBreakpoint in this._pendingBreakpoints)
			{
				if (lineBreakpoint.IsScriptBreakpoint && lineBreakpoint.Script.Equals(functionContext._file, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.SetPendingBreakpoints(functionContext);
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00047600 File Offset: 0x00045800
		private void OnDebuggerStop(InvocationInfo invocationInfo, List<Breakpoint> breakpoints)
		{
			LocalRunspace localRunspace = this._context.CurrentRunspace as LocalRunspace;
			if (localRunspace.PulsePipeline != null && localRunspace.PulsePipeline == localRunspace.GetCurrentlyRunningPipeline())
			{
				this._context.EngineHostInterface.UI.WriteWarningLine((breakpoints.Count > 0) ? string.Format(CultureInfo.CurrentCulture, DebuggerStrings.WarningBreakpointWillNotBeHit, new object[]
				{
					breakpoints[0]
				}) : new InvalidOperationException().Message);
				return;
			}
			this._currentInvocationInfo = invocationInfo;
			this._steppingMode = ScriptDebugger.SteppingMode.None;
			this._inBreakpoint = true;
			if (!this.WaitForDebugStopSubscriber())
			{
				this._inBreakpoint = false;
				return;
			}
			this._context.SetVariable(SpecialVariables.PSDebugContextVarPath, new PSDebugContext(invocationInfo, breakpoints));
			FunctionInfo functionInfo = null;
			string text = null;
			bool flag = false;
			try
			{
				Collection<PSObject> collection = this._context.SessionState.InvokeProvider.Item.Get("function:\\prompt");
				if (collection != null && collection.Count > 0)
				{
					functionInfo = (collection[0].BaseObject as FunctionInfo);
					text = functionInfo.Definition;
					if (text.Equals(InitialSessionState.DefaultPromptString, StringComparison.OrdinalIgnoreCase) || text.Trim().StartsWith(ScriptDebugger._processDebugPromptMatch, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
				}
			}
			catch (ItemNotFoundException)
			{
			}
			if (flag)
			{
				int num = text.IndexOf("\"", StringComparison.OrdinalIgnoreCase);
				if (num > -1)
				{
					num++;
					string script = "\"[DBG]: " + text.Substring(num, text.Length - num);
					functionInfo.Update(ScriptBlock.Create(script), true, ScopedItemOptions.Unspecified);
				}
				else
				{
					flag = false;
				}
			}
			PSLanguageMode languageMode = this._context.LanguageMode;
			PSLanguageMode? pslanguageMode = null;
			if (this._context.UseFullLanguageModeInDebugger && this._context.LanguageMode != PSLanguageMode.FullLanguage)
			{
				pslanguageMode = new PSLanguageMode?(this._context.LanguageMode);
				this._context.LanguageMode = PSLanguageMode.FullLanguage;
			}
			else if (SystemPolicy.GetSystemLockdownPolicy() == SystemEnforcementMode.Enforce)
			{
				pslanguageMode = new PSLanguageMode?(this._context.LanguageMode);
				this._context.LanguageMode = PSLanguageMode.ConstrainedLanguage;
			}
			RunspaceAvailability runspaceAvailability = this._context.CurrentRunspace.RunspaceAvailability;
			this._context.CurrentRunspace.UpdateRunspaceAvailability((this._context.CurrentRunspace.GetCurrentlyRunningPipeline() != null) ? RunspaceAvailability.AvailableForNestedCommand : RunspaceAvailability.Available, true);
			try
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.InScriptStop);
				if (this._callStack.Any())
				{
					this._callStack.Last().TopFrameAtBreakpoint = true;
				}
				this._commandProcessor.Reset();
				DebuggerStopEventArgs item = new DebuggerStopEventArgs(invocationInfo, breakpoints);
				this._debuggerStopEventArgs.Push(item);
				DebuggerStopEventArgs debuggerStopEventArgs = new DebuggerStopEventArgs(invocationInfo, breakpoints);
				base.RaiseDebuggerStopEvent(debuggerStopEventArgs);
				this.ResumeExecution(debuggerStopEventArgs.ResumeAction);
			}
			finally
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
				if (this._callStack.Any())
				{
					this._callStack.Last().TopFrameAtBreakpoint = false;
				}
				this._context.CurrentRunspace.UpdateRunspaceAvailability(runspaceAvailability, true);
				if (pslanguageMode != null)
				{
					this._context.LanguageMode = pslanguageMode.Value;
				}
				this._context.EngineSessionState.RemoveVariable("PSDebugContext");
				if (flag)
				{
					functionInfo.Update(ScriptBlock.Create(text), true, ScopedItemOptions.Unspecified);
				}
				DebuggerStopEventArgs debuggerStopEventArgs2;
				this._debuggerStopEventArgs.TryPop(out debuggerStopEventArgs2);
				this._inBreakpoint = false;
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00047948 File Offset: 0x00045B48
		private void ResumeExecution(DebuggerResumeAction action)
		{
			this._previousDebuggerAction = this._currentDebuggerAction;
			this._currentDebuggerAction = action;
			switch (action)
			{
			case DebuggerResumeAction.Continue:
				break;
			case DebuggerResumeAction.StepInto:
				this._steppingMode = ScriptDebugger.SteppingMode.StepIn;
				this._overOrOutFrame = null;
				return;
			case DebuggerResumeAction.StepOut:
				if (this._callStack.Count > 1)
				{
					this._steppingMode = ScriptDebugger.SteppingMode.StepIn;
					this._overOrOutFrame = this._callStack[this._callStack.Count - 2];
					return;
				}
				break;
			case DebuggerResumeAction.StepOver:
				this._steppingMode = ScriptDebugger.SteppingMode.StepIn;
				this._overOrOutFrame = this._callStack.Last();
				return;
			case DebuggerResumeAction.Stop:
				this._steppingMode = ScriptDebugger.SteppingMode.None;
				this._overOrOutFrame = null;
				throw new TerminateException();
			default:
				return;
			}
			this._steppingMode = ScriptDebugger.SteppingMode.None;
			this._overOrOutFrame = null;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00047A04 File Offset: 0x00045C04
		private bool WaitForDebugStopSubscriber()
		{
			if (base.IsDebuggerStopEventSubscribed())
			{
				return true;
			}
			if (!this._preserveUnhandledDebugStopEvent)
			{
				return false;
			}
			if (this._preserveDebugStopEvent == null)
			{
				this._preserveDebugStopEvent = new ManualResetEventSlim(true);
			}
			if (!this._preserveDebugStopEvent.IsSet)
			{
				return false;
			}
			this._preserveDebugStopEvent.Reset();
			this._preserveDebugStopEvent.Wait();
			return base.IsDebuggerStopEventSubscribed();
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00047A64 File Offset: 0x00045C64
		private void UnbindBoundBreakpoints(List<LineBreakpoint> boundBreakpoints)
		{
			foreach (LineBreakpoint lineBreakpoint in boundBreakpoints)
			{
				Tuple<List<LineBreakpoint>, BitArray> tuple;
				if (this._mapScriptToBreakpoints.TryGetValue(lineBreakpoint.SequencePoints, out tuple))
				{
					tuple.Item1.Remove(lineBreakpoint);
				}
				lineBreakpoint.SequencePoints = null;
				lineBreakpoint.SequencePointIndex = -1;
				lineBreakpoint.BreakpointBitArray = null;
				this._pendingBreakpoints.Add(lineBreakpoint);
			}
			boundBreakpoints.Clear();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00047AF4 File Offset: 0x00045CF4
		private void SetPendingBreakpoints(FunctionContext functionContext)
		{
			if (!this._pendingBreakpoints.Any<LineBreakpoint>())
			{
				return;
			}
			List<LineBreakpoint> list = new List<LineBreakpoint>();
			string file = functionContext._file;
			if (file == null)
			{
				return;
			}
			this.RegisterScriptFile(file, functionContext.CurrentPosition.StartScriptPosition.GetFullScript());
			Tuple<List<LineBreakpoint>, BitArray> tuple;
			this._mapScriptToBreakpoints.TryGetValue(functionContext._sequencePoints, out tuple);
			foreach (LineBreakpoint lineBreakpoint in this._pendingBreakpoints)
			{
				bool flag = false;
				if (lineBreakpoint.TrySetBreakpoint(file, functionContext))
				{
					if (this._context._debuggingMode == 0)
					{
						this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
					}
					flag = true;
					tuple.Item1.Add(lineBreakpoint);
					List<LineBreakpoint> item = this._boundBreakpoints[file].Item2;
					item.Add(lineBreakpoint);
				}
				if (!flag)
				{
					list.Add(lineBreakpoint);
				}
			}
			this._pendingBreakpoints = list;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00047BE8 File Offset: 0x00045DE8
		private void StopOnSequencePoint(FunctionContext functionContext, List<Breakpoint> breakpoints)
		{
			if (functionContext._debuggerHidden)
			{
				return;
			}
			if (functionContext._sequencePoints.Length == 1 && functionContext._scriptBlock != null && object.ReferenceEquals(functionContext._sequencePoints[0], functionContext._scriptBlock.Ast.Extent))
			{
				return;
			}
			InvocationInfo invocationInfo = new InvocationInfo(null, functionContext.CurrentPosition, this._context);
			this.OnDebuggerStop(invocationInfo, breakpoints);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00047C4C File Offset: 0x00045E4C
		private void SetInternalDebugMode(ScriptDebugger.InternalDebugMode mode)
		{
			lock (this._syncObject)
			{
				switch (mode)
				{
				case ScriptDebugger.InternalDebugMode.InPushedStop:
				case ScriptDebugger.InternalDebugMode.InScriptStop:
				case ScriptDebugger.InternalDebugMode.Disabled:
					this._context._debuggingMode = (int)mode;
					break;
				case ScriptDebugger.InternalDebugMode.Enabled:
					this._context._debuggingMode = (this.CanEnableDebugger ? 1 : 0);
					break;
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x00047CC8 File Offset: 0x00045EC8
		private bool CanEnableDebugger
		{
			get
			{
				return (base.DebugMode != DebugModes.RemoteScript || !this.IsLocalSession) && base.DebugMode != DebugModes.None;
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00047CEC File Offset: 0x00045EEC
		private void EnableDebuggerStepping(ScriptDebugger.EnableNestedType nestedType)
		{
			if (base.DebugMode == DebugModes.None)
			{
				throw new PSInvalidOperationException(DebuggerStrings.CannotEnableDebuggerSteppingInvalidMode, null, "Debugger:CannotEnableDebuggerSteppingInvalidMode", ErrorCategory.InvalidOperation, null);
			}
			lock (this._syncObject)
			{
				if (this._context._debuggingMode == 0)
				{
					this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
				}
				Debugger debugger;
				if (this._activeDebuggers.TryPeek(out debugger))
				{
					debugger.SetDebugMode(DebugModes.LocalScript | DebugModes.RemoteScript);
					debugger.SetDebuggerStepMode(true);
				}
				else
				{
					this.ResumeExecution(DebuggerResumeAction.StepInto);
				}
			}
			if ((nestedType & ScriptDebugger.EnableNestedType.NestedJob) == ScriptDebugger.EnableNestedType.NestedJob)
			{
				this.EnableRunningWorkflowJobsForStepping();
			}
			if ((nestedType & ScriptDebugger.EnableNestedType.NestedRunspace) == ScriptDebugger.EnableNestedType.NestedRunspace)
			{
				this.SetRunspaceListToStep(true);
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00047D94 File Offset: 0x00045F94
		private void DisableDebuggerStepping()
		{
			if (!this.IsDebuggerSteppingEnabled)
			{
				return;
			}
			lock (this._syncObject)
			{
				this.ResumeExecution(DebuggerResumeAction.Continue);
				this.RestoreInternalDebugMode();
				Debugger debugger;
				if (this._activeDebuggers.TryPeek(out debugger))
				{
					debugger.SetDebuggerStepMode(false);
				}
			}
			this.SetRunningJobListToStep(false);
			this.SetRunspaceListToStep(false);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00047E08 File Offset: 0x00046008
		private void RestoreInternalDebugMode()
		{
			ScriptDebugger.InternalDebugMode internalDebugMode = (base.DebugMode != DebugModes.None && this._idToBreakpoint.Count > 0) ? ScriptDebugger.InternalDebugMode.Enabled : ScriptDebugger.InternalDebugMode.Disabled;
			this.SetInternalDebugMode(internalDebugMode);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00047E37 File Offset: 0x00046037
		public override void SetDebuggerAction(DebuggerResumeAction resumeAction)
		{
			throw new PSNotSupportedException(StringUtil.Format(DebuggerStrings.CannotSetDebuggerAction, new object[0]));
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00047E50 File Offset: 0x00046050
		public override DebuggerStopEventArgs GetDebuggerStopArgs()
		{
			DebuggerStopEventArgs result;
			if (this._debuggerStopEventArgs.TryPeek(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00047F24 File Offset: 0x00046124
		public override DebuggerCommandResults ProcessCommand(PSCommand command, PSDataCollection<PSObject> output)
		{
			if (command == null)
			{
				throw new PSArgumentNullException("command");
			}
			if (output == null)
			{
				throw new PSArgumentNullException("output");
			}
			if (!base.DebuggerStopped)
			{
				throw new PSInvalidOperationException(DebuggerStrings.CannotProcessDebuggerCommandNotStopped, null, "Debugger:CannotProcessCommandNotStopped", ErrorCategory.InvalidOperation, null);
			}
			DebuggerCommandResults results = this.ProcessCommandForActiveDebugger(command, output);
			if (results != null)
			{
				return results;
			}
			LocalRunspace localRunspace = this._context.CurrentRunspace as LocalRunspace;
			if (localRunspace == null)
			{
				throw new PSInvalidOperationException(DebuggerStrings.CannotProcessDebuggerCommandNotStopped, null, "Debugger:CannotProcessCommandNotStopped", ErrorCategory.InvalidOperation, null);
			}
			try
			{
				using (this._psDebuggerCommand = PowerShell.Create())
				{
					if (localRunspace.GetCurrentlyRunningPipeline() != null)
					{
						this._psDebuggerCommand.SetIsNested(true);
					}
					this._psDebuggerCommand.Runspace = localRunspace;
					this._psDebuggerCommand.Commands = command;
					foreach (Command command2 in this._psDebuggerCommand.Commands.Commands)
					{
						command2.MergeMyResults(PipelineResultTypes.All, PipelineResultTypes.Output);
					}
					PSDataCollection<PSObject> internalOutput = new PSDataCollection<PSObject>();
					internalOutput.DataAdded += delegate(object sender, DataAddedEventArgs args)
					{
						foreach (PSObject psobject in internalOutput.ReadAll())
						{
							if (psobject != null)
							{
								DebuggerCommand debuggerCommand = psobject.BaseObject as DebuggerCommand;
								if (debuggerCommand != null)
								{
									bool evaluatedByDebugger = debuggerCommand.ResumeAction != null || debuggerCommand.ExecutedByDebugger;
									results = new DebuggerCommandResults(debuggerCommand.ResumeAction, evaluatedByDebugger);
								}
								else
								{
									output.Add(psobject);
								}
							}
						}
					};
					this._psDebuggerCommand.InvokeWithDebugger(null, internalOutput, null, false);
				}
			}
			finally
			{
				this._psDebuggerCommand = null;
			}
			DebuggerCommandResults result;
			if ((result = results) == null)
			{
				result = new DebuggerCommandResults(null, false);
			}
			return result;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000480E0 File Offset: 0x000462E0
		public override void StopProcessCommand()
		{
			if (this.StopCommandForActiveDebugger())
			{
				return;
			}
			PowerShell psDebuggerCommand = this._psDebuggerCommand;
			if (psDebuggerCommand != null)
			{
				psDebuggerCommand.BeginStop(null, null);
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0004810C File Offset: 0x0004630C
		public override void SetDebugMode(DebugModes mode)
		{
			lock (this._syncObject)
			{
				base.SetDebugMode(mode);
				if (!this.CanEnableDebugger)
				{
					this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
				}
				else if (this._idToBreakpoint.Count > 0 && this._context._debuggingMode == 0)
				{
					this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
				}
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00048308 File Offset: 0x00046508
		public override IEnumerable<CallStackFrame> GetCallStack()
		{
			ScriptDebugger.CallStackInfo[] callStack = this._callStack.ToArray();
			if (callStack.Length > 0)
			{
				int startingIndex = callStack.Length - 1;
				for (int j = startingIndex; j >= 0; j--)
				{
					if (callStack[j].TopFrameAtBreakpoint)
					{
						startingIndex = j;
						break;
					}
				}
				for (int i = startingIndex; i >= 0; i--)
				{
					FunctionContext funcContext = callStack[i].FunctionContext;
					yield return new CallStackFrame(funcContext, callStack[i].InvocationInfo);
				}
			}
			yield break;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00048328 File Offset: 0x00046528
		public override void SetBreakpoints(IEnumerable<Breakpoint> breakpoints)
		{
			if (breakpoints == null)
			{
				throw new PSArgumentNullException("breakpoints");
			}
			foreach (Breakpoint breakpoint in breakpoints)
			{
				if (!this._idToBreakpoint.ContainsKey(breakpoint.Id))
				{
					LineBreakpoint lineBreakpoint = breakpoint as LineBreakpoint;
					if (lineBreakpoint != null)
					{
						this.AddLineBreakpoint(lineBreakpoint);
					}
					else
					{
						CommandBreakpoint commandBreakpoint = breakpoint as CommandBreakpoint;
						if (commandBreakpoint != null)
						{
							this.AddCommandBreakpoint(commandBreakpoint);
						}
						else
						{
							VariableBreakpoint variableBreakpoint = breakpoint as VariableBreakpoint;
							if (variableBreakpoint != null)
							{
								this.AddVariableBreakpoint(variableBreakpoint);
							}
						}
					}
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x000483CC File Offset: 0x000465CC
		public override bool IsActive
		{
			get
			{
				int debuggingMode = this._context._debuggingMode;
				return debuggingMode != 0;
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x000483EC File Offset: 0x000465EC
		public override void ResetCommandProcessorSource()
		{
			this._commandProcessor.Reset();
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000483F9 File Offset: 0x000465F9
		public override void SetDebuggerStepMode(bool enabled)
		{
			if (enabled)
			{
				this.EnableDebuggerStepping(ScriptDebugger.EnableNestedType.NestedJob | ScriptDebugger.EnableNestedType.NestedRunspace);
				return;
			}
			this.DisableDebuggerStepping();
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0004840C File Offset: 0x0004660C
		internal override DebuggerCommand InternalProcessCommand(string command, IList<PSObject> output)
		{
			if (!base.DebuggerStopped)
			{
				return new DebuggerCommand(command, null, false, false);
			}
			if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
			{
				return new DebuggerCommand(command, new DebuggerResumeAction?(DebuggerResumeAction.Continue), false, true);
			}
			return this._commandProcessor.ProcessCommand(null, command, this._currentInvocationInfo, output);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00048464 File Offset: 0x00046664
		internal override bool InternalProcessListCommand(int lineNum, IList<PSObject> output)
		{
			if (!base.DebuggerStopped || this._currentInvocationInfo == null)
			{
				return false;
			}
			string fullScript = this._currentInvocationInfo.GetFullScript();
			ScriptPosition startPosition = new ScriptPosition(this._currentInvocationInfo.ScriptName, lineNum, this._currentInvocationInfo.ScriptPosition.StartScriptPosition.Offset, this._currentInvocationInfo.Line, fullScript);
			ScriptPosition endPosition = new ScriptPosition(this._currentInvocationInfo.ScriptName, lineNum, this._currentInvocationInfo.ScriptPosition.StartScriptPosition.Offset, this._currentInvocationInfo.Line, fullScript);
			InvocationInfo invocationInfo = InvocationInfo.Create(this._currentInvocationInfo.MyCommand, new ScriptExtent(startPosition, endPosition));
			this._commandProcessor.ProcessListCommand(invocationInfo, output);
			return true;
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0004851C File Offset: 0x0004671C
		internal override bool IsRemote
		{
			get
			{
				Debugger debugger;
				return this._activeDebuggers.TryPeek(out debugger) && debugger is RemotingJobDebugger;
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00048544 File Offset: 0x00046744
		internal override CallStackFrame[] GetActiveDebuggerCallStack()
		{
			Debugger debugger;
			if (this._activeDebuggers.TryPeek(out debugger))
			{
				return debugger.GetCallStack().ToArray<CallStackFrame>();
			}
			return null;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x0004856D File Offset: 0x0004676D
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x0004857C File Offset: 0x0004677C
		internal override UnhandledBreakpointProcessingMode UnhandledBreakpointMode
		{
			get
			{
				if (!this._preserveUnhandledDebugStopEvent)
				{
					return UnhandledBreakpointProcessingMode.Ignore;
				}
				return UnhandledBreakpointProcessingMode.Wait;
			}
			set
			{
				switch (value)
				{
				case UnhandledBreakpointProcessingMode.Ignore:
					this._preserveUnhandledDebugStopEvent = false;
					this.ReleaseSavedDebugStop();
					return;
				case UnhandledBreakpointProcessingMode.Wait:
					this._preserveUnhandledDebugStopEvent = true;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000485B4 File Offset: 0x000467B4
		internal override void DebugJob(Job job)
		{
			if (job == null)
			{
				throw new PSArgumentNullException("job");
			}
			lock (this._syncObject)
			{
				if (this._context._debuggingMode < 0)
				{
					throw new RuntimeException(DebuggerStrings.CannotStartJobDebuggingDebuggerBusy);
				}
			}
			bool flag2 = this.TryAddDebugJob(job);
			if (!flag2)
			{
				foreach (Job job2 in job.ChildJobs)
				{
					if (this.TryAddDebugJob(job2) && !flag2)
					{
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				throw new PSInvalidOperationException(DebuggerStrings.NoDebuggableJobsFound);
			}
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00048678 File Offset: 0x00046878
		private bool TryAddDebugJob(Job job)
		{
			IJobDebugger jobDebugger = job as IJobDebugger;
			if (jobDebugger != null && jobDebugger.Debugger != null && (job.JobStateInfo.State == JobState.Running || job.JobStateInfo.State == JobState.AtBreakpoint))
			{
				bool inBreakpoint = jobDebugger.Debugger.InBreakpoint;
				ScriptDebugger.SetDebugJobAsync(jobDebugger, false);
				this.AddToJobRunningList(new PSJobStartEventArgs(job, jobDebugger.Debugger, false), DebuggerResumeAction.StepInto);
				if (inBreakpoint)
				{
					RemotingJobDebugger remotingJobDebugger = jobDebugger.Debugger as RemotingJobDebugger;
					if (remotingJobDebugger != null)
					{
						remotingJobDebugger.CheckStateAndRaiseStopEvent();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000486F8 File Offset: 0x000468F8
		internal override void StopDebugJob(Job job)
		{
			if (job == null)
			{
				throw new PSArgumentNullException("job");
			}
			this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
			this.RemoveFromRunningJobList(job);
			ScriptDebugger.SetDebugJobAsync(job as IJobDebugger, true);
			foreach (Job job2 in job.ChildJobs)
			{
				this.RemoveFromRunningJobList(job2);
				ScriptDebugger.SetDebugJobAsync(job2 as IJobDebugger, true);
			}
			this.RestoreInternalDebugMode();
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00048780 File Offset: 0x00046980
		internal static void SetDebugJobAsync(IJobDebugger debuggableJob, bool isAsync)
		{
			if (debuggableJob != null)
			{
				debuggableJob.IsAsync = isAsync;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0004878C File Offset: 0x0004698C
		internal override void DebugRunspace(Runspace runspace)
		{
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			if (runspace.RunspaceStateInfo.State != RunspaceState.Opened)
			{
				throw new PSInvalidOperationException(string.Format(CultureInfo.InvariantCulture, DebuggerStrings.RunspaceDebuggingInvalidRunspaceState, new object[]
				{
					runspace.RunspaceStateInfo.State
				}));
			}
			lock (this._syncObject)
			{
				if (this._context._debuggingMode < 0)
				{
					throw new PSInvalidOperationException(DebuggerStrings.RunspaceDebuggingDebuggerBusy);
				}
			}
			if (runspace.Debugger == null)
			{
				throw new PSInvalidOperationException(string.Format(CultureInfo.InvariantCulture, DebuggerStrings.RunspaceDebuggingNoRunspaceDebugger, new object[]
				{
					runspace.Name
				}));
			}
			if (runspace.Debugger.DebugMode == DebugModes.None)
			{
				throw new PSInvalidOperationException(DebuggerStrings.RunspaceDebuggingDebuggerIsOff);
			}
			this.AddToRunningRunspaceList(new PSStandaloneMonitorRunspaceInfo(runspace));
			if (!runspace.Debugger.InBreakpoint)
			{
				this.EnableDebuggerStepping(ScriptDebugger.EnableNestedType.NestedRunspace);
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00048894 File Offset: 0x00046A94
		internal override void StopDebugRunspace(Runspace runspace)
		{
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
			this.RemoveFromRunningRunspaceList(runspace);
			this.RestoreInternalDebugMode();
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000488B8 File Offset: 0x00046AB8
		private void SubscribeToEngineWFJobStartEvent()
		{
			this._context.Events.SubscribeEvent(null, null, "PowerShell.WorkflowJobStartEvent", null, new PSEventReceivedEventHandler(this.HandleJobStartEvent), true, false);
			this._wfStartEventSubscribed = true;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x000488E8 File Offset: 0x00046AE8
		private void UnsubscribeFromEngineWFJobStartEvent()
		{
			PSEventManager events = this._context.Events;
			foreach (PSEventSubscriber subscriber in events.GetEventSubscribers("PowerShell.WorkflowJobStartEvent"))
			{
				events.UnsubscribeEvent(subscriber);
			}
			this._wfStartEventSubscribed = false;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00048950 File Offset: 0x00046B50
		private void HandleJobStartEvent(object sender, PSEventArgs args)
		{
			PSJobStartEventArgs psjobStartEventArgs = args.SourceArgs[0] as PSJobStartEventArgs;
			if (!psjobStartEventArgs.Debugger.GetType().FullName.Equals("Microsoft.PowerShell.Workflow.PSWorkflowDebugger", StringComparison.OrdinalIgnoreCase))
			{
				throw new PSInvalidOperationException();
			}
			DebuggerResumeAction startAction = (this._previousDebuggerAction == DebuggerResumeAction.StepInto) ? DebuggerResumeAction.StepInto : DebuggerResumeAction.Continue;
			this.AddToJobRunningList(psjobStartEventArgs, startAction);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000489A4 File Offset: 0x00046BA4
		private void AddToJobRunningList(PSJobStartEventArgs jobArgs, DebuggerResumeAction startAction)
		{
			bool flag = false;
			lock (this._syncObject)
			{
				jobArgs.Job.StateChanged += this.HandleJobStateChanged;
				if (jobArgs.Job.IsPersistentState(jobArgs.Job.JobStateInfo.State))
				{
					jobArgs.Job.StateChanged -= this.HandleJobStateChanged;
					return;
				}
				if (!this._runningJobs.ContainsKey(jobArgs.Job.InstanceId))
				{
					if (jobArgs.IsAsync)
					{
						return;
					}
					jobArgs.Job.OutputProcessingStateChanged += this.HandleOutputProcessingStateChanged;
					jobArgs.Job.MonitorOutputProcessing = true;
					this._runningJobs.Add(jobArgs.Job.InstanceId, jobArgs);
					jobArgs.Debugger.DebuggerStop += this.HandleMonitorRunningJobsDebuggerStop;
					jobArgs.Debugger.BreakpointUpdated += this.HandleBreakpointUpdated;
					flag = true;
				}
			}
			if (flag)
			{
				jobArgs.Debugger.SetParent(this, this._idToBreakpoint.Values.ToArray<Breakpoint>(), new DebuggerResumeAction?(startAction), this._context.EngineHostInterface.ExternalHost, this._context.SessionState.Path.CurrentLocation, this.GetFunctionToSourceMap());
				return;
			}
			jobArgs.Debugger.SetDebuggerStepMode(true);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00048B1C File Offset: 0x00046D1C
		private void SetRunningJobListToStep(bool enableStepping)
		{
			PSJobStartEventArgs[] array;
			lock (this._syncObject)
			{
				array = this._runningJobs.Values.ToArray<PSJobStartEventArgs>();
			}
			foreach (PSJobStartEventArgs psjobStartEventArgs in array)
			{
				try
				{
					psjobStartEventArgs.Debugger.SetDebuggerStepMode(enableStepping);
				}
				catch (PSNotImplementedException)
				{
				}
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00048BA0 File Offset: 0x00046DA0
		private void SetRunspaceListToStep(bool enableStepping)
		{
			PSMonitorRunspaceInfo[] array;
			lock (this._syncObject)
			{
				array = this._runningRunspaces.Values.ToArray<PSMonitorRunspaceInfo>();
			}
			foreach (PSMonitorRunspaceInfo psmonitorRunspaceInfo in array)
			{
				try
				{
					Debugger nestedDebugger = psmonitorRunspaceInfo.NestedDebugger;
					if (nestedDebugger != null)
					{
						nestedDebugger.SetDebuggerStepMode(enableStepping);
					}
				}
				catch (PSNotImplementedException)
				{
				}
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00048C2C File Offset: 0x00046E2C
		private Dictionary<string, DebugSource> GetFunctionToSourceMap()
		{
			Dictionary<string, DebugSource> dictionary = new Dictionary<string, DebugSource>();
			Collection<PSObject> collection = this._context.SessionState.InvokeProvider.Item.Get("function:\\*");
			foreach (PSObject psobject in collection)
			{
				WorkflowInfo workflowInfo = psobject.BaseObject as WorkflowInfo;
				if (workflowInfo != null && !string.IsNullOrEmpty(workflowInfo.Name) && workflowInfo.Module != null && workflowInfo.Module.Path != null)
				{
					string path = workflowInfo.Module.Path;
					string functionSource = this.GetFunctionSource(path);
					if (functionSource != null)
					{
						dictionary.Add(workflowInfo.Name, new DebugSource(functionSource, path, workflowInfo.XamlDefinition));
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00048D00 File Offset: 0x00046F00
		private string GetFunctionSource(string scriptFile)
		{
			if (File.Exists(scriptFile))
			{
				try
				{
					return File.ReadAllText(scriptFile);
				}
				catch (ArgumentException)
				{
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
				catch (NotSupportedException)
				{
				}
				catch (SecurityException)
				{
				}
			}
			return null;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00048D70 File Offset: 0x00046F70
		private void RemoveFromRunningJobList(Job job)
		{
			job.StateChanged -= this.HandleJobStateChanged;
			job.OutputProcessingStateChanged -= this.HandleOutputProcessingStateChanged;
			PSJobStartEventArgs psjobStartEventArgs = null;
			lock (this._syncObject)
			{
				if (this._runningJobs.ContainsKey(job.InstanceId))
				{
					psjobStartEventArgs = this._runningJobs[job.InstanceId];
					psjobStartEventArgs.Debugger.DebuggerStop -= this.HandleMonitorRunningJobsDebuggerStop;
					psjobStartEventArgs.Debugger.BreakpointUpdated -= this.HandleBreakpointUpdated;
					this._runningJobs.Remove(job.InstanceId);
				}
			}
			if (psjobStartEventArgs != null)
			{
				lock (this._syncActiveDebuggerStopObject)
				{
					Debugger debugger;
					if (this._activeDebuggers.TryPeek(out debugger) && debugger.Equals(psjobStartEventArgs.Debugger))
					{
						this.PopActiveDebugger();
					}
				}
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00048E8C File Offset: 0x0004708C
		private void ClearRunningJobList()
		{
			this.UnsubscribeFromEngineWFJobStartEvent();
			PSJobStartEventArgs[] array = null;
			lock (this._syncObject)
			{
				if (this._runningJobs.Count > 0)
				{
					array = new PSJobStartEventArgs[this._runningJobs.Values.Count];
					this._runningJobs.Values.CopyTo(array, 0);
				}
			}
			if (array != null)
			{
				foreach (PSJobStartEventArgs psjobStartEventArgs in array)
				{
					this.RemoveFromRunningJobList(psjobStartEventArgs.Job);
				}
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00048F30 File Offset: 0x00047130
		private bool PushActiveDebugger(Debugger debugger, int callstackOffset)
		{
			if (this._context._debuggingMode == -1)
			{
				return false;
			}
			this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.InPushedStop);
			this._nestedRunningFrame = this._callStack[this._callStack.Count - callstackOffset];
			this._commandProcessor.Reset();
			this._activeDebuggers.Push(debugger);
			return true;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00048F8C File Offset: 0x0004718C
		private Debugger PopActiveDebugger()
		{
			Debugger result = null;
			if (this._activeDebuggers.TryPop(out result))
			{
				int count;
				lock (this._syncObject)
				{
					count = this._runningJobs.Count;
				}
				if (count == 0)
				{
					switch (this._lastActiveDebuggerAction)
					{
					case DebuggerResumeAction.StepInto:
					case DebuggerResumeAction.StepOut:
					case DebuggerResumeAction.StepOver:
						this._steppingMode = ScriptDebugger.SteppingMode.StepIn;
						this._overOrOutFrame = this._nestedRunningFrame;
						this._nestedRunningFrame = null;
						break;
					case DebuggerResumeAction.Stop:
						this._nestedDebuggerStop = true;
						break;
					default:
						this.ResumeExecution(DebuggerResumeAction.Continue);
						break;
					}
					this._processingOutputCount = 0;
					this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
					this._currentDebuggerAction = this._lastActiveDebuggerAction;
					this._lastActiveDebuggerAction = DebuggerResumeAction.Continue;
				}
			}
			return result;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0004905C File Offset: 0x0004725C
		private void HandleActiveJobDebuggerStop(object sender, DebuggerStopEventArgs args)
		{
			if (this._runningRunspaces.Count > 0)
			{
				return;
			}
			if (args != null)
			{
				DebuggerStopEventArgs item = new DebuggerStopEventArgs(args.InvocationInfo, new Collection<Breakpoint>(args.Breakpoints), args.ResumeAction);
				this._debuggerStopEventArgs.Push(item);
				ScriptDebugger.CallStackInfo item2 = null;
				try
				{
					this._processingOutputCompleteEvent.Wait(5000);
					item2 = this.FixUpCallStack();
					base.RaiseDebuggerStopEvent(args);
					this._lastActiveDebuggerAction = args.ResumeAction;
				}
				finally
				{
					this.RestoreCallStack(item2);
					this._debuggerStopEventArgs.TryPop(out item);
				}
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000490FC File Offset: 0x000472FC
		private ScriptDebugger.CallStackInfo FixUpCallStack()
		{
			int count = this._callStack.Count;
			ScriptDebugger.CallStackInfo result = null;
			if (count > 1)
			{
				result = this._callStack.Last();
				this._callStack.RemoveAt(count - 1);
			}
			return result;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00049136 File Offset: 0x00047336
		private void RestoreCallStack(ScriptDebugger.CallStackInfo item)
		{
			if (item != null)
			{
				this._callStack.Add(item);
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00049148 File Offset: 0x00047348
		private void HandleMonitorRunningJobsDebuggerStop(object sender, DebuggerStopEventArgs args)
		{
			if (!this.IsJobDebuggingMode())
			{
				this.UnsubscribeFromEngineWFJobStartEvent();
				this.WriteWorkflowDebugNotSupportedError();
				args.ResumeAction = DebuggerResumeAction.Continue;
				return;
			}
			Debugger debugger = sender as Debugger;
			lock (this._syncActiveDebuggerStopObject)
			{
				Debugger debugger2 = null;
				if (this._activeDebuggers.TryPeek(out debugger2))
				{
					if (debugger2.Equals(debugger))
					{
						this.HandleActiveJobDebuggerStop(sender, args);
						return;
					}
					if (this.IsRunningWFJobsDebugger(debugger2))
					{
						this.PopActiveDebugger();
					}
				}
				if (this.PushActiveDebugger(debugger, 2))
				{
					this.HandleActiveJobDebuggerStop(sender, args);
				}
			}
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000491EC File Offset: 0x000473EC
		private bool IsJobDebuggingMode()
		{
			return ((base.DebugMode & DebugModes.LocalScript) == DebugModes.LocalScript && this.IsLocalSession) || ((base.DebugMode & DebugModes.RemoteScript) == DebugModes.RemoteScript && !this.IsLocalSession);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0004921C File Offset: 0x0004741C
		private void HandleBreakpointUpdated(object sender, BreakpointUpdatedEventArgs e)
		{
			switch (e.UpdateType)
			{
			case BreakpointUpdateType.Set:
				this.AddNewBreakpoint(e.Breakpoint);
				return;
			case BreakpointUpdateType.Removed:
				this.RemoveBreakpoint(e.Breakpoint);
				return;
			case BreakpointUpdateType.Enabled:
				this.EnableBreakpoint(e.Breakpoint);
				return;
			case BreakpointUpdateType.Disabled:
				this.DisableBreakpoint(e.Breakpoint);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0004927C File Offset: 0x0004747C
		private bool IsRunningWFJobsDebugger(Debugger debugger)
		{
			lock (this._syncObject)
			{
				foreach (PSJobStartEventArgs psjobStartEventArgs in this._runningJobs.Values)
				{
					if (psjobStartEventArgs.Debugger.Equals(debugger))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0004930C File Offset: 0x0004750C
		private void HandleJobStateChanged(object sender, JobStateEventArgs args)
		{
			Job job = sender as Job;
			if (job.IsPersistentState(args.JobStateInfo.State))
			{
				this.RemoveFromRunningJobList(job);
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0004933C File Offset: 0x0004753C
		private void HandleOutputProcessingStateChanged(object sender, OutputProcessingStateEventArgs e)
		{
			lock (this._syncObject)
			{
				if (e.ProcessingOutput)
				{
					if (++this._processingOutputCount == 1)
					{
						this._processingOutputCompleteEvent.Reset();
					}
				}
				else if (this._processingOutputCount > 0 && --this._processingOutputCount == 0)
				{
					this._processingOutputCompleteEvent.Set();
				}
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000493C8 File Offset: 0x000475C8
		private DebuggerCommandResults ProcessCommandForActiveDebugger(PSCommand command, PSDataCollection<PSObject> output)
		{
			bool flag = command.Commands.Count > 0 && (command.Commands[0].CommandText.Equals("Detach", StringComparison.OrdinalIgnoreCase) || command.Commands[0].CommandText.Equals("d", StringComparison.OrdinalIgnoreCase));
			Debugger debugger;
			if (this._activeDebuggers.TryPeek(out debugger))
			{
				if (flag)
				{
					this.UnhandledBreakpointMode = UnhandledBreakpointProcessingMode.Ignore;
					base.RaiseNestedDebuggingCancelEvent();
					return new DebuggerCommandResults(new DebuggerResumeAction?(DebuggerResumeAction.Continue), true);
				}
				if (command.Commands.Count > 0 && command.Commands[0].CommandText.IndexOf(".EnterNestedPrompt()", StringComparison.OrdinalIgnoreCase) > 0)
				{
					throw new PSNotSupportedException();
				}
				DebuggerStopEventArgs debuggerStopEventArgs;
				if (this._debuggerStopEventArgs.TryPeek(out debuggerStopEventArgs))
				{
					string commandText = command.Commands[0].CommandText;
					DebuggerCommand debuggerCommand = this._commandProcessor.ProcessBasicCommand(commandText);
					if (debuggerCommand != null && debuggerCommand.ResumeAction != null)
					{
						this._lastActiveDebuggerAction = debuggerCommand.ResumeAction.Value;
						return new DebuggerCommandResults(debuggerCommand.ResumeAction, true);
					}
					if (debugger.GetType().FullName.Equals("Microsoft.PowerShell.Workflow.PSWorkflowDebugger", StringComparison.OrdinalIgnoreCase))
					{
						DebuggerCommand debuggerCommand2 = this._commandProcessor.ProcessCommand(null, commandText, debuggerStopEventArgs.InvocationInfo, output);
						if (debuggerCommand2 != null && debuggerCommand2.ExecutedByDebugger)
						{
							return new DebuggerCommandResults(debuggerCommand2.ResumeAction, true);
						}
					}
				}
				return debugger.ProcessCommand(command, output);
			}
			else
			{
				if (flag)
				{
					throw new PSInvalidOperationException(DebuggerStrings.InvalidDetachCommand);
				}
				return null;
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00049554 File Offset: 0x00047754
		private bool StopCommandForActiveDebugger()
		{
			Debugger debugger;
			if (this._activeDebuggers.TryPeek(out debugger))
			{
				debugger.StopProcessCommand();
				return true;
			}
			return false;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0004957C File Offset: 0x0004777C
		private void WriteWorkflowDebugNotSupportedError()
		{
			if (!this._writeWFErrorOnce)
			{
				PSHost externalHost = this._context.EngineHostInterface.ExternalHost;
				if (externalHost != null && externalHost.UI != null)
				{
					externalHost.UI.WriteErrorLine(DebuggerStrings.WorkflowDebuggingNotSupported);
				}
				this._writeWFErrorOnce = true;
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x000495C4 File Offset: 0x000477C4
		private void EnableRunningWorkflowJobsForStepping()
		{
			if (!this._wfStartEventSubscribed)
			{
				lock (this._syncObject)
				{
					if (!this._wfStartEventSubscribed)
					{
						this.SubscribeToEngineWFJobStartEvent();
					}
				}
			}
			Collection<Job> collection;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Commands.Clear();
				powerShell.AddScript("Get-Job | ? {$_.PSJobTypeName -eq 'PSWorkflowJob'}");
				collection = powerShell.Invoke<Job>();
			}
			foreach (Job job in collection)
			{
				if (job != null)
				{
					foreach (Job job2 in job.ChildJobs)
					{
						IJobDebugger jobDebugger = job2 as IJobDebugger;
						if (jobDebugger != null)
						{
							this.AddToJobRunningList(new PSJobStartEventArgs(job2, jobDebugger.Debugger, jobDebugger.IsAsync), DebuggerResumeAction.StepInto);
						}
					}
				}
			}
			this.SetRunningJobListToStep(true);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000496F8 File Offset: 0x000478F8
		internal override void StartMonitoringRunspace(PSMonitorRunspaceInfo runspaceInfo)
		{
			if (runspaceInfo == null || runspaceInfo.Runspace == null)
			{
				return;
			}
			if (runspaceInfo.Runspace.Debugger != null && runspaceInfo.Runspace.Debugger.Equals(this))
			{
				return;
			}
			this.AddToRunningRunspaceList(runspaceInfo.Copy());
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00049733 File Offset: 0x00047933
		internal override void EndMonitoringRunspace(PSMonitorRunspaceInfo runspaceInfo)
		{
			if (runspaceInfo == null || runspaceInfo.Runspace == null)
			{
				return;
			}
			this.RemoveFromRunningRunspaceList(runspaceInfo.Runspace);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0004974D File Offset: 0x0004794D
		internal override void ReleaseSavedDebugStop()
		{
			if (this.IsPendingDebugStopEvent)
			{
				this._preserveDebugStopEvent.Set();
			}
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00049764 File Offset: 0x00047964
		private void AddToRunningRunspaceList(PSMonitorRunspaceInfo args)
		{
			Runspace runspace = args.Runspace;
			runspace.StateChanged += this.HandleRunspaceStateChanged;
			RunspaceState state = runspace.RunspaceStateInfo.State;
			if (state == RunspaceState.Broken || state == RunspaceState.Closed || state == RunspaceState.Disconnected)
			{
				runspace.StateChanged -= this.HandleRunspaceStateChanged;
				return;
			}
			lock (this._syncObject)
			{
				if (!this._runningRunspaces.ContainsKey(runspace.InstanceId))
				{
					this._runningRunspaces.Add(runspace.InstanceId, args);
				}
			}
			this.SetUpDebuggerOnRunspace(runspace);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00049810 File Offset: 0x00047A10
		private void RemoveFromRunningRunspaceList(Runspace runspace)
		{
			runspace.StateChanged -= this.HandleRunspaceStateChanged;
			PSMonitorRunspaceInfo psmonitorRunspaceInfo = null;
			lock (this._syncObject)
			{
				if (this._runningRunspaces.TryGetValue(runspace.InstanceId, out psmonitorRunspaceInfo))
				{
					this._runningRunspaces.Remove(runspace.InstanceId);
				}
			}
			NestedRunspaceDebugger nestedRunspaceDebugger = (psmonitorRunspaceInfo != null) ? psmonitorRunspaceInfo.NestedDebugger : null;
			if (nestedRunspaceDebugger != null)
			{
				nestedRunspaceDebugger.DebuggerStop -= this.HandleMonitorRunningRSDebuggerStop;
				nestedRunspaceDebugger.BreakpointUpdated -= this.HandleBreakpointUpdated;
				nestedRunspaceDebugger.Dispose();
				lock (this._syncActiveDebuggerStopObject)
				{
					Debugger debugger;
					if (this._activeDebuggers.TryPeek(out debugger) && debugger.Equals(nestedRunspaceDebugger))
					{
						this.PopActiveDebugger();
					}
				}
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00049910 File Offset: 0x00047B10
		private void ClearRunningRunspaceList()
		{
			PSMonitorRunspaceInfo[] array = null;
			lock (this._syncObject)
			{
				if (this._runningRunspaces.Count > 0)
				{
					array = new PSMonitorRunspaceInfo[this._runningRunspaces.Count];
					this._runningRunspaces.Values.CopyTo(array, 0);
				}
			}
			if (array != null)
			{
				foreach (PSMonitorRunspaceInfo psmonitorRunspaceInfo in array)
				{
					this.RemoveFromRunningRunspaceList(psmonitorRunspaceInfo.Runspace);
				}
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000499A8 File Offset: 0x00047BA8
		private void HandleRunspaceStateChanged(object sender, RunspaceStateEventArgs e)
		{
			Runspace runspace = sender as Runspace;
			bool flag = false;
			switch (e.RunspaceStateInfo.State)
			{
			case RunspaceState.Opened:
				flag = !this.SetUpDebuggerOnRunspace(runspace);
				break;
			case RunspaceState.Closed:
			case RunspaceState.Broken:
			case RunspaceState.Disconnected:
				flag = true;
				break;
			}
			if (flag)
			{
				this.RemoveFromRunningRunspaceList(runspace);
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00049A08 File Offset: 0x00047C08
		private void HandleMonitorRunningRSDebuggerStop(object sender, DebuggerStopEventArgs args)
		{
			if (sender == null || args == null)
			{
				return;
			}
			Debugger debugger = sender as Debugger;
			lock (this._syncActiveDebuggerStopObject)
			{
				Debugger debugger2;
				if (this._activeDebuggers.TryPeek(out debugger2) && this.IsRunningRSDebugger(debugger2))
				{
					this.PopActiveDebugger();
				}
				NestedRunspaceDebugger nestedRunspaceDebugger = debugger as NestedRunspaceDebugger;
				if (nestedRunspaceDebugger != null)
				{
					PSMonitorRunspaceType runspaceType = nestedRunspaceDebugger.RunspaceType;
					if (runspaceType == PSMonitorRunspaceType.WorkflowInlineScript)
					{
						bool flag2 = true;
						if (this._activeDebuggers.TryPeek(out debugger2))
						{
							flag2 = (debugger2.InstanceId != nestedRunspaceDebugger.ParentDebuggerId);
							if (flag2)
							{
								this.PopActiveDebugger();
							}
						}
						if (flag2)
						{
							PSJobStartEventArgs psjobStartEventArgs = null;
							lock (this._syncObject)
							{
								this._runningJobs.TryGetValue(nestedRunspaceDebugger.ParentDebuggerId, out psjobStartEventArgs);
							}
							if (psjobStartEventArgs == null)
							{
								return;
							}
							this.PushActiveDebugger(psjobStartEventArgs.Debugger, 2);
						}
					}
					args.InvocationInfo = nestedRunspaceDebugger.FixupInvocationInfo(args.InvocationInfo);
					if (this.PushActiveDebugger(debugger, 1))
					{
						this.HandleActiveRunspaceDebuggerStop(sender, args);
					}
				}
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00049B44 File Offset: 0x00047D44
		private void HandleActiveRunspaceDebuggerStop(object sender, DebuggerStopEventArgs args)
		{
			DebuggerStopEventArgs item = new DebuggerStopEventArgs(args.InvocationInfo, new Collection<Breakpoint>(args.Breakpoints), args.ResumeAction);
			this._debuggerStopEventArgs.Push(item);
			try
			{
				base.RaiseDebuggerStopEvent(args);
				this._lastActiveDebuggerAction = args.ResumeAction;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				this._debuggerStopEventArgs.TryPop(out item);
				this.PopActiveDebugger();
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00049BCC File Offset: 0x00047DCC
		private bool IsRunningRSDebugger(Debugger debugger)
		{
			lock (this._syncObject)
			{
				foreach (PSMonitorRunspaceInfo psmonitorRunspaceInfo in this._runningRunspaces.Values)
				{
					if (psmonitorRunspaceInfo.Runspace.Debugger.Equals(debugger))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00049C64 File Offset: 0x00047E64
		private bool SetUpDebuggerOnRunspace(Runspace runspace)
		{
			PSMonitorRunspaceInfo psmonitorRunspaceInfo = null;
			lock (this._syncObject)
			{
				this._runningRunspaces.TryGetValue(runspace.InstanceId, out psmonitorRunspaceInfo);
			}
			if (runspace.Debugger != null && psmonitorRunspaceInfo != null && psmonitorRunspaceInfo.NestedDebugger == null)
			{
				try
				{
					NestedRunspaceDebugger nestedRunspaceDebugger = psmonitorRunspaceInfo.CreateDebugger(this);
					psmonitorRunspaceInfo.NestedDebugger = nestedRunspaceDebugger;
					nestedRunspaceDebugger.DebuggerStop += this.HandleMonitorRunningRSDebuggerStop;
					nestedRunspaceDebugger.BreakpointUpdated += this.HandleBreakpointUpdated;
					if ((this._lastActiveDebuggerAction == DebuggerResumeAction.StepInto || this._currentDebuggerAction == DebuggerResumeAction.StepInto) && !nestedRunspaceDebugger.IsActive)
					{
						nestedRunspaceDebugger.SetDebuggerStepMode(true);
					}
					nestedRunspaceDebugger.CheckStateAndRaiseStopEvent();
					return true;
				}
				catch (InvalidRunspaceStateException)
				{
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00049D3C File Offset: 0x00047F3C
		public void Dispose()
		{
			this.UnsubscribeFromEngineWFJobStartEvent();
			foreach (PSJobStartEventArgs psjobStartEventArgs in this._runningJobs.Values)
			{
				Job job = psjobStartEventArgs.Job;
				if (job != null)
				{
					job.StateChanged -= this.HandleJobStateChanged;
					job.OutputProcessingStateChanged -= this.HandleOutputProcessingStateChanged;
				}
			}
			this._processingOutputCompleteEvent.Dispose();
			this._processingOutputCompleteEvent = null;
			if (this._preserveDebugStopEvent != null)
			{
				this._preserveDebugStopEvent.Dispose();
				this._preserveDebugStopEvent = null;
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00049DF0 File Offset: 0x00047FF0
		internal void EnableTracing(int traceLevel, bool? step)
		{
			if (traceLevel < 1 && (step == null || !step.Value))
			{
				this.DisableTracing();
				return;
			}
			this.savedIgnoreScriptDebug = this._context.IgnoreScriptDebug;
			this._context.IgnoreScriptDebug = false;
			this._context.PSDebugTraceLevel = traceLevel;
			if (step != null)
			{
				this._context.PSDebugTraceStep = step.Value;
			}
			this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Enabled);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00049E65 File Offset: 0x00048065
		internal void DisableTracing()
		{
			this._context.IgnoreScriptDebug = this.savedIgnoreScriptDebug;
			this._context.PSDebugTraceLevel = 0;
			this._context.PSDebugTraceStep = false;
			if (!this._idToBreakpoint.Any<KeyValuePair<int, Breakpoint>>())
			{
				this.SetInternalDebugMode(ScriptDebugger.InternalDebugMode.Disabled);
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00049EA4 File Offset: 0x000480A4
		internal void Trace(string messageId, string resourceString, params object[] args)
		{
			ActionPreference actionPreference = ActionPreference.Continue;
			string text;
			if (args == null || args.Length == 0)
			{
				text = resourceString;
			}
			else
			{
				text = StringUtil.Format(resourceString, args);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "Could not load text for msh script tracing message id '" + messageId + "'";
			}
			((InternalHostUserInterface)this._context.EngineHostInterface.UI).WriteDebugLine(text, ref actionPreference);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00049F00 File Offset: 0x00048100
		internal void TraceLine(IScriptExtent extent)
		{
			string message = PositionUtilities.BriefMessage(extent.StartScriptPosition);
			InternalHostUserInterface internalHostUserInterface = (InternalHostUserInterface)this._context.EngineHostInterface.UI;
			ActionPreference actionPreference = this._context.PSDebugTraceStep ? ActionPreference.Inquire : ActionPreference.Continue;
			internalHostUserInterface.WriteDebugLine(message, ref actionPreference);
			if (actionPreference == ActionPreference.Continue)
			{
				this._context.PSDebugTraceStep = false;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00049F5C File Offset: 0x0004815C
		internal void TraceScriptFunctionEntry(FunctionContext functionContext)
		{
			string functionName = functionContext._functionName;
			if (string.IsNullOrEmpty(functionContext._file))
			{
				this.Trace("TraceEnteringFunction", ParserStrings.TraceEnteringFunction, new object[]
				{
					functionName
				});
				return;
			}
			this.Trace("TraceEnteringFunctionDefinedInFile", ParserStrings.TraceEnteringFunctionDefinedInFile, new object[]
			{
				functionName,
				functionContext._file
			});
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00049FC0 File Offset: 0x000481C0
		internal void TraceVariableSet(string varName, object value)
		{
			if (this._callStack.Any() && this._context.PSDebugTraceLevel <= 2)
			{
				ScriptDebugger.CallStackInfo callStackInfo = this._callStack.Last();
				if (callStackInfo.IsFrameHidden || callStackInfo.DebuggerStepThrough)
				{
					return;
				}
			}
			string text = (PSObject.Base(value) is IEnumerator) ? typeof(IEnumerator).Name : PSObject.ToStringParser(this._context, value);
			int num = 60 - varName.Length;
			if (text.Length > num)
			{
				text = text.Substring(0, num) + "...";
			}
			this.Trace("TraceVariableAssignment", ParserStrings.TraceVariableAssignment, new object[]
			{
				varName,
				text
			});
		}

		// Token: 0x040005C9 RID: 1481
		private const int _jobCallStackOffset = 2;

		// Token: 0x040005CA RID: 1482
		private const int _runspaceCallStackOffset = 1;

		// Token: 0x040005CB RID: 1483
		private bool? _isLocalSession;

		// Token: 0x040005CC RID: 1484
		private readonly ConditionalWeakTable<IScriptExtent[], Tuple<List<LineBreakpoint>, BitArray>> _mapScriptToBreakpoints = new ConditionalWeakTable<IScriptExtent[], Tuple<List<LineBreakpoint>, BitArray>>();

		// Token: 0x040005CD RID: 1485
		private readonly ExecutionContext _context;

		// Token: 0x040005CE RID: 1486
		private List<LineBreakpoint> _pendingBreakpoints;

		// Token: 0x040005CF RID: 1487
		private readonly Dictionary<string, Tuple<WeakReference, List<LineBreakpoint>>> _boundBreakpoints;

		// Token: 0x040005D0 RID: 1488
		private readonly List<CommandBreakpoint> _commandBreakpoints;

		// Token: 0x040005D1 RID: 1489
		private readonly Dictionary<string, List<VariableBreakpoint>> _variableBreakpoints;

		// Token: 0x040005D2 RID: 1490
		private readonly Dictionary<int, Breakpoint> _idToBreakpoint;

		// Token: 0x040005D3 RID: 1491
		private ScriptDebugger.SteppingMode _steppingMode;

		// Token: 0x040005D4 RID: 1492
		private ScriptDebugger.CallStackInfo _overOrOutFrame;

		// Token: 0x040005D5 RID: 1493
		private readonly ScriptDebugger.CallStackList _callStack;

		// Token: 0x040005D6 RID: 1494
		private static readonly List<Breakpoint> _emptyBreakpointList = new List<Breakpoint>();

		// Token: 0x040005D7 RID: 1495
		private DebuggerCommandProcessor _commandProcessor = new DebuggerCommandProcessor();

		// Token: 0x040005D8 RID: 1496
		private InvocationInfo _currentInvocationInfo;

		// Token: 0x040005D9 RID: 1497
		private bool _inBreakpoint;

		// Token: 0x040005DA RID: 1498
		private PowerShell _psDebuggerCommand;

		// Token: 0x040005DB RID: 1499
		private bool _wfStartEventSubscribed;

		// Token: 0x040005DC RID: 1500
		private bool _nestedDebuggerStop;

		// Token: 0x040005DD RID: 1501
		private bool _writeWFErrorOnce;

		// Token: 0x040005DE RID: 1502
		private Dictionary<Guid, PSJobStartEventArgs> _runningJobs;

		// Token: 0x040005DF RID: 1503
		private ConcurrentStack<Debugger> _activeDebuggers;

		// Token: 0x040005E0 RID: 1504
		private ConcurrentStack<DebuggerStopEventArgs> _debuggerStopEventArgs;

		// Token: 0x040005E1 RID: 1505
		private DebuggerResumeAction _lastActiveDebuggerAction;

		// Token: 0x040005E2 RID: 1506
		private DebuggerResumeAction _currentDebuggerAction;

		// Token: 0x040005E3 RID: 1507
		private DebuggerResumeAction _previousDebuggerAction;

		// Token: 0x040005E4 RID: 1508
		private ScriptDebugger.CallStackInfo _nestedRunningFrame;

		// Token: 0x040005E5 RID: 1509
		private object _syncObject;

		// Token: 0x040005E6 RID: 1510
		private object _syncActiveDebuggerStopObject;

		// Token: 0x040005E7 RID: 1511
		private int _processingOutputCount;

		// Token: 0x040005E8 RID: 1512
		private ManualResetEventSlim _processingOutputCompleteEvent = new ManualResetEventSlim(true);

		// Token: 0x040005E9 RID: 1513
		private Dictionary<Guid, PSMonitorRunspaceInfo> _runningRunspaces;

		// Token: 0x040005EA RID: 1514
		private bool _preserveUnhandledDebugStopEvent;

		// Token: 0x040005EB RID: 1515
		private ManualResetEventSlim _preserveDebugStopEvent;

		// Token: 0x040005EC RID: 1516
		private static readonly string _processDebugPromptMatch = StringUtil.Format("\"[{0}:", DebuggerStrings.NestedRunspaceDebuggerPromptProcessName);

		// Token: 0x040005ED RID: 1517
		private bool savedIgnoreScriptDebug;

		// Token: 0x020000ED RID: 237
		[DebuggerDisplay("{FunctionContext.CurrentPosition}")]
		private class CallStackInfo
		{
			// Token: 0x170003AA RID: 938
			// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0004A07F File Offset: 0x0004827F
			// (set) Token: 0x06000D7B RID: 3451 RVA: 0x0004A087 File Offset: 0x00048287
			internal InvocationInfo InvocationInfo { get; set; }

			// Token: 0x170003AB RID: 939
			// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0004A090 File Offset: 0x00048290
			// (set) Token: 0x06000D7D RID: 3453 RVA: 0x0004A098 File Offset: 0x00048298
			internal string File { get; set; }

			// Token: 0x170003AC RID: 940
			// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0004A0A1 File Offset: 0x000482A1
			// (set) Token: 0x06000D7F RID: 3455 RVA: 0x0004A0A9 File Offset: 0x000482A9
			internal bool DebuggerStepThrough { get; set; }

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x06000D80 RID: 3456 RVA: 0x0004A0B2 File Offset: 0x000482B2
			// (set) Token: 0x06000D81 RID: 3457 RVA: 0x0004A0BA File Offset: 0x000482BA
			internal FunctionContext FunctionContext { get; set; }

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0004A0C3 File Offset: 0x000482C3
			// (set) Token: 0x06000D83 RID: 3459 RVA: 0x0004A0CB File Offset: 0x000482CB
			internal bool IsFrameHidden { get; set; }

			// Token: 0x170003AF RID: 943
			// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0004A0D4 File Offset: 0x000482D4
			// (set) Token: 0x06000D85 RID: 3461 RVA: 0x0004A0DC File Offset: 0x000482DC
			internal bool TopFrameAtBreakpoint { get; set; }
		}

		// Token: 0x020000EE RID: 238
		private class CallStackList
		{
			// Token: 0x06000D87 RID: 3463 RVA: 0x0004A0ED File Offset: 0x000482ED
			internal CallStackList()
			{
				this._callStackList = new List<ScriptDebugger.CallStackInfo>();
				this._syncObj = new object();
			}

			// Token: 0x06000D88 RID: 3464 RVA: 0x0004A10C File Offset: 0x0004830C
			internal void Add(ScriptDebugger.CallStackInfo item)
			{
				lock (this._syncObj)
				{
					this._callStackList.Add(item);
				}
			}

			// Token: 0x06000D89 RID: 3465 RVA: 0x0004A154 File Offset: 0x00048354
			internal void RemoveAt(int index)
			{
				lock (this._syncObj)
				{
					this._callStackList.RemoveAt(index);
				}
			}

			// Token: 0x170003B0 RID: 944
			internal ScriptDebugger.CallStackInfo this[int index]
			{
				get
				{
					ScriptDebugger.CallStackInfo result;
					lock (this._syncObj)
					{
						result = ((index > -1 && index < this._callStackList.Count) ? this._callStackList[index] : null);
					}
					return result;
				}
			}

			// Token: 0x06000D8B RID: 3467 RVA: 0x0004A1FC File Offset: 0x000483FC
			internal ScriptDebugger.CallStackInfo Last()
			{
				ScriptDebugger.CallStackInfo result;
				lock (this._syncObj)
				{
					result = ((this._callStackList.Count > 0) ? this._callStackList.Last<ScriptDebugger.CallStackInfo>() : null);
				}
				return result;
			}

			// Token: 0x06000D8C RID: 3468 RVA: 0x0004A254 File Offset: 0x00048454
			internal FunctionContext LastFunctionContext()
			{
				FunctionContext result;
				lock (this._syncObj)
				{
					result = ((this._callStackList.Count > 0) ? this._callStackList.Last<ScriptDebugger.CallStackInfo>().FunctionContext : null);
				}
				return result;
			}

			// Token: 0x06000D8D RID: 3469 RVA: 0x0004A2B4 File Offset: 0x000484B4
			internal bool Any()
			{
				bool result;
				lock (this._syncObj)
				{
					result = this._callStackList.Any<ScriptDebugger.CallStackInfo>();
				}
				return result;
			}

			// Token: 0x170003B1 RID: 945
			// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0004A2FC File Offset: 0x000484FC
			internal int Count
			{
				get
				{
					int count;
					lock (this._syncObj)
					{
						count = this._callStackList.Count;
					}
					return count;
				}
			}

			// Token: 0x06000D8F RID: 3471 RVA: 0x0004A344 File Offset: 0x00048544
			internal ScriptDebugger.CallStackInfo[] ToArray()
			{
				ScriptDebugger.CallStackInfo[] result;
				lock (this._syncObj)
				{
					result = this._callStackList.ToArray();
				}
				return result;
			}

			// Token: 0x06000D90 RID: 3472 RVA: 0x0004A38C File Offset: 0x0004858C
			internal void Clear()
			{
				lock (this._syncObj)
				{
					this._callStackList.Clear();
				}
			}

			// Token: 0x040005F5 RID: 1525
			private List<ScriptDebugger.CallStackInfo> _callStackList;

			// Token: 0x040005F6 RID: 1526
			private object _syncObj;
		}

		// Token: 0x020000EF RID: 239
		private enum SteppingMode
		{
			// Token: 0x040005F8 RID: 1528
			StepIn,
			// Token: 0x040005F9 RID: 1529
			None
		}

		// Token: 0x020000F0 RID: 240
		private enum InternalDebugMode
		{
			// Token: 0x040005FB RID: 1531
			InPushedStop = -2,
			// Token: 0x040005FC RID: 1532
			InScriptStop,
			// Token: 0x040005FD RID: 1533
			Disabled,
			// Token: 0x040005FE RID: 1534
			Enabled
		}

		// Token: 0x020000F1 RID: 241
		[Flags]
		private enum EnableNestedType
		{
			// Token: 0x04000600 RID: 1536
			None = 0,
			// Token: 0x04000601 RID: 1537
			NestedJob = 1,
			// Token: 0x04000602 RID: 1538
			NestedRunspace = 2
		}
	}
}
