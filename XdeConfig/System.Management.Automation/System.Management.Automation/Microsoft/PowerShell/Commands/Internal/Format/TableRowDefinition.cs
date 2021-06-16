using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000952 RID: 2386
	internal sealed class TableRowDefinition
	{
		// Token: 0x060057DD RID: 22493 RVA: 0x001C9DA0 File Offset: 0x001C7FA0
		internal TableRowDefinition Copy()
		{
			TableRowDefinition tableRowDefinition = new TableRowDefinition();
			tableRowDefinition.appliesTo = this.appliesTo;
			tableRowDefinition.multiLine = this.multiLine;
			foreach (TableRowItemDefinition item in this.rowItemDefinitionList)
			{
				tableRowDefinition.rowItemDefinitionList.Add(item);
			}
			return tableRowDefinition;
		}

		// Token: 0x04002F0C RID: 12044
		internal AppliesTo appliesTo;

		// Token: 0x04002F0D RID: 12045
		internal bool multiLine;

		// Token: 0x04002F0E RID: 12046
		internal List<TableRowItemDefinition> rowItemDefinitionList = new List<TableRowItemDefinition>();
	}
}
