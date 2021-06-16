using System;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x0200094C RID: 2380
	public sealed class ListControlEntryItem
	{
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x060057D0 RID: 22480 RVA: 0x001C9B8C File Offset: 0x001C7D8C
		// (set) Token: 0x060057D1 RID: 22481 RVA: 0x001C9B94 File Offset: 0x001C7D94
		public string Label
		{
			get
			{
				return this._label;
			}
			internal set
			{
				this._label = value;
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060057D2 RID: 22482 RVA: 0x001C9B9D File Offset: 0x001C7D9D
		// (set) Token: 0x060057D3 RID: 22483 RVA: 0x001C9BA5 File Offset: 0x001C7DA5
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

		// Token: 0x060057D4 RID: 22484 RVA: 0x001C9BAE File Offset: 0x001C7DAE
		internal ListControlEntryItem()
		{
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x001C9BB8 File Offset: 0x001C7DB8
		internal ListControlEntryItem(ListControlItemDefinition definition)
		{
			if (definition.label != null)
			{
				this._label = definition.label.text;
			}
			FieldPropertyToken fieldPropertyToken = definition.formatTokenList[0] as FieldPropertyToken;
			if (fieldPropertyToken != null)
			{
				if (fieldPropertyToken.expression.isScriptBlock)
				{
					this._entry = new DisplayEntry(fieldPropertyToken.expression.expressionValue, DisplayEntryValueType.ScriptBlock);
					return;
				}
				this._entry = new DisplayEntry(fieldPropertyToken.expression.expressionValue, DisplayEntryValueType.Property);
			}
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x001C9C35 File Offset: 0x001C7E35
		public ListControlEntryItem(string label, DisplayEntry entry)
		{
			this._label = label;
			this._entry = entry;
		}

		// Token: 0x04002EFD RID: 12029
		private string _label;

		// Token: 0x04002EFE RID: 12030
		private DisplayEntry _entry;
	}
}
