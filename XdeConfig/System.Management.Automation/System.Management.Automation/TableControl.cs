using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x02000957 RID: 2391
	public sealed class TableControl : PSControl
	{
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x060057F6 RID: 22518 RVA: 0x001CA098 File Offset: 0x001C8298
		// (set) Token: 0x060057F7 RID: 22519 RVA: 0x001CA0A0 File Offset: 0x001C82A0
		public List<TableControlColumnHeader> Headers
		{
			get
			{
				return this._headers;
			}
			internal set
			{
				this._headers = value;
			}
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060057F8 RID: 22520 RVA: 0x001CA0A9 File Offset: 0x001C82A9
		// (set) Token: 0x060057F9 RID: 22521 RVA: 0x001CA0B1 File Offset: 0x001C82B1
		public List<TableControlRow> Rows
		{
			get
			{
				return this._rows;
			}
			internal set
			{
				this._rows = value;
			}
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x001CA0BA File Offset: 0x001C82BA
		public TableControl()
		{
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x001CA0D8 File Offset: 0x001C82D8
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "TableControl", new object[0]);
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x001CA0F0 File Offset: 0x001C82F0
		internal override void WriteToXML(XmlWriter _writer, bool exportScriptBlock)
		{
			_writer.WriteStartElement(TableControl._tagTableControl);
			_writer.WriteStartElement(TableControl._tagTableHeaders);
			foreach (TableControlColumnHeader tableControlColumnHeader in this._headers)
			{
				_writer.WriteStartElement(TableControl._tagTableColumnHeader);
				if (!string.IsNullOrEmpty(tableControlColumnHeader.Label))
				{
					_writer.WriteElementString(TableControl._tagLabel, tableControlColumnHeader.Label);
				}
				if (tableControlColumnHeader.Width > 0)
				{
					_writer.WriteElementString(TableControl._tagWidth, tableControlColumnHeader.Width.ToString(CultureInfo.InvariantCulture));
				}
				if (tableControlColumnHeader.Alignment != Alignment.Undefined)
				{
					_writer.WriteElementString(TableControl._tagAlignment, tableControlColumnHeader.Alignment.ToString());
				}
				_writer.WriteEndElement();
			}
			_writer.WriteEndElement();
			_writer.WriteStartElement(TableControl._tagTableRowEntries);
			foreach (TableControlRow tableControlRow in this._rows)
			{
				_writer.WriteStartElement(TableControl._tagTableRowEntry);
				_writer.WriteStartElement(TableControl._tagTableColumnItems);
				foreach (TableControlColumn tableControlColumn in tableControlRow.Columns)
				{
					_writer.WriteStartElement(TableControl._tagTableColumnItem);
					if (tableControlColumn.Alignment != Alignment.Undefined)
					{
						_writer.WriteElementString(TableControl._tagAlignment, tableControlColumn.Alignment.ToString());
					}
					tableControlColumn.DisplayEntry.WriteToXML(_writer, exportScriptBlock);
					_writer.WriteEndElement();
				}
				_writer.WriteEndElement();
				_writer.WriteEndElement();
			}
			_writer.WriteEndElement();
			_writer.WriteEndElement();
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x001CA2D0 File Offset: 0x001C84D0
		internal override bool SafeForExport()
		{
			foreach (TableControlRow tableControlRow in this._rows)
			{
				foreach (TableControlColumn tableControlColumn in tableControlRow.Columns)
				{
					if (tableControlColumn.DisplayEntry.ValueType == DisplayEntryValueType.ScriptBlock)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x001CA370 File Offset: 0x001C8570
		internal TableControl(TableControlBody tcb)
		{
			TableControlRow item = new TableControlRow(tcb.defaultDefinition);
			this._rows.Add(item);
			foreach (TableRowDefinition rowdefinition in tcb.optionalDefinitionList)
			{
				item = new TableControlRow(rowdefinition);
				this._rows.Add(item);
			}
			foreach (TableColumnHeaderDefinition colheaderdefinition in tcb.header.columnHeaderDefinitionList)
			{
				TableControlColumnHeader item2 = new TableControlColumnHeader(colheaderdefinition);
				this._headers.Add(item2);
			}
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x001CA458 File Offset: 0x001C8658
		public TableControl(TableControlRow tableControlRow)
		{
			if (tableControlRow == null)
			{
				throw PSTraceSource.NewArgumentNullException("tableControlRows");
			}
			this._rows.Add(tableControlRow);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x001CA490 File Offset: 0x001C8690
		public TableControl(TableControlRow tableControlRow, IEnumerable<TableControlColumnHeader> tableControlColumnHeaders)
		{
			if (tableControlRow == null)
			{
				throw PSTraceSource.NewArgumentNullException("tableControlRows");
			}
			if (tableControlColumnHeaders == null)
			{
				throw PSTraceSource.NewArgumentNullException("tableControlColumnHeaders");
			}
			this._rows.Add(tableControlRow);
			foreach (TableControlColumnHeader item in tableControlColumnHeaders)
			{
				this._headers.Add(item);
			}
		}

		// Token: 0x04002F17 RID: 12055
		private List<TableControlColumnHeader> _headers = new List<TableControlColumnHeader>();

		// Token: 0x04002F18 RID: 12056
		private List<TableControlRow> _rows = new List<TableControlRow>();

		// Token: 0x04002F19 RID: 12057
		private static string _tagTableControl = "TableControl";

		// Token: 0x04002F1A RID: 12058
		private static string _tagTableHeaders = "TableHeaders";

		// Token: 0x04002F1B RID: 12059
		private static string _tagTableColumnHeader = "TableColumnHeader";

		// Token: 0x04002F1C RID: 12060
		private static string _tagLabel = "Label";

		// Token: 0x04002F1D RID: 12061
		private static string _tagWidth = "Width";

		// Token: 0x04002F1E RID: 12062
		private static string _tagAlignment = "Alignment";

		// Token: 0x04002F1F RID: 12063
		private static string _tagTableRowEntries = "TableRowEntries";

		// Token: 0x04002F20 RID: 12064
		private static string _tagTableRowEntry = "TableRowEntry";

		// Token: 0x04002F21 RID: 12065
		private static string _tagTableColumnItems = "TableColumnItems";

		// Token: 0x04002F22 RID: 12066
		private static string _tagTableColumnItem = "TableColumnItem";
	}
}
