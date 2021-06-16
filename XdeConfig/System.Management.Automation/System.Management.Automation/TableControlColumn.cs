using System;

namespace System.Management.Automation
{
	// Token: 0x02000955 RID: 2389
	public sealed class TableControlColumn
	{
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x001C9EE4 File Offset: 0x001C80E4
		// (set) Token: 0x060057EA RID: 22506 RVA: 0x001C9EEC File Offset: 0x001C80EC
		public Alignment Alignment
		{
			get
			{
				return this._alignment;
			}
			internal set
			{
				this._alignment = value;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x001C9EF5 File Offset: 0x001C80F5
		// (set) Token: 0x060057EC RID: 22508 RVA: 0x001C9EFD File Offset: 0x001C80FD
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

		// Token: 0x060057ED RID: 22509 RVA: 0x001C9F06 File Offset: 0x001C8106
		public override string ToString()
		{
			return this._entry.Value;
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x001C9F13 File Offset: 0x001C8113
		internal TableControlColumn()
		{
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x001C9F1B File Offset: 0x001C811B
		internal TableControlColumn(string text, int alignment, bool isscriptblock)
		{
			this._alignment = (Alignment)alignment;
			if (isscriptblock)
			{
				this._entry = new DisplayEntry(text, DisplayEntryValueType.ScriptBlock);
				return;
			}
			this._entry = new DisplayEntry(text, DisplayEntryValueType.Property);
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x001C9F48 File Offset: 0x001C8148
		public TableControlColumn(Alignment alignment, DisplayEntry entry)
		{
			this._alignment = alignment;
			this._entry = entry;
		}

		// Token: 0x04002F14 RID: 12052
		private Alignment _alignment;

		// Token: 0x04002F15 RID: 12053
		private DisplayEntry _entry;
	}
}
