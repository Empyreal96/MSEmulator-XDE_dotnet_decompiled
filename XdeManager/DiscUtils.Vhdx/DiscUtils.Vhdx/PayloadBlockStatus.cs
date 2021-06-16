using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200001E RID: 30
	internal enum PayloadBlockStatus : ulong
	{
		// Token: 0x0400007E RID: 126
		NotPresent,
		// Token: 0x0400007F RID: 127
		Undefined,
		// Token: 0x04000080 RID: 128
		Zero,
		// Token: 0x04000081 RID: 129
		Unmapped,
		// Token: 0x04000082 RID: 130
		FullyPresent = 6UL,
		// Token: 0x04000083 RID: 131
		PartiallyPresent
	}
}
