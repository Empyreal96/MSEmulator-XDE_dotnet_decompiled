using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000297 RID: 663
	internal class ClientRemoteSessionDSHandlerImpl : ClientRemoteSessionDataStructureHandler, IDisposable
	{
		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x000B89F7 File Offset: 0x000B6BF7
		internal override BaseClientSessionTransportManager TransportManager
		{
			get
			{
				return this._transportManager;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000B8A00 File Offset: 0x000B6C00
		internal override BaseClientCommandTransportManager CreateClientCommandTransportManager(ClientRemotePowerShell cmd, bool noInput)
		{
			BaseClientCommandTransportManager baseClientCommandTransportManager = this._transportManager.CreateClientCommandTransportManager(this._connectionInfo, cmd, noInput);
			baseClientCommandTransportManager.DataReceived += this.DispatchInputQueueData;
			return baseClientCommandTransportManager;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000B8A34 File Offset: 0x000B6C34
		internal ClientRemoteSessionDSHandlerImpl(ClientRemoteSession session, PSRemotingCryptoHelper cryptoHelper, RunspaceConnectionInfo connectionInfo, ClientRemoteSession.URIDirectionReported uriRedirectionHandler)
		{
			if (session == null)
			{
				throw PSTraceSource.NewArgumentNullException("session");
			}
			this._session = session;
			this._stateMachine = new ClientRemoteSessionDSHandlerStateMachine();
			this._stateMachine.StateChanged += this.HandleStateChanged;
			this._connectionInfo = connectionInfo;
			this._cryptoHelper = cryptoHelper;
			this._transportManager = this._connectionInfo.CreateClientSessionTransportManager(this._session.RemoteRunspacePoolInternal.InstanceId, this._session.RemoteRunspacePoolInternal.Name, cryptoHelper);
			this._transportManager.DataReceived += this.DispatchInputQueueData;
			this._transportManager.WSManTransportErrorOccured += this.HandleTransportError;
			this._transportManager.CloseCompleted += this.HandleCloseComplete;
			this._transportManager.DisconnectCompleted += this.HandleDisconnectComplete;
			this._transportManager.ReconnectCompleted += this.HandleReconnectComplete;
			this._transportManager.RobustConnectionNotification += this.HandleRobustConnectionNotification;
			WSManConnectionInfo wsmanConnectionInfo = this._connectionInfo as WSManConnectionInfo;
			if (wsmanConnectionInfo != null)
			{
				this.uriRedirectionHandler = uriRedirectionHandler;
				this.maxUriRedirectionCount = wsmanConnectionInfo.MaximumConnectionRedirectionCount;
			}
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000B8B76 File Offset: 0x000B6D76
		internal override void CreateAsync()
		{
			this._transportManager.CreateCompleted += new EventHandler<CreateCompleteEventArgs>(this.HandleCreateComplete);
			this._transportManager.CreateAsync();
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000B8B9A File Offset: 0x000B6D9A
		private void HandleCreateComplete(object sender, EventArgs args)
		{
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000B8B9C File Offset: 0x000B6D9C
		private void HandleConnectComplete(object sender, EventArgs args)
		{
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000B8B9E File Offset: 0x000B6D9E
		internal override void DisconnectAsync()
		{
			this._transportManager.DisconnectAsync();
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000B8BAC File Offset: 0x000B6DAC
		private void HandleDisconnectComplete(object sender, EventArgs args)
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.DisconnectCompleted);
			this.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000B8BD0 File Offset: 0x000B6DD0
		private void HandleRobustConnectionNotification(object sender, ConnectionStatusEventArgs e)
		{
			RemoteSessionStateMachineEventArgs remoteSessionStateMachineEventArgs = null;
			switch (e.Notification)
			{
			case ConnectionStatus.AutoDisconnectStarting:
				remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.RCDisconnectStarted);
				break;
			case ConnectionStatus.AutoDisconnectSucceeded:
				remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.DisconnectCompleted, new RuntimeException(StringUtil.Format(RemotingErrorIdStrings.RCAutoDisconnectingError, this._session.RemoteRunspacePoolInternal.ConnectionInfo.ComputerName)));
				break;
			case ConnectionStatus.InternalErrorAbort:
				remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.FatalError);
				break;
			}
			if (remoteSessionStateMachineEventArgs != null)
			{
				this.StateMachine.RaiseEvent(remoteSessionStateMachineEventArgs, false);
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000B8C4C File Offset: 0x000B6E4C
		internal override void ReconnectAsync()
		{
			this._transportManager.ReconnectAsync();
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000B8C5C File Offset: 0x000B6E5C
		private void HandleReconnectComplete(object sender, EventArgs args)
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.ReconnectCompleted);
			this.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000B8C80 File Offset: 0x000B6E80
		internal override void CloseConnectionAsync()
		{
			lock (this.syncObject)
			{
				if (!this.isCloseCalled)
				{
					this._transportManager.CloseAsync();
					this.isCloseCalled = true;
				}
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000B8CD8 File Offset: 0x000B6ED8
		private void HandleCloseComplete(object sender, EventArgs args)
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.CloseCompleted);
			this._stateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000B8CFC File Offset: 0x000B6EFC
		internal override void SendNegotiationAsync(RemoteSessionState sessionState)
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationSendCompleted);
			this._stateMachine.RaiseEvent(arg, false);
			if (sessionState == RemoteSessionState.NegotiationSending)
			{
				this._transportManager.CreateAsync();
				return;
			}
			if (sessionState == RemoteSessionState.NegotiationSendingOnConnect)
			{
				this._transportManager.ConnectCompleted += this.HandleConnectComplete;
				this._transportManager.ConnectAsync();
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06001FDA RID: 8154 RVA: 0x000B8D54 File Offset: 0x000B6F54
		// (remove) Token: 0x06001FDB RID: 8155 RVA: 0x000B8D8C File Offset: 0x000B6F8C
		internal override event EventHandler<RemoteSessionNegotiationEventArgs> NegotiationReceived;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06001FDC RID: 8156 RVA: 0x000B8DC4 File Offset: 0x000B6FC4
		// (remove) Token: 0x06001FDD RID: 8157 RVA: 0x000B8DFC File Offset: 0x000B6FFC
		internal override event EventHandler<RemoteSessionStateEventArgs> ConnectionStateChanged;

		// Token: 0x06001FDE RID: 8158 RVA: 0x000B8E34 File Offset: 0x000B7034
		private void HandleStateChanged(object sender, RemoteSessionStateEventArgs arg)
		{
			if (arg == null)
			{
				throw PSTraceSource.NewArgumentNullException("arg");
			}
			if (arg.SessionStateInfo.State == RemoteSessionState.NegotiationSending || arg.SessionStateInfo.State == RemoteSessionState.NegotiationSendingOnConnect)
			{
				this.HandleNegotiationSendingStateChange();
			}
			this.ConnectionStateChanged.SafeInvoke(this, arg);
			if (arg.SessionStateInfo.State == RemoteSessionState.NegotiationSending || arg.SessionStateInfo.State == RemoteSessionState.NegotiationSendingOnConnect)
			{
				this.SendNegotiationAsync(arg.SessionStateInfo.State);
			}
			if (arg.SessionStateInfo.State == RemoteSessionState.Established)
			{
				WSManClientSessionTransportManager wsmanClientSessionTransportManager = this._transportManager as WSManClientSessionTransportManager;
				if (wsmanClientSessionTransportManager != null)
				{
					wsmanClientSessionTransportManager.AdjustForProtocolVariations(this._session.ServerProtocolVersion);
					wsmanClientSessionTransportManager.StartReceivingData();
				}
			}
			if (arg.SessionStateInfo.State == RemoteSessionState.ClosingConnection)
			{
				this.CloseConnectionAsync();
			}
			if (arg.SessionStateInfo.State == RemoteSessionState.Disconnecting)
			{
				this.DisconnectAsync();
			}
			if (arg.SessionStateInfo.State == RemoteSessionState.Reconnecting)
			{
				this.ReconnectAsync();
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000B8F20 File Offset: 0x000B7120
		private void HandleNegotiationSendingStateChange()
		{
			RemoteSessionCapability clientCapability = this._session.Context.ClientCapability;
			RemoteDataObject remoteDataObject = RemotingEncoder.GenerateClientSessionCapability(clientCapability, this._session.RemoteRunspacePoolInternal.InstanceId);
			RemoteDataObject<PSObject> data = RemoteDataObject<PSObject>.CreateFrom(remoteDataObject.Destination, remoteDataObject.DataType, remoteDataObject.RunspacePoolId, remoteDataObject.PowerShellId, (PSObject)remoteDataObject.Data);
			this._transportManager.DataToBeSentCollection.Add<PSObject>(data);
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x000B8F8F File Offset: 0x000B718F
		internal override ClientRemoteSessionDSHandlerStateMachine StateMachine
		{
			get
			{
				return this._stateMachine;
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000B8F98 File Offset: 0x000B7198
		private void PerformURIRedirection(string newURIString)
		{
			this._redirectUri = new Uri(newURIString);
			lock (this.syncObject)
			{
				if (!this.isCloseCalled)
				{
					this._transportManager.CloseCompleted -= this.HandleCloseComplete;
					this._transportManager.WSManTransportErrorOccured -= this.HandleTransportError;
					this._transportManager.CloseCompleted += this.HandleTransportCloseCompleteForRedirection;
					this._transportManager.WSManTransportErrorOccured += this.HandleTransportErrorForRedirection;
					this._transportManager.PrepareForRedirection();
				}
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000B9050 File Offset: 0x000B7250
		private void HandleTransportCloseCompleteForRedirection(object source, EventArgs args)
		{
			this._transportManager.CloseCompleted -= this.HandleTransportCloseCompleteForRedirection;
			this._transportManager.WSManTransportErrorOccured -= this.HandleTransportErrorForRedirection;
			this._transportManager.CloseCompleted += this.HandleCloseComplete;
			this._transportManager.WSManTransportErrorOccured += this.HandleTransportError;
			this.PerformURIRedirectionStep2(this._redirectUri);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000B90C8 File Offset: 0x000B72C8
		private void HandleTransportErrorForRedirection(object sender, TransportErrorOccuredEventArgs e)
		{
			this._transportManager.CloseCompleted -= this.HandleTransportCloseCompleteForRedirection;
			this._transportManager.WSManTransportErrorOccured -= this.HandleTransportErrorForRedirection;
			this._transportManager.CloseCompleted += this.HandleCloseComplete;
			this._transportManager.WSManTransportErrorOccured += this.HandleTransportError;
			this.HandleTransportError(sender, e);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000B913C File Offset: 0x000B733C
		private void PerformURIRedirectionStep2(Uri newURI)
		{
			lock (this.syncObject)
			{
				if (!this.isCloseCalled)
				{
					if (this.uriRedirectionHandler != null)
					{
						this.uriRedirectionHandler(newURI);
					}
					this._transportManager.Redirect(newURI, this._connectionInfo);
				}
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000B91A8 File Offset: 0x000B73A8
		internal void HandleTransportError(object sender, TransportErrorOccuredEventArgs e)
		{
			PSRemotingTransportRedirectException ex = e.Exception as PSRemotingTransportRedirectException;
			if (ex != null && this.maxUriRedirectionCount > 0)
			{
				Exception ex2 = null;
				try
				{
					this.maxUriRedirectionCount--;
					this.PerformURIRedirection(ex.RedirectLocation);
					return;
				}
				catch (ArgumentNullException ex3)
				{
					ex2 = ex3;
				}
				catch (UriFormatException ex4)
				{
					ex2 = ex4;
				}
				if (ex2 != null)
				{
					e.Exception = new PSRemotingTransportException(PSRemotingErrorId.RedirectedURINotWellFormatted, RemotingErrorIdStrings.RedirectedURINotWellFormatted, new object[]
					{
						this._session.Context.RemoteAddress.OriginalString,
						ex.RedirectLocation
					})
					{
						TransportMessage = e.Exception.TransportMessage
					};
				}
			}
			RemoteSessionEvent stateEvent = RemoteSessionEvent.ConnectFailed;
			switch (e.ReportingTransportMethod)
			{
			case TransportMethodEnum.CreateShellEx:
				stateEvent = RemoteSessionEvent.ConnectFailed;
				break;
			case TransportMethodEnum.SendShellInputEx:
			case TransportMethodEnum.CommandInputEx:
				stateEvent = RemoteSessionEvent.SendFailed;
				break;
			case TransportMethodEnum.ReceiveShellOutputEx:
			case TransportMethodEnum.ReceiveCommandOutputEx:
				stateEvent = RemoteSessionEvent.ReceiveFailed;
				break;
			case TransportMethodEnum.CloseShellOperationEx:
				stateEvent = RemoteSessionEvent.CloseFailed;
				break;
			case TransportMethodEnum.DisconnectShellEx:
				stateEvent = RemoteSessionEvent.DisconnectFailed;
				break;
			case TransportMethodEnum.ReconnectShellEx:
				stateEvent = RemoteSessionEvent.ReconnectFailed;
				break;
			}
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(stateEvent, e.Exception);
			this._stateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000B92F0 File Offset: 0x000B74F0
		internal void DispatchInputQueueData(object sender, RemoteDataEventArgs dataArg)
		{
			if (dataArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataArg");
			}
			RemoteDataObject<PSObject> receivedData = dataArg.ReceivedData;
			if (receivedData == null)
			{
				throw PSTraceSource.NewArgumentException("dataArg");
			}
			RemotingDestination destination = receivedData.Destination;
			if ((destination & RemotingDestination.Client) != RemotingDestination.Client)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.RemotingDestinationNotForMe, new object[]
				{
					RemotingDestination.Client,
					destination
				});
			}
			switch (receivedData.TargetInterface)
			{
			case RemotingTargetInterface.Session:
				this.ProcessSessionMessages(dataArg);
				return;
			case RemotingTargetInterface.RunspacePool:
			case RemotingTargetInterface.PowerShell:
			{
				RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.MessageReceived, null);
				if (this.StateMachine.CanByPassRaiseEvent(arg))
				{
					this.ProcessNonSessionMessages(dataArg.ReceivedData);
					return;
				}
				this.StateMachine.RaiseEvent(arg, false);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000B93B0 File Offset: 0x000B75B0
		private void ProcessSessionMessages(RemoteDataEventArgs arg)
		{
			if (arg == null || arg.ReceivedData == null)
			{
				throw PSTraceSource.NewArgumentNullException("arg");
			}
			RemoteDataObject<PSObject> receivedData = arg.ReceivedData;
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
					throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ClientNotFoundCapabilityProperties, new object[]
					{
						ex.Message,
						PSVersionInfo.BuildVersion,
						RemotingConstants.ProtocolVersion
					});
				}
				RemoteSessionStateMachineEventArgs remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationReceived);
				remoteSessionStateMachineEventArgs.RemoteSessionCapability = remoteSessionCapability;
				this._stateMachine.RaiseEvent(remoteSessionStateMachineEventArgs, false);
				RemoteSessionNegotiationEventArgs eventArgs = new RemoteSessionNegotiationEventArgs(remoteSessionCapability);
				this.NegotiationReceived.SafeInvoke(this, eventArgs);
				return;
			}
			case RemotingDataType.CloseSession:
			{
				PSRemotingDataStructureException reason = new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerRequestedToCloseSession);
				RemoteSessionStateMachineEventArgs arg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close, reason);
				this._stateMachine.RaiseEvent(arg2, false);
				return;
			}
			case RemotingDataType.EncryptedSessionKey:
			{
				string encryptedSessionKey = RemotingDecoder.GetEncryptedSessionKey(receivedData.Data);
				this.EncryptedSessionKeyReceived.SafeInvoke(this, new RemoteDataEventArgs<string>(encryptedSessionKey));
				return;
			}
			case RemotingDataType.PublicKeyRequest:
				this.PublicKeyRequestReceived.SafeInvoke(this, new RemoteDataEventArgs<string>(string.Empty));
				return;
			}
			throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ReceivedUnsupportedAction, new object[]
			{
				dataType
			});
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000B951C File Offset: 0x000B771C
		internal void ProcessNonSessionMessages(RemoteDataObject<PSObject> rcvdData)
		{
			if (rcvdData == null)
			{
				throw PSTraceSource.NewArgumentNullException("rcvdData");
			}
			switch (rcvdData.TargetInterface)
			{
			case RemotingTargetInterface.Session:
				break;
			case RemotingTargetInterface.RunspacePool:
			{
				Guid runspacePoolId = rcvdData.RunspacePoolId;
				RemoteRunspacePoolInternal runspacePool = this._session.GetRunspacePool(runspacePoolId);
				if (runspacePool != null)
				{
					runspacePool.DataStructureHandler.ProcessReceivedData(rcvdData);
					return;
				}
				ClientRemoteSessionDSHandlerImpl._trace.WriteLine("Client received data for Runspace (id: {0}), \r\n                            but the Runspace cannot be found", new object[]
				{
					runspacePoolId
				});
				return;
			}
			case RemotingTargetInterface.PowerShell:
			{
				Guid runspacePoolId = rcvdData.RunspacePoolId;
				RemoteRunspacePoolInternal runspacePool = this._session.GetRunspacePool(runspacePoolId);
				runspacePool.DataStructureHandler.DispatchMessageToPowerShell(rcvdData);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000B95BD File Offset: 0x000B77BD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000B95CC File Offset: 0x000B77CC
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._transportManager.Dispose();
			}
		}

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06001FEB RID: 8171 RVA: 0x000B95DC File Offset: 0x000B77DC
		// (remove) Token: 0x06001FEC RID: 8172 RVA: 0x000B9614 File Offset: 0x000B7814
		internal override event EventHandler<RemoteDataEventArgs<string>> EncryptedSessionKeyReceived;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06001FED RID: 8173 RVA: 0x000B964C File Offset: 0x000B784C
		// (remove) Token: 0x06001FEE RID: 8174 RVA: 0x000B9684 File Offset: 0x000B7884
		internal override event EventHandler<RemoteDataEventArgs<string>> PublicKeyRequestReceived;

		// Token: 0x06001FEF RID: 8175 RVA: 0x000B96B9 File Offset: 0x000B78B9
		internal override void SendPublicKeyAsync(string localPublicKey)
		{
			this._transportManager.DataToBeSentCollection.Add<object>(RemotingEncoder.GenerateMyPublicKey(this._session.RemoteRunspacePoolInternal.InstanceId, localPublicKey, RemotingDestination.Server));
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000B96E2 File Offset: 0x000B78E2
		internal override void RaiseKeyExchangeMessageReceived(RemoteDataObject<PSObject> receivedData)
		{
			this.ProcessSessionMessages(new RemoteDataEventArgs(receivedData));
		}

		// Token: 0x04000E0F RID: 3599
		private const string resBaseName = "remotingerroridstrings";

		// Token: 0x04000E10 RID: 3600
		[TraceSource("CRSDSHdlerImpl", "ClientRemoteSessionDSHandlerImpl")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("CRSDSHdlerImpl", "ClientRemoteSessionDSHandlerImpl");

		// Token: 0x04000E11 RID: 3601
		private BaseClientSessionTransportManager _transportManager;

		// Token: 0x04000E12 RID: 3602
		private ClientRemoteSessionDSHandlerStateMachine _stateMachine;

		// Token: 0x04000E13 RID: 3603
		private ClientRemoteSession _session;

		// Token: 0x04000E14 RID: 3604
		private RunspaceConnectionInfo _connectionInfo;

		// Token: 0x04000E15 RID: 3605
		private Uri _redirectUri;

		// Token: 0x04000E16 RID: 3606
		private int maxUriRedirectionCount;

		// Token: 0x04000E17 RID: 3607
		private bool isCloseCalled;

		// Token: 0x04000E18 RID: 3608
		private object syncObject = new object();

		// Token: 0x04000E19 RID: 3609
		private PSRemotingCryptoHelper _cryptoHelper;

		// Token: 0x04000E1A RID: 3610
		private ClientRemoteSession.URIDirectionReported uriRedirectionHandler;
	}
}
