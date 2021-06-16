using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000953 RID: 2387
	internal sealed class TableRowItemDefinition
	{
		// Token: 0x04002F0F RID: 12047
		internal int alignment;

		// Token: 0x04002F10 RID: 12048
		internal List<FormatToken> formatTokenList = new List<FormatToken>();
	}
}
