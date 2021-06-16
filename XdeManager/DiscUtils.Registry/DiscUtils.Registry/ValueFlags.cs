using System;

namespace DiscUtils.Registry
{
	// Token: 0x02000012 RID: 18
	[Flags]
	internal enum ValueFlags : ushort
	{
		// Token: 0x04000061 RID: 97
		Named = 1,
		// Token: 0x04000062 RID: 98
		Unknown0002 = 2,
		// Token: 0x04000063 RID: 99
		Unknown0004 = 4,
		// Token: 0x04000064 RID: 100
		Unknown0008 = 8,
		// Token: 0x04000065 RID: 101
		Unknown0010 = 16,
		// Token: 0x04000066 RID: 102
		Unknown0020 = 32,
		// Token: 0x04000067 RID: 103
		Unknown0040 = 64,
		// Token: 0x04000068 RID: 104
		Unknown0080 = 128,
		// Token: 0x04000069 RID: 105
		Unknown0100 = 256,
		// Token: 0x0400006A RID: 106
		Unknown0200 = 512,
		// Token: 0x0400006B RID: 107
		Unknown0400 = 1024,
		// Token: 0x0400006C RID: 108
		Unknown0800 = 2048,
		// Token: 0x0400006D RID: 109
		Unknown1000 = 4096,
		// Token: 0x0400006E RID: 110
		Unknown2000 = 8192,
		// Token: 0x0400006F RID: 111
		Unknown4000 = 16384,
		// Token: 0x04000070 RID: 112
		Unknown8000 = 32768
	}
}
