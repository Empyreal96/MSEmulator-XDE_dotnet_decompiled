using System;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000314 RID: 788
	internal class ServerPowerShellDataStructureHandler
	{
		// Token: 0x06002587 RID: 9607 RVA: 0x000D1E88 File Offset: 0x000D0088
		internal ServerPowerShellDataStructureHandler(Guid instanceId, Guid runspacePoolId, RemoteStreamOptions remoteStreamOptions, AbstractServerTransportManager transportManager, PowerShell localPowerShell)
		{
			this.clientPowerShellId = instanceId;
			this.clientRunspacePoolId = runspacePoolId;
			this.transportManager = transportManager;
			this.streamSerializationOptions = remoteStreamOptions;
			transportManager.Closing += this.HandleTransportClosing;
			if (localPowerShell != null)
			{
				localPowerShell.RunspaceAssigned += this.LocalPowerShell_RunspaceAssigned;
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000D1EE2 File Offset: 0x000D00E2
		private void LocalPowerShell_RunspaceAssigned(object sender, PSEventArgs<Runspace> e)
		{
			this.rsUsedToInvokePowerShell = e.Args;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000D1EF0 File Offset: 0x000D00F0
		internal void Prepare()
		{
			if (this.clientPowerShellId != Guid.Empty)
			{
				this.transportManager.Prepare();
			}
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000D1F10 File Offset: 0x000D0110
		internal void SendStateChangedInformationToClient(PSInvocationStateInfo stateInfo)
		{
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellStateInfo(stateInfo, this.clientPowerShellId, this.clientRunspacePoolId));
			if (this.clientPowerShellId != Guid.Empty)
			{
				this.transportManager.Closing -= this.HandleTransportClosing;
				this.transportManager.Close(null);
			}
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000D1F6A File Offset: 0x000D016A
		internal void SendOutputDataToClient(PSObject data)
		{
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellOutput(data, this.clientPowerShellId, this.clientRunspacePoolId));
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000D1F84 File Offset: 0x000D0184
		internal void SendErrorRecordToClient(ErrorRecord errorRecord)
		{
			errorRecord.SerializeExtendedInfo = ((this.streamSerializationOptions & RemoteStreamOptions.AddInvocationInfoToErrorRecord) != (RemoteStreamOptions)0);
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellError(errorRecord, this.clientRunspacePoolId, this.clientPowerShellId));
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000D1FB2 File Offset: 0x000D01B2
		internal void SendWarningRecordToClient(WarningRecord record)
		{
			record.SerializeExtendedInfo = ((this.streamSerializationOptions & RemoteStreamOptions.AddInvocationInfoToWarningRecord) != (RemoteStreamOptions)0);
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellInformational(record, this.clientRunspacePoolId, this.clientPowerShellId, RemotingDataType.PowerShellWarning));
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000D1FE5 File Offset: 0x000D01E5
		internal void SendDebugRecordToClient(DebugRecord record)
		{
			record.SerializeExtendedInfo = ((this.streamSerializationOptions & RemoteStreamOptions.AddInvocationInfoToDebugRecord) != (RemoteStreamOptions)0);
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellInformational(record, this.clientRunspacePoolId, this.clientPowerShellId, RemotingDataType.PowerShellDebug));
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000D2018 File Offset: 0x000D0218
		internal void SendVerboseRecordToClient(VerboseRecord record)
		{
			record.SerializeExtendedInfo = ((this.streamSerializationOptions & RemoteStreamOptions.AddInvocationInfoToVerboseRecord) != (RemoteStreamOptions)0);
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellInformational(record, this.clientRunspacePoolId, this.clientPowerShellId, RemotingDataType.PowerShellVerbose));
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000D204B File Offset: 0x000D024B
		internal void SendProgressRecordToClient(ProgressRecord record)
		{
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellInformational(record, this.clientRunspacePoolId, this.clientPowerShellId));
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000D2065 File Offset: 0x000D0265
		internal void SendInformationRecordToClient(InformationRecord record)
		{
			this.SendDataAsync(RemotingEncoder.GeneratePowerShellInformational(record, this.clientRunspacePoolId, this.clientPowerShellId));
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000D207F File Offset: 0x000D027F
		internal void ProcessConnect()
		{
			this.OnSessionConnected.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000D2094 File Offset: 0x000D0294
		internal void ProcessReceivedData(RemoteDataObject<PSObject> receivedData)
		{
			if (receivedData == null)
			{
				throw PSTraceSource.NewArgumentNullException("receivedData");
			}
			RemotingDataType dataType = receivedData.DataType;
			switch (dataType)
			{
			case RemotingDataType.PowerShellInput:
				this.InputReceived.SafeInvoke(this, new RemoteDataEventArgs<object>(receivedData.Data));
				return;
			case RemotingDataType.PowerShellInputEnd:
				this.InputEndReceived.SafeInvoke(this, EventArgs.Empty);
				return;
			default:
			{
				if (dataType == RemotingDataType.StopPowerShell)
				{
					this.StopPowerShellReceived.SafeInvoke(this, EventArgs.Empty);
					return;
				}
				if (dataType != RemotingDataType.RemotePowerShellHostResponseData)
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

		// Token: 0x06002594 RID: 9620 RVA: 0x000D2140 File Offset: 0x000D0340
		internal void RaiseRemoveAssociationEvent()
		{
			this.RemoveAssociation.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000D2154 File Offset: 0x000D0354
		internal ServerRemoteHost GetHostAssociatedWithPowerShell(HostInfo powerShellHostInfo, ServerRemoteHost runspaceServerRemoteHost)
		{
			HostInfo hostInfo;
			if (powerShellHostInfo.UseRunspaceHost)
			{
				hostInfo = runspaceServerRemoteHost.HostInfo;
			}
			else
			{
				hostInfo = powerShellHostInfo;
			}
			return new ServerRemoteHost(this.clientRunspacePoolId, this.clientPowerShellId, hostInfo, this.transportManager, runspaceServerRemoteHost.Runspace, runspaceServerRemoteHost as ServerDriverRemoteHost);
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06002596 RID: 9622 RVA: 0x000D2198 File Offset: 0x000D0398
		// (remove) Token: 0x06002597 RID: 9623 RVA: 0x000D21D0 File Offset: 0x000D03D0
		internal event EventHandler RemoveAssociation;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06002598 RID: 9624 RVA: 0x000D2208 File Offset: 0x000D0408
		// (remove) Token: 0x06002599 RID: 9625 RVA: 0x000D2240 File Offset: 0x000D0440
		internal event EventHandler StopPowerShellReceived;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x0600259A RID: 9626 RVA: 0x000D2278 File Offset: 0x000D0478
		// (remove) Token: 0x0600259B RID: 9627 RVA: 0x000D22B0 File Offset: 0x000D04B0
		internal event EventHandler<RemoteDataEventArgs<object>> InputReceived;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x0600259C RID: 9628 RVA: 0x000D22E8 File Offset: 0x000D04E8
		// (remove) Token: 0x0600259D RID: 9629 RVA: 0x000D2320 File Offset: 0x000D0520
		internal event EventHandler InputEndReceived;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x0600259E RID: 9630 RVA: 0x000D2358 File Offset: 0x000D0558
		// (remove) Token: 0x0600259F RID: 9631 RVA: 0x000D2390 File Offset: 0x000D0590
		internal event EventHandler OnSessionConnected;

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x060025A0 RID: 9632 RVA: 0x000D23C8 File Offset: 0x000D05C8
		// (remove) Token: 0x060025A1 RID: 9633 RVA: 0x000D2400 File Offset: 0x000D0600
		internal event EventHandler<RemoteDataEventArgs<RemoteHostResponse>> HostResponseReceived;

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000D2435 File Offset: 0x000D0635
		internal Guid PowerShellId
		{
			get
			{
				return this.clientPowerShellId;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000D243D File Offset: 0x000D063D
		internal Runspace RunspaceUsedToInvokePowerShell
		{
			get
			{
				return this.rsUsedToInvokePowerShell;
			}
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000D2445 File Offset: 0x000D0645
		private void SendDataAsync(RemoteDataObject data)
		{
			this.transportManager.SendDataToClient(data, false, false);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x000D2455 File Offset: 0x000D0655
		private void HandleTransportClosing(object sender, EventArgs args)
		{
			this.StopPowerShellReceived.SafeInvoke(this, args);
		}

		// Token: 0x04001262 RID: 4706
		private AbstractServerTransportManager transportManager;

		// Token: 0x04001263 RID: 4707
		private Guid clientRunspacePoolId;

		// Token: 0x04001264 RID: 4708
		private Guid clientPowerShellId;

		// Token: 0x04001265 RID: 4709
		private RemoteStreamOptions streamSerializationOptions;

		// Token: 0x04001266 RID: 4710
		private Runspace rsUsedToInvokePowerShell;
	}
}
