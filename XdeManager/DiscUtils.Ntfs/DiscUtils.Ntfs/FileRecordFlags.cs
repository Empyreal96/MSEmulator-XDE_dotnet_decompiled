using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200001E RID: 30
	[Flags]
	internal enum FileRecordFlags : ushort
	{
		// Token: 0x040000AC RID: 172
		None = 0,
		// Token: 0x040000AD RID: 173
		InUse = 1,
		// Token: 0x040000AE RID: 174
		IsDirectory = 2,
		// Token: 0x040000AF RID: 175
		IsMetaFile = 4,
		// Token: 0x040000B0 RID: 176
		HasViewIndex = 8
	}
}
