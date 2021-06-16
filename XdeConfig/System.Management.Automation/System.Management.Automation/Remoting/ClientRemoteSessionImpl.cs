using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200026D RID: 621
	internal class ClientRemoteSessionImpl : ClientRemoteSession, IDisposable
	{
		// Token: 0x06001D55 RID: 7509 RVA: 0x000A9888 File Offset: 0x000A7A88
		internal ClientRemoteSessionImpl(RemoteRunspacePoolInternal rsPool, ClientRemoteSession.URIDirectionReported uriRedirectionHandler)
		{
			base.RemoteRunspacePoolInternal = rsPool;
			base.Context.RemoteAddress = WSManConnectionInfo.ExtractPropertyAsWsManConnectionInfo<Uri>(rsPool.ConnectionInfo, "ConnectionUri", null);
			this._cryptoHelper = new PSRemotingCryptoHelperClient();
			this._cryptoHelper.Session = this;
			base.Context.ClientCapability = RemoteSessionCapability.CreateClientCapability();
			base.Context.UserCredential = rsPool.ConnectionInfo.Credential;
			base.Context.ShellName = WSManConnectionInfo.ExtractPropertyAsWsManConnectionInfo<string>(rsPool.ConnectionInfo, "ShellUri", string.Empty);
			this._mySelf = RemotingDestination.Client;
			base.SessionDataStructureHandler = new ClientRemoteSessionDSHandlerImpl(this, this._cryptoHelper, rsPool.ConnectionInfo, uriRedirectionHandler);
			base.BaseSessionDataStructureHandler = base.SessionDataStructureHandler;
			this._waitHandleForConfigurationReceived = new ManualResetEvent(false);
			base.SessionDataStructureHandler.NegotiationReceived += this.HandleNegotiationReceived;
			base.SessionDataStructureHandler.ConnectionStateChanged += this.HandleConnectionStateChanged;
			base.SessionDataStructureHandler.EncryptedSessionKeyReceived += this.HandleEncryptedSessionKeyReceived;
			base.SessionDataStructureHandler.PublicKeyRequestReceived += this.HandlePublicKeyRequestReceived;
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000A99B0 File Offset: 0x000A7BB0
		public override void CreateAsync()
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.CreateSession);
			base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000A99D8 File Offset: 0x000A7BD8
		public override void ConnectAsync()
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.ConnectSession);
			base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000A9A00 File Offset: 0x000A7C00
		public override void CloseAsync()
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close);
			base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000A9A28 File Offset: 0x000A7C28
		public override void DisconnectAsync()
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.DisconnectStart);
			base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000A9A50 File Offset: 0x000A7C50
		public override void ReconnectAsync()
		{
			RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.ReconnectStart);
			base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06001D5B RID: 7515 RVA: 0x000A9A78 File Offset: 0x000A7C78
		// (remove) Token: 0x06001D5C RID: 7516 RVA: 0x000A9AB0 File Offset: 0x000A7CB0
		public override event EventHandler<RemoteSessionStateEventArgs> StateChanged;

		// Token: 0x06001D5D RID: 7517 RVA: 0x000A9AE8 File Offset: 0x000A7CE8
		private void HandleConnectionStateChanged(object sender, RemoteSessionStateEventArgs arg)
		{
			using (ClientRemoteSessionImpl._trace.TraceEventHandlers())
			{
				if (arg == null)
				{
					throw PSTraceSource.NewArgumentNullException("arg");
				}
				if (arg.SessionStateInfo.State == RemoteSessionState.EstablishedAndKeyReceived)
				{
					this.StartKeyExchange();
				}
				if (arg.SessionStateInfo.State == RemoteSessionState.ClosingConnection)
				{
					this.CompleteKeyExchange();
				}
				this.StateChanged.SafeInvoke(this, arg);
			}
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000A9B64 File Offset: 0x000A7D64
		internal override void StartKeyExchange()
		{
			if (base.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.Established || base.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.EstablishedAndKeyRequested)
			{
				string localPublicKey = null;
				bool flag = false;
				Exception reason = null;
				try
				{
					flag = this._cryptoHelper.ExportLocalPublicKey(out localPublicKey);
				}
				catch (PSCryptoException ex)
				{
					flag = false;
					reason = ex;
				}
				RemoteSessionStateMachineEventArgs arg;
				if (!flag)
				{
					this.CompleteKeyExchange();
					arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeySendFailed, reason);
					base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
				}
				arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeySent);
				base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
				base.SessionDataStructureHandler.SendPublicKeyAsync(localPublicKey);
			}
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000A9C14 File Offset: 0x000A7E14
		internal override void CompleteKeyExchange()
		{
			this._cryptoHelper.CompleteKeyExchange();
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000A9C24 File Offset: 0x000A7E24
		private void HandleEncryptedSessionKeyReceived(object sender, RemoteDataEventArgs<string> eventArgs)
		{
			if (base.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.EstablishedAndKeySent)
			{
				string data = eventArgs.Data;
				RemoteSessionStateMachineEventArgs arg;
				if (!this._cryptoHelper.ImportEncryptedSessionKey(data))
				{
					arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyReceiveFailed);
					base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
				}
				this.CompleteKeyExchange();
				arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyReceived);
				base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
			}
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000A9C98 File Offset: 0x000A7E98
		private void HandlePublicKeyRequestReceived(object sender, RemoteDataEventArgs<string> eventArgs)
		{
			if (base.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.Established)
			{
				RemoteSessionStateMachineEventArgs arg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyRequested);
				base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg, false);
				this.StartKeyExchange();
			}
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000A9CDC File Offset: 0x000A7EDC
		private void HandleNegotiationReceived(object sender, RemoteSessionNegotiationEventArgs arg)
		{
			using (ClientRemoteSessionImpl._trace.TraceEventHandlers())
			{
				if (arg == null)
				{
					throw PSTraceSource.NewArgumentNullException("arg");
				}
				if (arg.RemoteSessionCapability == null)
				{
					throw PSTraceSource.NewArgumentException("arg");
				}
				base.Context.ServerCapability = arg.RemoteSessionCapability;
				try
				{
					this.RunClientNegotiationAlgorithm(base.Context.ServerCapability);
					RemoteSessionStateMachineEventArgs arg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationCompleted);
					base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg2, false);
				}
				catch (PSRemotingDataStructureException reason)
				{
					RemoteSessionStateMachineEventArgs arg3 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationFailed, reason);
					base.SessionDataStructureHandler.StateMachine.RaiseEvent(arg3, false);
				}
			}
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000A9D9C File Offset: 0x000A7F9C
		private bool RunClientNegotiationAlgorithm(RemoteSessionCapability serverRemoteSessionCapability)
		{
			Version protocolVersion = serverRemoteSessionCapability.ProtocolVersion;
			this._serverProtocolVersion = protocolVersion;
			Version protocolVersion2 = base.Context.ClientCapability.ProtocolVersion;
			if (!protocolVersion2.Equals(protocolVersion) && (!(protocolVersion2 == RemotingConstants.ProtocolVersionWin7RTM) || !(protocolVersion == RemotingConstants.ProtocolVersionWin7RC)) && (!(protocolVersion2 == RemotingConstants.ProtocolVersionWin8RTM) || (!(protocolVersion == RemotingConstants.ProtocolVersionWin7RC) && !(protocolVersion == RemotingConstants.ProtocolVersionWin7RTM))) && (!(protocolVersion2 == RemotingConstants.ProtocolVersionWin10RTM) || (!(protocolVersion == RemotingConstants.ProtocolVersionWin7RC) && !(protocolVersion == RemotingConstants.ProtocolVersionWin7RTM) && !(protocolVersion == RemotingConstants.ProtocolVersionWin8RTM))))
			{
				PSRemotingDataStructureException ex = new PSRemotingDataStructureException(RemotingErrorIdStrings.ClientNegotiationFailed, new object[]
				{
					"protocolversion",
					protocolVersion,
					PSVersionInfo.BuildVersion,
					RemotingConstants.ProtocolVersion
				});
				throw ex;
			}
			Version psversion = serverRemoteSessionCapability.PSVersion;
			Version psversion2 = base.Context.ClientCapability.PSVersion;
			if (!psversion2.Equals(psversion))
			{
				PSRemotingDataStructureException ex2 = new PSRemotingDataStructureException(RemotingErrorIdStrings.ClientNegotiationFailed, new object[]
				{
					"PSVersion",
					psversion.ToString(),
					PSVersionInfo.BuildVersion,
					RemotingConstants.ProtocolVersion
				});
				throw ex2;
			}
			Version serializationVersion = serverRemoteSessionCapability.SerializationVersion;
			Version serializationVersion2 = base.Context.ClientCapability.SerializationVersion;
			if (!serializationVersion2.Equals(serializationVersion))
			{
				PSRemotingDataStructureException ex3 = new PSRemotingDataStructureException(RemotingErrorIdStrings.ClientNegotiationFailed, new object[]
				{
					"SerializationVersion",
					serializationVersion.ToString(),
					PSVersionInfo.BuildVersion,
					RemotingConstants.ProtocolVersion
				});
				throw ex3;
			}
			return true;
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x000A9F49 File Offset: 0x000A8149
		internal override RemotingDestination MySelf
		{
			get
			{
				return this._mySelf;
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000A9F51 File Offset: 0x000A8151
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000A9F60 File Offset: 0x000A8160
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._waitHandleForConfigurationReceived != null)
				{
					this._waitHandleForConfigurationReceived.Dispose();
					this._waitHandleForConfigurationReceived = null;
				}
				((ClientRemoteSessionDSHandlerImpl)base.SessionDataStructureHandler).Dispose();
				base.SessionDataStructureHandler = null;
				this._cryptoHelper.Dispose();
				this._cryptoHelper = null;
			}
		}

		// Token: 0x04000CFE RID: 3326
		[TraceSource("CRSessionImpl", "ClientRemoteSessionImpl")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("CRSessionImpl", "ClientRemoteSessionImpl");

		// Token: 0x04000CFF RID: 3327
		private PSRemotingCryptoHelperClient _cryptoHelper;

		// Token: 0x04000D01 RID: 3329
		private ManualResetEvent _waitHandleForConfigurationReceived;

		// Token: 0x04000D02 RID: 3330
		private RemotingDestination _mySelf;
	}
}
