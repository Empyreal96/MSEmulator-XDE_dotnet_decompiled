using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000947 RID: 2375
	internal sealed class ListControlEntryDefinition
	{
		// Token: 0x060057B9 RID: 22457 RVA: 0x001C94CC File Offset: 0x001C76CC
		internal ListControlEntryDefinition Copy()
		{
			ListControlEntryDefinition listControlEntryDefinition = new ListControlEntryDefinition();
			listControlEntryDefinition.appliesTo = this.appliesTo;
			foreach (ListControlItemDefinition item in this.itemDefinitionList)
			{
				listControlEntryDefinition.itemDefinitionList.Add(item);
			}
			return listControlEntryDefinition;
		}

		// Token: 0x04002EED RID: 12013
		internal AppliesTo appliesTo;

		// Token: 0x04002EEE RID: 12014
		internal List<ListControlItemDefinition> itemDefinitionList = new List<ListControlItemDefinition>();
	}
}
