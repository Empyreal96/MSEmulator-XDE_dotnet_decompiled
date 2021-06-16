using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000312 RID: 786
	internal class ServerPowerShellDriver
	{
		// Token: 0x06002547 RID: 9543 RVA: 0x000D07DC File Offset: 0x000CE9DC
		internal ServerPowerShellDriver(PowerShell powershell, PowerShell extraPowerShell, bool noInput, Guid clientPowerShellId, Guid clientRunspacePoolId, ServerRunspacePoolDriver runspacePoolDriver, ApartmentState apartmentState, HostInfo hostInfo, RemoteStreamOptions streamOptions, bool addToHistory, Runspace rsToUse) : this(powershell, extraPowerShell, noInput, clientPowerShellId, clientRunspacePoolId, runspacePoolDriver, apartmentState, hostInfo, streamOptions, addToHistory, rsToUse, null)
		{
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000D087C File Offset: 0x000CEA7C
		internal ServerPowerShellDriver(PowerShell powershell, PowerShell extraPowerShell, bool noInput, Guid clientPowerShellId, Guid clientRunspacePoolId, ServerRunspacePoolDriver runspacePoolDriver, ApartmentState apartmentState, HostInfo hostInfo, RemoteStreamOptions streamOptions, bool addToHistory, Runspace rsToUse, PSDataCollection<PSObject> output)
		{
			ServerPowerShellDriver <>4__this = this;
			this.clientPowerShellId = clientPowerShellId;
			this.clientRunspacePoolId = clientRunspacePoolId;
			this.remoteStreamOptions = streamOptions;
			this.apartmentState = apartmentState;
			this.localPowerShell = powershell;
			this.extraPowerShell = extraPowerShell;
			this.localPowerShellOutput = new PSDataCollection<PSObject>();
			this.noInput = noInput;
			this.addToHistory = addToHistory;
			this.psDriverInvoker = runspacePoolDriver;
			this.dsHandler = runspacePoolDriver.DataStructureHandler.CreatePowerShellDataStructureHandler(clientPowerShellId, clientRunspacePoolId, this.remoteStreamOptions, this.localPowerShell);
			this.remoteHost = this.dsHandler.GetHostAssociatedWithPowerShell(hostInfo, runspacePoolDriver.ServerRemoteHost);
			if (!noInput)
			{
				this.input = new PSDataCollection<object>();
				this.input.ReleaseOnEnumeration = true;
				this.input.IdleEvent += this.HandleIdleEvent;
			}
			this.RegisterPipelineOutputEventHandlers(this.localPowerShellOutput);
			if (this.localPowerShell != null)
			{
				this.RegisterPowerShellEventHandlers(this.localPowerShell);
				this.datasent[0] = false;
			}
			if (extraPowerShell != null)
			{
				this.RegisterPowerShellEventHandlers(extraPowerShell);
				this.datasent[1] = false;
			}
			this.RegisterDataStructureHandlerEventHandlers(this.dsHandler);
			if (rsToUse != null)
			{
				this.localPowerShell.Runspace = rsToUse;
				if (extraPowerShell != null)
				{
					extraPowerShell.Runspace = rsToUse;
				}
			}
			else
			{
				this.localPowerShell.RunspacePool = runspacePoolDriver.RunspacePool;
				if (extraPowerShell != null)
				{
					extraPowerShell.RunspacePool = runspacePoolDriver.RunspacePool;
				}
			}
			if (output != null)
			{
				output.DataAdded += delegate(object sender, DataAddedEventArgs args)
				{
					if (<>4__this.localPowerShellOutput.IsOpen)
					{
						Collection<PSObject> collection = output.ReadAll();
						foreach (PSObject item in collection)
						{
							<>4__this.localPowerShellOutput.Add(item);
						}
					}
				};
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x000D0A26 File Offset: 0x000CEC26
		internal PSDataCollection<object> InputCollection
		{
			get
			{
				return this.input;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x000D0A2E File Offset: 0x000CEC2E
		internal PowerShell LocalPowerShell
		{
			get
			{
				return this.localPowerShell;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x000D0A36 File Offset: 0x000CEC36
		internal Guid InstanceId
		{
			get
			{
				return this.clientPowerShellId;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000D0A3E File Offset: 0x000CEC3E
		internal RemoteStreamOptions RemoteStreamOptions
		{
			get
			{
				return this.remoteStreamOptions;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x000D0A46 File Offset: 0x000CEC46
		internal Guid RunspacePoolId
		{
			get
			{
				return this.clientRunspacePoolId;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x000D0A4E File Offset: 0x000CEC4E
		internal ServerPowerShellDataStructureHandler DataStructureHandler
		{
			get
			{
				return this.dsHandler;
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000D0A58 File Offset: 0x000CEC58
		private PSInvocationSettings PrepInvoke(bool startMainPowerShell)
		{
			if (startMainPowerShell)
			{
				this.dsHandler.Prepare();
			}
			PSInvocationSettings psinvocationSettings = new PSInvocationSettings();
			psinvocationSettings.ApartmentState = this.apartmentState;
			psinvocationSettings.Host = this.remoteHost;
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			switch (current.ImpersonationLevel)
			{
			case TokenImpersonationLevel.Impersonation:
			case TokenImpersonationLevel.Delegation:
				psinvocationSettings.FlowImpersonationPolicy = true;
				break;
			default:
				psinvocationSettings.FlowImpersonationPolicy = false;
				break;
			}
			psinvocationSettings.AddToHistory = this.addToHistory;
			return psinvocationSettings;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000D0AD0 File Offset: 0x000CECD0
		private IAsyncResult Start(bool startMainPowerShell)
		{
			PSInvocationSettings settings = this.PrepInvoke(startMainPowerShell);
			if (startMainPowerShell)
			{
				return this.localPowerShell.BeginInvoke<object, PSObject>(this.input, this.localPowerShellOutput, settings, null, null);
			}
			return this.extraPowerShell.BeginInvoke<object, PSObject>(this.input, this.localPowerShellOutput, settings, null, null);
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000D0B1D File Offset: 0x000CED1D
		internal IAsyncResult Start()
		{
			return this.Start(true);
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000D0B4C File Offset: 0x000CED4C
		internal void RunNoOpCommand()
		{
			if (this.localPowerShell != null)
			{
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					this.localPowerShell.SetStateChanged(new PSInvocationStateInfo(PSInvocationState.Running, null));
					this.localPowerShell.SetStateChanged(new PSInvocationStateInfo(PSInvocationState.Completed, null));
				});
			}
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000D0B7C File Offset: 0x000CED7C
		internal void InvokeMain()
		{
			PSInvocationSettings settings = this.PrepInvoke(true);
			Exception ex = null;
			try
			{
				this.localPowerShell.InvokeWithDebugger(this.input, this.localPowerShellOutput, settings, true);
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				ex = ex2;
			}
			if (ex != null)
			{
				string commandText = this.localPowerShell.Commands.Commands[0].CommandText;
				this.localPowerShell.Commands.Clear();
				string value = StringUtil.Format(RemotingErrorIdStrings.ServerSideNestedCommandInvokeFailed, commandText ?? string.Empty, ex.Message ?? string.Empty);
				this.localPowerShell.AddCommand("Write-Error").AddArgument(value);
				this.localPowerShell.Invoke();
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000D0C44 File Offset: 0x000CEE44
		private void RegisterPowerShellEventHandlers(PowerShell powerShell)
		{
			powerShell.InvocationStateChanged += this.HandlePowerShellInvocationStateChanged;
			powerShell.Streams.Error.DataAdded += this.HandleErrorDataAdded;
			powerShell.Streams.Debug.DataAdded += this.HandleDebugAdded;
			powerShell.Streams.Verbose.DataAdded += this.HandleVerboseAdded;
			powerShell.Streams.Warning.DataAdded += this.HandleWarningAdded;
			powerShell.Streams.Progress.DataAdded += this.HandleProgressAdded;
			powerShell.Streams.Information.DataAdded += this.HandleInformationAdded;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000D0D0C File Offset: 0x000CEF0C
		private void UnregisterPowerShellEventHandlers(PowerShell powerShell)
		{
			powerShell.InvocationStateChanged -= this.HandlePowerShellInvocationStateChanged;
			powerShell.Streams.Error.DataAdded -= this.HandleErrorDataAdded;
			powerShell.Streams.Debug.DataAdded -= this.HandleDebugAdded;
			powerShell.Streams.Verbose.DataAdded -= this.HandleVerboseAdded;
			powerShell.Streams.Warning.DataAdded -= this.HandleWarningAdded;
			powerShell.Streams.Progress.DataAdded -= this.HandleProgressAdded;
			powerShell.Streams.Information.DataAdded -= this.HandleInformationAdded;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000D0DD4 File Offset: 0x000CEFD4
		private void RegisterDataStructureHandlerEventHandlers(ServerPowerShellDataStructureHandler dsHandler)
		{
			dsHandler.InputEndReceived += this.HandleInputEndReceived;
			dsHandler.InputReceived += this.HandleInputReceived;
			dsHandler.StopPowerShellReceived += this.HandleStopReceived;
			dsHandler.HostResponseReceived += this.HandleHostResponseReceived;
			dsHandler.OnSessionConnected += this.HandleSessionConnected;
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000D0E3C File Offset: 0x000CF03C
		private void UnregisterDataStructureHandlerEventHandlers(ServerPowerShellDataStructureHandler dsHandler)
		{
			dsHandler.InputEndReceived -= this.HandleInputEndReceived;
			dsHandler.InputReceived -= this.HandleInputReceived;
			dsHandler.StopPowerShellReceived -= this.HandleStopReceived;
			dsHandler.HostResponseReceived -= this.HandleHostResponseReceived;
			dsHandler.OnSessionConnected -= this.HandleSessionConnected;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000D0EA3 File Offset: 0x000CF0A3
		private void RegisterPipelineOutputEventHandlers(PSDataCollection<PSObject> pipelineOutput)
		{
			pipelineOutput.DataAdded += this.HandleOutputDataAdded;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000D0EB7 File Offset: 0x000CF0B7
		private void UnregisterPipelineOutputEventHandlers(PSDataCollection<PSObject> pipelineOutput)
		{
			pipelineOutput.DataAdded -= this.HandleOutputDataAdded;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000D0ECC File Offset: 0x000CF0CC
		private void HandlePowerShellInvocationStateChanged(object sender, PSInvocationStateChangedEventArgs eventArgs)
		{
			PSInvocationState state = eventArgs.InvocationStateInfo.State;
			switch (state)
			{
			case PSInvocationState.Stopping:
				this.remoteHost.ServerMethodExecutor.AbortAllCalls();
				return;
			case PSInvocationState.Stopped:
			case PSInvocationState.Completed:
			case PSInvocationState.Failed:
				if (this.localPowerShell.RunningExtraCommands && state == PSInvocationState.Completed)
				{
					return;
				}
				this.SendRemainingData();
				if (state == PSInvocationState.Completed && this.extraPowerShell != null && !this.extraPowerShellAlreadyScheduled)
				{
					this.extraPowerShellAlreadyScheduled = true;
					this.Start(false);
					return;
				}
				this.dsHandler.RaiseRemoveAssociationEvent();
				this.dsHandler.SendStateChangedInformationToClient(eventArgs.InvocationStateInfo);
				this.UnregisterPowerShellEventHandlers(this.localPowerShell);
				if (this.extraPowerShell != null)
				{
					this.UnregisterPowerShellEventHandlers(this.extraPowerShell);
				}
				this.UnregisterDataStructureHandlerEventHandlers(this.dsHandler);
				this.UnregisterPipelineOutputEventHandlers(this.localPowerShellOutput);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000D0FA4 File Offset: 0x000CF1A4
		private void HandleOutputDataAdded(object sender, DataAddedEventArgs e)
		{
			int index = e.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (!this.datasent[num])
				{
					PSObject data = this.localPowerShellOutput[index];
					this.localPowerShellOutput.RemoveAt(index);
					this.dsHandler.SendOutputDataToClient(data);
				}
			}
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000D1024 File Offset: 0x000CF224
		private void HandleErrorDataAdded(object sender, DataAddedEventArgs e)
		{
			int index = e.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (num == 0 && !this.datasent[num])
				{
					ErrorRecord errorRecord = this.localPowerShell.Streams.Error[index];
					this.localPowerShell.Streams.Error.RemoveAt(index);
					this.dsHandler.SendErrorRecordToClient(errorRecord);
				}
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000D10BC File Offset: 0x000CF2BC
		private void HandleProgressAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (num == 0 && !this.datasent[num])
				{
					ProgressRecord record = this.localPowerShell.Streams.Progress[index];
					this.localPowerShell.Streams.Progress.RemoveAt(index);
					this.dsHandler.SendProgressRecordToClient(record);
				}
			}
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000D1154 File Offset: 0x000CF354
		private void HandleWarningAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (num == 0 && !this.datasent[num])
				{
					WarningRecord record = this.localPowerShell.Streams.Warning[index];
					this.localPowerShell.Streams.Warning.RemoveAt(index);
					this.dsHandler.SendWarningRecordToClient(record);
				}
			}
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000D11EC File Offset: 0x000CF3EC
		private void HandleVerboseAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (num == 0 && !this.datasent[num])
				{
					VerboseRecord record = this.localPowerShell.Streams.Verbose[index];
					this.localPowerShell.Streams.Verbose.RemoveAt(index);
					this.dsHandler.SendVerboseRecordToClient(record);
				}
			}
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000D1284 File Offset: 0x000CF484
		private void HandleDebugAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (num == 0 && !this.datasent[num])
				{
					DebugRecord record = this.localPowerShell.Streams.Debug[index];
					this.localPowerShell.Streams.Debug.RemoveAt(index);
					this.dsHandler.SendDebugRecordToClient(record);
				}
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000D131C File Offset: 0x000CF51C
		private void HandleInformationAdded(object sender, DataAddedEventArgs eventArgs)
		{
			int index = eventArgs.Index;
			lock (this.syncObject)
			{
				int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
				if (num == 0 && !this.datasent[num])
				{
					InformationRecord record = this.localPowerShell.Streams.Information[index];
					this.localPowerShell.Streams.Information.RemoveAt(index);
					this.dsHandler.SendInformationRecordToClient(record);
				}
			}
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000D13B4 File Offset: 0x000CF5B4
		private void SendRemainingData()
		{
			int num = (!this.extraPowerShellAlreadyScheduled) ? 0 : 1;
			lock (this.syncObject)
			{
				this.datasent[num] = true;
			}
			try
			{
				for (int i = 0; i < this.localPowerShellOutput.Count; i++)
				{
					PSObject data = this.localPowerShellOutput[i];
					this.dsHandler.SendOutputDataToClient(data);
				}
				this.localPowerShellOutput.Clear();
				for (int j = 0; j < this.localPowerShell.Streams.Error.Count; j++)
				{
					ErrorRecord errorRecord = this.localPowerShell.Streams.Error[j];
					this.dsHandler.SendErrorRecordToClient(errorRecord);
				}
				this.localPowerShell.Streams.Error.Clear();
			}
			finally
			{
				lock (this.syncObject)
				{
					this.datasent[num] = true;
				}
			}
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000D14E4 File Offset: 0x000CF6E4
		private void HandleStopReceived(object sender, EventArgs eventArgs)
		{
			if (this.localPowerShell.InvocationStateInfo.State != PSInvocationState.Stopped && this.localPowerShell.InvocationStateInfo.State != PSInvocationState.Completed && this.localPowerShell.InvocationStateInfo.State != PSInvocationState.Failed && this.localPowerShell.InvocationStateInfo.State != PSInvocationState.Stopping)
			{
				bool flag = false;
				if (!this.localPowerShell.IsNested && this.psDriverInvoker != null)
				{
					flag = this.psDriverInvoker.HandleStopSignal();
				}
				if (!flag)
				{
					this.localPowerShell.Stop();
				}
			}
			if (this.extraPowerShell != null && this.extraPowerShell.InvocationStateInfo.State != PSInvocationState.Stopped && this.extraPowerShell.InvocationStateInfo.State != PSInvocationState.Completed && this.extraPowerShell.InvocationStateInfo.State != PSInvocationState.Failed)
			{
				if (this.extraPowerShell.InvocationStateInfo.State == PSInvocationState.Stopping)
				{
					return;
				}
				this.extraPowerShell.Stop();
			}
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000D15CE File Offset: 0x000CF7CE
		private void HandleInputReceived(object sender, RemoteDataEventArgs<object> eventArgs)
		{
			if (this.input != null)
			{
				this.input.Add(eventArgs.Data);
			}
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000D15E9 File Offset: 0x000CF7E9
		private void HandleInputEndReceived(object sender, EventArgs eventArgs)
		{
			if (this.input != null)
			{
				this.input.Complete();
			}
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000D15FE File Offset: 0x000CF7FE
		private void HandleSessionConnected(object sender, EventArgs eventArgs)
		{
			if (this.input != null)
			{
				this.input.Complete();
			}
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000D1613 File Offset: 0x000CF813
		private void HandleHostResponseReceived(object sender, RemoteDataEventArgs<RemoteHostResponse> eventArgs)
		{
			this.remoteHost.ServerMethodExecutor.HandleRemoteHostResponseFromClient(eventArgs.Data);
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000D162C File Offset: 0x000CF82C
		private void HandleIdleEvent(object sender, EventArgs args)
		{
			Runspace runspaceUsedToInvokePowerShell = this.dsHandler.RunspaceUsedToInvokePowerShell;
			if (runspaceUsedToInvokePowerShell != null)
			{
				PSLocalEventManager pslocalEventManager = runspaceUsedToInvokePowerShell.Events as PSLocalEventManager;
				if (pslocalEventManager != null)
				{
					foreach (PSEventSubscriber subscriber in pslocalEventManager.Subscribers)
					{
						pslocalEventManager.DrainPendingActions(subscriber);
					}
				}
			}
		}

		// Token: 0x04001247 RID: 4679
		private bool extraPowerShellAlreadyScheduled;

		// Token: 0x04001248 RID: 4680
		private PowerShell extraPowerShell;

		// Token: 0x04001249 RID: 4681
		private PowerShell localPowerShell;

		// Token: 0x0400124A RID: 4682
		private PSDataCollection<PSObject> localPowerShellOutput;

		// Token: 0x0400124B RID: 4683
		private Guid clientPowerShellId;

		// Token: 0x0400124C RID: 4684
		private Guid clientRunspacePoolId;

		// Token: 0x0400124D RID: 4685
		private ServerPowerShellDataStructureHandler dsHandler;

		// Token: 0x0400124E RID: 4686
		private PSDataCollection<object> input;

		// Token: 0x0400124F RID: 4687
		private bool[] datasent = new bool[2];

		// Token: 0x04001250 RID: 4688
		private object syncObject = new object();

		// Token: 0x04001251 RID: 4689
		private bool noInput;

		// Token: 0x04001252 RID: 4690
		private bool addToHistory;

		// Token: 0x04001253 RID: 4691
		private ServerRemoteHost remoteHost;

		// Token: 0x04001254 RID: 4692
		private ApartmentState apartmentState;

		// Token: 0x04001255 RID: 4693
		private RemoteStreamOptions remoteStreamOptions;

		// Token: 0x04001256 RID: 4694
		private IRSPDriverInvoke psDriverInvoker;
	}
}
