using System;
using System.CodeDom.Compiler;

namespace HCS.Resource.Storage
{
	// Token: 0x020000A7 RID: 167
	[GeneratedCode("MarsComp", "")]
	public enum DiskCachingMode
	{
		// Token: 0x0400035B RID: 859
		Default,
		// Token: 0x0400035C RID: 860
		None,
		// Token: 0x0400035D RID: 861
		SharableParents,
		// Token: 0x0400035E RID: 862
		FullChain,
		// Token: 0x0400035F RID: 863
		AzureLegacyBehaviour = 65535
	}
}
