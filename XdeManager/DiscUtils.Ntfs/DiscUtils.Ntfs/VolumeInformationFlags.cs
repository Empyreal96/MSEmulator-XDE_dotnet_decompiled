using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000055 RID: 85
	[Flags]
	internal enum VolumeInformationFlags : ushort
	{
		// Token: 0x04000198 RID: 408
		None = 0,
		// Token: 0x04000199 RID: 409
		Dirty = 1,
		// Token: 0x0400019A RID: 410
		ResizeLogFile = 2,
		// Token: 0x0400019B RID: 411
		UpgradeOnMount = 4,
		// Token: 0x0400019C RID: 412
		MountedOnNT4 = 8,
		// Token: 0x0400019D RID: 413
		DeleteUSNUnderway = 16,
		// Token: 0x0400019E RID: 414
		RepairObjectIds = 32,
		// Token: 0x0400019F RID: 415
		DisableShortNameCreation = 128,
		// Token: 0x040001A0 RID: 416
		ModifiedByChkDsk = 32768
	}
}
