using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x020002A6 RID: 678
	internal class ClientPowerShellDataStructureHandler
	{
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060020CD RID: 8397 RVA: 0x000BDDBC File Offset: 0x000BBFBC
		// (remove) Token: 0x060020CE RID: 8398 RVA: 0x000BDDF4 File Offset: 0x000BBFF4
		internal event EventHandler RemoveAssociation;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x060020CF RID: 8399 RVA: 0x000BDE2C File Offset: 0x000BC02C
		// (remove) Token: 0x060020D0 RID: 8400 RVA: 0x000BDE64 File Offset: 0x000BC064
		internal event EventHandler<RemoteDataEventArgs<PSInvocationStateInfo>> InvocationStateInfoReceived;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x060020D1 RID: 8401 RVA: 0x000BDE9C File Offset: 0x000BC09C
		// (remove) Token: 0x060020D2 RID: 8402 RVA: 0x000BDED4 File Offset: 0x000BC0D4
		internal event EventHandler<RemoteDataEventArgs<object>> OutputReceived;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x060020D3 RID: 8403 RVA: 0x000BDF0C File Offset: 0x000BC10C
		// (remove) Token: 0x060020D4 RID: 8404 RVA: 0x000BDF44 File Offset: 0x000BC144
		internal event EventHandler<RemoteDataEventArgs<ErrorRecord>> ErrorReceived;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x060020D5 RID: 8405 RVA: 0x000BDF7C File Offset: 0x000BC17C
		// (remove) Token: 0x060020D6 RID: 8406 RVA: 0x000BDFB4 File Offset: 0x000BC1B4
		internal event EventHandler<RemoteDataEventArgs<InformationalMessage>> InformationalMessageReceived;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x060020D7 RID: 8407 RVA: 0x000BDFEC File Offset: 0x000BC1EC
		// (remove) Token: 0x060020D8 RID: 8408 RVA: 0x000BE024 File Offset: 0x000BC224
		internal event EventHandler<RemoteDataEventArgs<RemoteHostCall>> HostCallReceived;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x060020D9 RID: 8409 RVA: 0x000BE05C File Offset: 0x000BC25C
		// (remove) Token: 0x060020DA RID: 8410 RVA: 0x000BE094 File Offset: 0x000BC294
		internal event EventHandler<RemoteDataEventArgs<Exception>> ClosedNotificationFromRunspacePool;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x060020DB RID: 8411 RVA: 0x000BE0CC File Offset: 0x000BC2CC
		// (remove) Token: 0x060020DC RID: 8412 RVA: 0x000BE104 File Offset: 0x000BC304
		internal event EventHandler<EventArgs> CloseCompleted;

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x060020DD RID: 8413 RVA: 0x000BE13C File Offset: 0x000BC33C
		// (remove) Token: 0x060020DE RID: 8414 RVA: 0x000BE174 File Offset: 0x000BC374
		internal event EventHandler<RemoteDataEventArgs<Exception>> BrokenNotificationFromRunspacePool;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060020DF RID: 8415 RVA: 0x000BE1AC File Offset: 0x000BC3AC
		// (remove) Token: 0x060020E0 RID: 8416 RVA: 0x000BE1E4 File Offset: 0x000BC3E4
		internal event EventHandler<RemoteDataEventArgs<Exception>> ReconnectCompleted;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060020E1 RID: 8417 RVA: 0x000BE21C File Offset: 0x000BC41C
		// (remove) Token: 0x060020E2 RID: 8418 RVA: 0x000BE254 File Offset: 0x000BC454
		internal event EventHandler<RemoteDataEventArgs<Exception>> ConnectCompleted;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x060020E3 RID: 8419 RVA: 0x000BE28C File Offset: 0x000BC48C
		// (remove) Token: 0x060020E4 RID: 8420 RVA: 0x000BE2C4 File Offset: 0x000BC4C4
		internal event EventHandler<ConnectionStatusEventArgs> RobustConnectionNotification;

		// Token: 0x060020E5 RID: 8421 RVA: 0x000BE2F9 File Offset: 0x000BC4F9
		internal void Start(ClientRemoteSessionDSHandlerStateMachine stateMachine, bool inDisconnectMode)
		{
			this.SetupTransportManager(inDisconnectMode);
			this.transportManager.CreateAsync();
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000BE30D File Offset: 0x000BC50D
		private void HandleDelayStreamRequestProcessed(object sender, EventArgs e)
		{
			this.ProcessDisconnect(null);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000BE316 File Offset: 0x000BC516
		internal void HandleReconnectCompleted(object sender, EventArgs args)
		{
			Interlocked.CompareExchange(ref this.connectionState, 1, 4);
			this.ReconnectCompleted.SafeInvoke(this, new RemoteDataEventArgs<Exception>(null));
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000BE338 File Offset: 0x000BC538
		internal void HandleConnectCompleted(object sender, EventArgs args)
		{
			Interlocked.CompareExchange(ref this.connectionState, 1, 5);
			this.ConnectCompleted.SafeInvoke(this, new RemoteDataEventArgs<Exception>(null));
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000BE35C File Offset: 0x000BC55C
		internal void HandleTransportError(object sender, TransportErrorOccuredEventArgs e)
		{
			PSInvocationStateInfo data = new PSInvocationStateInfo(PSInvocationState.Failed, e.Exception);
			this.InvocationStateInfoReceived.SafeInvoke(this, new RemoteDataEventArgs<PSInvocationStateInfo>(data));
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000BE388 File Offset: 0x000BC588
		internal void SendStopPowerShellMessage()
		{
			this.transportManager.CryptoHelper.CompleteKeyExchange();
			this.transportManager.SendStopSignal();
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000BE3A8 File Offset: 0x000BC5A8
		private void OnSignalCompleted(object sender, EventArgs e)
		{
			PSRemotingDataStructureException reason = new PSRemotingDataStructureException(RemotingErrorIdStrings.PipelineStopped);
			this.InvocationStateInfoReceived.SafeInvoke(this, new RemoteDataEventArgs<PSInvocationStateInfo>(new PSInvocationStateInfo(PSInvocationState.Stopped, reason)));
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000BE3D8 File Offset: 0x000BC5D8
		internal void SendHostResponseToServer(RemoteHostResponse hostResponse)
		{
			RemoteDataObject<PSObject> data = RemoteDataObject<PSObject>.CreateFrom(RemotingDestination.Server, RemotingDataType.RemotePowerShellHostResponseData, this.clientRunspacePoolId, this.clientPowerShellId, hostResponse.Encode());
			this.transportManager.DataToBeSentCollection.Add<PSObject>(data, DataPriorityType.PromptResponse);
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000BE418 File Offset: 0x000BC618
		internal void SendInput(ObjectStreamBase inputstream)
		{
			if (!inputstream.IsOpen && inputstream.Count == 0)
			{
				lock (this.inputSyncObject)
				{
					this.SendDataAsync(RemotingEncoder.GeneratePowerShellInputEnd(this.clientRunspacePoolId, this.clientPowerShellId));
					return;
				}
			}
			lock (this.inputSyncObject)
			{
				inputstream.DataReady += this.HandleInputDataReady;
				this.WriteInput(inputstream);
			}
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000BE4BC File Offset: 0x000BC6BC
		internal void ProcessReceivedData(RemoteDataObject<PSObject> receivedData)
		{
			if (receivedData.PowerShellId != this.clientPowerShellId)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.PipelineIdsDoNotMatch, new object[]
				{
					receivedData.PowerShellId,
					this.clientPowerShellId
				});
			}
			RemotingDataType dataType = receivedData.DataType;
			switch (dataType)
			{
			case RemotingDataType.PowerShellOutput:
			{
				object powerShellOutput = RemotingDecoder.GetPowerShellOutput(receivedData.Data);
				this.OutputReceived.SafeInvoke(this, new RemoteDataEventArgs<object>(powerShellOutput));
				return;
			}
			case RemotingDataType.PowerShellErrorRecord:
			{
				ErrorRecord powerShellError = RemotingDecoder.GetPowerShellError(receivedData.Data);
				this.ErrorReceived.SafeInvoke(this, new RemoteDataEventArgs<ErrorRecord>(powerShellError));
				return;
			}
			case RemotingDataType.PowerShellStateInfo:
			{
				PSInvocationStateInfo powerShellStateInfo = RemotingDecoder.GetPowerShellStateInfo(receivedData.Data);
				this.InvocationStateInfoReceived.SafeInvoke(this, new RemoteDataEventArgs<PSInvocationStateInfo>(powerShellStateInfo));
				return;
			}
			case RemotingDataType.PowerShellDebug:
			{
				DebugRecord powerShellDebug = RemotingDecoder.GetPowerShellDebug(receivedData.Data);
				this.InformationalMessageReceived.SafeInvoke(this, new RemoteDataEventArgs<InformationalMessage>(new InformationalMessage(powerShellDebug, RemotingDataType.PowerShellDebug)));
				return;
			}
			case RemotingDataType.PowerShellVerbose:
			{
				VerboseRecord powerShellVerbose = RemotingDecoder.GetPowerShellVerbose(receivedData.Data);
				this.InformationalMessageReceived.SafeInvoke(this, new RemoteDataEventArgs<InformationalMessage>(new InformationalMessage(powerShellVerbose, RemotingDataType.PowerShellVerbose)));
				return;
			}
			case RemotingDataType.PowerShellWarning:
			{
				WarningRecord powerShellWarning = RemotingDecoder.GetPowerShellWarning(receivedData.Data);
				this.InformationalMessageReceived.SafeInvoke(this, new RemoteDataEventArgs<InformationalMessage>(new InformationalMessage(powerShellWarning, RemotingDataType.PowerShellWarning)));
				return;
			}
			case (RemotingDataType)266250U:
			case (RemotingDataType)266251U:
			case (RemotingDataType)266252U:
			case (RemotingDataType)266253U:
			case (RemotingDataType)266254U:
			case (RemotingDataType)266255U:
				break;
			case RemotingDataType.PowerShellProgress:
			{
				ProgressRecord powerShellProgress = RemotingDecoder.GetPowerShellProgress(receivedData.Data);
				this.InformationalMessageReceived.SafeInvoke(this, new RemoteDataEventArgs<InformationalMessage>(new InformationalMessage(powerShellProgress, RemotingDataType.PowerShellProgress)));
				return;
			}
			case RemotingDataType.PowerShellInformationStream:
			{
				InformationRecord powerShellInformation = RemotingDecoder.GetPowerShellInformation(receivedData.Data);
				this.InformationalMessageReceived.SafeInvoke(this, new RemoteDataEventArgs<InformationalMessage>(new InformationalMessage(powerShellInformation, RemotingDataType.PowerShellInformationStream)));
				return;
			}
			default:
			{
				if (dataType != RemotingDataType.RemoteHostCallUsingPowerShellHost)
				{
					return;
				}
				RemoteHostCall data = RemoteHostCall.Decode(receivedData.Data);
				this.HostCallReceived.SafeInvoke(this, new RemoteDataEventArgs<RemoteHostCall>(data));
				break;
			}
			}
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000BE6BB File Offset: 0x000BC8BB
		internal void SetStateToFailed(Exception reason)
		{
			this.BrokenNotificationFromRunspacePool.SafeInvoke(this, new RemoteDataEventArgs<Exception>(reason));
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000BE6CF File Offset: 0x000BC8CF
		internal void SetStateToStopped(Exception reason)
		{
			this.ClosedNotificationFromRunspacePool.SafeInvoke(this, new RemoteDataEventArgs<Exception>(reason));
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000BE72F File Offset: 0x000BC92F
		internal void CloseConnectionAsync(Exception sessionCloseReason)
		{
			this._sessionClosedReason = sessionCloseReason;
			this.transportManager.CloseCompleted += delegate(object source, EventArgs args)
			{
				if (this.CloseCompleted != null)
				{
					EventArgs e = (args == EventArgs.Empty) ? new RemoteSessionStateEventArgs(new RemoteSessionStateInfo(RemoteSessionState.Closed, this._sessionClosedReason)) : args;
					this.CloseCompleted(this, e);
				}
				this.transportManager.Dispose();
			};
			this.transportManager.CloseAsync();
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000BE75A File Offset: 0x000BC95A
		internal void RaiseRemoveAssociationEvent()
		{
			this.RemoveAssociation.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000BE770 File Offset: 0x000BC970
		internal void ProcessDisconnect(RunspacePoolStateInfo rsStateInfo)
		{
			PSInvocationStateInfo data = new PSInvocationStateInfo(PSInvocationState.Disconnected, (rsStateInfo != null) ? rsStateInfo.Reason : null);
			this.InvocationStateInfoReceived.SafeInvoke(this, new RemoteDataEventArgs<PSInvocationStateInfo>(data));
			Interlocked.CompareExchange(ref this.connectionState, 3, 1);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000BE7B0 File Offset: 0x000BC9B0
		internal void ReconnectAsync()
		{
			int num = Interlocked.CompareExchange(ref this.connectionState, 4, 3);
			if (num != 3)
			{
				return;
			}
			this.transportManager.ReconnectAsync();
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000BE7DB File Offset: 0x000BC9DB
		internal void ConnectAsync()
		{
			Interlocked.CompareExchange(ref this.connectionState, 5, 3);
			this.SetupTransportManager(false);
			this.transportManager.ConnectAsync();
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000BE7FD File Offset: 0x000BC9FD
		internal void ProcessRobustConnectionNotification(ConnectionStatusEventArgs e)
		{
			this.RobustConnectionNotification.SafeInvoke(this, e);
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000BE80C File Offset: 0x000BCA0C
		internal ClientPowerShellDataStructureHandler(BaseClientCommandTransportManager transportManager, Guid clientRunspacePoolId, Guid clientPowerShellId)
		{
			this.transportManager = transportManager;
			this.clientRunspacePoolId = clientRunspacePoolId;
			this.clientPowerShellId = clientPowerShellId;
			transportManager.SignalCompleted += this.OnSignalCompleted;
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000BE858 File Offset: 0x000BCA58
		internal Guid PowerShellId
		{
			get
			{
				return this.clientPowerShellId;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000BE860 File Offset: 0x000BCA60
		internal BaseClientCommandTransportManager TransportManager
		{
			get
			{
				return this.transportManager;
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000BE868 File Offset: 0x000BCA68
		private void SendDataAsync(RemoteDataObject data)
		{
			this.transportManager.DataToBeSentCollection.Add<object>(data);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000BE888 File Offset: 0x000BCA88
		private void HandleInputDataReady(object sender, EventArgs e)
		{
			lock (this.inputSyncObject)
			{
				ObjectStreamBase inputstream = sender as ObjectStreamBase;
				this.WriteInput(inputstream);
			}
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000BE8D0 File Offset: 0x000BCAD0
		private void WriteInput(ObjectStreamBase inputstream)
		{
			Collection<object> collection = inputstream.ObjectReader.NonBlockingRead(int.MaxValue);
			foreach (object data in collection)
			{
				this.SendDataAsync(RemotingEncoder.GeneratePowerShellInput(data, this.clientRunspacePoolId, this.clientPowerShellId));
			}
			if (!inputstream.IsOpen)
			{
				collection = inputstream.ObjectReader.NonBlockingRead(int.MaxValue);
				foreach (object data2 in collection)
				{
					this.SendDataAsync(RemotingEncoder.GeneratePowerShellInput(data2, this.clientRunspacePoolId, this.clientPowerShellId));
				}
				inputstream.DataReady -= this.HandleInputDataReady;
				this.SendDataAsync(RemotingEncoder.GeneratePowerShellInputEnd(this.clientRunspacePoolId, this.clientPowerShellId));
			}
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000BE9CC File Offset: 0x000BCBCC
		private void SetupTransportManager(bool inDisconnectMode)
		{
			this.transportManager.WSManTransportErrorOccured += this.HandleTransportError;
			this.transportManager.ReconnectCompleted += this.HandleReconnectCompleted;
			this.transportManager.ConnectCompleted += this.HandleConnectCompleted;
			this.transportManager.DelayStreamRequestProcessed += this.HandleDelayStreamRequestProcessed;
			this.transportManager.startInDisconnectedMode = inDisconnectMode;
		}

		// Token: 0x04000E8F RID: 3727
		protected Guid clientRunspacePoolId;

		// Token: 0x04000E90 RID: 3728
		protected Guid clientPowerShellId;

		// Token: 0x04000E91 RID: 3729
		private BaseClientCommandTransportManager transportManager;

		// Token: 0x04000E92 RID: 3730
		private object inputSyncObject = new object();

		// Token: 0x04000E93 RID: 3731
		private int connectionState = 1;

		// Token: 0x04000E94 RID: 3732
		private Exception _sessionClosedReason;

		// Token: 0x020002A7 RID: 679
		private enum connectionStates
		{
			// Token: 0x04000E96 RID: 3734
			Connected = 1,
			// Token: 0x04000E97 RID: 3735
			Disconnected = 3,
			// Token: 0x04000E98 RID: 3736
			Reconnecting,
			// Token: 0x04000E99 RID: 3737
			Connecting
		}
	}
}
