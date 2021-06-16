using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001EC RID: 492
	[Flags]
	public enum PipelineResultTypes
	{
		// Token: 0x0400099B RID: 2459
		None = 0,
		// Token: 0x0400099C RID: 2460
		Output = 1,
		// Token: 0x0400099D RID: 2461
		Error = 2,
		// Token: 0x0400099E RID: 2462
		Warning = 3,
		// Token: 0x0400099F RID: 2463
		Verbose = 4,
		// Token: 0x040009A0 RID: 2464
		Debug = 5,
		// Token: 0x040009A1 RID: 2465
		Information = 6,
		// Token: 0x040009A2 RID: 2466
		All = 7,
		// Token: 0x040009A3 RID: 2467
		Null = 8
	}
}
