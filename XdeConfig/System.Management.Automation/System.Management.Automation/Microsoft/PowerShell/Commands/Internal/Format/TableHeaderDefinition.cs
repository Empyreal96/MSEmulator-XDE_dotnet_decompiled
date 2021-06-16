using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000950 RID: 2384
	internal sealed class TableHeaderDefinition
	{
		// Token: 0x060057DA RID: 22490 RVA: 0x001C9D18 File Offset: 0x001C7F18
		internal TableHeaderDefinition Copy()
		{
			TableHeaderDefinition tableHeaderDefinition = new TableHeaderDefinition();
			tableHeaderDefinition.hideHeader = this.hideHeader;
			foreach (TableColumnHeaderDefinition item in this.columnHeaderDefinitionList)
			{
				tableHeaderDefinition.columnHeaderDefinitionList.Add(item);
			}
			return tableHeaderDefinition;
		}

		// Token: 0x04002F07 RID: 12039
		internal bool hideHeader;

		// Token: 0x04002F08 RID: 12040
		internal List<TableColumnHeaderDefinition> columnHeaderDefinitionList = new List<TableColumnHeaderDefinition>();
	}
}
