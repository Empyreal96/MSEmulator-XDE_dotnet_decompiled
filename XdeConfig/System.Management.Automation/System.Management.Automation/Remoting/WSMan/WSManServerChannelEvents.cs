using System;

namespace System.Management.Automation.Remoting.WSMan
{
	// Token: 0x02000315 RID: 789
	public static class WSManServerChannelEvents
	{
		// Token: 0x14000082 RID: 130
		// (add) Token: 0x060025A6 RID: 9638 RVA: 0x000D2464 File Offset: 0x000D0664
		// (remove) Token: 0x060025A7 RID: 9639 RVA: 0x000D2498 File Offset: 0x000D0698
		public static event EventHandler ShuttingDown;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x060025A8 RID: 9640 RVA: 0x000D24CC File Offset: 0x000D06CC
		// (remove) Token: 0x060025A9 RID: 9641 RVA: 0x000D2500 File Offset: 0x000D0700
		public static event EventHandler<ActiveSessionsChangedEventArgs> ActiveSessionsChanged;

		// Token: 0x060025AA RID: 9642 RVA: 0x000D2534 File Offset: 0x000D0734
		internal static void RaiseShuttingDownEvent()
		{
			EventHandler shuttingDown = WSManServerChannelEvents.ShuttingDown;
			if (shuttingDown != null)
			{
				shuttingDown(null, EventArgs.Empty);
			}
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000D2558 File Offset: 0x000D0758
		internal static void RaiseActiveSessionsChangedEvent(ActiveSessionsChangedEventArgs eventArgs)
		{
			EventHandler<ActiveSessionsChangedEventArgs> activeSessionsChanged = WSManServerChannelEvents.ActiveSessionsChanged;
			if (activeSessionsChanged != null)
			{
				activeSessionsChanged(null, eventArgs);
			}
		}
	}
}
