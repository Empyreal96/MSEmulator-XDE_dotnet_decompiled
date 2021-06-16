using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008DE RID: 2270
	public abstract class BaseChannelWriter : IDisposable
	{
		// Token: 0x0600557D RID: 21885 RVA: 0x001C22DA File Offset: 0x001C04DA
		public virtual void Dispose()
		{
			if (!this.disposed)
			{
				GC.SuppressFinalize(this);
				this.disposed = true;
			}
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x001C22F1 File Offset: 0x001C04F1
		public virtual bool TraceError(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x001C22F4 File Offset: 0x001C04F4
		public virtual bool TraceWarning(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x001C22F7 File Offset: 0x001C04F7
		public virtual bool TraceInformational(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x001C22FA File Offset: 0x001C04FA
		public virtual bool TraceVerbose(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x001C22FD File Offset: 0x001C04FD
		public virtual bool TraceDebug(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x001C2300 File Offset: 0x001C0500
		public virtual bool TraceLogAlways(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x001C2303 File Offset: 0x001C0503
		public virtual bool TraceCritical(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return true;
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x06005585 RID: 21893 RVA: 0x001C2306 File Offset: 0x001C0506
		// (set) Token: 0x06005586 RID: 21894 RVA: 0x001C230A File Offset: 0x001C050A
		public virtual PowerShellTraceKeywords Keywords
		{
			get
			{
				return PowerShellTraceKeywords.None;
			}
			set
			{
			}
		}

		// Token: 0x04002D49 RID: 11593
		private bool disposed;
	}
}
