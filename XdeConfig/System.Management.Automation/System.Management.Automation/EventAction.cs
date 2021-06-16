using System;

namespace System.Management.Automation
{
	// Token: 0x020000D8 RID: 216
	internal class EventAction
	{
		// Token: 0x06000BFD RID: 3069 RVA: 0x00043F6C File Offset: 0x0004216C
		public EventAction(PSEventSubscriber sender, PSEventArgs args)
		{
			this.sender = sender;
			this.args = args;
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00043F82 File Offset: 0x00042182
		public PSEventSubscriber Sender
		{
			get
			{
				return this.sender;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00043F8A File Offset: 0x0004218A
		public PSEventArgs Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x04000555 RID: 1365
		private PSEventSubscriber sender;

		// Token: 0x04000556 RID: 1366
		private PSEventArgs args;
	}
}
