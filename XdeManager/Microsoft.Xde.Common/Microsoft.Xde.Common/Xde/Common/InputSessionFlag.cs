using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200002F RID: 47
	public enum InputSessionFlag : short
	{
		// Token: 0x040000F2 RID: 242
		None,
		// Token: 0x040000F3 RID: 243
		Begin,
		// Token: 0x040000F4 RID: 244
		End = 4,
		// Token: 0x040000F5 RID: 245
		ContactsChange = 16,
		// Token: 0x040000F6 RID: 246
		MoreData = 32
	}
}
