using System;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace System.Management.Automation
{
	// Token: 0x02000954 RID: 2388
	public sealed class TableControlColumnHeader
	{
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060057E0 RID: 22496 RVA: 0x001C9E3E File Offset: 0x001C803E
		// (set) Token: 0x060057E1 RID: 22497 RVA: 0x001C9E46 File Offset: 0x001C8046
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

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x060057E2 RID: 22498 RVA: 0x001C9E4F File Offset: 0x001C804F
		// (set) Token: 0x060057E3 RID: 22499 RVA: 0x001C9E57 File Offset: 0x001C8057
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

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x060057E4 RID: 22500 RVA: 0x001C9E60 File Offset: 0x001C8060
		// (set) Token: 0x060057E5 RID: 22501 RVA: 0x001C9E68 File Offset: 0x001C8068
		public int Width
		{
			get
			{
				return this._width;
			}
			internal set
			{
				this._width = value;
			}
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x001C9E71 File Offset: 0x001C8071
		internal TableControlColumnHeader(TableColumnHeaderDefinition colheaderdefinition)
		{
			if (colheaderdefinition.label != null)
			{
				this._label = colheaderdefinition.label.text;
			}
			this._alignment = (Alignment)colheaderdefinition.alignment;
			this._width = colheaderdefinition.width;
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x001C9EAA File Offset: 0x001C80AA
		internal TableControlColumnHeader()
		{
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x001C9EB2 File Offset: 0x001C80B2
		public TableControlColumnHeader(string label, int width, Alignment alignment)
		{
			if (width < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("width", width);
			}
			this._label = label;
			this._width = width;
			this._alignment = alignment;
		}

		// Token: 0x04002F11 RID: 12049
		private string _label;

		// Token: 0x04002F12 RID: 12050
		private Alignment _alignment;

		// Token: 0x04002F13 RID: 12051
		private int _width;
	}
}
