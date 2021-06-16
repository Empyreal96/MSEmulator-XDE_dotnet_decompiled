using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000946 RID: 2374
	internal sealed class ListControlBody : ControlBody
	{
		// Token: 0x060057B7 RID: 22455 RVA: 0x001C9434 File Offset: 0x001C7634
		internal override ControlBase Copy()
		{
			ListControlBody listControlBody = new ListControlBody();
			listControlBody.autosize = this.autosize;
			if (this.defaultEntryDefinition != null)
			{
				listControlBody.defaultEntryDefinition = this.defaultEntryDefinition.Copy();
			}
			foreach (ListControlEntryDefinition item in this.optionalEntryList)
			{
				listControlBody.optionalEntryList.Add(item);
			}
			return listControlBody;
		}

		// Token: 0x04002EEB RID: 12011
		internal ListControlEntryDefinition defaultEntryDefinition;

		// Token: 0x04002EEC RID: 12012
		internal List<ListControlEntryDefinition> optionalEntryList = new List<ListControlEntryDefinition>();
	}
}
