using System;
using System.Collections.Generic;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x02000960 RID: 2400
	public sealed class WideControlEntryItem
	{
		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x06005827 RID: 22567 RVA: 0x001CAC3D File Offset: 0x001C8E3D
		// (set) Token: 0x06005828 RID: 22568 RVA: 0x001CAC45 File Offset: 0x001C8E45
		public DisplayEntry DisplayEntry
		{
			get
			{
				return this._entry;
			}
			internal set
			{
				this._entry = value;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x06005829 RID: 22569 RVA: 0x001CAC4E File Offset: 0x001C8E4E
		// (set) Token: 0x0600582A RID: 22570 RVA: 0x001CAC56 File Offset: 0x001C8E56
		public List<string> SelectedBy
		{
			get
			{
				return this._selectedBy;
			}
			internal set
			{
				this._selectedBy = value;
			}
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x001CAC5F File Offset: 0x001C8E5F
		internal WideControlEntryItem()
		{
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x001CAC74 File Offset: 0x001C8E74
		internal WideControlEntryItem(WideControlEntryDefinition definition)
		{
			FieldPropertyToken fieldPropertyToken = definition.formatTokenList[0] as FieldPropertyToken;
			if (fieldPropertyToken != null)
			{
				if (fieldPropertyToken.expression.isScriptBlock)
				{
					this._entry = new DisplayEntry(fieldPropertyToken.expression.expressionValue, DisplayEntryValueType.ScriptBlock);
				}
				else
				{
					this._entry = new DisplayEntry(fieldPropertyToken.expression.expressionValue, DisplayEntryValueType.Property);
				}
			}
			if (definition.appliesTo != null)
			{
				foreach (TypeOrGroupReference typeOrGroupReference in definition.appliesTo.referenceList)
				{
					this._selectedBy.Add(typeOrGroupReference.name);
				}
			}
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x001CAD44 File Offset: 0x001C8F44
		public WideControlEntryItem(DisplayEntry entry)
		{
			if (entry == null)
			{
				throw PSTraceSource.NewArgumentNullException("entry");
			}
			this._entry = entry;
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x001CAD6C File Offset: 0x001C8F6C
		public WideControlEntryItem(DisplayEntry entry, IEnumerable<string> selectedBy)
		{
			if (entry == null)
			{
				throw PSTraceSource.NewArgumentNullException("entry");
			}
			if (selectedBy == null)
			{
				throw PSTraceSource.NewArgumentNullException("selectedBy");
			}
			this._entry = entry;
			foreach (string item in selectedBy)
			{
				this._selectedBy.Add(item);
			}
		}

		// Token: 0x04002F46 RID: 12102
		private DisplayEntry _entry;

		// Token: 0x04002F47 RID: 12103
		private List<string> _selectedBy = new List<string>();
	}
}
