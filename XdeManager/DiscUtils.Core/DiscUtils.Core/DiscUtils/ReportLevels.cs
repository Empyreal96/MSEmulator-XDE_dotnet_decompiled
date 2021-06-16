using System;

namespace DiscUtils
{
	// Token: 0x02000027 RID: 39
	[Flags]
	public enum ReportLevels
	{
		// Token: 0x0400004D RID: 77
		None = 0,
		// Token: 0x0400004E RID: 78
		Information = 1,
		// Token: 0x0400004F RID: 79
		Warnings = 2,
		// Token: 0x04000050 RID: 80
		Errors = 4,
		// Token: 0x04000051 RID: 81
		All = 7
	}
}
