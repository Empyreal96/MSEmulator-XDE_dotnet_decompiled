using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008DD RID: 2269
	[Flags]
	public enum PowerShellTraceKeywords : ulong
	{
		// Token: 0x04002D3C RID: 11580
		None = 0UL,
		// Token: 0x04002D3D RID: 11581
		Runspace = 1UL,
		// Token: 0x04002D3E RID: 11582
		Pipeline = 2UL,
		// Token: 0x04002D3F RID: 11583
		Protocol = 4UL,
		// Token: 0x04002D40 RID: 11584
		Transport = 8UL,
		// Token: 0x04002D41 RID: 11585
		Host = 16UL,
		// Token: 0x04002D42 RID: 11586
		Cmdlets = 32UL,
		// Token: 0x04002D43 RID: 11587
		Serializer = 64UL,
		// Token: 0x04002D44 RID: 11588
		Session = 128UL,
		// Token: 0x04002D45 RID: 11589
		ManagedPlugIn = 256UL,
		// Token: 0x04002D46 RID: 11590
		UseAlwaysDebug = 2305843009213693952UL,
		// Token: 0x04002D47 RID: 11591
		UseAlwaysOperational = 9223372036854775808UL,
		// Token: 0x04002D48 RID: 11592
		UseAlwaysAnalytic = 4611686018427387904UL
	}
}
