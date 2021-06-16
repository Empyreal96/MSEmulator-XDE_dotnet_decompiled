using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002EC RID: 748
	internal sealed class ListenerEndedEventArgs : EventArgs
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x000C79D8 File Offset: 0x000C5BD8
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x000C79CF File Offset: 0x000C5BCF
		public Exception Reason { get; private set; }

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x000C79E9 File Offset: 0x000C5BE9
		// (set) Token: 0x06002393 RID: 9107 RVA: 0x000C79E0 File Offset: 0x000C5BE0
		public bool RestartListener { get; private set; }

		// Token: 0x06002395 RID: 9109 RVA: 0x000C79F1 File Offset: 0x000C5BF1
		private ListenerEndedEventArgs()
		{
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x000C79F9 File Offset: 0x000C5BF9
		public ListenerEndedEventArgs(Exception reason, bool restartListener)
		{
			this.Reason = reason;
			this.RestartListener = restartListener;
		}
	}
}
