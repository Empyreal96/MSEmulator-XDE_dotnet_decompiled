using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000267 RID: 615
	internal sealed class RemoteDebugger : Debugger, IDisposable
	{
		// Token: 0x06001D02 RID: 7426 RVA: 0x000A8613 File Offset: 0x000A6813
		private RemoteDebugger()
		{
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000A861C File Offset: 0x000A681C
		public RemoteDebugger(RemoteRunspace runspace)
		{
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			this._runspace = runspace;
			this._unhandledBreakpointMode = UnhandledBreakpointProcessingMode.Ignore;
			this._runspace.RemoteDebuggerStop += this.HandleForwardedDebuggerStopEvent;
			this._runspace.RemoteDebuggerBreakpointUpdated += this.HandleForwardedDebuggerBreakpointUpdatedEvent;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000A8760 File Offset: 0x000A6960
		public override DebuggerCommandResults ProcessCommand(PSCommand command, PSDataCollection<PSObject> output)
		{
			this.CheckForValidateState();
			this._detachCommand = false;
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
			DebuggerCommandResults results = null;
			bool flag = false;
			using (this._psDebuggerCommand = this.GetNestedPowerShell())
			{
				foreach (Command command2 in command.Commands)
				{
					command2.MergeMyResults(PipelineResultTypes.All, PipelineResultTypes.Output);
					this._psDebuggerCommand.AddCommand(command2);
				}
				PSDataCollection<PSObject> internalOutput = new PSDataCollection<PSObject>();
				internalOutput.DataAdded += delegate(object sender, DataAddedEventArgs args)
				{
					foreach (PSObject psobject in internalOutput.ReadAll())
					{
						if (psobject == null)
						{
							break;
						}
						DebuggerCommand debuggerCommand = psobject.BaseObject as DebuggerCommand;
						if (debuggerCommand != null)
						{
							bool evaluatedByDebugger = debuggerCommand.ResumeAction != null || debuggerCommand.ExecutedByDebugger;
							results = new DebuggerCommandResults(debuggerCommand.ResumeAction, evaluatedByDebugger);
						}
						else if (psobject.BaseObject is DebuggerCommandResults)
						{
							results = (psobject.BaseObject as DebuggerCommandResults);
						}
						else
						{
							output.Add(psobject);
						}
					}
				};
				try
				{
					this._psDebuggerCommand.Invoke<PSObject>(null, internalOutput, null);
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					flag = true;
					RemoteException ex2 = ex as RemoteException;
					if (ex2 != null && ex2.ErrorRecord != null)
					{
						if (ex2.ErrorRecord.CategoryInfo.Reason == typeof(IncompleteParseException).Name)
						{
							throw new IncompleteParseException((ex2.ErrorRecord.Exception != null) ? ex2.ErrorRecord.Exception.Message : null, ex2.ErrorRecord.FullyQualifiedErrorId);
						}
						if (ex2.ErrorRecord.CategoryInfo.Reason == typeof(InvalidRunspacePoolStateException).Name || ex2.ErrorRecord.CategoryInfo.Reason == typeof(RemoteException).Name)
						{
							throw new PSRemotingTransportException((ex2.ErrorRecord.Exception != null) ? ex2.ErrorRecord.Exception.Message : string.Empty);
						}
					}
					if (ex is PSRemotingTransportException || ex is RemoteException)
					{
						throw;
					}
					output.Add(new PSObject(new ErrorRecord(ex, "DebuggerError", ErrorCategory.InvalidOperation, null)));
				}
			}
			flag = (flag || this._psDebuggerCommand.HadErrors);
			this._psDebuggerCommand = null;
			this._detachCommand = (!flag && command.Commands.Count > 0 && command.Commands[0].CommandText.Equals("Detach", StringComparison.OrdinalIgnoreCase));
			DebuggerCommandResults result;
			if ((result = results) == null)
			{
				result = new DebuggerCommandResults(null, false);
			}
			return result;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000A8A58 File Offset: 0x000A6C58
		public override void StopProcessCommand()
		{
			this.CheckForValidateState();
			PowerShell psDebuggerCommand = this._psDebuggerCommand;
			if (psDebuggerCommand != null && psDebuggerCommand.InvocationStateInfo.State == PSInvocationState.Running)
			{
				psDebuggerCommand.BeginStop(null, null);
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000A8A8C File Offset: 0x000A6C8C
		public override void SetDebuggerAction(DebuggerResumeAction resumeAction)
		{
			this.CheckForValidateState();
			this.SetRemoteDebug(false, RunspaceAvailability.Busy);
			using (PowerShell nestedPowerShell = this.GetNestedPowerShell())
			{
				nestedPowerShell.AddCommand("__Set-PSDebuggerAction").AddParameter("ResumeAction", resumeAction);
				nestedPowerShell.Invoke();
				if (nestedPowerShell.ErrorBuffer.Count > 0)
				{
					Exception exception = nestedPowerShell.ErrorBuffer[0].Exception;
					if (exception != null)
					{
						throw exception;
					}
				}
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000A8B14 File Offset: 0x000A6D14
		public override DebuggerStopEventArgs GetDebuggerStopArgs()
		{
			this.CheckForValidateState();
			DebuggerStopEventArgs debuggerStopEventArgs = null;
			try
			{
				using (PowerShell nestedPowerShell = this.GetNestedPowerShell())
				{
					nestedPowerShell.AddCommand("__Get-PSDebuggerStopArgs");
					Collection<PSObject> collection = nestedPowerShell.Invoke<PSObject>();
					foreach (PSObject psobject in collection)
					{
						if (psobject != null)
						{
							debuggerStopEventArgs = (psobject.BaseObject as DebuggerStopEventArgs);
							if (debuggerStopEventArgs != null)
							{
								break;
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			return debuggerStopEventArgs;
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000A8BC0 File Offset: 0x000A6DC0
		public override void SetDebugMode(DebugModes mode)
		{
			this.CheckForValidateState();
			if (this._runspace.GetCurrentlyRunningPipeline() != null || this._runspace.RemoteCommand != null)
			{
				return;
			}
			using (PowerShell nestedPowerShell = this.GetNestedPowerShell())
			{
				nestedPowerShell.SetIsNested(false);
				nestedPowerShell.AddCommand("__Set-PSDebugMode").AddParameter("Mode", mode);
				nestedPowerShell.Invoke();
			}
			base.SetDebugMode(mode);
			this.SetIsActive(this._breakpointCount);
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000A8C50 File Offset: 0x000A6E50
		public override void SetDebuggerStepMode(bool enabled)
		{
			this.CheckForValidateState();
			if (this._serverPSVersion == null || this._serverPSVersion.Major < PSVersionInfo.PSV5Version.Major)
			{
				return;
			}
			try
			{
				base.SetDebugMode(DebugModes.LocalScript | DebugModes.RemoteScript);
				using (PowerShell nestedPowerShell = this.GetNestedPowerShell())
				{
					nestedPowerShell.AddCommand("__Set-PSDebuggerStepMode").AddParameter("Enabled", enabled);
					nestedPowerShell.Invoke();
					this._isDebuggerSteppingEnabled = enabled;
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000A8CF4 File Offset: 0x000A6EF4
		public override bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000A8CFC File Offset: 0x000A6EFC
		public override bool InBreakpoint
		{
			get
			{
				return this._handleDebuggerStop || this._runspace.RunspaceAvailability == RunspaceAvailability.RemoteDebug;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000A8D18 File Offset: 0x000A6F18
		internal override DebuggerCommand InternalProcessCommand(string command, IList<PSObject> output)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x000A8D1F File Offset: 0x000A6F1F
		internal override bool IsRemote
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x000A8D22 File Offset: 0x000A6F22
		// (set) Token: 0x06001D0F RID: 7439 RVA: 0x000A8D2C File Offset: 0x000A6F2C
		internal override UnhandledBreakpointProcessingMode UnhandledBreakpointMode
		{
			get
			{
				return this._unhandledBreakpointMode;
			}
			set
			{
				this.CheckForValidateState();
				if (this._serverPSVersion == null || this._serverPSVersion < PSVersionInfo.PSV5Version)
				{
					return;
				}
				this.SetRemoteDebug(false, RunspaceAvailability.Busy);
				using (PowerShell nestedPowerShell = this.GetNestedPowerShell())
				{
					nestedPowerShell.AddCommand("__Set-PSUnhandledBreakpointMode").AddParameter("UnhandledBreakpointMode", value);
					nestedPowerShell.Invoke();
				}
				this._unhandledBreakpointMode = value;
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000A8DB8 File Offset: 0x000A6FB8
		public void Dispose()
		{
			this._runspace.RemoteDebuggerStop -= this.HandleForwardedDebuggerStopEvent;
			this._runspace.RemoteDebuggerBreakpointUpdated -= this.HandleForwardedDebuggerBreakpointUpdatedEvent;
			if (this._identityToPersonate != null)
			{
				this._identityToPersonate.Dispose();
				this._identityToPersonate = null;
			}
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000A8E10 File Offset: 0x000A7010
		internal void CheckStateAndRaiseStopEvent()
		{
			DebuggerStopEventArgs debuggerStopArgs = this.GetDebuggerStopArgs();
			if (debuggerStopArgs != null)
			{
				this.ProcessDebuggerStopEvent(debuggerStopArgs);
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x000A8E37 File Offset: 0x000A7037
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x000A8E2E File Offset: 0x000A702E
		internal bool IsRemoteDebug { get; private set; }

		// Token: 0x06001D14 RID: 7444 RVA: 0x000A8E40 File Offset: 0x000A7040
		internal void SetClientDebugInfo(DebugModes? debugMode, bool inBreakpoint, int breakpointCount, bool breakAll, UnhandledBreakpointProcessingMode unhandledBreakpointMode, Version serverPSVersion)
		{
			if (debugMode != null)
			{
				this._remoteDebugSupported = true;
				base.DebugMode = debugMode.Value;
			}
			else
			{
				this._remoteDebugSupported = false;
			}
			if (inBreakpoint)
			{
				this.SetRemoteDebug(true, RunspaceAvailability.RemoteDebug);
			}
			this._serverPSVersion = serverPSVersion;
			this._breakpointCount = breakpointCount;
			this._isDebuggerSteppingEnabled = breakAll;
			this._unhandledBreakpointMode = unhandledBreakpointMode;
			this.SetIsActive(breakpointCount);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000A8EA4 File Offset: 0x000A70A4
		internal void OnCommandStopped()
		{
			if (this.IsRemoteDebug)
			{
				this.IsRemoteDebug = false;
			}
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000A8EB8 File Offset: 0x000A70B8
		internal void SendBreakpointUpdatedEvents()
		{
			if (!base.IsDebuggerBreakpointUpdatedEventSubscribed() || this._breakpointCount == 0)
			{
				return;
			}
			PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
			using (PowerShell nestedPowerShell = this.GetNestedPowerShell())
			{
				if (!this.InBreakpoint)
				{
					nestedPowerShell.SetIsNested(false);
				}
				nestedPowerShell.AddCommand("Get-PSBreakpoint");
				nestedPowerShell.Invoke<PSObject>(null, psdataCollection);
			}
			foreach (PSObject psobject in psdataCollection)
			{
				Breakpoint breakpoint = psobject.BaseObject as Breakpoint;
				if (breakpoint != null)
				{
					base.RaiseBreakpointUpdatedEvent(new BreakpointUpdatedEventArgs(breakpoint, BreakpointUpdateType.Set, this._breakpointCount));
				}
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000A8F7C File Offset: 0x000A717C
		internal override bool IsDebuggerSteppingEnabled
		{
			get
			{
				return this._isDebuggerSteppingEnabled;
			}
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000A8F84 File Offset: 0x000A7184
		private void HandleForwardedDebuggerStopEvent(object sender, PSEventArgs e)
		{
			DebuggerStopEventArgs args;
			if (e.SourceArgs[0] is PSObject)
			{
				args = (((PSObject)e.SourceArgs[0]).BaseObject as DebuggerStopEventArgs);
			}
			else
			{
				args = (e.SourceArgs[0] as DebuggerStopEventArgs);
			}
			this.ProcessDebuggerStopEvent(args);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000A8FD0 File Offset: 0x000A71D0
		private void ProcessDebuggerStopEvent(DebuggerStopEventArgs args)
		{
			if (this._handleDebuggerStop)
			{
				return;
			}
			PowerShell currentRunningPowerShell = this._runspace.RunspacePool.RemoteRunspacePoolInternal.GetCurrentRunningPowerShell();
			AsyncResult asyncResult = (currentRunningPowerShell != null) ? currentRunningPowerShell.EndInvokeAsyncResult : null;
			bool flag = false;
			if (asyncResult != null && !asyncResult.IsCompleted)
			{
				flag = asyncResult.InvokeCallbackOnThread(new WaitCallback(this.ProcessDebuggerStopEventProc), args);
			}
			if (!flag)
			{
				Utils.QueueWorkItemWithImpersonation(this._identityToPersonate, new WaitCallback(this.ProcessDebuggerStopEventProc), args);
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000A9048 File Offset: 0x000A7248
		private void ProcessDebuggerStopEventProc(object state)
		{
			RunspaceAvailability runspaceAvailability = this._runspace.RunspaceAvailability;
			bool flag = true;
			try
			{
				this._handleDebuggerStop = true;
				this.SetRemoteDebug(true, RunspaceAvailability.RemoteDebug);
				DebuggerStopEventArgs debuggerStopEventArgs = state as DebuggerStopEventArgs;
				if (debuggerStopEventArgs != null)
				{
					if (base.IsDebuggerStopEventSubscribed())
					{
						try
						{
							base.RaiseDebuggerStopEvent(debuggerStopEventArgs);
							goto IL_75;
						}
						finally
						{
							this._handleDebuggerStop = false;
							if (!this._detachCommand)
							{
								this.SetDebuggerAction(debuggerStopEventArgs.ResumeAction);
							}
						}
					}
					flag = false;
					this._handleDebuggerStop = false;
				}
				else
				{
					this._handleDebuggerStop = false;
					this.SetDebuggerAction(DebuggerResumeAction.Continue);
				}
				IL_75:;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				this._handleDebuggerStop = false;
			}
			finally
			{
				if (flag)
				{
					this.SetRemoteDebug(false, runspaceAvailability);
				}
				if (this._detachCommand)
				{
					this._detachCommand = false;
				}
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000A9124 File Offset: 0x000A7324
		private void HandleForwardedDebuggerBreakpointUpdatedEvent(object sender, PSEventArgs e)
		{
			BreakpointUpdatedEventArgs breakpointUpdatedEventArgs = e.SourceArgs[0] as BreakpointUpdatedEventArgs;
			if (breakpointUpdatedEventArgs != null)
			{
				this.UpdateBreakpointCount(breakpointUpdatedEventArgs.BreakpointCount);
				base.RaiseBreakpointUpdatedEvent(breakpointUpdatedEventArgs);
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000A9158 File Offset: 0x000A7358
		private PowerShell GetNestedPowerShell()
		{
			PowerShell powerShell = PowerShell.Create();
			powerShell.Runspace = this._runspace;
			powerShell.SetIsNested(true);
			return powerShell;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000A9180 File Offset: 0x000A7380
		private void CheckForValidateState()
		{
			if (!this._remoteDebugSupported)
			{
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.RemoteDebuggingEndpointVersionError, PSVersionInfo.PSV4Version), null, "RemoteDebugger:RemoteDebuggingNotSupported", ErrorCategory.NotImplemented, null);
			}
			if (this._runspace.RunspaceStateInfo.State != RunspaceState.Opened)
			{
				throw new InvalidRunspaceStateException();
			}
			if (!this._identityPersonationChecked)
			{
				this._identityPersonationChecked = true;
				WindowsIdentity windowsIdentity = null;
				try
				{
					windowsIdentity = WindowsIdentity.GetCurrent();
				}
				catch (SecurityException)
				{
				}
				this._identityToPersonate = ((windowsIdentity != null && windowsIdentity.ImpersonationLevel == TokenImpersonationLevel.Impersonation) ? windowsIdentity : null);
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000A9210 File Offset: 0x000A7410
		private void SetRemoteDebug(bool remoteDebug, RunspaceAvailability availability)
		{
			if (this._runspace.RunspaceStateInfo.State != RunspaceState.Opened || this.IsRemoteDebug == remoteDebug)
			{
				return;
			}
			this.IsRemoteDebug = remoteDebug;
			this._runspace.RunspacePool.RemoteRunspacePoolInternal.IsRemoteDebugStop = remoteDebug;
			try
			{
				this._runspace.UpdateRunspaceAvailability(availability, true);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000A9280 File Offset: 0x000A7480
		private void UpdateBreakpointCount(int bpCount)
		{
			this._breakpointCount = bpCount;
			this.SetIsActive(bpCount);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000A9290 File Offset: 0x000A7490
		private void SetIsActive(int breakpointCount)
		{
			if ((base.DebugMode & DebugModes.RemoteScript) == DebugModes.None)
			{
				if (this._isActive)
				{
					this._isActive = false;
				}
				return;
			}
			if (breakpointCount > 0)
			{
				if (!this._isActive)
				{
					this._isActive = true;
					return;
				}
			}
			else if (this._isActive)
			{
				this._isActive = false;
			}
		}

		// Token: 0x04000CDA RID: 3290
		public const string RemoteDebuggerStopEvent = "PSInternalRemoteDebuggerStopEvent";

		// Token: 0x04000CDB RID: 3291
		public const string RemoteDebuggerBreakpointUpdatedEvent = "PSInternalRemoteDebuggerBreakpointUpdatedEvent";

		// Token: 0x04000CDC RID: 3292
		public const string DebugModeSetting = "DebugMode";

		// Token: 0x04000CDD RID: 3293
		public const string DebugStopState = "DebugStop";

		// Token: 0x04000CDE RID: 3294
		public const string DebugBreakpointCount = "DebugBreakpointCount";

		// Token: 0x04000CDF RID: 3295
		public const string BreakAllSetting = "BreakAll";

		// Token: 0x04000CE0 RID: 3296
		public const string UnhandledBreakpointModeSetting = "UnhandledBreakpointMode";

		// Token: 0x04000CE1 RID: 3297
		private RemoteRunspace _runspace;

		// Token: 0x04000CE2 RID: 3298
		private PowerShell _psDebuggerCommand;

		// Token: 0x04000CE3 RID: 3299
		private bool _remoteDebugSupported;

		// Token: 0x04000CE4 RID: 3300
		private bool _isActive;

		// Token: 0x04000CE5 RID: 3301
		private int _breakpointCount;

		// Token: 0x04000CE6 RID: 3302
		private Version _serverPSVersion;

		// Token: 0x04000CE7 RID: 3303
		private volatile bool _handleDebuggerStop;

		// Token: 0x04000CE8 RID: 3304
		private bool _isDebuggerSteppingEnabled;

		// Token: 0x04000CE9 RID: 3305
		private UnhandledBreakpointProcessingMode _unhandledBreakpointMode;

		// Token: 0x04000CEA RID: 3306
		private bool _detachCommand;

		// Token: 0x04000CEB RID: 3307
		private WindowsIdentity _identityToPersonate;

		// Token: 0x04000CEC RID: 3308
		private bool _identityPersonationChecked;
	}
}
