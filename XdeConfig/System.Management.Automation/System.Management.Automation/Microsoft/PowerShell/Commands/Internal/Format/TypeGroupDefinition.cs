using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000929 RID: 2345
	internal sealed class TypeGroupDefinition
	{
		// Token: 0x04002EBB RID: 11963
		internal string name;

		// Token: 0x04002EBC RID: 11964
		internal List<TypeReference> typeReferenceList = new List<TypeReference>();
	}
}
