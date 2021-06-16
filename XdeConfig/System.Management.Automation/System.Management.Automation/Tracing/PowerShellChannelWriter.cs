using System;
using System.Diagnostics.Eventing;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008E0 RID: 2272
	public sealed class PowerShellChannelWriter : BaseChannelWriter
	{
		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600558B RID: 21899 RVA: 0x001C232F File Offset: 0x001C052F
		// (set) Token: 0x0600558C RID: 21900 RVA: 0x001C2337 File Offset: 0x001C0537
		public override PowerShellTraceKeywords Keywords
		{
			get
			{
				return this._keywords;
			}
			set
			{
				this._keywords = value;
			}
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x001C2340 File Offset: 0x001C0540
		internal PowerShellChannelWriter(PowerShellTraceChannel traceChannel, PowerShellTraceKeywords keywords)
		{
			this._traceChannel = traceChannel;
			this._keywords = keywords;
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x001C2356 File Offset: 0x001C0556
		public override void Dispose()
		{
			if (!this.disposed)
			{
				GC.SuppressFinalize(this);
				this.disposed = true;
			}
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x001C2370 File Offset: 0x001C0570
		private bool Trace(PowerShellTraceEvent traceEvent, PowerShellTraceLevel level, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			EventDescriptor eventDescriptor = new EventDescriptor((int)traceEvent, 1, (byte)this._traceChannel, (byte)level, (byte)operationCode, (int)task, (long)this._keywords);
			if (args != null)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] == null)
					{
						args[i] = string.Empty;
					}
				}
			}
			return PowerShellChannelWriter._provider.WriteEvent(ref eventDescriptor, args);
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x001C23CE File Offset: 0x001C05CE
		public override bool TraceError(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.Error, operationCode, task, args);
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x001C23DC File Offset: 0x001C05DC
		public override bool TraceWarning(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.Warning, operationCode, task, args);
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x001C23EA File Offset: 0x001C05EA
		public override bool TraceInformational(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.Informational, operationCode, task, args);
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x001C23F8 File Offset: 0x001C05F8
		public override bool TraceVerbose(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.Verbose, operationCode, task, args);
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x001C2406 File Offset: 0x001C0606
		public override bool TraceDebug(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.Informational, operationCode, task, args);
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x001C2414 File Offset: 0x001C0614
		public override bool TraceLogAlways(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.LogAlways, operationCode, task, args);
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x001C2422 File Offset: 0x001C0622
		public override bool TraceCritical(PowerShellTraceEvent traceEvent, PowerShellTraceOperationCode operationCode, PowerShellTraceTask task, params object[] args)
		{
			return this.Trace(traceEvent, PowerShellTraceLevel.Critical, operationCode, task, args);
		}

		// Token: 0x04002D4B RID: 11595
		private readonly PowerShellTraceChannel _traceChannel;

		// Token: 0x04002D4C RID: 11596
		private static readonly EventProvider _provider = new EventProvider(new Guid("A0C1853B-5C40-4b15-8766-3CF1C58F985A"));

		// Token: 0x04002D4D RID: 11597
		private bool disposed;

		// Token: 0x04002D4E RID: 11598
		private PowerShellTraceKeywords _keywords;
	}
}
