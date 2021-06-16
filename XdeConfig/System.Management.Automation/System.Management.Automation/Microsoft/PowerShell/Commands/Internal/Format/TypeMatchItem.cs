using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200096A RID: 2410
	internal sealed class TypeMatchItem
	{
		// Token: 0x0600585A RID: 22618 RVA: 0x001CBC34 File Offset: 0x001C9E34
		internal TypeMatchItem(object obj, AppliesTo a)
		{
			this._item = obj;
			this._appliesTo = a;
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x001CBC4A File Offset: 0x001C9E4A
		internal TypeMatchItem(object obj, AppliesTo a, PSObject currentObject)
		{
			this._item = obj;
			this._appliesTo = a;
			this._currentObject = currentObject;
		}

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x0600585C RID: 22620 RVA: 0x001CBC67 File Offset: 0x001C9E67
		internal object Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x0600585D RID: 22621 RVA: 0x001CBC6F File Offset: 0x001C9E6F
		internal AppliesTo AppliesTo
		{
			get
			{
				return this._appliesTo;
			}
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x0600585E RID: 22622 RVA: 0x001CBC77 File Offset: 0x001C9E77
		internal PSObject CurrentObject
		{
			get
			{
				return this._currentObject;
			}
		}

		// Token: 0x04002F5E RID: 12126
		private object _item;

		// Token: 0x04002F5F RID: 12127
		private AppliesTo _appliesTo;

		// Token: 0x04002F60 RID: 12128
		private PSObject _currentObject;
	}
}
