using System;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200080A RID: 2058
	internal enum CertificatePurpose
	{
		// Token: 0x04002895 RID: 10389
		NotSpecified,
		// Token: 0x04002896 RID: 10390
		CodeSigning,
		// Token: 0x04002897 RID: 10391
		DocumentEncryption,
		// Token: 0x04002898 RID: 10392
		All = 65535
	}
}
