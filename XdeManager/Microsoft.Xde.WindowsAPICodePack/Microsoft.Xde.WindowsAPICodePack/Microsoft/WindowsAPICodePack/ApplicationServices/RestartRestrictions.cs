using System;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x0200003B RID: 59
	[Flags]
	public enum RestartRestrictions
	{
		// Token: 0x040001A2 RID: 418
		None = 0,
		// Token: 0x040001A3 RID: 419
		NotOnCrash = 1,
		// Token: 0x040001A4 RID: 420
		NotOnHang = 2,
		// Token: 0x040001A5 RID: 421
		NotOnPatch = 4,
		// Token: 0x040001A6 RID: 422
		NotOnReboot = 8
	}
}
