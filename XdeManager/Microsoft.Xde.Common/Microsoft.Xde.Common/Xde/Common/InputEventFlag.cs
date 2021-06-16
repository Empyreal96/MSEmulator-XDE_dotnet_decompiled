using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000030 RID: 48
	[Flags]
	public enum InputEventFlag : short
	{
		// Token: 0x040000F8 RID: 248
		None = 0,
		// Token: 0x040000F9 RID: 249
		Down = 1,
		// Token: 0x040000FA RID: 250
		Move = 2,
		// Token: 0x040000FB RID: 251
		Hold = 2,
		// Token: 0x040000FC RID: 252
		Up = 4,
		// Token: 0x040000FD RID: 253
		InRange = 8,
		// Token: 0x040000FE RID: 254
		Empty = 4096,
		// Token: 0x040000FF RID: 255
		Invalid = 8192
	}
}
