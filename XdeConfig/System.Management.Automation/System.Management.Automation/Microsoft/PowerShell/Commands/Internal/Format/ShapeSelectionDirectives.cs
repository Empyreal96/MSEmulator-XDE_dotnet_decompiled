using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000923 RID: 2339
	internal sealed class ShapeSelectionDirectives
	{
		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x0600578F RID: 22415 RVA: 0x001C9179 File Offset: 0x001C7379
		// (set) Token: 0x0600578E RID: 22414 RVA: 0x001C915E File Offset: 0x001C735E
		internal int PropertyCountForTable
		{
			get
			{
				if (this._propertyCountForTable != null)
				{
					return this._propertyCountForTable.Value;
				}
				return 4;
			}
			set
			{
				if (this._propertyCountForTable == null)
				{
					this._propertyCountForTable = new int?(value);
				}
			}
		}

		// Token: 0x04002EAE RID: 11950
		private int? _propertyCountForTable = null;

		// Token: 0x04002EAF RID: 11951
		internal List<FormatShapeSelectionOnType> formatShapeSelectionOnTypeList = new List<FormatShapeSelectionOnType>();
	}
}
