using System;
using System.Collections.Generic;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000310 RID: 784
	internal sealed class ServerRemoteDebugger : Debugger, IDisposable
	{
		// Token: 0x06002516 RID: 9494 RVA: 0x000CFB31 File Offset: 0x000CDD31
		private ServerRemoteDebugger()
		{
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000CFB3C File Offset: 0x000CDD3C
		internal ServerRemoteDebugger(IRSPDriverInvoke driverInvoker, Runspace runspace, Debugger debugger)
		{
			if (driverInvoker == null)
			{
				throw new PSArgumentNullException("driverInvoker");
			}
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			if (debugger == null)
			{
				throw new PSArgumentNullException("debugger");
			}
			this._driverInvoker = driverInvoker;
			this._runspace = runspace;
			this._wrappedDebugger = new ObjectRef<Debugger>(debugger);
			this.SetDebuggerCallbacks();
			this._runspace.Name = "RemoteHost";
			this._runspace.InternalDebugger = this;
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x000CFBB5 File Offset: 0x000CDDB5
		public override bool InBreakpoint
		{
			get
			{
				return this._inDebugMode;
			}
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000CFBBD File Offset: 0x000CDDBD
		public override void SetDebuggerAction(DebuggerResumeAction resumeAction)
		{
			if (!this._inDebugMode)
			{
				throw new PSInvalidOperationException(StringUtil.Format(DebuggerStrings.CannotSetRemoteDebuggerAction, new object[0]));
			}
			this.ExitDebugMode(resumeAction);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000CFBE4 File Offset: 0x000CDDE4
		public override DebuggerStopEventArgs GetDebuggerStopArgs()
		{
			return this._wrappedDebugger.Value.GetDebuggerStopArgs();
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000CFBF8 File Offset: 0x000CDDF8
		public override DebuggerCommandResults ProcessCommand(PSCommand command, PSDataCollection<PSObject> output)
		{
			if (this.LocalDebugMode)
			{
				return this._wrappedDebugger.Value.ProcessCommand(command, output);
			}
			if (!this.InBreakpoint || this._threadCommandProcessing != null)
			{
				throw new PSInvalidOperationException(StringUtil.Format(DebuggerStrings.CannotProcessDebuggerCommandNotStopped, new object[0]));
			}
			if (this._processCommandCompleteEvent == null)
			{
				this._processCommandCompleteEvent = new ManualResetEventSlim(false);
			}
			this._threadCommandProcessing = new ServerRemoteDebugger.ThreadCommandProcessing(command, output, this._wrappedDebugger.Value, this._processCommandCompleteEvent);
			DebuggerCommandResults result;
			try
			{
				result = this._threadCommandProcessing.Invoke(this._nestedDebugStopCompleteEvent);
			}
			finally
			{
				this._threadCommandProcessing = null;
			}
			return result;
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000CFCA8 File Offset: 0x000CDEA8
		public override void StopProcessCommand()
		{
			if (this.LocalDebugMode)
			{
				this._wrappedDebugger.Value.StopProcessCommand();
			}
			ServerRemoteDebugger.ThreadCommandProcessing threadCommandProcessing = this._threadCommandProcessing;
			if (threadCommandProcessing != null)
			{
				threadCommandProcessing.Stop();
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000CFCDD File Offset: 0x000CDEDD
		public override void SetDebugMode(DebugModes mode)
		{
			this._wrappedDebugger.Value.SetDebugMode(mode);
			base.SetDebugMode(mode);
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x000CFCF7 File Offset: 0x000CDEF7
		public override bool IsActive
		{
			get
			{
				return this.InBreakpoint || this._wrappedDebugger.Value.IsActive || this._wrappedDebugger.Value.InBreakpoint;
			}
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000CFD28 File Offset: 0x000CDF28
		public override void SetDebuggerStepMode(bool enabled)
		{
			DebugModes debugMode = DebugModes.LocalScript | DebugModes.RemoteScript;
			base.SetDebugMode(debugMode);
			this._wrappedDebugger.Value.SetDebugMode(debugMode);
			this._wrappedDebugger.Value.SetDebuggerStepMode(enabled);
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000CFD60 File Offset: 0x000CDF60
		internal override DebuggerCommand InternalProcessCommand(string command, IList<PSObject> output)
		{
			return this._wrappedDebugger.Value.InternalProcessCommand(command, output);
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000CFD74 File Offset: 0x000CDF74
		internal override void DebugJob(Job job)
		{
			this._wrappedDebugger.Value.DebugJob(job);
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000CFD87 File Offset: 0x000CDF87
		internal override void StopDebugJob(Job job)
		{
			this._wrappedDebugger.Value.StopDebugJob(job);
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000CFD9A File Offset: 0x000CDF9A
		internal override void DebugRunspace(Runspace runspace)
		{
			this._wrappedDebugger.Value.DebugRunspace(runspace);
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000CFDAD File Offset: 0x000CDFAD
		internal override void StopDebugRunspace(Runspace runspace)
		{
			this._wrappedDebugger.Value.StopDebugRunspace(runspace);
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x000CFDC0 File Offset: 0x000CDFC0
		internal override bool IsPushed
		{
			get
			{
				return this._wrappedDebugger.Value.IsPushed;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x000CFDD2 File Offset: 0x000CDFD2
		internal override bool IsRemote
		{
			get
			{
				return this._wrappedDebugger.Value.IsRemote;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x000CFDE4 File Offset: 0x000CDFE4
		internal override bool IsDebuggerSteppingEnabled
		{
			get
			{
				return this._wrappedDebugger.Value.IsDebuggerSteppingEnabled;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x000CFDF6 File Offset: 0x000CDFF6
		// (set) Token: 0x06002529 RID: 9513 RVA: 0x000CFE08 File Offset: 0x000CE008
		internal override UnhandledBreakpointProcessingMode UnhandledBreakpointMode
		{
			get
			{
				return this._wrappedDebugger.Value.UnhandledBreakpointMode;
			}
			set
			{
				this._wrappedDebugger.Value.UnhandledBreakpointMode = value;
				if (value == UnhandledBreakpointProcessingMode.Ignore && this._inDebugMode)
				{
					this.ExitDebugMode(DebuggerResumeAction.Continue);
				}
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x000CFE2E File Offset: 0x000CE02E
		internal override bool IsPendingDebugStopEvent
		{
			get
			{
				return this._wrappedDebugger.Value.IsPendingDebugStopEvent;
			}
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000CFE40 File Offset: 0x000CE040
		internal override void ReleaseSavedDebugStop()
		{
			this._wrappedDebugger.Value.ReleaseSavedDebugStop();
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000CFE52 File Offset: 0x000CE052
		public void Dispose()
		{
			this.RemoveDebuggerCallbacks();
			if (this._inDebugMode)
			{
				this.ExitDebugMode(DebuggerResumeAction.Stop);
			}
			if (this._nestedDebugStopCompleteEvent != null)
			{
				this._nestedDebugStopCompleteEvent.Dispose();
			}
			if (this._processCommandCompleteEvent != null)
			{
				this._processCommandCompleteEvent.Dispose();
			}
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000CFE90 File Offset: 0x000CE090
		private void SetDebuggerCallbacks()
		{
			if (this._runspace != null && this._runspace.ExecutionContext != null && this._wrappedDebugger.Value != null)
			{
				this.SubscribeWrappedDebugger(this._wrappedDebugger.Value);
				PSLocalEventManager events = this._runspace.ExecutionContext.Events;
				if (!events.GetEventSubscribers("PSInternalRemoteDebuggerStopEvent").GetEnumerator().MoveNext())
				{
					events.SubscribeEvent(null, null, "PSInternalRemoteDebuggerStopEvent", null, null, true, true);
				}
				if (!events.GetEventSubscribers("PSInternalRemoteDebuggerBreakpointUpdatedEvent").GetEnumerator().MoveNext())
				{
					events.SubscribeEvent(null, null, "PSInternalRemoteDebuggerBreakpointUpdatedEvent", null, null, true, true);
				}
			}
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000CFF3C File Offset: 0x000CE13C
		private void RemoveDebuggerCallbacks()
		{
			if (this._runspace != null && this._runspace.ExecutionContext != null && this._wrappedDebugger.Value != null)
			{
				this.UnsubscribeWrappedDebugger(this._wrappedDebugger.Value);
				PSLocalEventManager events = this._runspace.ExecutionContext.Events;
				foreach (PSEventSubscriber subscriber in events.GetEventSubscribers("PSInternalRemoteDebuggerStopEvent"))
				{
					events.UnsubscribeEvent(subscriber);
				}
				foreach (PSEventSubscriber subscriber2 in events.GetEventSubscribers("PSInternalRemoteDebuggerBreakpointUpdatedEvent"))
				{
					events.UnsubscribeEvent(subscriber2);
				}
			}
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000D0024 File Offset: 0x000CE224
		private void HandleDebuggerStop(object sender, DebuggerStopEventArgs e)
		{
			if (!this.IsDebuggingSupported())
			{
				return;
			}
			if (this.LocalDebugMode)
			{
				base.RaiseDebuggerStopEvent(e);
				return;
			}
			if ((base.DebugMode & DebugModes.RemoteScript) != DebugModes.RemoteScript)
			{
				return;
			}
			this._debuggerStopEventArgs = e;
			try
			{
				PSHost externalHost = this._runspace.ExecutionContext.InternalHost.ExternalHost;
				this._runspace.ExecutionContext.Events.GenerateEvent("PSInternalRemoteDebuggerStopEvent", null, new object[]
				{
					e
				}, null);
				this.EnterDebugMode(this._wrappedDebugger.Value.IsPushed);
				this._runspace.ExecutionContext.InternalHost.SetHostRef(externalHost);
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
			finally
			{
				this._debuggerStopEventArgs = null;
			}
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000D00FC File Offset: 0x000CE2FC
		private void HandleBreakpointUpdated(object sender, BreakpointUpdatedEventArgs e)
		{
			if (!this.IsDebuggingSupported())
			{
				return;
			}
			if (this.LocalDebugMode)
			{
				base.RaiseBreakpointUpdatedEvent(e);
				return;
			}
			try
			{
				this._runspace.ExecutionContext.Events.GenerateEvent("PSInternalRemoteDebuggerBreakpointUpdatedEvent", null, new object[]
				{
					e
				}, null);
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000D0168 File Offset: 0x000CE368
		private void HandleNestedDebuggingCancelEvent(object sender, EventArgs e)
		{
			base.RaiseNestedDebuggingCancelEvent();
			if (this._inDebugMode)
			{
				this.ExitDebugMode(DebuggerResumeAction.Continue);
			}
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000D0180 File Offset: 0x000CE380
		private void EnterDebugMode(bool isNestedStop)
		{
			this._inDebugMode = true;
			try
			{
				this._runspace.ExecutionContext.SetVariable(SpecialVariables.NestedPromptCounterVarPath, 1);
				if (isNestedStop)
				{
					if (this._nestedDebugStopCompleteEvent == null)
					{
						this._nestedDebugStopCompleteEvent = new ManualResetEventSlim(false);
					}
					this._nestedDebugging = true;
					this.OnEnterDebugMode(this._nestedDebugStopCompleteEvent);
				}
				else
				{
					this._driverInvoker.EnterNestedPipeline();
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				this._inDebugMode = false;
				this._nestedDebugging = false;
			}
			if (this._raiseStopEventLocally)
			{
				this._raiseStopEventLocally = false;
				this.LocalDebugMode = true;
				this.HandleDebuggerStop(this, this._debuggerStopEventArgs);
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000D0244 File Offset: 0x000CE444
		private void OnEnterDebugMode(ManualResetEventSlim debugModeCompletedEvent)
		{
			for (;;)
			{
				debugModeCompletedEvent.Wait();
				debugModeCompletedEvent.Reset();
				if (this._threadCommandProcessing == null)
				{
					break;
				}
				this._threadCommandProcessing.DoInvoke();
				this._threadCommandProcessing = null;
			}
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000D0270 File Offset: 0x000CE470
		private void ExitDebugMode(DebuggerResumeAction resumeAction)
		{
			this._debuggerStopEventArgs.ResumeAction = resumeAction;
			try
			{
				if (this._nestedDebugging)
				{
					this._nestedDebugStopCompleteEvent.Set();
				}
				else
				{
					this._driverInvoker.ExitNestedPipeline();
				}
				this._runspace.ExecutionContext.SetVariable(SpecialVariables.NestedPromptCounterVarPath, 0);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000D02E0 File Offset: 0x000CE4E0
		private void SubscribeWrappedDebugger(Debugger wrappedDebugger)
		{
			wrappedDebugger.DebuggerStop += this.HandleDebuggerStop;
			wrappedDebugger.BreakpointUpdated += this.HandleBreakpointUpdated;
			wrappedDebugger.NestedDebuggingCancelledEvent += this.HandleNestedDebuggingCancelEvent;
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000D0318 File Offset: 0x000CE518
		private void UnsubscribeWrappedDebugger(Debugger wrappedDebugger)
		{
			wrappedDebugger.DebuggerStop -= this.HandleDebuggerStop;
			wrappedDebugger.BreakpointUpdated -= this.HandleBreakpointUpdated;
			wrappedDebugger.NestedDebuggingCancelledEvent -= this.HandleNestedDebuggingCancelEvent;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000D0350 File Offset: 0x000CE550
		private bool IsDebuggingSupported()
		{
			LocalRunspace localRunspace = this._runspace as LocalRunspace;
			if (localRunspace != null)
			{
				CmdletInfo cmdlet = localRunspace.ExecutionContext.EngineSessionState.GetCmdlet("Set-PSBreakpoint");
				if (cmdlet != null && cmdlet.Visibility != SessionStateEntryVisibility.Public)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000D0390 File Offset: 0x000CE590
		internal bool HandleStopSignal()
		{
			if (this.IsPushed && this._threadCommandProcessing != null)
			{
				this.StopProcessCommand();
				return true;
			}
			this._wrappedDebugger.Value.SetDebugMode(DebugModes.None);
			if (this.InBreakpoint)
			{
				try
				{
					this.SetDebuggerAction(DebuggerResumeAction.Continue);
				}
				catch (PSInvalidOperationException)
				{
				}
			}
			return false;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000D03EC File Offset: 0x000CE5EC
		internal void CheckDebuggerState()
		{
			if (this._wrappedDebugger.Value.DebugMode == DebugModes.None && (base.DebugMode & DebugModes.RemoteScript) == DebugModes.RemoteScript)
			{
				this._wrappedDebugger.Value.SetDebugMode(base.DebugMode);
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000D0424 File Offset: 0x000CE624
		internal void StartPowerShellCommand(PowerShell powershell, Guid powershellId, Guid runspacePoolId, ServerRunspacePoolDriver runspacePoolDriver, ApartmentState apartmentState, ServerRemoteHost remoteHost, HostInfo hostInfo, RemoteStreamOptions streamOptions, bool addToHistory)
		{
			Runspace runspace = (remoteHost != null) ? RunspaceFactory.CreateRunspace(remoteHost) : RunspaceFactory.CreateRunspace();
			runspace.Open();
			try
			{
				powershell.InvocationStateChanged += this.HandlePowerShellInvocationStateChanged;
				powershell.SetIsNested(false);
				string script = "\r\n                    param ($Debugger, $Commands, $output)\r\n                    trap { throw $_ }\r\n                    $Debugger.ProcessCommand($Commands, $output)\r\n                    ";
				PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
				PSCommand value = new PSCommand(powershell.Commands);
				powershell.Commands.Clear();
				powershell.AddScript(script).AddParameter("Debugger", this).AddParameter("Commands", value).AddParameter("output", psdataCollection);
				ServerPowerShellDriver serverPowerShellDriver = new ServerPowerShellDriver(powershell, null, true, powershellId, runspacePoolId, runspacePoolDriver, apartmentState, hostInfo, streamOptions, addToHistory, runspace, psdataCollection);
				serverPowerShellDriver.Start();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				runspace.Close();
				runspace.Dispose();
			}
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000D04F8 File Offset: 0x000CE6F8
		private void HandlePowerShellInvocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
		{
			if (e.InvocationStateInfo.State == PSInvocationState.Completed || e.InvocationStateInfo.State == PSInvocationState.Stopped || e.InvocationStateInfo.State == PSInvocationState.Failed)
			{
				PowerShell powerShell = sender as PowerShell;
				powerShell.InvocationStateChanged -= this.HandlePowerShellInvocationStateChanged;
				Runspace runspace = powerShell.GetRunspaceConnection() as Runspace;
				runspace.Close();
				runspace.Dispose();
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000D0560 File Offset: 0x000CE760
		internal int GetBreakpointCount()
		{
			ScriptDebugger scriptDebugger = this._wrappedDebugger.Value as ScriptDebugger;
			if (scriptDebugger != null)
			{
				return scriptDebugger.GetBreakpoints().Count;
			}
			return 0;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000D0590 File Offset: 0x000CE790
		internal void PushDebugger(Debugger debugger)
		{
			if (debugger == null)
			{
				throw new PSArgumentNullException("debugger");
			}
			if (debugger.Equals(this))
			{
				throw new PSInvalidOperationException(DebuggerStrings.RemoteServerDebuggerCannotPushSelf);
			}
			if (this._wrappedDebugger.IsOverridden)
			{
				throw new PSInvalidOperationException(DebuggerStrings.RemoteServerDebuggerAlreadyPushed);
			}
			this.UnsubscribeWrappedDebugger(this._wrappedDebugger.Value);
			this._wrappedDebugger.Override(debugger);
			this.SubscribeWrappedDebugger(this._wrappedDebugger.Value);
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000D0605 File Offset: 0x000CE805
		internal void PopDebugger()
		{
			if (!this._wrappedDebugger.IsOverridden)
			{
				return;
			}
			this.UnsubscribeWrappedDebugger(this._wrappedDebugger.Value);
			this._wrappedDebugger.Revert();
			this.SubscribeWrappedDebugger(this._wrappedDebugger.Value);
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000D0642 File Offset: 0x000CE842
		internal void ReleaseAndRaiseDebugStopLocal()
		{
			if (this._inDebugMode)
			{
				this._raiseStopEventLocally = true;
				this.ExitDebugMode(DebuggerResumeAction.Continue);
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002540 RID: 9536 RVA: 0x000D065A File Offset: 0x000CE85A
		// (set) Token: 0x06002541 RID: 9537 RVA: 0x000D0662 File Offset: 0x000CE862
		internal bool LocalDebugMode { get; set; }

		// Token: 0x04001234 RID: 4660
		internal const string SetPSBreakCommandText = "Set-PSBreakpoint";

		// Token: 0x04001235 RID: 4661
		private IRSPDriverInvoke _driverInvoker;

		// Token: 0x04001236 RID: 4662
		private Runspace _runspace;

		// Token: 0x04001237 RID: 4663
		private ObjectRef<Debugger> _wrappedDebugger;

		// Token: 0x04001238 RID: 4664
		private bool _inDebugMode;

		// Token: 0x04001239 RID: 4665
		private DebuggerStopEventArgs _debuggerStopEventArgs;

		// Token: 0x0400123A RID: 4666
		private ManualResetEventSlim _nestedDebugStopCompleteEvent;

		// Token: 0x0400123B RID: 4667
		private bool _nestedDebugging;

		// Token: 0x0400123C RID: 4668
		private ManualResetEventSlim _processCommandCompleteEvent;

		// Token: 0x0400123D RID: 4669
		private ServerRemoteDebugger.ThreadCommandProcessing _threadCommandProcessing;

		// Token: 0x0400123E RID: 4670
		private bool _raiseStopEventLocally;

		// Token: 0x02000311 RID: 785
		private sealed class ThreadCommandProcessing
		{
			// Token: 0x06002542 RID: 9538 RVA: 0x000D066B File Offset: 0x000CE86B
			private ThreadCommandProcessing()
			{
			}

			// Token: 0x06002543 RID: 9539 RVA: 0x000D0673 File Offset: 0x000CE873
			public ThreadCommandProcessing(PSCommand command, PSDataCollection<PSObject> output, Debugger debugger, ManualResetEventSlim processCommandCompleteEvent)
			{
				this._command = command;
				this._output = output;
				this._wrappedDebugger = debugger;
				this._commandCompleteEvent = processCommandCompleteEvent;
			}

			// Token: 0x06002544 RID: 9540 RVA: 0x000D0698 File Offset: 0x000CE898
			public DebuggerCommandResults Invoke(ManualResetEventSlim startInvokeEvent)
			{
				WindowsIdentity windowsIdentity = null;
				try
				{
					windowsIdentity = WindowsIdentity.GetCurrent();
				}
				catch (SecurityException)
				{
				}
				this._identityToImpersonate = ((windowsIdentity != null && windowsIdentity.ImpersonationLevel == TokenImpersonationLevel.Impersonation) ? windowsIdentity : null);
				startInvokeEvent.Set();
				this._commandCompleteEvent.Wait();
				this._commandCompleteEvent.Reset();
				this._identityToImpersonate = null;
				if (this._exception != null)
				{
					throw this._exception;
				}
				return this._results;
			}

			// Token: 0x06002545 RID: 9541 RVA: 0x000D0710 File Offset: 0x000CE910
			public void Stop()
			{
				Debugger wrappedDebugger = this._wrappedDebugger;
				if (wrappedDebugger != null)
				{
					wrappedDebugger.StopProcessCommand();
				}
			}

			// Token: 0x06002546 RID: 9542 RVA: 0x000D0730 File Offset: 0x000CE930
			internal void DoInvoke()
			{
				WindowsImpersonationContext windowsImpersonationContext = null;
				if (this._identityToImpersonate != null && this._identityToImpersonate.ImpersonationLevel == TokenImpersonationLevel.Impersonation)
				{
					windowsImpersonationContext = this._identityToImpersonate.Impersonate();
				}
				try
				{
					this._results = this._wrappedDebugger.ProcessCommand(this._command, this._output);
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					this._exception = ex;
				}
				finally
				{
					this._commandCompleteEvent.Set();
					if (windowsImpersonationContext != null)
					{
						try
						{
							windowsImpersonationContext.Undo();
							windowsImpersonationContext.Dispose();
						}
						catch (SecurityException)
						{
						}
					}
				}
			}

			// Token: 0x04001240 RID: 4672
			private ManualResetEventSlim _commandCompleteEvent;

			// Token: 0x04001241 RID: 4673
			private Debugger _wrappedDebugger;

			// Token: 0x04001242 RID: 4674
			private PSCommand _command;

			// Token: 0x04001243 RID: 4675
			private PSDataCollection<PSObject> _output;

			// Token: 0x04001244 RID: 4676
			private DebuggerCommandResults _results;

			// Token: 0x04001245 RID: 4677
			private Exception _exception;

			// Token: 0x04001246 RID: 4678
			private WindowsIdentity _identityToImpersonate;
		}
	}
}
