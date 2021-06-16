using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200008D RID: 141
	[Flags]
	public enum EventKeywords : long
	{
		// Token: 0x040001E0 RID: 480
		None = 0L,
		// Token: 0x040001E1 RID: 481
		All = -1L,
		// Token: 0x040001E2 RID: 482
		WdiContext = 562949953421312L,
		// Token: 0x040001E3 RID: 483
		WdiDiagnostic = 1125899906842624L,
		// Token: 0x040001E4 RID: 484
		Sqm = 2251799813685248L,
		// Token: 0x040001E5 RID: 485
		AuditFailure = 4503599627370496L,
		// Token: 0x040001E6 RID: 486
		AuditSuccess = 9007199254740992L,
		// Token: 0x040001E7 RID: 487
		CorrelationHint = 18014398509481984L,
		// Token: 0x040001E8 RID: 488
		EventLogClassic = 36028797018963968L
	}
}
