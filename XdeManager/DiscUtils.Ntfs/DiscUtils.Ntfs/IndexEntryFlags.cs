using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200002B RID: 43
	[Flags]
	internal enum IndexEntryFlags : ushort
	{
		// Token: 0x040000D2 RID: 210
		None = 0,
		// Token: 0x040000D3 RID: 211
		Node = 1,
		// Token: 0x040000D4 RID: 212
		End = 2
	}
}
