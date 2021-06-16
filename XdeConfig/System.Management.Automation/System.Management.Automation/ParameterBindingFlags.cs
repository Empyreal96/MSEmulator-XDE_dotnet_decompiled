using System;

namespace System.Management.Automation
{
	// Token: 0x02000074 RID: 116
	[Flags]
	internal enum ParameterBindingFlags
	{
		// Token: 0x04000280 RID: 640
		None = 0,
		// Token: 0x04000281 RID: 641
		ShouldCoerceType = 1,
		// Token: 0x04000282 RID: 642
		IsDefaultValue = 2,
		// Token: 0x04000283 RID: 643
		DelayBindScriptBlock = 4,
		// Token: 0x04000284 RID: 644
		ThrowOnParameterNotFound = 8
	}
}
