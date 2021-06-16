using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000259 RID: 601
	internal enum PSLevel : byte
	{
		// Token: 0x04000BC7 RID: 3015
		LogAlways,
		// Token: 0x04000BC8 RID: 3016
		Critical,
		// Token: 0x04000BC9 RID: 3017
		Error,
		// Token: 0x04000BCA RID: 3018
		Warning,
		// Token: 0x04000BCB RID: 3019
		Informational,
		// Token: 0x04000BCC RID: 3020
		Verbose,
		// Token: 0x04000BCD RID: 3021
		Debug = 20
	}
}
