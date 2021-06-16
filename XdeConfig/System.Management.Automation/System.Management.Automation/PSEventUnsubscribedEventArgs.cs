using System;

namespace System.Management.Automation
{
	// Token: 0x020000D5 RID: 213
	public class PSEventUnsubscribedEventArgs : EventArgs
	{
		// Token: 0x06000BEB RID: 3051 RVA: 0x00043E2B File Offset: 0x0004202B
		internal PSEventUnsubscribedEventArgs(PSEventSubscriber eventSubscriber)
		{
			this.eventSubscriber = eventSubscriber;
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00043E3A File Offset: 0x0004203A
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x00043E42 File Offset: 0x00042042
		public PSEventSubscriber EventSubscriber
		{
			get
			{
				return this.eventSubscriber;
			}
			internal set
			{
				this.eventSubscriber = value;
			}
		}

		// Token: 0x04000551 RID: 1361
		private PSEventSubscriber eventSubscriber;
	}
}
