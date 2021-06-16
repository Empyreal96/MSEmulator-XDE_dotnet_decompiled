using System;

namespace System.Management.Automation
{
	// Token: 0x020007FA RID: 2042
	public enum SigningOption
	{
		// Token: 0x04002833 RID: 10291
		AddOnlyCertificate,
		// Token: 0x04002834 RID: 10292
		AddFullCertificateChain,
		// Token: 0x04002835 RID: 10293
		AddFullCertificateChainExceptRoot,
		// Token: 0x04002836 RID: 10294
		Default = 2
	}
}
