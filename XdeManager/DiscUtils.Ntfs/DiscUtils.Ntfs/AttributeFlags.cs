using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000008 RID: 8
	[Flags]
	internal enum AttributeFlags : ushort
	{
		// Token: 0x04000013 RID: 19
		None = 0,
		// Token: 0x04000014 RID: 20
		Compressed = 1,
		// Token: 0x04000015 RID: 21
		Encrypted = 16384,
		// Token: 0x04000016 RID: 22
		Sparse = 32768
	}
}
