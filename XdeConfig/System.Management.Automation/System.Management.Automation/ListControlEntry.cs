using System;
using System.Collections.Generic;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x0200094B RID: 2379
	public sealed class ListControlEntry
	{
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x060057C8 RID: 22472 RVA: 0x001C9939 File Offset: 0x001C7B39
		// (set) Token: 0x060057C9 RID: 22473 RVA: 0x001C9941 File Offset: 0x001C7B41
		public List<ListControlEntryItem> Items
		{
			get
			{
				return this._items;
			}
			internal set
			{
				this._items = value;
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x060057CA RID: 22474 RVA: 0x001C994A File Offset: 0x001C7B4A
		// (set) Token: 0x060057CB RID: 22475 RVA: 0x001C9952 File Offset: 0x001C7B52
		public List<string> SelectedBy
		{
			get
			{
				return this._entrySelectedBy;
			}
			internal set
			{
				this._entrySelectedBy = value;
			}
		}

		// Token: 0x060057CC RID: 22476 RVA: 0x001C995B File Offset: 0x001C7B5B
		public ListControlEntry()
		{
		}

		// Token: 0x060057CD RID: 22477 RVA: 0x001C997C File Offset: 0x001C7B7C
		internal ListControlEntry(ListControlEntryDefinition entrydefn)
		{
			if (entrydefn.appliesTo != null)
			{
				foreach (TypeOrGroupReference typeOrGroupReference in entrydefn.appliesTo.referenceList)
				{
					this._entrySelectedBy.Add(typeOrGroupReference.name);
				}
			}
			foreach (ListControlItemDefinition definition in entrydefn.itemDefinitionList)
			{
				this._items.Add(new ListControlEntryItem(definition));
			}
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x001C9A50 File Offset: 0x001C7C50
		public ListControlEntry(IEnumerable<ListControlEntryItem> listItems)
		{
			if (listItems == null)
			{
				throw PSTraceSource.NewArgumentNullException("listItems");
			}
			foreach (ListControlEntryItem item in listItems)
			{
				this._items.Add(item);
			}
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x001C9AC8 File Offset: 0x001C7CC8
		public ListControlEntry(IEnumerable<ListControlEntryItem> listItems, IEnumerable<string> selectedBy)
		{
			if (listItems == null)
			{
				throw PSTraceSource.NewArgumentNullException("listItems");
			}
			if (selectedBy == null)
			{
				throw PSTraceSource.NewArgumentNullException("selectedBy");
			}
			foreach (string item in selectedBy)
			{
				this._entrySelectedBy.Add(item);
			}
			foreach (ListControlEntryItem item2 in listItems)
			{
				this._items.Add(item2);
			}
		}

		// Token: 0x04002EFB RID: 12027
		private List<ListControlEntryItem> _items = new List<ListControlEntryItem>();

		// Token: 0x04002EFC RID: 12028
		private List<string> _entrySelectedBy = new List<string>();
	}
}
