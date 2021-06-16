using System;
using System.Collections.Generic;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000302 RID: 770
	internal class ServerSteppablePipelineDriver
	{
		// Token: 0x06002456 RID: 9302 RVA: 0x000CC4DC File Offset: 0x000CA6DC
		internal ServerSteppablePipelineDriver(PowerShell powershell, bool noInput, Guid clientPowerShellId, Guid clientRunspacePoolId, ServerRunspacePoolDriver runspacePoolDriver, ApartmentState apartmentState, HostInfo hostInfo, RemoteStreamOptions streamOptions, bool addToHistory, Runspace rsToUse, ServerSteppablePipelineSubscriber eventSubscriber, PSDataCollection<object> powershellInput)
		{
			this.localPowerShell = powershell;
			this.clientPowerShellId = clientPowerShellId;
			this.clientRunspacePoolId = clientRunspacePoolId;
			this.remoteStreamOptions = streamOptions;
			this.apartmentState = apartmentState;
			this.noInput = noInput;
			this.addToHistory = addToHistory;
			this.eventSubscriber = eventSubscriber;
			this.powershellInput = powershellInput;
			this.input = new PSDataCollection<object>();
			this.inputEnumerator = this.input.GetEnumerator();
			this.input.ReleaseOnEnumeration = true;
			this.dsHandler = runspacePoolDriver.DataStructureHandler.CreatePowerShellDataStructureHandler(clientPowerShellId, clientRunspacePoolId, this.remoteStreamOptions, null);
			this.remoteHost = this.dsHandler.GetHostAssociatedWithPowerShell(hostInfo, runspacePoolDriver.ServerRemoteHost);
			this.dsHandler.InputEndReceived += this.HandleInputEndReceived;
			this.dsHandler.InputReceived += this.HandleInputReceived;
			this.dsHandler.StopPowerShellReceived += this.HandleStopReceived;
			this.dsHandler.HostResponseReceived += this.HandleHostResponseReceived;
			this.dsHandler.OnSessionConnected += this.HandleSessionConnected;
			if (rsToUse == null)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.NestedPipelineMissingRunspace, new object[0]);
			}
			this.localPowerShell.Runspace = rsToUse;
			eventSubscriber.SubscribeEvents(this);
			this.stateOfSteppablePipeline = PSInvocationState.NotStarted;
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000CC641 File Offset: 0x000CA841
		internal PowerShell LocalPowerShell
		{
			get
			{
				return this.localPowerShell;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x000CC649 File Offset: 0x000CA849
		internal Guid InstanceId
		{
			get
			{
				return this.clientPowerShellId;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x000CC651 File Offset: 0x000CA851
		internal ServerRemoteHost RemoteHost
		{
			get
			{
				return this.remoteHost;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x000CC659 File Offset: 0x000CA859
		internal RemoteStreamOptions RemoteStreamOptions
		{
			get
			{
				return this.remoteStreamOptions;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600245B RID: 9307 RVA: 0x000CC661 File Offset: 0x000CA861
		internal Guid RunspacePoolId
		{
			get
			{
				return this.clientRunspacePoolId;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x000CC669 File Offset: 0x000CA869
		internal ServerPowerShellDataStructureHandler DataStructureHandler
		{
			get
			{
				return this.dsHandler;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x000CC671 File Offset: 0x000CA871
		internal PSInvocationState PipelineState
		{
			get
			{
				return this.stateOfSteppablePipeline;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000CC679 File Offset: 0x000CA879
		internal bool NoInput
		{
			get
			{
				return this.noInput;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000CC681 File Offset: 0x000CA881
		// (set) Token: 0x06002460 RID: 9312 RVA: 0x000CC689 File Offset: 0x000CA889
		internal SteppablePipeline SteppablePipeline
		{
			get
			{
				return this.steppablePipeline;
			}
			set
			{
				this.steppablePipeline = value;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x000CC692 File Offset: 0x000CA892
		internal object SyncObject
		{
			get
			{
				return this.syncObject;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x000CC69A File Offset: 0x000CA89A
		// (set) Token: 0x06002463 RID: 9315 RVA: 0x000CC6A2 File Offset: 0x000CA8A2
		internal bool ProcessingInput
		{
			get
			{
				return this.isProcessingInput;
			}
			set
			{
				this.isProcessingInput = value;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000CC6AB File Offset: 0x000CA8AB
		internal IEnumerator<object> InputEnumerator
		{
			get
			{
				return this.inputEnumerator;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000CC6B3 File Offset: 0x000CA8B3
		internal PSDataCollection<object> Input
		{
			get
			{
				return this.input;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000CC6BB File Offset: 0x000CA8BB
		// (set) Token: 0x06002467 RID: 9319 RVA: 0x000CC6C3 File Offset: 0x000CA8C3
		internal bool Pulsed
		{
			get
			{
				return this.isPulsed;
			}
			set
			{
				this.isPulsed = value;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000CC6CC File Offset: 0x000CA8CC
		// (set) Token: 0x06002469 RID: 9321 RVA: 0x000CC6D4 File Offset: 0x000CA8D4
		internal int TotalObjectsProcessed
		{
			get
			{
				return this.totalObjectsProcessedSoFar;
			}
			set
			{
				this.totalObjectsProcessedSoFar = value;
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000CC6DD File Offset: 0x000CA8DD
		internal void Start()
		{
			this.stateOfSteppablePipeline = PSInvocationState.Running;
			this.eventSubscriber.FireStartSteppablePipeline(this);
			if (this.powershellInput != null)
			{
				this.powershellInput.Pulse();
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000CC705 File Offset: 0x000CA905
		internal void HandleInputEndReceived(object sender, EventArgs eventArgs)
		{
			this.input.Complete();
			this.CheckAndPulseForProcessing(true);
			if (this.powershellInput != null)
			{
				this.powershellInput.Pulse();
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000CC72C File Offset: 0x000CA92C
		private void HandleSessionConnected(object sender, EventArgs eventArgs)
		{
			if (this.input != null)
			{
				this.input.Complete();
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000CC741 File Offset: 0x000CA941
		internal void HandleHostResponseReceived(object sender, RemoteDataEventArgs<RemoteHostResponse> eventArgs)
		{
			this.remoteHost.ServerMethodExecutor.HandleRemoteHostResponseFromClient(eventArgs.Data);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000CC75C File Offset: 0x000CA95C
		private void HandleStopReceived(object sender, EventArgs eventArgs)
		{
			lock (this.syncObject)
			{
				this.stateOfSteppablePipeline = PSInvocationState.Stopping;
			}
			this.PerformStop();
			if (this.powershellInput != null)
			{
				this.powershellInput.Pulse();
			}
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000CC7B8 File Offset: 0x000CA9B8
		private void HandleInputReceived(object sender, RemoteDataEventArgs<object> eventArgs)
		{
			if (this.input != null)
			{
				lock (this.syncObject)
				{
					this.input.Add(eventArgs.Data);
				}
				this.CheckAndPulseForProcessing(false);
				if (this.powershellInput != null)
				{
					this.powershellInput.Pulse();
				}
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000CC828 File Offset: 0x000CAA28
		internal void CheckAndPulseForProcessing(bool complete)
		{
			if (complete)
			{
				this.eventSubscriber.FireHandleProcessRecord(this);
				return;
			}
			if (!this.isPulsed)
			{
				bool flag = false;
				lock (this.syncObject)
				{
					if (this.isPulsed)
					{
						return;
					}
					if (!this.isProcessingInput && this.input.Count > this.totalObjectsProcessedSoFar)
					{
						flag = true;
						this.isPulsed = true;
					}
				}
				if (flag && this.stateOfSteppablePipeline == PSInvocationState.Running)
				{
					this.eventSubscriber.FireHandleProcessRecord(this);
				}
			}
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000CC8C4 File Offset: 0x000CAAC4
		internal void PerformStop()
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (!this.isProcessingInput && this.stateOfSteppablePipeline == PSInvocationState.Stopping)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.SetState(PSInvocationState.Stopped, new PipelineStoppedException());
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000CC924 File Offset: 0x000CAB24
		internal void SetState(PSInvocationState newState, Exception reason)
		{
			PSInvocationState state = PSInvocationState.NotStarted;
			bool flag = false;
			lock (this.syncObject)
			{
				switch (this.stateOfSteppablePipeline)
				{
				case PSInvocationState.NotStarted:
					switch (newState)
					{
					case PSInvocationState.Running:
					case PSInvocationState.Stopping:
					case PSInvocationState.Stopped:
					case PSInvocationState.Completed:
					case PSInvocationState.Failed:
						state = newState;
						break;
					}
					break;
				case PSInvocationState.Running:
					switch (newState)
					{
					case PSInvocationState.NotStarted:
						throw new InvalidOperationException();
					case PSInvocationState.Stopping:
						state = newState;
						break;
					case PSInvocationState.Stopped:
					case PSInvocationState.Completed:
					case PSInvocationState.Failed:
						state = newState;
						flag = true;
						break;
					}
					break;
				case PSInvocationState.Stopping:
					switch (newState)
					{
					case PSInvocationState.Stopped:
						state = newState;
						flag = true;
						break;
					case PSInvocationState.Completed:
					case PSInvocationState.Failed:
						state = PSInvocationState.Stopped;
						flag = true;
						break;
					default:
						throw new InvalidOperationException();
					}
					break;
				}
				this.stateOfSteppablePipeline = state;
			}
			if (flag)
			{
				this.dsHandler.SendStateChangedInformationToClient(new PSInvocationStateInfo(state, reason));
			}
			if (this.stateOfSteppablePipeline == PSInvocationState.Completed || this.stateOfSteppablePipeline == PSInvocationState.Stopped || this.stateOfSteppablePipeline == PSInvocationState.Failed)
			{
				this.dsHandler.RaiseRemoveAssociationEvent();
			}
		}

		// Token: 0x040011DB RID: 4571
		private PowerShell localPowerShell;

		// Token: 0x040011DC RID: 4572
		private Guid clientPowerShellId;

		// Token: 0x040011DD RID: 4573
		private Guid clientRunspacePoolId;

		// Token: 0x040011DE RID: 4574
		private ServerPowerShellDataStructureHandler dsHandler;

		// Token: 0x040011DF RID: 4575
		private PSDataCollection<object> input;

		// Token: 0x040011E0 RID: 4576
		private IEnumerator<object> inputEnumerator;

		// Token: 0x040011E1 RID: 4577
		private object syncObject = new object();

		// Token: 0x040011E2 RID: 4578
		private bool noInput;

		// Token: 0x040011E3 RID: 4579
		private bool addToHistory;

		// Token: 0x040011E4 RID: 4580
		private ServerRemoteHost remoteHost;

		// Token: 0x040011E5 RID: 4581
		private ApartmentState apartmentState;

		// Token: 0x040011E6 RID: 4582
		private RemoteStreamOptions remoteStreamOptions;

		// Token: 0x040011E7 RID: 4583
		private int totalObjectsProcessedSoFar;

		// Token: 0x040011E8 RID: 4584
		private SteppablePipeline steppablePipeline;

		// Token: 0x040011E9 RID: 4585
		private bool isProcessingInput;

		// Token: 0x040011EA RID: 4586
		private bool isPulsed;

		// Token: 0x040011EB RID: 4587
		private PSInvocationState stateOfSteppablePipeline;

		// Token: 0x040011EC RID: 4588
		private ServerSteppablePipelineSubscriber eventSubscriber;

		// Token: 0x040011ED RID: 4589
		private PSDataCollection<object> powershellInput;
	}
}
