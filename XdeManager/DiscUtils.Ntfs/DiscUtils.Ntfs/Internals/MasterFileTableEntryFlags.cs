using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000061 RID: 97
	[Flags]
	public enum MasterFileTableEntryFlags
	{
		// Token: 0x040001C8 RID: 456
		None = 0,
		// Token: 0x040001C9 RID: 457
		InUse = 1,
		// Token: 0x040001CA RID: 458
		IsDirectory = 2,
		// Token: 0x040001CB RID: 459
		IsMetaFile = 4,
		// Token: 0x040001CC RID: 460
		HasViewIndex = 8
	}
}
