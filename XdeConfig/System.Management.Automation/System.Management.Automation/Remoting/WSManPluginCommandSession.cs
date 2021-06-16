using System;
using System.Management.Automation.Remoting.Client;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003F0 RID: 1008
	internal class WSManPluginCommandSession : WSManPluginServerSession
	{
		// Token: 0x06002D7F RID: 11647 RVA: 0x000FC054 File Offset: 0x000FA254
		internal WSManPluginCommandSession(WSManNativeApi.WSManPluginRequest creationRequestDetails, WSManPluginServerTransportManager trnsprtMgr, ServerRemoteSession remoteSession) : base(creationRequestDetails, trnsprtMgr)
		{
			this.remoteSession = remoteSession;
			this.cmdSyncObject = new object();
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000FC070 File Offset: 0x000FA270
		internal bool ProcessArguments(WSManNativeApi.WSManCommandArgSet arguments)
		{
			if (1 != arguments.argsCount)
			{
				return false;
			}
			byte[] data = Convert.FromBase64String(arguments.args[0]);
			this.transportMgr.ProcessRawData(data, "stdin");
			return true;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000FC0AA File Offset: 0x000FA2AA
		internal void Stop(WSManNativeApi.WSManPluginRequest requestDetails)
		{
			this.transportMgr.PerformStop();
			WSManPluginInstance.ReportWSManOperationComplete(requestDetails, null);
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000FC0C0 File Offset: 0x000FA2C0
		internal override void CloseOperation(WSManPluginOperationShutdownContext context, Exception reasonForClose)
		{
			lock (this.cmdSyncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				if (!context.isReceiveOperation)
				{
					this.isClosed = true;
				}
			}
			bool isReceiveOperation = context.isReceiveOperation;
			bool isShuttingDown = context.isShuttingDown && context.isReceiveOperation && context.commandContext == this.creationRequestDetails.unmanagedHandle;
			bool isShuttingDown2 = context.isShuttingDown && !context.isReceiveOperation && context.commandContext == this.creationRequestDetails.unmanagedHandle;
			base.ReportSendOperationComplete();
			this.transportMgr.DoClose(isShuttingDown, reasonForClose);
			if (!isReceiveOperation)
			{
				base.SafeInvokeSessionClosed(this.creationRequestDetails.unmanagedHandle, EventArgs.Empty);
				WSManPluginInstance.ReportWSManOperationComplete(this.creationRequestDetails, reasonForClose);
				base.Close(isShuttingDown2);
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000FC1B8 File Offset: 0x000FA3B8
		internal override void ExecuteConnect(WSManNativeApi.WSManPluginRequest requestDetails, int flags, WSManNativeApi.WSManData_UnToMan inboundConnectInformation)
		{
			WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NoError);
		}

		// Token: 0x040017E2 RID: 6114
		private ServerRemoteSession remoteSession;

		// Token: 0x040017E3 RID: 6115
		internal object cmdSyncObject;
	}
}
