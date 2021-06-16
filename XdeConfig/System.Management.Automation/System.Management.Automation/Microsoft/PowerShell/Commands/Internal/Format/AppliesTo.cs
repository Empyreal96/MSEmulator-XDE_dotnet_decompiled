using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200093C RID: 2364
	internal sealed class AppliesTo
	{
		// Token: 0x060057AB RID: 22443 RVA: 0x001C9350 File Offset: 0x001C7550
		internal void AddAppliesToType(string typeName)
		{
			TypeReference typeReference = new TypeReference();
			typeReference.name = typeName;
			this.referenceList.Add(typeReference);
		}

		// Token: 0x04002ED4 RID: 11988
		internal List<TypeOrGroupReference> referenceList = new List<TypeOrGroupReference>();
	}
}
