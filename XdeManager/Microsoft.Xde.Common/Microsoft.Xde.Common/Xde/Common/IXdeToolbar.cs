using System;
using System.Collections.Generic;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000037 RID: 55
	public interface IXdeToolbar
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600015C RID: 348
		Rectangle DesktopBounds { get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600015D RID: 349
		bool IsToolsButtonEnabled { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600015E RID: 350
		XdeToolbarButtonAutomation[] Buttons { get; }

		// Token: 0x0600015F RID: 351
		void ClickButton(string buttonName);

		// Token: 0x06000160 RID: 352
		void LoadFromSku(IXdeSku sku);

		// Token: 0x06000161 RID: 353
		void ShowToolbarFlyout(string buttonNameOrigin, IEnumerable<IXdeToolbarItem> items, ToolbarFlags toolbarFlags);
	}
}
