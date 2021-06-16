using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Remoting.Server;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003F1 RID: 1009
	internal class WSManPluginServerTransportManager : AbstractServerSessionTransportManager
	{
		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06002D84 RID: 11652 RVA: 0x000FC1C4 File Offset: 0x000FA3C4
		// (remove) Token: 0x06002D85 RID: 11653 RVA: 0x000FC1FC File Offset: 0x000FA3FC
		public event EventHandler<EventArgs> PrepareCalled;

		// Token: 0x06002D86 RID: 11654 RVA: 0x000FC231 File Offset: 0x000FA431
		internal WSManPluginServerTransportManager(int fragmentSize, PSRemotingCryptoHelper cryptoHelper) : base(fragmentSize, cryptoHelper)
		{
			this.syncObject = new object();
			this.activeCmdTransportManagers = new Dictionary<Guid, WSManPluginServerTransportManager>();
			this.waitHandle = new ManualResetEvent(false);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000FC25D File Offset: 0x000FA45D
		internal override void Close(Exception reasonForClose)
		{
			this.DoClose(false, reasonForClose);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000FC268 File Offset: 0x000FA468
		internal void DoClose(bool isShuttingDown, Exception reasonForClose)
		{
			if (this.isClosed)
			{
				return;
			}
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				this.isClosed = true;
				this.lastErrorReported = reasonForClose;
				if (!this.isRequestPending)
				{
					this.waitHandle.Set();
				}
			}
			try
			{
				base.RaiseClosingEvent();
				foreach (KeyValuePair<Guid, WSManPluginServerTransportManager> keyValuePair in this.activeCmdTransportManagers)
				{
					keyValuePair.Value.Close(reasonForClose);
				}
				this.activeCmdTransportManagers.Clear();
				if (this.registeredShutDownWaitHandle != null)
				{
					this.registeredShutDownWaitHandle.Unregister(null);
					this.registeredShutDownWaitHandle = null;
				}
				if (this.shutDownContext != null)
				{
					this.shutDownContext = null;
				}
				if (this.requestDetails != null)
				{
					WSManNativeApi.WSManPluginReceiveResult(this.requestDetails.unmanagedHandle, 1, "stdout", IntPtr.Zero, "http://schemas.microsoft.com/wbem/wsman/1/windows/shell/CommandState/Done", 0);
					WSManPluginInstance.ReportWSManOperationComplete(this.requestDetails, reasonForClose);
					this.requestDetails = null;
				}
			}
			finally
			{
				this.waitHandle.Dispose();
			}
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000FC3B8 File Offset: 0x000FA5B8
		internal override void ReportExecutionStatusAsRunning()
		{
			if (this.isClosed)
			{
				return;
			}
			int num = 0;
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					num = WSManNativeApi.WSManPluginReceiveResult(this.requestDetails.unmanagedHandle, 0, null, IntPtr.Zero, "http://schemas.microsoft.com/wbem/wsman/1/windows/shell/CommandState/Running", 0);
				}
			}
			if (num != 0)
			{
				base.ReportError(num, "WSManPluginReceiveResult");
			}
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000FC434 File Offset: 0x000FA634
		protected override void SendDataToClient(byte[] data, bool flush, bool reportAsPending, bool reportAsDataBoundary)
		{
			if (this.isClosed)
			{
				return;
			}
			if (!this.isRequestPending)
			{
				this.waitHandle.WaitOne();
				this.isRequestPending = true;
				this.waitHandle.Dispose();
			}
			int num = 0;
			using (WSManNativeApi.WSManData_ManToUn wsmanData_ManToUn = new WSManNativeApi.WSManData_ManToUn(data))
			{
				lock (this.syncObject)
				{
					if (!this.isClosed)
					{
						int num2 = 0;
						if (flush)
						{
							num2 |= 2;
						}
						if (reportAsDataBoundary)
						{
							num2 |= 4;
						}
						num = WSManNativeApi.WSManPluginReceiveResult(this.requestDetails.unmanagedHandle, num2, "stdout", wsmanData_ManToUn, reportAsPending ? "http://schemas.microsoft.com/wbem/wsman/1/windows/shell/CommandState/Pending" : null, 0);
					}
				}
			}
			if (num != 0)
			{
				base.ReportError(num, "WSManPluginReceiveResult");
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000FC514 File Offset: 0x000FA714
		internal override void Prepare()
		{
			base.Prepare();
			this.PrepareCalled(this, EventArgs.Empty);
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000FC52D File Offset: 0x000FA72D
		internal override AbstractServerTransportManager GetCommandTransportManager(Guid powerShellCmdId)
		{
			return this.activeCmdTransportManagers[powerShellCmdId];
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000FC53C File Offset: 0x000FA73C
		internal void ReportTransportMgrForCmd(Guid cmdId, WSManPluginServerTransportManager transportManager)
		{
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					if (!this.activeCmdTransportManagers.ContainsKey(cmdId))
					{
						this.activeCmdTransportManagers.Add(cmdId, transportManager);
					}
				}
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000FC59C File Offset: 0x000FA79C
		internal override void RemoveCommandTransportManager(Guid cmdId)
		{
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					if (this.activeCmdTransportManagers.ContainsKey(cmdId))
					{
						this.activeCmdTransportManagers.Remove(cmdId);
					}
				}
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000FC5FC File Offset: 0x000FA7FC
		internal bool EnableTransportManagerSendDataToClient(WSManNativeApi.WSManPluginRequest requestDetails, WSManPluginOperationShutdownContext ctxtToReport)
		{
			this.shutDownContext = ctxtToReport;
			bool flag = true;
			lock (this.syncObject)
			{
				if (this.isRequestPending)
				{
					WSManPluginInstance.ReportWSManOperationComplete(requestDetails, WSManPluginErrorCodes.NoError);
					return false;
				}
				if (this.isClosed)
				{
					WSManPluginInstance.ReportWSManOperationComplete(requestDetails, this.lastErrorReported);
					return false;
				}
				this.isRequestPending = true;
				this.requestDetails = requestDetails;
				SafeWaitHandle value = new SafeWaitHandle(requestDetails.shutdownNotificationHandle, false);
				EventWaitHandle waitObject = new EventWaitHandle(false, EventResetMode.AutoReset);
				ClrFacade.SetSafeWaitHandle(waitObject, value);
				this.registeredShutDownWaitHandle = ThreadPool.RegisterWaitForSingleObject(waitObject, new WaitOrTimerCallback(WSManPluginManagedEntryWrapper.PSPluginOperationShutdownCallback), this.shutDownContext, -1, true);
				if (this.registeredShutDownWaitHandle == null)
				{
					flag = false;
				}
				this.waitHandle.Set();
			}
			if (!flag)
			{
				WSManPluginInstance.PerformCloseOperation(ctxtToReport);
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ShutdownRegistrationFailed, StringUtil.Format(RemotingErrorIdStrings.WSManPluginShutdownRegistrationFailed, new object[0]));
				return false;
			}
			return true;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000FC700 File Offset: 0x000FA900
		internal void PerformStop()
		{
			if (this.isRequestPending)
			{
				base.RaiseClosingEvent();
				return;
			}
			this.DoClose(false, null);
		}

		// Token: 0x040017E4 RID: 6116
		private WSManNativeApi.WSManPluginRequest requestDetails;

		// Token: 0x040017E5 RID: 6117
		private bool isRequestPending;

		// Token: 0x040017E6 RID: 6118
		private object syncObject;

		// Token: 0x040017E7 RID: 6119
		private ManualResetEvent waitHandle;

		// Token: 0x040017E8 RID: 6120
		private Dictionary<Guid, WSManPluginServerTransportManager> activeCmdTransportManagers;

		// Token: 0x040017E9 RID: 6121
		private bool isClosed;

		// Token: 0x040017EA RID: 6122
		private Exception lastErrorReported;

		// Token: 0x040017EB RID: 6123
		private WSManPluginOperationShutdownContext shutDownContext;

		// Token: 0x040017EC RID: 6124
		private RegisteredWaitHandle registeredShutDownWaitHandle;
	}
}
