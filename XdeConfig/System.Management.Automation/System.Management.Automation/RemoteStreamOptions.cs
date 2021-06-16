using System;

namespace System.Management.Automation
{
	// Token: 0x02000236 RID: 566
	[Flags]
	public enum RemoteStreamOptions
	{
		// Token: 0x04000ADF RID: 2783
		AddInvocationInfoToErrorRecord = 1,
		// Token: 0x04000AE0 RID: 2784
		AddInvocationInfoToWarningRecord = 2,
		// Token: 0x04000AE1 RID: 2785
		AddInvocationInfoToDebugRecord = 4,
		// Token: 0x04000AE2 RID: 2786
		AddInvocationInfoToVerboseRecord = 8,
		// Token: 0x04000AE3 RID: 2787
		AddInvocationInfo = 15
	}
}
