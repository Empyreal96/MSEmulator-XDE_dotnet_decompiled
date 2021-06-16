using System;
using System.Collections.Generic;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000313 RID: 787
	internal class ServerRunspacePoolDataStructureHandler
	{
		// Token: 0x0600256A RID: 9578 RVA: 0x000D16A0 File Offset: 0x000CF8A0
		internal ServerRunspacePoolDataStructureHandler(ServerRunspacePoolDriver driver, AbstractServerSessionTransportManager transportManager)
		{
			this.clientRunspacePoolId = driver.InstanceId;
			this.transportManager = transportManager;
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000D16D4 File Offset: 0x000CF8D4
		internal void SendApplicationPrivateDataToClient(PSPrimitiveDictionary applicationPrivateData, RemoteSessionCapability serverCapability)
		{
			PSPrimitiveDictionary psprimitiveDictionary = PSPrimitiveDictionary.CloneAndAddPSVersionTable(applicationPrivateData);
			PSPrimitiveDictionary psprimitiveDictionary2 = (PSPrimitiveDictionary)psprimitiveDictionary["PSVersionTable"];
			psprimitiveDictionary2["PSRemotingProtocolVersion"] = serverCapability.ProtocolVersion;
			psprimitiveDictionary2["SerializationVersion"] = serverCapability.SerializationVersion;
			psprimitiveDictionary2["PSVersion"] = PSVersionInfo.PSVersion;
			RemoteDataObject data = RemotingEncoder.GenerateApplicationPrivateData(this.clientRunspacePoolId, psprimitiveDictionary);
			this.SendDataAsync(data);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000D1740 File Offset: 0x000CF940
		internal void SendStateInfoToClient(RunspacePoolStateInfo stateInfo)
		{
			RemoteDataObject data = RemotingEncoder.GenerateRunspacePoolStateInfo(this.clientRunspacePoolId, stateInfo);
			this.SendDataAsync(data);
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000D1764 File Offset: 0x000CF964
		internal void SendPSEventArgsToClient(PSEventArgs e)
		{
			RemoteDataObject data = RemotingEncoder.GeneratePSEventArgs(this.clientRunspacePoolId, e);
			this.SendDataAsync(data);
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000D1788 File Offset: 0x000CF988
		internal void ProcessConnect()
		{
			List<ServerPowerShellDataStructureHandler> list;
			lock (this.associationSyncObject)
			{
				list = new List<ServerPowerShellDataStructureHandler>(this.associatedShells.Values);
			}
			foreach (ServerPowerShellDataStructureHandler serverPowerShellDataStructureHandler in list)
			{
				serverPowerShellDataStructureHandler.ProcessConnect();
			}
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000D1810 File Offset: 0x000CFA10
		internal void ProcessReceivedData(RemoteDataObject<PSObject> receivedData)
		{
			if (receivedData == null)
			{
				throw PSTraceSource.NewArgumentNullException("receivedData");
			}
			RemotingDataType dataType = receivedData.DataType;
			switch (dataType)
			{
			case RemotingDataType.SetMaxRunspaces:
				this.SetMaxRunspacesReceived.SafeInvoke(this, new RemoteDataEventArgs<PSObject>(receivedData.Data));
				return;
			case RemotingDataType.SetMinRunspaces:
				this.SetMinRunspacesReceived.SafeInvoke(this, new RemoteDataEventArgs<PSObject>(receivedData.Data));
				return;
			case RemotingDataType.RunspacePoolOperationResponse:
			case RemotingDataType.RunspacePoolStateInfo:
			case RemotingDataType.PSEventArgs:
			case RemotingDataType.ApplicationPrivateData:
			case RemotingDataType.RunspacePoolInitData:
				break;
			case RemotingDataType.CreatePowerShell:
				this.CreateAndInvokePowerShell.SafeInvoke(this, new RemoteDataEventArgs<RemoteDataObject<PSObject>>(receivedData));
				return;
			case RemotingDataType.AvailableRunspaces:
				this.GetAvailableRunspacesReceived.SafeInvoke(this, new RemoteDataEventArgs<PSObject>(receivedData.Data));
				return;
			case RemotingDataType.GetCommandMetadata:
				this.GetCommandMetadata.SafeInvoke(this, new RemoteDataEventArgs<RemoteDataObject<PSObject>>(receivedData));
				return;
			case RemotingDataType.ResetRunspaceState:
				this.ResetRunspaceState.SafeInvoke(this, new RemoteDataEventArgs<PSObject>(receivedData.Data));
				break;
			default:
			{
				if (dataType != RemotingDataType.RemoteRunspaceHostResponseData)
				{
					return;
				}
				RemoteHostResponse data = RemoteHostResponse.Decode(receivedData.Data);
				this.transportManager.ReportExecutionStatusAsRunning();
				this.HostResponseReceived.SafeInvoke(this, new RemoteDataEventArgs<RemoteHostResponse>(data));
				return;
			}
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000D1924 File Offset: 0x000CFB24
		internal ServerPowerShellDataStructureHandler CreatePowerShellDataStructureHandler(Guid instanceId, Guid runspacePoolId, RemoteStreamOptions remoteStreamOptions, PowerShell localPowerShell)
		{
			AbstractServerTransportManager commandTransportManager = this.transportManager;
			if (instanceId != Guid.Empty)
			{
				commandTransportManager = this.transportManager.GetCommandTransportManager(instanceId);
			}
			ServerPowerShellDataStructureHandler serverPowerShellDataStructureHandler = new ServerPowerShellDataStructureHandler(instanceId, runspacePoolId, remoteStreamOptions, commandTransportManager, localPowerShell);
			lock (this.associationSyncObject)
			{
				this.associatedShells.Add(serverPowerShellDataStructureHandler.PowerShellId, serverPowerShellDataStructureHandler);
			}
			serverPowerShellDataStructureHandler.RemoveAssociation += this.HandleRemoveAssociation;
			return serverPowerShellDataStructureHandler;
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000D19B0 File Offset: 0x000CFBB0
		internal ServerPowerShellDataStructureHandler GetPowerShellDataStructureHandler()
		{
			lock (this.associationSyncObject)
			{
				if (this.associatedShells.Count > 0)
				{
					foreach (object obj2 in this.associatedShells.Values)
					{
						ServerPowerShellDataStructureHandler serverPowerShellDataStructureHandler = obj2 as ServerPowerShellDataStructureHandler;
						if (serverPowerShellDataStructureHandler != null)
						{
							return serverPowerShellDataStructureHandler;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000D1A4C File Offset: 0x000CFC4C
		internal void DispatchMessageToPowerShell(RemoteDataObject<PSObject> rcvdData)
		{
			ServerPowerShellDataStructureHandler associatedPowerShellDataStructureHandler = this.GetAssociatedPowerShellDataStructureHandler(rcvdData.PowerShellId);
			if (associatedPowerShellDataStructureHandler != null)
			{
				associatedPowerShellDataStructureHandler.ProcessReceivedData(rcvdData);
			}
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000D1A70 File Offset: 0x000CFC70
		internal void SendResponseToClient(long callId, object response)
		{
			RemoteDataObject data = RemotingEncoder.GenerateRunspacePoolOperationResponse(this.clientRunspacePoolId, response, callId);
			this.SendDataAsync(data);
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x000D1A92 File Offset: 0x000CFC92
		// (set) Token: 0x06002575 RID: 9589 RVA: 0x000D1A9F File Offset: 0x000CFC9F
		internal TypeTable TypeTable
		{
			get
			{
				return this.transportManager.TypeTable;
			}
			set
			{
				this.transportManager.TypeTable = value;
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06002576 RID: 9590 RVA: 0x000D1AB0 File Offset: 0x000CFCB0
		// (remove) Token: 0x06002577 RID: 9591 RVA: 0x000D1AE8 File Offset: 0x000CFCE8
		internal event EventHandler<RemoteDataEventArgs<RemoteDataObject<PSObject>>> CreateAndInvokePowerShell;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002578 RID: 9592 RVA: 0x000D1B20 File Offset: 0x000CFD20
		// (remove) Token: 0x06002579 RID: 9593 RVA: 0x000D1B58 File Offset: 0x000CFD58
		internal event EventHandler<RemoteDataEventArgs<RemoteDataObject<PSObject>>> GetCommandMetadata;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x0600257A RID: 9594 RVA: 0x000D1B90 File Offset: 0x000CFD90
		// (remove) Token: 0x0600257B RID: 9595 RVA: 0x000D1BC8 File Offset: 0x000CFDC8
		internal event EventHandler<RemoteDataEventArgs<RemoteHostResponse>> HostResponseReceived;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x0600257C RID: 9596 RVA: 0x000D1C00 File Offset: 0x000CFE00
		// (remove) Token: 0x0600257D RID: 9597 RVA: 0x000D1C38 File Offset: 0x000CFE38
		internal event EventHandler<RemoteDataEventArgs<PSObject>> SetMaxRunspacesReceived;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x0600257E RID: 9598 RVA: 0x000D1C70 File Offset: 0x000CFE70
		// (remove) Token: 0x0600257F RID: 9599 RVA: 0x000D1CA8 File Offset: 0x000CFEA8
		internal event EventHandler<RemoteDataEventArgs<PSObject>> SetMinRunspacesReceived;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06002580 RID: 9600 RVA: 0x000D1CE0 File Offset: 0x000CFEE0
		// (remove) Token: 0x06002581 RID: 9601 RVA: 0x000D1D18 File Offset: 0x000CFF18
		internal event EventHandler<RemoteDataEventArgs<PSObject>> GetAvailableRunspacesReceived;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06002582 RID: 9602 RVA: 0x000D1D50 File Offset: 0x000CFF50
		// (remove) Token: 0x06002583 RID: 9603 RVA: 0x000D1D88 File Offset: 0x000CFF88
		internal event EventHandler<RemoteDataEventArgs<PSObject>> ResetRunspaceState;

		// Token: 0x06002584 RID: 9604 RVA: 0x000D1DBD File Offset: 0x000CFFBD
		private void SendDataAsync(RemoteDataObject data)
		{
			this.transportManager.SendDataToClient(data, true, false);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000D1DD0 File Offset: 0x000CFFD0
		internal ServerPowerShellDataStructureHandler GetAssociatedPowerShellDataStructureHandler(Guid clientPowerShellId)
		{
			ServerPowerShellDataStructureHandler result = null;
			lock (this.associationSyncObject)
			{
				if (!this.associatedShells.TryGetValue(clientPowerShellId, out result))
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000D1E24 File Offset: 0x000D0024
		private void HandleRemoveAssociation(object sender, EventArgs e)
		{
			ServerPowerShellDataStructureHandler serverPowerShellDataStructureHandler = sender as ServerPowerShellDataStructureHandler;
			lock (this.associationSyncObject)
			{
				this.associatedShells.Remove(serverPowerShellDataStructureHandler.PowerShellId);
			}
			this.transportManager.RemoveCommandTransportManager(serverPowerShellDataStructureHandler.PowerShellId);
		}

		// Token: 0x0400125E RID: 4702
		private Guid clientRunspacePoolId;

		// Token: 0x0400125F RID: 4703
		private AbstractServerSessionTransportManager transportManager;

		// Token: 0x04001260 RID: 4704
		private Dictionary<Guid, ServerPowerShellDataStructureHandler> associatedShells = new Dictionary<Guid, ServerPowerShellDataStructureHandler>();

		// Token: 0x04001261 RID: 4705
		private object associationSyncObject = new object();
	}
}
