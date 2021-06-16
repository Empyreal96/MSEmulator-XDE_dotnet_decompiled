using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200059B RID: 1435
	[Flags]
	internal enum CharTraits
	{
		// Token: 0x04001DAA RID: 7594
		None = 0,
		// Token: 0x04001DAB RID: 7595
		IdentifierStart = 2,
		// Token: 0x04001DAC RID: 7596
		MultiplierStart = 4,
		// Token: 0x04001DAD RID: 7597
		TypeSuffix = 8,
		// Token: 0x04001DAE RID: 7598
		Whitespace = 16,
		// Token: 0x04001DAF RID: 7599
		Newline = 32,
		// Token: 0x04001DB0 RID: 7600
		HexDigit = 64,
		// Token: 0x04001DB1 RID: 7601
		Digit = 128,
		// Token: 0x04001DB2 RID: 7602
		VarNameFirst = 256,
		// Token: 0x04001DB3 RID: 7603
		ForceStartNewToken = 512,
		// Token: 0x04001DB4 RID: 7604
		ForceStartNewAssemblyNameSpecToken = 1024,
		// Token: 0x04001DB5 RID: 7605
		ForceStartNewTokenAfterNumber = 2048
	}
}
