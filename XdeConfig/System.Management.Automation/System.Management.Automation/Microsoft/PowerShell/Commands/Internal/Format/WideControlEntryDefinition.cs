using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200095E RID: 2398
	internal sealed class WideControlEntryDefinition
	{
		// Token: 0x04002F39 RID: 12089
		internal AppliesTo appliesTo;

		// Token: 0x04002F3A RID: 12090
		internal List<FormatToken> formatTokenList = new List<FormatToken>();
	}
}
