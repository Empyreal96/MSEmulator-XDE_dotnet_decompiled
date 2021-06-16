using System;

namespace System.Management.Automation
{
	// Token: 0x02000486 RID: 1158
	[Flags]
	public enum SplitOptions
	{
		// Token: 0x04001AA0 RID: 6816
		SimpleMatch = 1,
		// Token: 0x04001AA1 RID: 6817
		RegexMatch = 2,
		// Token: 0x04001AA2 RID: 6818
		CultureInvariant = 4,
		// Token: 0x04001AA3 RID: 6819
		IgnorePatternWhitespace = 8,
		// Token: 0x04001AA4 RID: 6820
		Multiline = 16,
		// Token: 0x04001AA5 RID: 6821
		Singleline = 32,
		// Token: 0x04001AA6 RID: 6822
		IgnoreCase = 64,
		// Token: 0x04001AA7 RID: 6823
		ExplicitCapture = 128
	}
}
