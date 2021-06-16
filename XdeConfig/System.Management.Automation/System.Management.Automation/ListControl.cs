using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x0200094A RID: 2378
	public sealed class ListControl : PSControl
	{
		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060057BF RID: 22463 RVA: 0x001C9566 File Offset: 0x001C7766
		// (set) Token: 0x060057C0 RID: 22464 RVA: 0x001C956E File Offset: 0x001C776E
		public List<ListControlEntry> Entries
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

		// Token: 0x060057C1 RID: 22465 RVA: 0x001C9578 File Offset: 0x001C7778
		internal override void WriteToXML(XmlWriter _writer, bool exportScriptBlock)
		{
			_writer.WriteStartElement(ListControl._tagListControl);
			_writer.WriteStartElement(ListControl._tagListEntries);
			foreach (ListControlEntry listControlEntry in this._entries)
			{
				_writer.WriteStartElement(ListControl._tagListEntry);
				if (listControlEntry.SelectedBy.Count > 0)
				{
					_writer.WriteStartElement(ListControl._tagEntrySelectedBy);
					foreach (string value in listControlEntry.SelectedBy)
					{
						_writer.WriteElementString(ListControl._tagTypeName, value);
					}
					_writer.WriteEndElement();
				}
				if (listControlEntry.Items.Count > 0)
				{
					_writer.WriteStartElement(ListControl._tagListItems);
					foreach (ListControlEntryItem listControlEntryItem in listControlEntry.Items)
					{
						_writer.WriteStartElement(ListControl._tagListItem);
						if (!string.IsNullOrEmpty(listControlEntryItem.Label))
						{
							_writer.WriteElementString(ListControl._tagLabel, listControlEntryItem.Label);
						}
						listControlEntryItem.DisplayEntry.WriteToXML(_writer, exportScriptBlock);
						_writer.WriteEndElement();
					}
					_writer.WriteEndElement();
				}
				_writer.WriteEndElement();
			}
			_writer.WriteEndElement();
			_writer.WriteEndElement();
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x001C9720 File Offset: 0x001C7920
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "ListControl", new object[0]);
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x001C9738 File Offset: 0x001C7938
		internal override bool SafeForExport()
		{
			foreach (ListControlEntry listControlEntry in this._entries)
			{
				foreach (ListControlEntryItem listControlEntryItem in listControlEntry.Items)
				{
					if (listControlEntryItem.DisplayEntry.ValueType == DisplayEntryValueType.ScriptBlock)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x001C97D8 File Offset: 0x001C79D8
		public ListControl()
		{
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x001C97EC File Offset: 0x001C79EC
		internal ListControl(ListControlBody listcontrolbody)
		{
			this._entries.Add(new ListControlEntry(listcontrolbody.defaultEntryDefinition));
			foreach (ListControlEntryDefinition entrydefn in listcontrolbody.optionalEntryList)
			{
				this._entries.Add(new ListControlEntry(entrydefn));
			}
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x001C9870 File Offset: 0x001C7A70
		public ListControl(IEnumerable<ListControlEntry> entries)
		{
			if (entries == null)
			{
				throw PSTraceSource.NewArgumentNullException("entries");
			}
			foreach (ListControlEntry item in entries)
			{
				this._entries.Add(item);
			}
		}

		// Token: 0x04002EF2 RID: 12018
		private List<ListControlEntry> _entries = new List<ListControlEntry>();

		// Token: 0x04002EF3 RID: 12019
		private static string _tagListControl = "ListControl";

		// Token: 0x04002EF4 RID: 12020
		private static string _tagListEntries = "ListEntries";

		// Token: 0x04002EF5 RID: 12021
		private static string _tagListEntry = "ListEntry";

		// Token: 0x04002EF6 RID: 12022
		private static string _tagListItems = "ListItems";

		// Token: 0x04002EF7 RID: 12023
		private static string _tagListItem = "ListItem";

		// Token: 0x04002EF8 RID: 12024
		private static string _tagLabel = "Label";

		// Token: 0x04002EF9 RID: 12025
		private static string _tagEntrySelectedBy = "EntrySelectedBy";

		// Token: 0x04002EFA RID: 12026
		private static string _tagTypeName = "TypeName";
	}
}
