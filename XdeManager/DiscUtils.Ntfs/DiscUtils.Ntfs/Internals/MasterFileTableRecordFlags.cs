using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000063 RID: 99
	[Flags]
	public enum MasterFileTableRecordFlags
	{
		// Token: 0x040001CF RID: 463
		None = 0,
		// Token: 0x040001D0 RID: 464
		InUse = 1,
		// Token: 0x040001D1 RID: 465
		IsDirectory = 2,
		// Token: 0x040001D2 RID: 466
		IsMetaFile = 4,
		// Token: 0x040001D3 RID: 467
		HasViewIndex = 8
	}
}
