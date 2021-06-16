using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008DB RID: 2267
	public enum PowerShellTraceOperationCode
	{
		// Token: 0x04002D1C RID: 11548
		None,
		// Token: 0x04002D1D RID: 11549
		Open = 10,
		// Token: 0x04002D1E RID: 11550
		Close,
		// Token: 0x04002D1F RID: 11551
		Connect,
		// Token: 0x04002D20 RID: 11552
		Disconnect,
		// Token: 0x04002D21 RID: 11553
		Negotiate,
		// Token: 0x04002D22 RID: 11554
		Create,
		// Token: 0x04002D23 RID: 11555
		Constructor,
		// Token: 0x04002D24 RID: 11556
		Dispose,
		// Token: 0x04002D25 RID: 11557
		EventHandler,
		// Token: 0x04002D26 RID: 11558
		Exception,
		// Token: 0x04002D27 RID: 11559
		Method,
		// Token: 0x04002D28 RID: 11560
		Send,
		// Token: 0x04002D29 RID: 11561
		Receive,
		// Token: 0x04002D2A RID: 11562
		WorkflowLoad,
		// Token: 0x04002D2B RID: 11563
		SerializationSettings,
		// Token: 0x04002D2C RID: 11564
		WinInfo,
		// Token: 0x04002D2D RID: 11565
		WinStart,
		// Token: 0x04002D2E RID: 11566
		WinStop,
		// Token: 0x04002D2F RID: 11567
		WinDCStart,
		// Token: 0x04002D30 RID: 11568
		WinDCStop,
		// Token: 0x04002D31 RID: 11569
		WinExtension,
		// Token: 0x04002D32 RID: 11570
		WinReply,
		// Token: 0x04002D33 RID: 11571
		WinResume,
		// Token: 0x04002D34 RID: 11572
		WinSuspend
	}
}
