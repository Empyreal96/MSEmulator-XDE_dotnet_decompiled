using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000065 RID: 101
	[Flags]
	public enum NtfsFileAttributes
	{
		// Token: 0x040001D6 RID: 470
		None = 0,
		// Token: 0x040001D7 RID: 471
		ReadOnly = 1,
		// Token: 0x040001D8 RID: 472
		Hidden = 2,
		// Token: 0x040001D9 RID: 473
		System = 4,
		// Token: 0x040001DA RID: 474
		Archive = 32,
		// Token: 0x040001DB RID: 475
		Device = 64,
		// Token: 0x040001DC RID: 476
		Normal = 128,
		// Token: 0x040001DD RID: 477
		Temporary = 256,
		// Token: 0x040001DE RID: 478
		Sparse = 512,
		// Token: 0x040001DF RID: 479
		ReparsePoint = 1024,
		// Token: 0x040001E0 RID: 480
		Compressed = 2048,
		// Token: 0x040001E1 RID: 481
		Offline = 4096,
		// Token: 0x040001E2 RID: 482
		NotIndexed = 8192,
		// Token: 0x040001E3 RID: 483
		Encrypted = 16384,
		// Token: 0x040001E4 RID: 484
		Directory = 268435456,
		// Token: 0x040001E5 RID: 485
		IndexView = 536870912
	}
}
