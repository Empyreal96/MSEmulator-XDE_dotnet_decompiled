using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200054E RID: 1358
	[Flags]
	public enum PropertyAttributes
	{
		// Token: 0x04001CBB RID: 7355
		None = 0,
		// Token: 0x04001CBC RID: 7356
		Public = 1,
		// Token: 0x04001CBD RID: 7357
		Private = 2,
		// Token: 0x04001CBE RID: 7358
		Static = 16,
		// Token: 0x04001CBF RID: 7359
		Literal = 32,
		// Token: 0x04001CC0 RID: 7360
		Hidden = 64
	}
}
