using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000026 RID: 38
	[Flags]
	public enum EventSourceSettings
	{
		// Token: 0x040000B5 RID: 181
		Default = 0,
		// Token: 0x040000B6 RID: 182
		ThrowOnEventWriteErrors = 1,
		// Token: 0x040000B7 RID: 183
		EtwManifestEventFormat = 4,
		// Token: 0x040000B8 RID: 184
		EtwSelfDescribingEventFormat = 8
	}
}
