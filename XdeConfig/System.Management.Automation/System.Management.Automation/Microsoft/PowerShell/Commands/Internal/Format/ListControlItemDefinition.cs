using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000948 RID: 2376
	internal sealed class ListControlItemDefinition
	{
		// Token: 0x04002EEF RID: 12015
		internal ExpressionToken conditionToken;

		// Token: 0x04002EF0 RID: 12016
		internal TextToken label;

		// Token: 0x04002EF1 RID: 12017
		internal List<FormatToken> formatTokenList = new List<FormatToken>();
	}
}
