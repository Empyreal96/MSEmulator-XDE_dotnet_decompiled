using System;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000007 RID: 7
	public interface ISkinButtonInfo
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000010 RID: 16
		Rectangle Bounds { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000011 RID: 17
		SkinButtonType ButtonType { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000012 RID: 18
		SkinButtonAnchor Anchor { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000013 RID: 19
		Keys[] KeyCode { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000014 RID: 20
		bool IsEnabled { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000015 RID: 21
		bool IsVisible { get; }
	}
}
