using System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200001C RID: 28
	public class ToolBarItemViewModel : ViewModelBase
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00007FC9 File Offset: 0x000061C9
		public ToolBarItemViewModel(IXdeToolbarItem item)
		{
			this.item = item;
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00007FD8 File Offset: 0x000061D8
		public string Name
		{
			get
			{
				return this.item.Name;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007FE5 File Offset: 0x000061E5
		public string ToolTipText
		{
			get
			{
				return this.item.Tooltip;
			}
		}

		// Token: 0x040000AB RID: 171
		private IXdeToolbarItem item;
	}
}
