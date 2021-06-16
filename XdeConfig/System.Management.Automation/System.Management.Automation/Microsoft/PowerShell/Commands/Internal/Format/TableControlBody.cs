using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200094F RID: 2383
	internal sealed class TableControlBody : ControlBody
	{
		// Token: 0x060057D8 RID: 22488 RVA: 0x001C9C60 File Offset: 0x001C7E60
		internal override ControlBase Copy()
		{
			TableControlBody tableControlBody = new TableControlBody();
			tableControlBody.autosize = this.autosize;
			tableControlBody.header = this.header.Copy();
			if (this.defaultDefinition != null)
			{
				tableControlBody.defaultDefinition = this.defaultDefinition.Copy();
			}
			foreach (TableRowDefinition item in this.optionalDefinitionList)
			{
				tableControlBody.optionalDefinitionList.Add(item);
			}
			return tableControlBody;
		}

		// Token: 0x04002F04 RID: 12036
		internal TableHeaderDefinition header = new TableHeaderDefinition();

		// Token: 0x04002F05 RID: 12037
		internal TableRowDefinition defaultDefinition;

		// Token: 0x04002F06 RID: 12038
		internal List<TableRowDefinition> optionalDefinitionList = new List<TableRowDefinition>();
	}
}
