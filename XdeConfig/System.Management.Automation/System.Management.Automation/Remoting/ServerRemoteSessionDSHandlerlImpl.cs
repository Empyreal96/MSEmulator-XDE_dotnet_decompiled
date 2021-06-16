using System;
using System.Management.Automation.Remoting.Server;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000305 RID: 773
	internal class ServerRemoteSessionDSHandlerlImpl : ServerRemoteSessionDataStructureHandler
	{
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x000CD1D3 File Offset: 0x000CB3D3
		internal override AbstractServerSessionTransportManager TransportManager
		{
			get
			{
				return this._transportManager;
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000CD1DB File Offset: 0x000CB3DB
		internal ServerRemoteSessionDSHandlerlImpl(ServerRemoteSession session, AbstractServerSessionTransportManager transportManager)
		{
			this._session = session;
			this._stateMachine = new ServerRemoteSessionDSHandlerStateMachine(session);
			this._transportManager = transportManager;
			this._transportManager.DataReceived += session.DispatchInputQueueData;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000CD214 File Offset: 0x000CB414
		internal override void ConnectAsync()
		{
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000CD218 File Offset: 0x000CB418
		internal override void SendNegotiationAsync()
		{
			RemoteSessionCapability serverCapability = this._session.Context.ServerCapability;
			RemoteDataObject remoteDataObject = RemotingEncoder.GenerateServerSessionCapability(serverCapability, Guid.Empty);
			RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationSendCompleted);
			this._stateMachine.RaiseEvent(fsmEventArg);
			RemoteDataObject<PSObject> data = RemoteDataObject<PSObject>.CreateFrom(remoteDataObject.Destination, remoteDataObject.DataType, remoteDataObject.RunspacePoolId, remoteDataObject.PowerShellId, (PSObject)remoteDataObject.Data);
			this._transportManager.SendDataToClient<PSObject>(data, false, false);
		}

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06002482 RID: 9346 RVA: 0x000CD28C File Offset: 0x000CB48C
		// (remove) Token: 0x06002483 RID: 9347 RVA: 0x000CD2C4 File Offset: 0x000CB4C4
		internal override event EventHandler<RemoteSessionNegotiationEventArgs> NegotiationReceived;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06002484 RID: 9348 RVA: 0x000CD2FC File Offset: 0x000CB4FC
		// (remove) Token: 0x06002485 RID: 9349 RVA: 0x000CD334 File Offset: 0x000CB534
		internal override event EventHandler<EventArgs> SessionClosing;

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06002486 RID: 9350 RVA: 0x000CD36C File Offset: 0x000CB56C
		// (remove) Token: 0x06002487 RID: 9351 RVA: 0x000CD3A4 File Offset: 0x000CB5A4
		internal override event EventHandler<RemoteDataEventArgs<string>> PublicKeyReceived;

		// Token: 0x06002488 RID: 9352 RVA: 0x000CD3D9 File Offset: 0x000CB5D9
		internal override void SendEncryptedSessionKey(string encryptedSessionKey)
		{
			this._transportManager.SendDataToClient<object>(RemotingEncoder.GenerateEncryptedSessionKeyResponse(Guid.Empty, encryptedSessionKey), true, false);
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000CD3F3 File Offset: 0x000CB5F3
		internal override void SendRequestForPublicKey()
		{
			this._transportManager.SendDataToClient<object>(RemotingEncoder.GeneratePublicKeyRequest(Guid.Empty), true, false);
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000CD40C File Offset: 0x000CB60C
		internal override void RaiseKeyExchangeMessageReceived(RemoteDataObject<PSObject> receivedData)
		{
			this.RaiseDataReceivedEvent(new RemoteDataEventArgs(receivedData));
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000CD41C File Offset: 0x000CB61C
		internal override void CloseConnectionAsync(Exception reasonForClose)
		{
			this.SessionClosing.SafeInvoke(this, EventArgs.Empty);
			this._transportManager.Close(reasonForClose);
			RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.CloseCompleted);
			this._stateMachine.RaiseEvent(fsmEventArg);
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x0600248C RID: 9356 RVA: 0x000CD45C File Offset: 0x000CB65C
		// (remove) Token: 0x0600248D RID: 9357 RVA: 0x000CD494 File Offset: 0x000CB694
		internal override event EventHandler<RemoteDataEventArgs> CreateRunspacePoolReceived;

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x000CD4C9 File Offset: 0x000CB6C9
		internal override ServerRemoteSessionDSHandlerStateMachine StateMachine
		{
			get
			{
				return this._stateMachine;
			}
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000CD4D4 File Offset: 0x000CB6D4
		internal override void RaiseDataReceivedEvent(RemoteDataEventArgs dataArg)
		{
			if (dataArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataArg");
			}
			RemoteDataObject<PSObject> receivedData = dataArg.ReceivedData;
			RemotingTargetInterface targetInterface = receivedData.TargetInterface;
			RemotingDataType dataType = receivedData.DataType;
			switch (dataType)
			{
			case RemotingDataType.SessionCapability:
			{
				RemoteSessionCapability remoteSessionCapability = null;
				try
				{
					remoteSessionCapability = RemotingDecoder.GetSessionCapability(receivedData.Data);
				}
				catch (PSRemotingDataStructureException ex)
				{
					throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerNotFoundCapabilityProperties, new object[]
					{
						ex.Message,
						PSVersionInfo.BuildVersion,
						RemotingConstants.ProtocolVersion
					});
				}
				RemoteSessionStateMachineEventArgs remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationReceived);
				remoteSessionStateMachineEventArgs.RemoteSessionCapability = remoteSessionCapability;
				this._stateMachine.RaiseEvent(remoteSessionStateMachineEventArgs);
				if (this.NegotiationReceived != null)
				{
					RemoteSessionNegotiationEventArgs remoteSessionNegotiationEventArgs = new RemoteSessionNegotiationEventArgs(remoteSessionCapability);
					remoteSessionNegotiationEventArgs.RemoteData = receivedData;
					this.NegotiationReceived.SafeInvoke(this, remoteSessionNegotiationEventArgs);
					return;
				}
				return;
			}
			case RemotingDataType.CloseSession:
			{
				PSRemotingDataStructureException reason = new PSRemotingDataStructureException(RemotingErrorIdStrings.ClientRequestedToCloseSession);
				RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close, reason);
				this._stateMachine.RaiseEvent(fsmEventArg);
				return;
			}
			case RemotingDataType.CreateRunspacePool:
				this.CreateRunspacePoolReceived.SafeInvoke(this, dataArg);
				return;
			case RemotingDataType.PublicKey:
			{
				string publicKey = RemotingDecoder.GetPublicKey(receivedData.Data);
				this.PublicKeyReceived.SafeInvoke(this, new RemoteDataEventArgs<string>(publicKey));
				return;
			}
			default:
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ReceivedUnsupportedAction, new object[]
				{
					dataType
				});
			}
		}

		// Token: 0x040011F6 RID: 4598
		private AbstractServerSessionTransportManager _transportManager;

		// Token: 0x040011F7 RID: 4599
		private ServerRemoteSessionDSHandlerStateMachine _stateMachine;

		// Token: 0x040011F8 RID: 4600
		private ServerRemoteSession _session;
	}
}
