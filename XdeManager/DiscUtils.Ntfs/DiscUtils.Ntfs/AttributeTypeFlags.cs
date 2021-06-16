using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200000E RID: 14
	[Flags]
	internal enum AttributeTypeFlags
	{
		// Token: 0x0400003A RID: 58
		None = 0,
		// Token: 0x0400003B RID: 59
		Indexed = 2,
		// Token: 0x0400003C RID: 60
		Multiple = 4,
		// Token: 0x0400003D RID: 61
		NotZero = 8,
		// Token: 0x0400003E RID: 62
		IndexedUnique = 16,
		// Token: 0x0400003F RID: 63
		NamedUnique = 32,
		// Token: 0x04000040 RID: 64
		MustBeResident = 64,
		// Token: 0x04000041 RID: 65
		CanBeNonResident = 128
	}
}
