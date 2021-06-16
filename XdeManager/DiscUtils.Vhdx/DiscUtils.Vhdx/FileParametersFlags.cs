using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x0200000F RID: 15
	[Flags]
	internal enum FileParametersFlags : uint
	{
		// Token: 0x0400003E RID: 62
		None = 0U,
		// Token: 0x0400003F RID: 63
		LeaveBlocksAllocated = 1U,
		// Token: 0x04000040 RID: 64
		HasParent = 2U
	}
}
