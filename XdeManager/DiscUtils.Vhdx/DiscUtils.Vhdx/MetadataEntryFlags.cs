using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000018 RID: 24
	[Flags]
	internal enum MetadataEntryFlags : uint
	{
		// Token: 0x04000063 RID: 99
		None = 0U,
		// Token: 0x04000064 RID: 100
		IsUser = 1U,
		// Token: 0x04000065 RID: 101
		IsVirtualDisk = 2U,
		// Token: 0x04000066 RID: 102
		IsRequired = 4U
	}
}
