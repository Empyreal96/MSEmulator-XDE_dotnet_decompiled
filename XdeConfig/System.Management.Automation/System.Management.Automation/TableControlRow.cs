using System;
using System.Collections.Generic;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x02000956 RID: 2390
	public sealed class TableControlRow
	{
		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x060057F1 RID: 22513 RVA: 0x001C9F5E File Offset: 0x001C815E
		// (set) Token: 0x060057F2 RID: 22514 RVA: 0x001C9F66 File Offset: 0x001C8166
		public List<TableControlColumn> Columns
		{
			get
			{
				return this._columns;
			}
			internal set
			{
				this._columns = value;
			}
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x001C9F6F File Offset: 0x001C816F
		public TableControlRow()
		{
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x001C9F84 File Offset: 0x001C8184
		internal TableControlRow(TableRowDefinition rowdefinition)
		{
			foreach (TableRowItemDefinition tableRowItemDefinition in rowdefinition.rowItemDefinitionList)
			{
				FieldPropertyToken fieldPropertyToken = tableRowItemDefinition.formatTokenList[0] as FieldPropertyToken;
				TableControlColumn item;
				if (fieldPropertyToken != null)
				{
					item = new TableControlColumn(fieldPropertyToken.expression.expressionValue, tableRowItemDefinition.alignment, fieldPropertyToken.expression.isScriptBlock);
				}
				else
				{
					item = new TableControlColumn();
				}
				this._columns.Add(item);
			}
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x001CA02C File Offset: 0x001C822C
		public TableControlRow(IEnumerable<TableControlColumn> columns)
		{
			if (columns == null)
			{
				throw PSTraceSource.NewArgumentNullException("columns");
			}
			foreach (TableControlColumn item in columns)
			{
				this._columns.Add(item);
			}
		}

		// Token: 0x04002F16 RID: 12054
		private List<TableControlColumn> _columns = new List<TableControlColumn>();
	}
}
