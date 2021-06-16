using System;

namespace System.Management.Automation
{
	// Token: 0x02000844 RID: 2116
	[Flags]
	public enum ScopedItemOptions
	{
		// Token: 0x040029C4 RID: 10692
		None = 0,
		// Token: 0x040029C5 RID: 10693
		ReadOnly = 1,
		// Token: 0x040029C6 RID: 10694
		Constant = 2,
		// Token: 0x040029C7 RID: 10695
		Private = 4,
		// Token: 0x040029C8 RID: 10696
		AllScope = 8,
		// Token: 0x040029C9 RID: 10697
		Unspecified = 16
	}
}
