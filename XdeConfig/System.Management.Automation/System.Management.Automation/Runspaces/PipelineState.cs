using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200022C RID: 556
	public enum PipelineState
	{
		// Token: 0x04000AB9 RID: 2745
		NotStarted,
		// Token: 0x04000ABA RID: 2746
		Running,
		// Token: 0x04000ABB RID: 2747
		Stopping,
		// Token: 0x04000ABC RID: 2748
		Stopped,
		// Token: 0x04000ABD RID: 2749
		Completed,
		// Token: 0x04000ABE RID: 2750
		Failed,
		// Token: 0x04000ABF RID: 2751
		Disconnected
	}
}
