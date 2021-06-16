using System;

namespace DiscUtils
{
	// Token: 0x02000029 RID: 41
	[Flags]
	public enum UnixFilePermissions
	{
		// Token: 0x04000053 RID: 83
		None = 0,
		// Token: 0x04000054 RID: 84
		OthersExecute = 1,
		// Token: 0x04000055 RID: 85
		OthersWrite = 2,
		// Token: 0x04000056 RID: 86
		OthersRead = 4,
		// Token: 0x04000057 RID: 87
		OthersAll = 7,
		// Token: 0x04000058 RID: 88
		GroupExecute = 8,
		// Token: 0x04000059 RID: 89
		GroupWrite = 16,
		// Token: 0x0400005A RID: 90
		GroupRead = 32,
		// Token: 0x0400005B RID: 91
		GroupAll = 56,
		// Token: 0x0400005C RID: 92
		OwnerExecute = 64,
		// Token: 0x0400005D RID: 93
		OwnerWrite = 128,
		// Token: 0x0400005E RID: 94
		OwnerRead = 256,
		// Token: 0x0400005F RID: 95
		OwnerAll = 448,
		// Token: 0x04000060 RID: 96
		Sticky = 512,
		// Token: 0x04000061 RID: 97
		SetGroupId = 1024,
		// Token: 0x04000062 RID: 98
		SetUserId = 2048
	}
}
