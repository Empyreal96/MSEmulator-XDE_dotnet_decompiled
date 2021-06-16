using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Sqm;
using System.Management.Automation.Tracing;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200021B RID: 539
	internal sealed class LocalPipeline : PipelineBase
	{
		// Token: 0x0600197D RID: 6525 RVA: 0x0009908A File Offset: 0x0009728A
		internal LocalPipeline(LocalRunspace runspace, string command, bool addToHistory, bool isNested) : base(runspace, command, addToHistory, isNested)
		{
			this._stopper = new PipelineStopper(this);
			this.InitStreams();
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x000990BC File Offset: 0x000972BC
		internal LocalPipeline(LocalRunspace runspace, CommandCollection command, bool addToHistory, bool isNested, ObjectStreamBase inputStream, ObjectStreamBase outputStream, ObjectStreamBase errorStream, PSInformationalBuffers infoBuffers) : base(runspace, command, addToHistory, isNested, inputStream, outputStream, errorStream, infoBuffers)
		{
			this._stopper = new PipelineStopper(this);
			this.InitStreams();
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00099101 File Offset: 0x00097301
		internal LocalPipeline(LocalPipeline pipeline) : base(pipeline)
		{
			this._stopper = new PipelineStopper(this);
			this.InitStreams();
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0009912F File Offset: 0x0009732F
		public override Pipeline Copy()
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("pipeline");
			}
			return new LocalPipeline(this);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0009914C File Offset: 0x0009734C
		protected override void StartPipelineExecution()
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("pipeline");
			}
			this.useExternalInput = (base.InputStream.IsOpen || base.InputStream.Count > 0);
			switch (this.IsNested ? PSThreadOptions.UseCurrentThread : this.LocalRunspace.ThreadOptions)
			{
			case PSThreadOptions.Default:
			case PSThreadOptions.UseNewThread:
			{
				Thread thread = new Thread(new ThreadStart(this.InvokeThreadProc), LocalPipeline.MaxStack);
				this.SetupInvokeThread(thread, true);
				ApartmentState apartmentState;
				if (base.InvocationSettings != null && base.InvocationSettings.ApartmentState != ApartmentState.Unknown)
				{
					apartmentState = base.InvocationSettings.ApartmentState;
				}
				else
				{
					apartmentState = this.LocalRunspace.ApartmentState;
				}
				if (apartmentState != ApartmentState.Unknown)
				{
					thread.SetApartmentState(apartmentState);
				}
				thread.Start();
				return;
			}
			case PSThreadOptions.ReuseThread:
			{
				if (this.IsNested)
				{
					this.SetupInvokeThread(Thread.CurrentThread, true);
					this.InvokeThreadProc();
					return;
				}
				PipelineThread pipelineThread = this.LocalRunspace.GetPipelineThread();
				this.SetupInvokeThread(pipelineThread.Worker, true);
				pipelineThread.Start(new ThreadStart(this.InvokeThreadProc));
				return;
			}
			case PSThreadOptions.UseCurrentThread:
			{
				Thread nestedPipelineExecutionThread = base.NestedPipelineExecutionThread;
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
				try
				{
					this.SetupInvokeThread(Thread.CurrentThread, false);
					this.InvokeThreadProc();
				}
				finally
				{
					base.NestedPipelineExecutionThread = nestedPipelineExecutionThread;
					Thread.CurrentThread.CurrentCulture = currentCulture;
					Thread.CurrentThread.CurrentUICulture = currentUICulture;
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x000992D0 File Offset: 0x000974D0
		private void SetupInvokeThread(Thread invokeThread, bool changeName)
		{
			base.NestedPipelineExecutionThread = invokeThread;
			invokeThread.CurrentCulture = this.LocalRunspace.ExecutionContext.EngineHostInterface.CurrentCulture;
			invokeThread.CurrentUICulture = this.LocalRunspace.ExecutionContext.EngineHostInterface.CurrentUICulture;
			if (invokeThread.Name == null && changeName)
			{
				invokeThread.Name = "Pipeline Execution Thread";
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x00099330 File Offset: 0x00097530
		internal static int MaxStack
		{
			get
			{
				int num = LocalPipeline.ReadRegistryInt("PipelineMaxStackSizeMB", 10);
				if (num < 10)
				{
					num = 10;
				}
				else if (num > 100)
				{
					num = 100;
				}
				return num * 1000000;
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00099364 File Offset: 0x00097564
		private FlowControlException InvokeHelper()
		{
			FlowControlException result = null;
			PipelineProcessor pipelineProcessor = null;
			try
			{
				base.RaisePipelineStateEvents();
				this.RecordPipelineStartTime();
				if (base.AddToHistory || !this.IsNested)
				{
					bool flag = true;
					CommandInfo commandInfo = new CmdletInfo("Out-Default", typeof(OutDefaultCommand), null, null, null);
					foreach (Command command in base.Commands)
					{
						if (command.IsScript && !base.IsPulsePipeline)
						{
							this.Runspace.GetExecutionContext.EngineHostInterface.UI.TranscribeCommand(command.CommandText, null);
						}
						if (string.Equals(commandInfo.Name, command.CommandText, StringComparison.OrdinalIgnoreCase) || string.Equals("PSConsoleHostReadLine", command.CommandText, StringComparison.OrdinalIgnoreCase) || string.Equals("TabExpansion2", command.CommandText, StringComparison.OrdinalIgnoreCase) || base.IsPulsePipeline)
						{
							flag = false;
						}
					}
					if (this.Runspace.GetExecutionContext.EngineHostInterface.UI.IsTranscribing && flag)
					{
						Command command2 = new Command(commandInfo);
						command2.Parameters.Add(new CommandParameter("Transcript", true));
						command2.Parameters.Add(new CommandParameter("OutVariable", null));
						base.Commands.Add(command2);
					}
				}
				try
				{
					pipelineProcessor = this.CreatePipelineProcessor();
				}
				catch (Exception obj)
				{
					if (base.SetPipelineSessionState)
					{
						base.SetHadErrors(true);
						this.Runspace.ExecutionContext.AppendDollarError(obj);
					}
					throw;
				}
				if (this.useExternalInput)
				{
					pipelineProcessor.ExternalInput = base.InputStream.ObjectReader;
				}
				pipelineProcessor.ExternalSuccessOutput = base.OutputStream.ObjectWriter;
				pipelineProcessor.ExternalErrorOutput = base.ErrorStream.ObjectWriter;
				if (!this.IsChild)
				{
					this.LocalRunspace.ExecutionContext.InternalHost.InternalUI.SetInformationalMessageBuffers(base.InformationalBuffers);
				}
				bool questionMarkVariableValue = true;
				bool ignoreScriptDebug = this.LocalRunspace.ExecutionContext.IgnoreScriptDebug;
				bool propagateExceptionsToEnclosingStatementBlock = this.LocalRunspace.ExecutionContext.PropagateExceptionsToEnclosingStatementBlock;
				this.LocalRunspace.ExecutionContext.PropagateExceptionsToEnclosingStatementBlock = false;
				try
				{
					this._stopper.Push(pipelineProcessor);
					if (!base.AddToHistory)
					{
						questionMarkVariableValue = this.LocalRunspace.ExecutionContext.QuestionMarkVariableValue;
						this.LocalRunspace.ExecutionContext.IgnoreScriptDebug = true;
					}
					else
					{
						this.LocalRunspace.ExecutionContext.IgnoreScriptDebug = false;
					}
					if (!this.IsNested && !base.IsPulsePipeline)
					{
						this.LocalRunspace.ExecutionContext.ResetRedirection();
					}
					try
					{
						pipelineProcessor.Execute();
						base.SetHadErrors(pipelineProcessor.ExecutionFailed);
					}
					catch (ExitException ex)
					{
						base.SetHadErrors(pipelineProcessor.ExecutionFailed);
						int num = 1;
						if (this.IsNested)
						{
							try
							{
								num = (int)ex.Argument;
								this.LocalRunspace.ExecutionContext.SetVariable(SpecialVariables.LastExitCodeVarPath, num);
								goto IL_33E;
							}
							finally
							{
								try
								{
									this.LocalRunspace.ExecutionContext.EngineHostInterface.ExitNestedPrompt();
								}
								catch (ExitNestedPromptException)
								{
								}
							}
						}
						try
						{
							num = (int)ex.Argument;
							if (base.InvocationSettings != null && base.InvocationSettings.ExposeFlowControlExceptions)
							{
								result = ex;
							}
						}
						finally
						{
							this.LocalRunspace.ExecutionContext.EngineHostInterface.SetShouldExit(num);
						}
						IL_33E:;
					}
					catch (ExitNestedPromptException)
					{
					}
					catch (FlowControlException ex2)
					{
						if (base.InvocationSettings != null && base.InvocationSettings.ExposeFlowControlExceptions && (ex2 is BreakException || ex2 is ContinueException))
						{
							result = ex2;
						}
					}
					catch (Exception)
					{
						base.SetHadErrors(true);
						throw;
					}
				}
				finally
				{
					if (pipelineProcessor != null && pipelineProcessor.Commands != null)
					{
						for (int i = 0; i < pipelineProcessor.Commands.Count; i++)
						{
							CommandProcessorBase commandProcessorBase = pipelineProcessor.Commands[i];
							EtwActivity.SetActivityId(commandProcessorBase.PipelineActivityId);
							MshLog.LogCommandLifecycleEvent(commandProcessorBase.Context, CommandState.Terminated, commandProcessorBase.Command.MyInvocation);
						}
					}
					PSLocalEventManager pslocalEventManager = this.LocalRunspace.Events as PSLocalEventManager;
					if (pslocalEventManager != null)
					{
						pslocalEventManager.ProcessPendingActions();
					}
					this.LocalRunspace.ExecutionContext.PropagateExceptionsToEnclosingStatementBlock = propagateExceptionsToEnclosingStatementBlock;
					if (!this.IsChild)
					{
						this.LocalRunspace.ExecutionContext.InternalHost.InternalUI.SetInformationalMessageBuffers(null);
					}
					this._stopper.Pop(false);
					if (!base.AddToHistory)
					{
						this.LocalRunspace.ExecutionContext.QuestionMarkVariableValue = questionMarkVariableValue;
					}
					this.LocalRunspace.ExecutionContext.IgnoreScriptDebug = ignoreScriptDebug;
				}
			}
			catch (FlowControlException)
			{
			}
			finally
			{
				if (pipelineProcessor != null)
				{
					pipelineProcessor.Dispose();
					pipelineProcessor = null;
				}
			}
			return result;
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00099910 File Offset: 0x00097B10
		internal static int ReadRegistryInt(string policyValueName, int defaultValue)
		{
			RegistryKey registryKey;
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey(Utils.GetRegistryConfigurationPrefix());
			}
			catch (SecurityException)
			{
				return defaultValue;
			}
			if (registryKey == null)
			{
				return defaultValue;
			}
			object value;
			try
			{
				value = registryKey.GetValue(policyValueName);
			}
			catch (SecurityException)
			{
				return defaultValue;
			}
			if (!(value is int))
			{
				return defaultValue;
			}
			return (int)value;
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00099978 File Offset: 0x00097B78
		private void InvokeThreadProc()
		{
			bool flag = false;
			Runspace defaultRunspace = Runspace.DefaultRunspace;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = null;
				try
				{
					if (base.InvocationSettings != null && base.InvocationSettings.FlowImpersonationPolicy)
					{
						WindowsIdentity windowsIdentity = new WindowsIdentity(base.InvocationSettings.WindowsIdentityToImpersonate.Token);
						windowsImpersonationContext = windowsIdentity.Impersonate();
					}
					if (base.InvocationSettings != null && base.InvocationSettings.Host != null)
					{
						InternalHost internalHost = base.InvocationSettings.Host as InternalHost;
						if (internalHost != null)
						{
							this.LocalRunspace.ExecutionContext.InternalHost.SetHostRef(internalHost.ExternalHost);
						}
						else
						{
							this.LocalRunspace.ExecutionContext.InternalHost.SetHostRef(base.InvocationSettings.Host);
						}
					}
					if (this.LocalRunspace.ExecutionContext.InternalHost.ExternalHost.ShouldSetThreadUILanguageToZero)
					{
						NativeCultureResolver.SetThreadUILanguage(0);
					}
					Runspace.DefaultRunspace = this.LocalRunspace;
					FlowControlException ex = this.InvokeHelper();
					if (ex != null)
					{
						base.SetPipelineState(PipelineState.Failed, ex);
					}
					else
					{
						base.SetPipelineState(PipelineState.Completed);
					}
				}
				finally
				{
					if (windowsImpersonationContext != null)
					{
						try
						{
							windowsImpersonationContext.Undo();
							windowsImpersonationContext.Dispose();
							windowsImpersonationContext = null;
						}
						catch (SecurityException)
						{
						}
					}
				}
			}
			catch (PipelineStoppedException reason)
			{
				base.SetPipelineState(PipelineState.Stopped, reason);
			}
			catch (RuntimeException ex2)
			{
				flag = (ex2 is IncompleteParseException);
				base.SetPipelineState(PipelineState.Failed, ex2);
				base.SetHadErrors(true);
			}
			catch (ScriptCallDepthException reason2)
			{
				base.SetPipelineState(PipelineState.Failed, reason2);
				base.SetHadErrors(true);
			}
			catch (SecurityException reason3)
			{
				base.SetPipelineState(PipelineState.Failed, reason3);
				base.SetHadErrors(true);
			}
			catch (ThreadAbortException reason4)
			{
				base.SetPipelineState(PipelineState.Failed, reason4);
				base.SetHadErrors(true);
			}
			catch (HaltCommandException)
			{
				base.SetPipelineState(PipelineState.Completed);
			}
			finally
			{
				if (base.InvocationSettings != null && base.InvocationSettings.Host != null && this.LocalRunspace.ExecutionContext.InternalHost.IsHostRefSet)
				{
					this.LocalRunspace.ExecutionContext.InternalHost.RevertHostRef();
				}
				Runspace.DefaultRunspace = defaultRunspace;
				if (!flag)
				{
					try
					{
						bool inBreakpoint = this.LocalRunspace.ExecutionContext.Debugger.InBreakpoint;
						if (this._historyIdForThisPipeline == -1L)
						{
							this.AddHistoryEntry(inBreakpoint);
						}
						else
						{
							this.UpdateHistoryEntryAddedByAddHistoryCmdlet(inBreakpoint);
						}
					}
					catch (TerminateException)
					{
					}
				}
				if (base.OutputStream.IsOpen && !this.IsChild)
				{
					try
					{
						base.OutputStream.Close();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				if (base.ErrorStream.IsOpen && !this.IsChild)
				{
					try
					{
						base.ErrorStream.Close();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				if (base.InputStream.IsOpen && !this.IsChild)
				{
					try
					{
						base.InputStream.Close();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				this.ClearStreams();
				this.LocalRunspace.RemoveFromRunningPipelineList(this);
				if (!base.SyncInvokeCall)
				{
					base.RaisePipelineStateEvents();
				}
			}
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00099D58 File Offset: 0x00097F58
		protected override void ImplementStop(bool syncCall)
		{
			if (syncCall)
			{
				this.StopHelper();
				return;
			}
			Thread thread = new Thread(new ThreadStart(this.StopThreadProc));
			thread.Start();
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00099D87 File Offset: 0x00097F87
		private void StopThreadProc()
		{
			this.StopHelper();
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x00099D8F File Offset: 0x00097F8F
		internal PipelineStopper Stopper
		{
			get
			{
				return this._stopper;
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00099D98 File Offset: 0x00097F98
		private void StopHelper()
		{
			this.LocalRunspace.ReleaseDebugger();
			this.LocalRunspace.StopNestedPipelines(this);
			if (base.InputStream.IsOpen)
			{
				try
				{
					base.InputStream.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
			this._stopper.Stop();
			base.PipelineFinishedEvent.WaitOne();
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x00099E00 File Offset: 0x00098000
		internal bool IsStopping
		{
			get
			{
				return this._stopper.IsStopping;
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00099E10 File Offset: 0x00098010
		private PipelineProcessor CreatePipelineProcessor()
		{
			CommandCollection commands = base.Commands;
			if (commands == null || commands.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RunspaceStrings.NoCommandInPipeline, new object[0]);
			}
			PipelineProcessor pipelineProcessor = new PipelineProcessor();
			pipelineProcessor.TopLevel = true;
			bool flag = false;
			PipelineProcessor result;
			try
			{
				foreach (Command command in commands)
				{
					CommandProcessorBase commandProcessorBase;
					if (command.CommandInfo == null)
					{
						try
						{
							CommandOrigin origin = command.CommandOrigin;
							if (this.IsNested)
							{
								origin = CommandOrigin.Internal;
							}
							commandProcessorBase = command.CreateCommandProcessor(this.LocalRunspace.ExecutionContext, this.LocalRunspace.CommandFactory, base.AddToHistory, origin);
							goto IL_174;
						}
						catch
						{
							if (this.Runspace.GetExecutionContext.EngineHostInterface.UI.IsTranscribing && !command.IsScript)
							{
								this.Runspace.ExecutionContext.InternalHost.UI.TranscribeCommand(command.CommandText, null);
							}
							throw;
						}
						goto IL_D8;
					}
					goto IL_D8;
					IL_174:
					commandProcessorBase.RedirectShellErrorOutputPipe = base.RedirectShellErrorOutputPipe;
					pipelineProcessor.Add(commandProcessorBase);
					continue;
					IL_D8:
					CmdletInfo cmdletInfo = (CmdletInfo)command.CommandInfo;
					commandProcessorBase = new CommandProcessor(cmdletInfo, this.LocalRunspace.ExecutionContext);
					PSSQMAPI.IncrementData(cmdletInfo.CommandType);
					commandProcessorBase.Command.CommandOriginInternal = CommandOrigin.Internal;
					commandProcessorBase.Command.MyInvocation.InvocationName = cmdletInfo.Name;
					if (command.Parameters != null)
					{
						foreach (CommandParameter publicParameter in command.Parameters)
						{
							CommandParameterInternal parameter = CommandParameter.ToCommandParameterInternal(publicParameter, false);
							commandProcessorBase.AddParameter(parameter);
						}
						goto IL_174;
					}
					goto IL_174;
				}
				result = pipelineProcessor;
			}
			catch (RuntimeException)
			{
				flag = true;
				throw;
			}
			catch (Exception ex)
			{
				flag = true;
				CommandProcessorBase.CheckForSevereException(ex);
				throw new RuntimeException(PipelineStrings.CannotCreatePipeline, ex);
			}
			finally
			{
				if (flag)
				{
					base.SetHadErrors(true);
					pipelineProcessor.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0009A08C File Offset: 0x0009828C
		private void InitStreams()
		{
			if (this.LocalRunspace.ExecutionContext != null)
			{
				this.oldExternalErrorOutput = this.LocalRunspace.ExecutionContext.ExternalErrorOutput;
				this.oldExternalSuccessOutput = this.LocalRunspace.ExecutionContext.ExternalSuccessOutput;
				this.LocalRunspace.ExecutionContext.ExternalErrorOutput = base.ErrorStream.ObjectWriter;
				this.LocalRunspace.ExecutionContext.ExternalSuccessOutput = base.OutputStream.ObjectWriter;
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0009A108 File Offset: 0x00098308
		private void ClearStreams()
		{
			if (this.LocalRunspace.ExecutionContext != null)
			{
				this.LocalRunspace.ExecutionContext.ExternalErrorOutput = this.oldExternalErrorOutput;
				this.LocalRunspace.ExecutionContext.ExternalSuccessOutput = this.oldExternalSuccessOutput;
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0009A143 File Offset: 0x00098343
		private void RecordPipelineStartTime()
		{
			this._pipelineStartTime = DateTime.Now;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0009A150 File Offset: 0x00098350
		private void AddHistoryEntry(bool skipIfLocked)
		{
			if (base.AddToHistory)
			{
				this.LocalRunspace.History.AddEntry(base.InstanceId, base.HistoryString, base.PipelineState, this._pipelineStartTime, DateTime.Now, skipIfLocked);
			}
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0009A18C File Offset: 0x0009838C
		internal void AddHistoryEntryFromAddHistoryCmdlet()
		{
			if (this._historyIdForThisPipeline != -1L)
			{
				return;
			}
			if (base.AddToHistory)
			{
				this._historyIdForThisPipeline = this.LocalRunspace.History.AddEntry(base.InstanceId, base.HistoryString, base.PipelineState, this._pipelineStartTime, DateTime.Now, false);
			}
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0009A1E0 File Offset: 0x000983E0
		internal void UpdateHistoryEntryAddedByAddHistoryCmdlet(bool skipIfLocked)
		{
			if (base.AddToHistory && this._historyIdForThisPipeline != -1L)
			{
				this.LocalRunspace.History.UpdateEntry(this._historyIdForThisPipeline, base.PipelineState, DateTime.Now, skipIfLocked);
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0009A216 File Offset: 0x00098416
		internal override void SetHistoryString(string historyString)
		{
			base.HistoryString = historyString;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0009A220 File Offset: 0x00098420
		internal static ExecutionContext GetExecutionContextFromTLS()
		{
			Runspace defaultRunspace = Runspace.DefaultRunspace;
			if (defaultRunspace == null)
			{
				return null;
			}
			return defaultRunspace.ExecutionContext;
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x0009A23E File Offset: 0x0009843E
		private LocalRunspace LocalRunspace
		{
			get
			{
				return (LocalRunspace)this.Runspace;
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0009A24B File Offset: 0x0009844B
		internal bool PresentInInvokeHistoryEntryList(HistoryInfo entry)
		{
			return this._invokeHistoryIds.Contains(entry.Id);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0009A25E File Offset: 0x0009845E
		internal void AddToInvokeHistoryEntryList(HistoryInfo entry)
		{
			this._invokeHistoryIds.Add(entry.Id);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0009A271 File Offset: 0x00098471
		internal void RemoveFromInvokeHistoryEntryList(HistoryInfo entry)
		{
			this._invokeHistoryIds.Remove(entry.Id);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0009A288 File Offset: 0x00098488
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					this._disposed = true;
					if (disposing)
					{
						this.Stop();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x04000A74 RID: 2676
		private PipelineStopper _stopper;

		// Token: 0x04000A75 RID: 2677
		private DateTime _pipelineStartTime;

		// Token: 0x04000A76 RID: 2678
		private long _historyIdForThisPipeline = -1L;

		// Token: 0x04000A77 RID: 2679
		private bool useExternalInput;

		// Token: 0x04000A78 RID: 2680
		private PipelineWriter oldExternalErrorOutput;

		// Token: 0x04000A79 RID: 2681
		private PipelineWriter oldExternalSuccessOutput;

		// Token: 0x04000A7A RID: 2682
		private List<long> _invokeHistoryIds = new List<long>();

		// Token: 0x04000A7B RID: 2683
		private bool _disposed;
	}
}
