using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x0200005B RID: 91
	[Flags]
	public enum EntryStates
	{
		// Token: 0x040001AF RID: 431
		None = 0,
		// Token: 0x040001B0 RID: 432
		InUse = 1,
		// Token: 0x040001B1 RID: 433
		NotInUse = 2,
		// Token: 0x040001B2 RID: 434
		All = 3
	}
}
