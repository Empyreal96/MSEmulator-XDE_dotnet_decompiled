using System;

namespace DiscUtils
{
	// Token: 0x02000005 RID: 5
	[Flags]
	public enum ClusterRoles
	{
		// Token: 0x0400000A RID: 10
		None = 0,
		// Token: 0x0400000B RID: 11
		Free = 1,
		// Token: 0x0400000C RID: 12
		DataFile = 2,
		// Token: 0x0400000D RID: 13
		SystemFile = 4,
		// Token: 0x0400000E RID: 14
		Metadata = 8,
		// Token: 0x0400000F RID: 15
		BootArea = 16,
		// Token: 0x04000010 RID: 16
		Bad = 32
	}
}
