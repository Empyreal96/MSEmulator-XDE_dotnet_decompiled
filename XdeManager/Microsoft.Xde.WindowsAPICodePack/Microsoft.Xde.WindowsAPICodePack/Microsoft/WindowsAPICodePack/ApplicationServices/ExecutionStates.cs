using System;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000042 RID: 66
	[Flags]
	public enum ExecutionStates
	{
		// Token: 0x040001C1 RID: 449
		None = 0,
		// Token: 0x040001C2 RID: 450
		SystemRequired = 1,
		// Token: 0x040001C3 RID: 451
		DisplayRequired = 2,
		// Token: 0x040001C4 RID: 452
		AwayModeRequired = 64,
		// Token: 0x040001C5 RID: 453
		Continuous = -2147483648
	}
}
