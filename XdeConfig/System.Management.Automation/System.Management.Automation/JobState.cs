using System;

namespace System.Management.Automation
{
	// Token: 0x0200026E RID: 622
	public enum JobState
	{
		// Token: 0x04000D04 RID: 3332
		NotStarted,
		// Token: 0x04000D05 RID: 3333
		Running,
		// Token: 0x04000D06 RID: 3334
		Completed,
		// Token: 0x04000D07 RID: 3335
		Failed,
		// Token: 0x04000D08 RID: 3336
		Stopped,
		// Token: 0x04000D09 RID: 3337
		Blocked,
		// Token: 0x04000D0A RID: 3338
		Suspended,
		// Token: 0x04000D0B RID: 3339
		Disconnected,
		// Token: 0x04000D0C RID: 3340
		Suspending,
		// Token: 0x04000D0D RID: 3341
		Stopping,
		// Token: 0x04000D0E RID: 3342
		AtBreakpoint
	}
}
