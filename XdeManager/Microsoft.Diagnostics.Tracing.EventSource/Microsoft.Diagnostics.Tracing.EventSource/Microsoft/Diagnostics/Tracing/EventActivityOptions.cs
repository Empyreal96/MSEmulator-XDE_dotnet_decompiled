using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000012 RID: 18
	[Flags]
	public enum EventActivityOptions
	{
		// Token: 0x04000062 RID: 98
		None = 0,
		// Token: 0x04000063 RID: 99
		Disable = 2,
		// Token: 0x04000064 RID: 100
		Recursive = 4,
		// Token: 0x04000065 RID: 101
		Detachable = 8
	}
}
