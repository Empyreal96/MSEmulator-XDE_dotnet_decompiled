using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x0200095F RID: 2399
	public sealed class WideControl : PSControl
	{
		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06005818 RID: 22552 RVA: 0x001CA833 File Offset: 0x001C8A33
		// (set) Token: 0x06005819 RID: 22553 RVA: 0x001CA83B File Offset: 0x001C8A3B
		public List<WideControlEntryItem> Entries
		{
			get
			{
				return this._entries;
			}
			internal set
			{
				this._entries = value;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x0600581A RID: 22554 RVA: 0x001CA844 File Offset: 0x001C8A44
		// (set) Token: 0x0600581B RID: 22555 RVA: 0x001CA84C File Offset: 0x001C8A4C
		public Alignment Alignment
		{
			get
			{
				return this._aligment;
			}
			internal set
			{
				this._aligment = value;
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x0600581C RID: 22556 RVA: 0x001CA855 File Offset: 0x001C8A55
		// (set) Token: 0x0600581D RID: 22557 RVA: 0x001CA85D File Offset: 0x001C8A5D
		public uint Columns
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

		// Token: 0x0600581E RID: 22558 RVA: 0x001CA868 File Offset: 0x001C8A68
		internal override void WriteToXML(XmlWriter _writer, bool exportScriptBlock)
		{
			_writer.WriteStartElement(WideControl._tagWideControl);
			if (this._columns > 0U)
			{
				_writer.WriteElementString(WideControl._tagColumnNumber, this._columns.ToString(CultureInfo.InvariantCulture));
			}
			if (this._aligment != Alignment.Undefined)
			{
				_writer.WriteElementString(WideControl._tagAlignment, this._aligment.ToString());
			}
			_writer.WriteStartElement(WideControl._tagWideEntries);
			foreach (WideControlEntryItem wideControlEntryItem in this._entries)
			{
				_writer.WriteStartElement(WideControl._tagWideEntry);
				if (wideControlEntryItem.SelectedBy.Count > 0)
				{
					_writer.WriteStartElement(WideControl._tagSelectedBy);
					foreach (string value in wideControlEntryItem.SelectedBy)
					{
						_writer.WriteElementString(WideControl._tagTypeName, value);
					}
					_writer.WriteEndElement();
				}
				_writer.WriteStartElement(WideControl._tagWideItem);
				wideControlEntryItem.DisplayEntry.WriteToXML(_writer, exportScriptBlock);
				_writer.WriteEndElement();
				_writer.WriteEndElement();
			}
			_writer.WriteEndElement();
			_writer.WriteEndElement();
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x001CA9B8 File Offset: 0x001C8BB8
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "WideControl", new object[0]);
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x001CA9D0 File Offset: 0x001C8BD0
		internal override bool SafeForExport()
		{
			foreach (WideControlEntryItem wideControlEntryItem in this._entries)
			{
				if (wideControlEntryItem.DisplayEntry.ValueType == DisplayEntryValueType.ScriptBlock)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x001CAA34 File Offset: 0x001C8C34
		public WideControl()
		{
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x001CAA48 File Offset: 0x001C8C48
		internal WideControl(WideControlBody widecontrolbody)
		{
			this._columns = (uint)widecontrolbody.columns;
			this._aligment = (Alignment)widecontrolbody.alignment;
			this._entries.Add(new WideControlEntryItem(widecontrolbody.defaultEntryDefinition));
			foreach (WideControlEntryDefinition definition in widecontrolbody.optionalEntryList)
			{
				this._entries.Add(new WideControlEntryItem(definition));
			}
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001CAAE4 File Offset: 0x001C8CE4
		public WideControl(IEnumerable<WideControlEntryItem> wideEntries)
		{
			if (wideEntries == null)
			{
				throw PSTraceSource.NewArgumentNullException("wideEntries");
			}
			foreach (WideControlEntryItem item in wideEntries)
			{
				this._entries.Add(item);
			}
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x001CAB50 File Offset: 0x001C8D50
		public WideControl(IEnumerable<WideControlEntryItem> wideEntries, uint columns)
		{
			if (wideEntries == null)
			{
				throw PSTraceSource.NewArgumentNullException("wideEntries");
			}
			foreach (WideControlEntryItem item in wideEntries)
			{
				this._entries.Add(item);
			}
			this._columns = columns;
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x001CABC4 File Offset: 0x001C8DC4
		public WideControl(uint columns)
		{
			this._columns = columns;
		}

		// Token: 0x04002F3B RID: 12091
		private List<WideControlEntryItem> _entries = new List<WideControlEntryItem>();

		// Token: 0x04002F3C RID: 12092
		private uint _columns;

		// Token: 0x04002F3D RID: 12093
		private Alignment _aligment;

		// Token: 0x04002F3E RID: 12094
		private static string _tagWideControl = "WideControl";

		// Token: 0x04002F3F RID: 12095
		private static string _tagWideEntries = "WideEntries";

		// Token: 0x04002F40 RID: 12096
		private static string _tagWideEntry = "WideEntry";

		// Token: 0x04002F41 RID: 12097
		private static string _tagWideItem = "WideItem";

		// Token: 0x04002F42 RID: 12098
		private static string _tagAlignment = "Alignment";

		// Token: 0x04002F43 RID: 12099
		private static string _tagColumnNumber = "ColumnNumber";

		// Token: 0x04002F44 RID: 12100
		private static string _tagSelectedBy = "EntrySelectedBy";

		// Token: 0x04002F45 RID: 12101
		private static string _tagTypeName = "TypeName";
	}
}
