using System;

namespace System.Management.Automation
{
	// Token: 0x0200042A RID: 1066
	[Flags]
	public enum SessionCapabilities
	{
		// Token: 0x040018B2 RID: 6322
		RemoteServer = 1,
		// Token: 0x040018B3 RID: 6323
		WorkflowServer = 2,
		// Token: 0x040018B4 RID: 6324
		Language = 4
	}
}
