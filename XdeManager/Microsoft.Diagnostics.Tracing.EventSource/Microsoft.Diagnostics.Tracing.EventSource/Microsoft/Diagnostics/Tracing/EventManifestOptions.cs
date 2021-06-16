using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000033 RID: 51
	[Flags]
	public enum EventManifestOptions
	{
		// Token: 0x040000F9 RID: 249
		None = 0,
		// Token: 0x040000FA RID: 250
		Strict = 1,
		// Token: 0x040000FB RID: 251
		AllCultures = 2,
		// Token: 0x040000FC RID: 252
		OnlyIfNeededForRegistration = 4,
		// Token: 0x040000FD RID: 253
		AllowEventSourceOverride = 8
	}
}
