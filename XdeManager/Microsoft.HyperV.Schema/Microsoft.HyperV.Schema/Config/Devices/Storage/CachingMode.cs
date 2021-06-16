using System;
using System.CodeDom.Compiler;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000119 RID: 281
	[GeneratedCode("MarsComp", "")]
	public enum CachingMode
	{
		// Token: 0x04000598 RID: 1432
		Default,
		// Token: 0x04000599 RID: 1433
		None,
		// Token: 0x0400059A RID: 1434
		SharableParents,
		// Token: 0x0400059B RID: 1435
		FullChain,
		// Token: 0x0400059C RID: 1436
		AzureLegacyBehavior = 65535
	}
}
