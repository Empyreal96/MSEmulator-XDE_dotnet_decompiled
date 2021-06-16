using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Tracing;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003EE RID: 1006
	internal abstract class WSManPluginServerSession : IDisposable
	{
		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06002D62 RID: 11618 RVA: 0x000FB5D8 File Offset: 0x000F97D8
		// (remove) Token: 0x06002D63 RID: 11619 RVA: 0x000FB610 File Offset: 0x000F9810
		internal event EventHandler<EventArgs> SessionClosed;

		// Token: 0x06002D64 RID: 11620 RVA: 0x000FB648 File Offset: 0x000F9848
		protected WSManPluginServerSession(WSManNativeApi.WSManPluginRequest creationRequestDetails, WSManPluginServerTransportManager trnsprtMgr)
		{
			this.syncObject = new object();
			this.creationRequestDetails = creationRequestDetails;
			this.transportMgr = trnsprtMgr;
			this.transportMgr.PrepareCalled += this.HandlePrepareFromTransportManager;
			this.transportMgr.WSManTransportErrorOccured += this.HandleTransportError;
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000FB6A2 File Offset: 0x000F98A2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000FB6B1 File Offset: 0x000F98B1
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.Close(false);
				this.disposed = true;
			}
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000FB6CC File Offset: 0x000F98CC
		~WSManPluginServerSession()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000FB6FC File Offset: 0x000F98FC
		internal void SendOneItemToSession(WSManNativeApi.WSManPluginRequest requestDetails, int flags, string stream, WSManNativeApi.WSManData_UnToMan inboundData)
		{
			if (!string.Equals(stream, "stdin", StringComparison.Ordinal) && !string.Equals(stream, "pr", StringComparison.Ordinal))
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidInputStream, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidInputStream, "stdin"));
				return;
			}
			if (inboundData == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NoError);
				return;
			}
			if (2U != inboundData.Type)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidInputDatatype, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidInputStream, "WSMAN_DATA_TYPE_BINARY"));
				return;
			}
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					WSManPluginInstance.ReportWSManOperationComplete(requestDetails, this.lastErrorReported);
					return;
				}
				this.sendRequestDetails = requestDetails;
			}
			this.SendOneItemToSessionHelper(inboundData.Data, stream);
			this.ReportSendOperationComplete();
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000FB7D0 File Offset: 0x000F99D0
		internal void SendOneItemToSessionHelper(byte[] data, string stream)
		{
			this.transportMgr.ProcessRawData(data, stream);
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000FB7E0 File Offset: 0x000F99E0
		internal bool EnableSessionToSendDataToClient(WSManNativeApi.WSManPluginRequest requestDetails, int flags, WSManNativeApi.WSManStreamIDSet_UnToMan streamSet, WSManPluginOperationShutdownContext ctxtToReport)
		{
			if (this.isClosed)
			{
				WSManPluginInstance.ReportWSManOperationComplete(requestDetails, this.lastErrorReported);
				return false;
			}
			if (streamSet == null || 1 != streamSet.streamIDsCount)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidOutputStream, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidOutputStream, "stdout"));
				return false;
			}
			if (!string.Equals(streamSet.streamIDs[0], "stdout", StringComparison.Ordinal))
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidOutputStream, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidOutputStream, "stdout"));
				return false;
			}
			return this.transportMgr.EnableTransportManagerSendDataToClient(requestDetails, ctxtToReport);
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000FB86C File Offset: 0x000F9A6C
		internal void ReportContext()
		{
			int num = 0;
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				if (!this.isContextReported)
				{
					this.isContextReported = true;
					PSEtwLog.LogAnalyticInformational(PSEventId.ReportContext, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
					{
						this.creationRequestDetails.ToString(),
						this.creationRequestDetails.ToString()
					});
					num = WSManNativeApi.WSManPluginReportContext(this.creationRequestDetails.unmanagedHandle, 0, this.creationRequestDetails.unmanagedHandle);
					if (num == 0)
					{
						this.registeredShutdownNotification = 1;
						SafeWaitHandle value = new SafeWaitHandle(this.creationRequestDetails.shutdownNotificationHandle, false);
						EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
						ClrFacade.SetSafeWaitHandle(eventWaitHandle, value);
						this.registeredShutDownWaitHandle = ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, new WaitOrTimerCallback(WSManPluginManagedEntryWrapper.PSPluginOperationShutdownCallback), this.shutDownContext, -1, true);
						if (this.registeredShutDownWaitHandle == null)
						{
							flag = true;
							this.registeredShutdownNotification = 0;
						}
					}
				}
			}
			if (num != 0 || flag)
			{
				string message;
				if (flag)
				{
					message = StringUtil.Format(RemotingErrorIdStrings.WSManPluginShutdownRegistrationFailed, new object[0]);
				}
				else
				{
					message = StringUtil.Format(RemotingErrorIdStrings.WSManPluginReportContextFailed, new object[0]);
				}
				Exception reasonForClose = new InvalidOperationException(message);
				this.Close(reasonForClose);
			}
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000FB9C4 File Offset: 0x000F9BC4
		protected internal void SafeInvokeSessionClosed(object sender, EventArgs eventArgs)
		{
			this.SessionClosed.SafeInvoke(sender, eventArgs);
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000FB9D4 File Offset: 0x000F9BD4
		internal void HandleTransportError(object sender, TransportErrorOccuredEventArgs eventArgs)
		{
			Exception reasonForClose = null;
			if (eventArgs != null)
			{
				reasonForClose = eventArgs.Exception;
			}
			this.Close(reasonForClose);
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000FB9F4 File Offset: 0x000F9BF4
		internal void HandlePrepareFromTransportManager(object sender, EventArgs eventArgs)
		{
			this.ReportContext();
			this.ReportSendOperationComplete();
			this.transportMgr.PrepareCalled -= this.HandlePrepareFromTransportManager;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000FBA1C File Offset: 0x000F9C1C
		internal void Close(bool isShuttingDown)
		{
			if (Interlocked.Exchange(ref this.registeredShutdownNotification, 0) == 1 && this.registeredShutDownWaitHandle != null)
			{
				this.registeredShutDownWaitHandle.Unregister(null);
				this.registeredShutDownWaitHandle = null;
			}
			if (this.shutDownContext != null)
			{
				this.shutDownContext = null;
			}
			this.transportMgr.WSManTransportErrorOccured -= this.HandleTransportError;
			this.creationRequestDetails = null;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000FBA84 File Offset: 0x000F9C84
		internal void Close(Exception reasonForClose)
		{
			this.lastErrorReported = reasonForClose;
			WSManPluginOperationShutdownContext context = new WSManPluginOperationShutdownContext(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, false);
			this.CloseOperation(context, reasonForClose);
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000FBAB8 File Offset: 0x000F9CB8
		internal void ReportSendOperationComplete()
		{
			lock (this.syncObject)
			{
				if (this.sendRequestDetails != null)
				{
					WSManPluginInstance.ReportWSManOperationComplete(this.sendRequestDetails, this.lastErrorReported);
					this.sendRequestDetails = null;
				}
			}
		}

		// Token: 0x06002D72 RID: 11634
		internal abstract void CloseOperation(WSManPluginOperationShutdownContext context, Exception reasonForClose);

		// Token: 0x06002D73 RID: 11635
		internal abstract void ExecuteConnect(WSManNativeApi.WSManPluginRequest requestDetails, int flags, WSManNativeApi.WSManData_UnToMan inboundConnectInformation);

		// Token: 0x040017D3 RID: 6099
		private object syncObject;

		// Token: 0x040017D4 RID: 6100
		protected bool isClosed;

		// Token: 0x040017D5 RID: 6101
		protected bool isContextReported;

		// Token: 0x040017D6 RID: 6102
		protected Exception lastErrorReported;

		// Token: 0x040017D7 RID: 6103
		internal WSManNativeApi.WSManPluginRequest creationRequestDetails;

		// Token: 0x040017D8 RID: 6104
		internal WSManNativeApi.WSManPluginRequest sendRequestDetails;

		// Token: 0x040017D9 RID: 6105
		internal WSManPluginOperationShutdownContext shutDownContext;

		// Token: 0x040017DA RID: 6106
		internal RegisteredWaitHandle registeredShutDownWaitHandle;

		// Token: 0x040017DB RID: 6107
		internal WSManPluginServerTransportManager transportMgr;

		// Token: 0x040017DC RID: 6108
		internal int registeredShutdownNotification;

		// Token: 0x040017DE RID: 6110
		private bool disposed;
	}
}
