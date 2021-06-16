using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000005 RID: 5
	internal enum AttributeCollationRule
	{
		// Token: 0x04000002 RID: 2
		Binary,
		// Token: 0x04000003 RID: 3
		Filename,
		// Token: 0x04000004 RID: 4
		UnicodeString,
		// Token: 0x04000005 RID: 5
		UnsignedLong = 16,
		// Token: 0x04000006 RID: 6
		Sid,
		// Token: 0x04000007 RID: 7
		SecurityHash,
		// Token: 0x04000008 RID: 8
		MultipleUnsignedLongs
	}
}
