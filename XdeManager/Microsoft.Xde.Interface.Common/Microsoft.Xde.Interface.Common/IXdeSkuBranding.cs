using System;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000018 RID: 24
	public interface IXdeSkuBranding : IXdePluginComponent
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600008B RID: 139
		string DisplayName { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600008C RID: 140
		Icon Icon { get; }
	}
}
