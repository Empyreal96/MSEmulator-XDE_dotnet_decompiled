using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000943 RID: 2371
	internal sealed class ComplexControlBody : ControlBody
	{
		// Token: 0x04002EE6 RID: 12006
		internal ComplexControlEntryDefinition defaultEntry;

		// Token: 0x04002EE7 RID: 12007
		internal List<ComplexControlEntryDefinition> optionalEntryList = new List<ComplexControlEntryDefinition>();
	}
}
