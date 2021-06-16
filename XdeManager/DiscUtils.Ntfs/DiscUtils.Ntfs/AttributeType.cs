using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200000D RID: 13
	public enum AttributeType
	{
		// Token: 0x04000028 RID: 40
		None,
		// Token: 0x04000029 RID: 41
		StandardInformation = 16,
		// Token: 0x0400002A RID: 42
		AttributeList = 32,
		// Token: 0x0400002B RID: 43
		FileName = 48,
		// Token: 0x0400002C RID: 44
		ObjectId = 64,
		// Token: 0x0400002D RID: 45
		SecurityDescriptor = 80,
		// Token: 0x0400002E RID: 46
		VolumeName = 96,
		// Token: 0x0400002F RID: 47
		VolumeInformation = 112,
		// Token: 0x04000030 RID: 48
		Data = 128,
		// Token: 0x04000031 RID: 49
		IndexRoot = 144,
		// Token: 0x04000032 RID: 50
		IndexAllocation = 160,
		// Token: 0x04000033 RID: 51
		Bitmap = 176,
		// Token: 0x04000034 RID: 52
		ReparsePoint = 192,
		// Token: 0x04000035 RID: 53
		ExtendedAttributesInformation = 208,
		// Token: 0x04000036 RID: 54
		ExtendedAttributes = 224,
		// Token: 0x04000037 RID: 55
		PropertySet = 240,
		// Token: 0x04000038 RID: 56
		LoggedUtilityStream = 256
	}
}
