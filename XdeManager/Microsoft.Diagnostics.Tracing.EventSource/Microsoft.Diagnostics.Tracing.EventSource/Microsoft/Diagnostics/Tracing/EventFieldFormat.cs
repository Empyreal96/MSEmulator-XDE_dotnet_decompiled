using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200007C RID: 124
	public enum EventFieldFormat
	{
		// Token: 0x04000186 RID: 390
		Default,
		// Token: 0x04000187 RID: 391
		String = 2,
		// Token: 0x04000188 RID: 392
		Boolean,
		// Token: 0x04000189 RID: 393
		Hexadecimal,
		// Token: 0x0400018A RID: 394
		Xml = 11,
		// Token: 0x0400018B RID: 395
		Json,
		// Token: 0x0400018C RID: 396
		HResult = 15
	}
}
