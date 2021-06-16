using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200035E RID: 862
	internal enum ConnectionStatus
	{
		// Token: 0x04001528 RID: 5416
		NetworkFailureDetected = 1,
		// Token: 0x04001529 RID: 5417
		ConnectionRetryAttempt,
		// Token: 0x0400152A RID: 5418
		ConnectionRetrySucceeded,
		// Token: 0x0400152B RID: 5419
		AutoDisconnectStarting,
		// Token: 0x0400152C RID: 5420
		AutoDisconnectSucceeded,
		// Token: 0x0400152D RID: 5421
		InternalErrorAbort
	}
}
