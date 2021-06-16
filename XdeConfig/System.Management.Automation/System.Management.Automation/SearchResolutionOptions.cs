using System;

namespace System.Management.Automation
{
	// Token: 0x0200005E RID: 94
	[Flags]
	internal enum SearchResolutionOptions
	{
		// Token: 0x04000201 RID: 513
		None = 0,
		// Token: 0x04000202 RID: 514
		ResolveAliasPatterns = 1,
		// Token: 0x04000203 RID: 515
		ResolveFunctionPatterns = 2,
		// Token: 0x04000204 RID: 516
		CommandNameIsPattern = 4,
		// Token: 0x04000205 RID: 517
		SearchAllScopes = 8
	}
}
