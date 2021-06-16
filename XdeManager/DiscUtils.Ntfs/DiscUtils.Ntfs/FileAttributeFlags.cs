using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200001A RID: 26
	[Flags]
	internal enum FileAttributeFlags : uint
	{
		// Token: 0x0400007E RID: 126
		None = 0U,
		// Token: 0x0400007F RID: 127
		ReadOnly = 1U,
		// Token: 0x04000080 RID: 128
		Hidden = 2U,
		// Token: 0x04000081 RID: 129
		System = 4U,
		// Token: 0x04000082 RID: 130
		Archive = 32U,
		// Token: 0x04000083 RID: 131
		Device = 64U,
		// Token: 0x04000084 RID: 132
		Normal = 128U,
		// Token: 0x04000085 RID: 133
		Temporary = 256U,
		// Token: 0x04000086 RID: 134
		Sparse = 512U,
		// Token: 0x04000087 RID: 135
		ReparsePoint = 1024U,
		// Token: 0x04000088 RID: 136
		Compressed = 2048U,
		// Token: 0x04000089 RID: 137
		Offline = 4096U,
		// Token: 0x0400008A RID: 138
		NotIndexed = 8192U,
		// Token: 0x0400008B RID: 139
		Encrypted = 16384U,
		// Token: 0x0400008C RID: 140
		Directory = 268435456U,
		// Token: 0x0400008D RID: 141
		IndexView = 536870912U
	}
}
