using System;
using System.Collections.Generic;
using System.Management.Automation.Host;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x020002A5 RID: 677
	internal class ClientRunspacePoolDataStructureHandler : IDisposable
	{
		// Token: 0x0600208E RID: 8334 RVA: 0x000BC9C8 File Offset: 0x000BABC8
		internal ClientRunspacePoolDataStructureHandler(RemoteRunspacePoolInternal clientRunspacePool, TypeTable typeTable)
		{
			this.clientRunspacePoolId = clientRunspacePool.InstanceId;
			this.minRunspaces = clientRunspacePool.GetMinRunspaces();
			this.maxRunspaces = clientRunspacePool.GetMaxRunspaces();
			this.host = clientRunspacePool.Host;
			this.applicationArguments = clientRunspacePool.ApplicationArguments;
			this.remoteSession = this.CreateClientRemoteSession(clientRunspacePool);
			this.transportManager = this.remoteSession.SessionDataStructureHandler.TransportManager;
			this.transportManager.TypeTable = typeTable;
			this.remoteSession.StateChanged += this.HandleClientRemoteSessionStateChanged;
			this._reconnecting = false;
			this.transportManager.RobustConnectionNotification += this.HandleRobustConnectionNotification;
			this.transportManager.CreateCompleted += this.HandleSessionCreateCompleted;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000BCAB3 File Offset: 0x000BACB3
		internal void CreateRunspacePoolAndOpenAsync()
		{
			this.remoteSession.CreateAsync();
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000BCAC0 File Offset: 0x000BACC0
		internal void CloseRunspacePoolAsync()
		{
			this.remoteSession.CloseAsync();
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000BCACD File Offset: 0x000BACCD
		internal void DisconnectPoolAsync()
		{
			this.PrepareForAndStartDisconnect();
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000BCAD5 File Offset: 0x000BACD5
		internal void ReconnectPoolAsync()
		{
			this._reconnecting = true;
			this.PrepareForConnect();
			this.remoteSession.ReconnectAsync();
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000BCAEF File Offset: 0x000BACEF
		internal void ConnectPoolAsync()
		{
			this.PrepareForConnect();
			this.remoteSession.ConnectAsync();
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000BCB04 File Offset: 0x000BAD04
		internal void ProcessReceivedData(RemoteDataObject<PSObject> receivedData)
		{
			if (receivedData.RunspacePoolId != this.clientRunspacePoolId)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.RunspaceIdsDoNotMatch, new object[]
				{
					receivedData.RunspacePoolId,
					this.clientRunspacePoolId
				});
			}
			RemotingDataType dataType = receivedData.DataType;
			switch (dataType)
			{
			case RemotingDataType.RunspacePoolOperationResponse:
				this.SetMaxMinRunspacesResponseRecieved.SafeInvoke(this, new RemoteDataEventArgs<PSObject>(receivedData.Data));
				return;
			case RemotingDataType.RunspacePoolStateInfo:
			{
				RunspacePoolStateInfo runspacePoolStateInfo = RemotingDecoder.GetRunspacePoolStateInfo(receivedData.Data);
				this.StateInfoReceived.SafeInvoke(this, new RemoteDataEventArgs<RunspacePoolStateInfo>(runspacePoolStateInfo));
				this.NotifyAssociatedPowerShells(runspacePoolStateInfo);
				return;
			}
			case RemotingDataType.CreatePowerShell:
			case RemotingDataType.AvailableRunspaces:
			case RemotingDataType.GetCommandMetadata:
				break;
			case RemotingDataType.PSEventArgs:
			{
				PSEventArgs pseventArgs = RemotingDecoder.GetPSEventArgs(receivedData.Data);
				this.PSEventArgsReceived.SafeInvoke(this, new RemoteDataEventArgs<PSEventArgs>(pseventArgs));
				break;
			}
			case RemotingDataType.ApplicationPrivateData:
			{
				PSPrimitiveDictionary applicationPrivateData = RemotingDecoder.GetApplicationPrivateData(receivedData.Data);
				this.ApplicationPrivateDataReceived.SafeInvoke(this, new RemoteDataEventArgs<PSPrimitiveDictionary>(applicationPrivateData));
				return;
			}
			case RemotingDataType.RunspacePoolInitData:
			{
				RunspacePoolInitInfo runspacePoolInitInfo = RemotingDecoder.GetRunspacePoolInitInfo(receivedData.Data);
				this.RSPoolInitInfoReceived.SafeInvoke(this, new RemoteDataEventArgs<RunspacePoolInitInfo>(runspacePoolInitInfo));
				return;
			}
			default:
			{
				if (dataType != RemotingDataType.RemoteHostCallUsingRunspaceHost)
				{
					return;
				}
				RemoteHostCall data = RemoteHostCall.Decode(receivedData.Data);
				this.RemoteHostCallReceived.SafeInvoke(this, new RemoteDataEventArgs<RemoteHostCall>(data));
				return;
			}
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000BCC54 File Offset: 0x000BAE54
		internal ClientPowerShellDataStructureHandler CreatePowerShellDataStructureHandler(ClientRemotePowerShell shell)
		{
			BaseClientCommandTransportManager baseClientCommandTransportManager = this.remoteSession.SessionDataStructureHandler.CreateClientCommandTransportManager(shell, shell.NoInput);
			return new ClientPowerShellDataStructureHandler(baseClientCommandTransportManager, this.clientRunspacePoolId, shell.InstanceId);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000BCC8C File Offset: 0x000BAE8C
		internal void CreatePowerShellOnServerAndInvoke(ClientRemotePowerShell shell)
		{
			lock (this.associationSyncObject)
			{
				this.associatedPowerShellDSHandlers.Add(shell.InstanceId, shell.DataStructureHandler);
			}
			shell.DataStructureHandler.RemoveAssociation += this.HandleRemoveAssociation;
			bool flag2 = shell.Settings != null && shell.Settings.InvokeAndDisconnect;
			if (flag2 && !this.EndpointSupportsDisconnect)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.EndpointDoesNotSupportDisconnect);
			}
			if (this.remoteSession == null)
			{
				throw new ObjectDisposedException("ClientRunspacePoolDataStructureHandler");
			}
			shell.DataStructureHandler.Start(this.remoteSession.SessionDataStructureHandler.StateMachine, flag2);
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000BCD50 File Offset: 0x000BAF50
		internal void AddRemotePowerShellDSHandler(Guid psShellInstanceId, ClientPowerShellDataStructureHandler psDSHandler)
		{
			lock (this.associationSyncObject)
			{
				if (this.associatedPowerShellDSHandlers.ContainsKey(psShellInstanceId))
				{
					this.associatedPowerShellDSHandlers.Remove(psShellInstanceId);
				}
				this.associatedPowerShellDSHandlers.Add(psShellInstanceId, psDSHandler);
			}
			psDSHandler.RemoveAssociation += this.HandleRemoveAssociation;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000BCDC4 File Offset: 0x000BAFC4
		internal void DispatchMessageToPowerShell(RemoteDataObject<PSObject> rcvdData)
		{
			ClientPowerShellDataStructureHandler associatedPowerShellDataStructureHandler = this.GetAssociatedPowerShellDataStructureHandler(rcvdData.PowerShellId);
			if (associatedPowerShellDataStructureHandler != null)
			{
				associatedPowerShellDataStructureHandler.ProcessReceivedData(rcvdData);
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x000BCDE8 File Offset: 0x000BAFE8
		internal void SendHostResponseToServer(RemoteHostResponse hostResponse)
		{
			this.SendDataAsync(hostResponse.Encode(), DataPriorityType.PromptResponse);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000BCDF8 File Offset: 0x000BAFF8
		internal void SendResetRunspaceStateToServer(long callId)
		{
			RemoteDataObject data = RemotingEncoder.GenerateResetRunspaceState(this.clientRunspacePoolId, callId);
			this.SendDataAsync(data);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000BCE1C File Offset: 0x000BB01C
		internal void SendSetMaxRunspacesToServer(int maxRunspaces, long callId)
		{
			RemoteDataObject data = RemotingEncoder.GenerateSetMaxRunspaces(this.clientRunspacePoolId, maxRunspaces, callId);
			this.SendDataAsync(data);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000BCE40 File Offset: 0x000BB040
		internal void SendSetMinRunspacesToServer(int minRunspaces, long callId)
		{
			RemoteDataObject data = RemotingEncoder.GenerateSetMinRunspaces(this.clientRunspacePoolId, minRunspaces, callId);
			this.SendDataAsync(data);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000BCE62 File Offset: 0x000BB062
		internal void SendGetAvailableRunspacesToServer(long callId)
		{
			this.SendDataAsync(RemotingEncoder.GenerateGetAvailableRunspaces(this.clientRunspacePoolId, callId));
		}

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x0600209E RID: 8350 RVA: 0x000BCE78 File Offset: 0x000BB078
		// (remove) Token: 0x0600209F RID: 8351 RVA: 0x000BCEB0 File Offset: 0x000BB0B0
		internal event EventHandler<RemoteDataEventArgs<RemoteHostCall>> RemoteHostCallReceived;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060020A0 RID: 8352 RVA: 0x000BCEE8 File Offset: 0x000BB0E8
		// (remove) Token: 0x060020A1 RID: 8353 RVA: 0x000BCF20 File Offset: 0x000BB120
		internal event EventHandler<RemoteDataEventArgs<RunspacePoolStateInfo>> StateInfoReceived;

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060020A2 RID: 8354 RVA: 0x000BCF58 File Offset: 0x000BB158
		// (remove) Token: 0x060020A3 RID: 8355 RVA: 0x000BCF90 File Offset: 0x000BB190
		internal event EventHandler<RemoteDataEventArgs<RunspacePoolInitInfo>> RSPoolInitInfoReceived;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060020A4 RID: 8356 RVA: 0x000BCFC8 File Offset: 0x000BB1C8
		// (remove) Token: 0x060020A5 RID: 8357 RVA: 0x000BD000 File Offset: 0x000BB200
		internal event EventHandler<RemoteDataEventArgs<PSPrimitiveDictionary>> ApplicationPrivateDataReceived;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060020A6 RID: 8358 RVA: 0x000BD038 File Offset: 0x000BB238
		// (remove) Token: 0x060020A7 RID: 8359 RVA: 0x000BD070 File Offset: 0x000BB270
		internal event EventHandler<RemoteDataEventArgs<PSEventArgs>> PSEventArgsReceived;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x060020A8 RID: 8360 RVA: 0x000BD0A8 File Offset: 0x000BB2A8
		// (remove) Token: 0x060020A9 RID: 8361 RVA: 0x000BD0E0 File Offset: 0x000BB2E0
		internal event EventHandler<RemoteDataEventArgs<Exception>> SessionClosed;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060020AA RID: 8362 RVA: 0x000BD118 File Offset: 0x000BB318
		// (remove) Token: 0x060020AB RID: 8363 RVA: 0x000BD150 File Offset: 0x000BB350
		internal event EventHandler<RemoteDataEventArgs<Exception>> SessionDisconnected;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060020AC RID: 8364 RVA: 0x000BD188 File Offset: 0x000BB388
		// (remove) Token: 0x060020AD RID: 8365 RVA: 0x000BD1C0 File Offset: 0x000BB3C0
		internal event EventHandler<RemoteDataEventArgs<Exception>> SessionReconnected;

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060020AE RID: 8366 RVA: 0x000BD1F8 File Offset: 0x000BB3F8
		// (remove) Token: 0x060020AF RID: 8367 RVA: 0x000BD230 File Offset: 0x000BB430
		internal event EventHandler<RemoteDataEventArgs<Exception>> SessionClosing;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060020B0 RID: 8368 RVA: 0x000BD268 File Offset: 0x000BB468
		// (remove) Token: 0x060020B1 RID: 8369 RVA: 0x000BD2A0 File Offset: 0x000BB4A0
		internal event EventHandler<RemoteDataEventArgs<PSObject>> SetMaxMinRunspacesResponseRecieved;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060020B2 RID: 8370 RVA: 0x000BD2D8 File Offset: 0x000BB4D8
		// (remove) Token: 0x060020B3 RID: 8371 RVA: 0x000BD310 File Offset: 0x000BB510
		internal event EventHandler<RemoteDataEventArgs<Uri>> URIRedirectionReported;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060020B4 RID: 8372 RVA: 0x000BD348 File Offset: 0x000BB548
		// (remove) Token: 0x060020B5 RID: 8373 RVA: 0x000BD380 File Offset: 0x000BB580
		internal event EventHandler<RemoteDataEventArgs<Exception>> SessionRCDisconnecting;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x060020B6 RID: 8374 RVA: 0x000BD3B8 File Offset: 0x000BB5B8
		// (remove) Token: 0x060020B7 RID: 8375 RVA: 0x000BD3F0 File Offset: 0x000BB5F0
		internal event EventHandler<CreateCompleteEventArgs> SessionCreateCompleted;

		// Token: 0x060020B8 RID: 8376 RVA: 0x000BD425 File Offset: 0x000BB625
		private void SendDataAsync(RemoteDataObject data)
		{
			this.transportManager.DataToBeSentCollection.Add<object>(data);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000BD438 File Offset: 0x000BB638
		internal void SendDataAsync<T>(RemoteDataObject<T> data, DataPriorityType priority)
		{
			this.transportManager.DataToBeSentCollection.Add<T>(data, priority);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000BD44C File Offset: 0x000BB64C
		internal void SendDataAsync(PSObject data, DataPriorityType priority)
		{
			RemoteDataObject<PSObject> data2 = RemoteDataObject<PSObject>.CreateFrom(RemotingDestination.Server, RemotingDataType.InvalidDataType, this.clientRunspacePoolId, Guid.Empty, data);
			this.transportManager.DataToBeSentCollection.Add<PSObject>(data2);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000BD480 File Offset: 0x000BB680
		private ClientRemoteSessionImpl CreateClientRemoteSession(RemoteRunspacePoolInternal rsPoolInternal)
		{
			ClientRemoteSession.URIDirectionReported uriRedirectionHandler = new ClientRemoteSession.URIDirectionReported(this.HandleURIDirectionReported);
			return new ClientRemoteSessionImpl(rsPoolInternal, uriRedirectionHandler);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000BD4A4 File Offset: 0x000BB6A4
		private void HandleClientRemoteSessionStateChanged(object sender, RemoteSessionStateEventArgs e)
		{
			if (e.SessionStateInfo.State == RemoteSessionState.NegotiationSending)
			{
				if (this.createRunspaceCalled)
				{
					return;
				}
				lock (this.syncObject)
				{
					if (this.createRunspaceCalled)
					{
						return;
					}
					this.createRunspaceCalled = true;
				}
				PSPrimitiveDictionary psprimitiveDictionary = PSPrimitiveDictionary.CloneAndAddPSVersionTable(this.applicationArguments);
				this.SendDataAsync(RemotingEncoder.GenerateCreateRunspacePool(this.clientRunspacePoolId, this.minRunspaces, this.maxRunspaces, this.remoteSession.RemoteRunspacePoolInternal, this.host, psprimitiveDictionary));
			}
			if (e.SessionStateInfo.State == RemoteSessionState.NegotiationSendingOnConnect)
			{
				this.SendDataAsync(RemotingEncoder.GenerateConnectRunspacePool(this.clientRunspacePoolId, this.minRunspaces, this.maxRunspaces));
				return;
			}
			if (e.SessionStateInfo.State == RemoteSessionState.ClosingConnection)
			{
				Exception reason = this.closingReason;
				if (reason == null)
				{
					reason = e.SessionStateInfo.Reason;
					this.closingReason = reason;
				}
				List<ClientPowerShellDataStructureHandler> list;
				lock (this.associationSyncObject)
				{
					list = new List<ClientPowerShellDataStructureHandler>(this.associatedPowerShellDSHandlers.Values);
				}
				foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler in list)
				{
					clientPowerShellDataStructureHandler.CloseConnectionAsync(this.closingReason);
				}
				this.SessionClosing.SafeInvoke(this, new RemoteDataEventArgs<Exception>(reason));
				return;
			}
			if (e.SessionStateInfo.State == RemoteSessionState.Closed)
			{
				Exception reason2 = this.closingReason;
				if (reason2 == null)
				{
					reason2 = e.SessionStateInfo.Reason;
					this.closingReason = reason2;
				}
				if (reason2 != null)
				{
					this.NotifyAssociatedPowerShells(new RunspacePoolStateInfo(RunspacePoolState.Broken, reason2));
				}
				else
				{
					this.NotifyAssociatedPowerShells(new RunspacePoolStateInfo(RunspacePoolState.Closed, reason2));
				}
				this.SessionClosed.SafeInvoke(this, new RemoteDataEventArgs<Exception>(reason2));
				return;
			}
			if (e.SessionStateInfo.State == RemoteSessionState.Connected)
			{
				PSEtwLog.ReplaceActivityIdForCurrentThread(this.clientRunspacePoolId, PSEventId.OperationalTransferEventRunspacePool, PSEventId.AnalyticTransferEventRunspacePool, PSKeyword.Runspace, PSTask.CreateRunspace);
				return;
			}
			if (e.SessionStateInfo.State == RemoteSessionState.Disconnected)
			{
				this.NotifyAssociatedPowerShells(new RunspacePoolStateInfo(RunspacePoolState.Disconnected, e.SessionStateInfo.Reason));
				this.SessionDisconnected.SafeInvoke(this, new RemoteDataEventArgs<Exception>(e.SessionStateInfo.Reason));
				return;
			}
			if (this._reconnecting && e.SessionStateInfo.State == RemoteSessionState.Established)
			{
				this.SessionReconnected.SafeInvoke(this, new RemoteDataEventArgs<Exception>(null));
				this._reconnecting = false;
				return;
			}
			if (e.SessionStateInfo.State == RemoteSessionState.RCDisconnecting)
			{
				this.SessionRCDisconnecting.SafeInvoke(this, new RemoteDataEventArgs<Exception>(null));
				return;
			}
			if (e.SessionStateInfo.Reason != null)
			{
				this.closingReason = e.SessionStateInfo.Reason;
			}
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000BD778 File Offset: 0x000BB978
		private void HandleURIDirectionReported(Uri newURI)
		{
			this.URIRedirectionReported.SafeInvoke(this, new RemoteDataEventArgs<Uri>(newURI));
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000BD78C File Offset: 0x000BB98C
		private void NotifyAssociatedPowerShells(RunspacePoolStateInfo stateInfo)
		{
			if (stateInfo.State == RunspacePoolState.Disconnected)
			{
				List<ClientPowerShellDataStructureHandler> list;
				lock (this.associationSyncObject)
				{
					list = new List<ClientPowerShellDataStructureHandler>(this.associatedPowerShellDSHandlers.Values);
				}
				foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler in list)
				{
					clientPowerShellDataStructureHandler.ProcessDisconnect(stateInfo);
				}
				return;
			}
			if (stateInfo.State == RunspacePoolState.Broken || stateInfo.State == RunspacePoolState.Closed)
			{
				List<ClientPowerShellDataStructureHandler> list;
				lock (this.associationSyncObject)
				{
					list = new List<ClientPowerShellDataStructureHandler>(this.associatedPowerShellDSHandlers.Values);
					this.associatedPowerShellDSHandlers.Clear();
				}
				if (stateInfo.State == RunspacePoolState.Broken)
				{
					using (List<ClientPowerShellDataStructureHandler>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler2 = enumerator2.Current;
							clientPowerShellDataStructureHandler2.SetStateToFailed(stateInfo.Reason);
						}
						return;
					}
				}
				if (stateInfo.State == RunspacePoolState.Closed)
				{
					foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler3 in list)
					{
						clientPowerShellDataStructureHandler3.SetStateToStopped(stateInfo.Reason);
					}
				}
			}
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000BD91C File Offset: 0x000BBB1C
		private ClientPowerShellDataStructureHandler GetAssociatedPowerShellDataStructureHandler(Guid clientPowerShellId)
		{
			ClientPowerShellDataStructureHandler result = null;
			lock (this.associationSyncObject)
			{
				if (!this.associatedPowerShellDSHandlers.TryGetValue(clientPowerShellId, out result))
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000BD970 File Offset: 0x000BBB70
		private void HandleRemoveAssociation(object sender, EventArgs e)
		{
			ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler = sender as ClientPowerShellDataStructureHandler;
			lock (this.associationSyncObject)
			{
				this.associatedPowerShellDSHandlers.Remove(clientPowerShellDataStructureHandler.PowerShellId);
			}
			this.transportManager.RemoveCommandTransportManager(clientPowerShellDataStructureHandler.PowerShellId);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000BD9D4 File Offset: 0x000BBBD4
		private void PrepareForAndStartDisconnect()
		{
			bool flag2;
			lock (this.associationSyncObject)
			{
				if (this.associatedPowerShellDSHandlers.Count == 0)
				{
					flag2 = true;
					this.preparingForDisconnectList = null;
				}
				else
				{
					flag2 = false;
					this.preparingForDisconnectList = new List<BaseClientCommandTransportManager>();
					foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler in this.associatedPowerShellDSHandlers.Values)
					{
						this.preparingForDisconnectList.Add(clientPowerShellDataStructureHandler.TransportManager);
						clientPowerShellDataStructureHandler.TransportManager.ReadyForDisconnect += this.HandleReadyForDisconnect;
					}
				}
			}
			if (flag2)
			{
				this.StartDisconnectAsync(this.remoteSession);
				return;
			}
			List<ClientPowerShellDataStructureHandler> list;
			lock (this.associationSyncObject)
			{
				list = new List<ClientPowerShellDataStructureHandler>(this.associatedPowerShellDSHandlers.Values);
			}
			foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler2 in list)
			{
				clientPowerShellDataStructureHandler2.TransportManager.PrepareForDisconnect();
			}
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000BDB30 File Offset: 0x000BBD30
		private void PrepareForConnect()
		{
			List<ClientPowerShellDataStructureHandler> list;
			lock (this.associationSyncObject)
			{
				list = new List<ClientPowerShellDataStructureHandler>(this.associatedPowerShellDSHandlers.Values);
			}
			foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler in list)
			{
				clientPowerShellDataStructureHandler.TransportManager.ReadyForDisconnect -= this.HandleReadyForDisconnect;
				clientPowerShellDataStructureHandler.TransportManager.PrepareForConnect();
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000BDBD4 File Offset: 0x000BBDD4
		private void HandleReadyForDisconnect(object sender, EventArgs args)
		{
			if (sender == null)
			{
				return;
			}
			BaseClientCommandTransportManager item = (BaseClientCommandTransportManager)sender;
			lock (this.associationSyncObject)
			{
				if (this.preparingForDisconnectList != null)
				{
					if (this.preparingForDisconnectList.Contains(item))
					{
						this.preparingForDisconnectList.Remove(item);
					}
					if (this.preparingForDisconnectList.Count == 0)
					{
						this.preparingForDisconnectList = null;
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.StartDisconnectAsync), this.remoteSession);
					}
				}
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000BDC6C File Offset: 0x000BBE6C
		private void StartDisconnectAsync(object remoteSession)
		{
			((ClientRemoteSession)remoteSession).DisconnectAsync();
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000BDC7C File Offset: 0x000BBE7C
		private void HandleRobustConnectionNotification(object sender, ConnectionStatusEventArgs e)
		{
			List<ClientPowerShellDataStructureHandler> list;
			lock (this.associationSyncObject)
			{
				list = new List<ClientPowerShellDataStructureHandler>(this.associatedPowerShellDSHandlers.Values);
			}
			foreach (ClientPowerShellDataStructureHandler clientPowerShellDataStructureHandler in list)
			{
				clientPowerShellDataStructureHandler.ProcessRobustConnectionNotification(e);
			}
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000BDD08 File Offset: 0x000BBF08
		private void HandleSessionCreateCompleted(object sender, CreateCompleteEventArgs eventArgs)
		{
			this.SessionCreateCompleted.SafeInvoke(this, eventArgs);
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x000BDD17 File Offset: 0x000BBF17
		internal ClientRemoteSession RemoteSession
		{
			get
			{
				return this.remoteSession;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x000BDD1F File Offset: 0x000BBF1F
		internal BaseClientSessionTransportManager TransportManager
		{
			get
			{
				if (this.remoteSession != null)
				{
					return this.remoteSession.SessionDataStructureHandler.TransportManager;
				}
				return null;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000BDD3B File Offset: 0x000BBF3B
		internal int MaxRetryConnectionTime
		{
			get
			{
				if (this.transportManager != null && this.transportManager is WSManClientSessionTransportManager)
				{
					return ((WSManClientSessionTransportManager)this.transportManager).MaxRetryConnectionTime;
				}
				return 0;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x000BDD64 File Offset: 0x000BBF64
		internal bool EndpointSupportsDisconnect
		{
			get
			{
				WSManClientSessionTransportManager wsmanClientSessionTransportManager = this.transportManager as WSManClientSessionTransportManager;
				return wsmanClientSessionTransportManager != null && wsmanClientSessionTransportManager.SupportsDisconnect;
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000BDD88 File Offset: 0x000BBF88
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000BDD97 File Offset: 0x000BBF97
		public void Dispose(bool disposing)
		{
			if (disposing && this.remoteSession != null)
			{
				((ClientRemoteSessionImpl)this.remoteSession).Dispose();
				this.remoteSession = null;
			}
		}

		// Token: 0x04000E68 RID: 3688
		private bool _reconnecting;

		// Token: 0x04000E76 RID: 3702
		private Guid clientRunspacePoolId;

		// Token: 0x04000E77 RID: 3703
		private ClientRemoteSession remoteSession;

		// Token: 0x04000E78 RID: 3704
		private object syncObject = new object();

		// Token: 0x04000E79 RID: 3705
		private bool createRunspaceCalled;

		// Token: 0x04000E7A RID: 3706
		private Exception closingReason;

		// Token: 0x04000E7B RID: 3707
		private int minRunspaces;

		// Token: 0x04000E7C RID: 3708
		private int maxRunspaces;

		// Token: 0x04000E7D RID: 3709
		private PSHost host;

		// Token: 0x04000E7E RID: 3710
		private PSPrimitiveDictionary applicationArguments;

		// Token: 0x04000E7F RID: 3711
		private Dictionary<Guid, ClientPowerShellDataStructureHandler> associatedPowerShellDSHandlers = new Dictionary<Guid, ClientPowerShellDataStructureHandler>();

		// Token: 0x04000E80 RID: 3712
		private object associationSyncObject = new object();

		// Token: 0x04000E81 RID: 3713
		private BaseClientSessionTransportManager transportManager;

		// Token: 0x04000E82 RID: 3714
		private List<BaseClientCommandTransportManager> preparingForDisconnectList;
	}
}
