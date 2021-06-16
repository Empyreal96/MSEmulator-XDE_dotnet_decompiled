using System;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x020002A3 RID: 675
	internal enum PSConnectionRetryStatus
	{
		// Token: 0x04000E57 RID: 3671
		None,
		// Token: 0x04000E58 RID: 3672
		NetworkFailureDetected,
		// Token: 0x04000E59 RID: 3673
		ConnectionRetryAttempt,
		// Token: 0x04000E5A RID: 3674
		ConnectionRetrySucceeded,
		// Token: 0x04000E5B RID: 3675
		AutoDisconnectStarting,
		// Token: 0x04000E5C RID: 3676
		AutoDisconnectSucceeded,
		// Token: 0x04000E5D RID: 3677
		InternalErrorAbort
	}
}
