using System;
using System.CodeDom.Compiler;

namespace HCS.Compute.System
{
	// Token: 0x0200019C RID: 412
	[GeneratedCode("MarsComp", "")]
	public enum ActiveOperation
	{
		// Token: 0x0400090E RID: 2318
		None,
		// Token: 0x0400090F RID: 2319
		Construct,
		// Token: 0x04000910 RID: 2320
		Start,
		// Token: 0x04000911 RID: 2321
		Pause,
		// Token: 0x04000912 RID: 2322
		Resume,
		// Token: 0x04000913 RID: 2323
		Shutdown,
		// Token: 0x04000914 RID: 2324
		Terminate,
		// Token: 0x04000915 RID: 2325
		Save,
		// Token: 0x04000916 RID: 2326
		GetProperties,
		// Token: 0x04000917 RID: 2327
		Modify
	}
}
