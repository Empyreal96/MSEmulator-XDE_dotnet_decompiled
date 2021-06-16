using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200095D RID: 2397
	internal sealed class WideControlBody : ControlBody
	{
		// Token: 0x04002F35 RID: 12085
		internal int columns;

		// Token: 0x04002F36 RID: 12086
		internal int alignment;

		// Token: 0x04002F37 RID: 12087
		internal WideControlEntryDefinition defaultEntryDefinition;

		// Token: 0x04002F38 RID: 12088
		internal List<WideControlEntryDefinition> optionalEntryList = new List<WideControlEntryDefinition>();
	}
}
