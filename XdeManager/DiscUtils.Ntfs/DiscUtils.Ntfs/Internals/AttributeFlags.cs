using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x02000057 RID: 87
	[Flags]
	public enum AttributeFlags
	{
		// Token: 0x040001A3 RID: 419
		None = 0,
		// Token: 0x040001A4 RID: 420
		Compressed = 1,
		// Token: 0x040001A5 RID: 421
		Encrypted = 16384,
		// Token: 0x040001A6 RID: 422
		Sparse = 32768
	}
}
