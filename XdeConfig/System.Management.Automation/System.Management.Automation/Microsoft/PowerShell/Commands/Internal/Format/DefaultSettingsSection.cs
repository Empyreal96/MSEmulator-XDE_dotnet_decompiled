using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000921 RID: 2337
	internal sealed class DefaultSettingsSection
	{
		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06005787 RID: 22407 RVA: 0x001C9081 File Offset: 0x001C7281
		// (set) Token: 0x06005786 RID: 22406 RVA: 0x001C9066 File Offset: 0x001C7266
		internal bool MultilineTables
		{
			get
			{
				return this._multilineTables != null && this._multilineTables.Value;
			}
			set
			{
				if (this._multilineTables == null)
				{
					this._multilineTables = new bool?(value);
				}
			}
		}

		// Token: 0x04002EA6 RID: 11942
		private bool? _multilineTables;

		// Token: 0x04002EA7 RID: 11943
		internal FormatErrorPolicy formatErrorPolicy = new FormatErrorPolicy();

		// Token: 0x04002EA8 RID: 11944
		internal ShapeSelectionDirectives shapeSelectionDirectives = new ShapeSelectionDirectives();

		// Token: 0x04002EA9 RID: 11945
		internal List<EnumerableExpansionDirective> enumerableExpansionDirectiveList = new List<EnumerableExpansionDirective>();
	}
}
