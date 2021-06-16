using System;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000068 RID: 104
	internal enum RecordType : byte
	{
		// Token: 0x04000154 RID: 340
		None,
		// Token: 0x04000155 RID: 341
		Volume,
		// Token: 0x04000156 RID: 342
		Component,
		// Token: 0x04000157 RID: 343
		Extent,
		// Token: 0x04000158 RID: 344
		Disk,
		// Token: 0x04000159 RID: 345
		DiskGroup
	}
}
