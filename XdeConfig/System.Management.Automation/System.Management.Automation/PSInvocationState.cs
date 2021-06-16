using System;

namespace System.Management.Automation
{
	// Token: 0x02000230 RID: 560
	public enum PSInvocationState
	{
		// Token: 0x04000AC5 RID: 2757
		NotStarted,
		// Token: 0x04000AC6 RID: 2758
		Running,
		// Token: 0x04000AC7 RID: 2759
		Stopping,
		// Token: 0x04000AC8 RID: 2760
		Stopped,
		// Token: 0x04000AC9 RID: 2761
		Completed,
		// Token: 0x04000ACA RID: 2762
		Failed,
		// Token: 0x04000ACB RID: 2763
		Disconnected
	}
}
