using System;

namespace System.Management.Automation.Remoting.WSMan
{
	// Token: 0x02000316 RID: 790
	public sealed class ActiveSessionsChangedEventArgs : EventArgs
	{
		// Token: 0x060025AC RID: 9644 RVA: 0x000D2576 File Offset: 0x000D0776
		public ActiveSessionsChangedEventArgs(int activeSessionsCount)
		{
			this.ActiveSessionsCount = activeSessionsCount;
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x000D2585 File Offset: 0x000D0785
		// (set) Token: 0x060025AE RID: 9646 RVA: 0x000D258D File Offset: 0x000D078D
		public int ActiveSessionsCount { get; internal set; }
	}
}
