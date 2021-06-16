using System;

namespace DiscUtils
{
	// Token: 0x0200002B RID: 43
	public enum UnixFileType
	{
		// Token: 0x0400006B RID: 107
		None,
		// Token: 0x0400006C RID: 108
		Fifo,
		// Token: 0x0400006D RID: 109
		Character,
		// Token: 0x0400006E RID: 110
		Directory = 4,
		// Token: 0x0400006F RID: 111
		Block = 6,
		// Token: 0x04000070 RID: 112
		Regular = 8,
		// Token: 0x04000071 RID: 113
		Link = 10,
		// Token: 0x04000072 RID: 114
		Socket = 12
	}
}
