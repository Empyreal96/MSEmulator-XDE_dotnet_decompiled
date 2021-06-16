using System;

namespace DiscUtils.Ntfs.Internals
{
	// Token: 0x0200005A RID: 90
	[Flags]
	public enum EntryState
	{
		// Token: 0x040001AA RID: 426
		None = 0,
		// Token: 0x040001AB RID: 427
		InUse = 1,
		// Token: 0x040001AC RID: 428
		NotInUse = 2,
		// Token: 0x040001AD RID: 429
		All = 3
	}
}
