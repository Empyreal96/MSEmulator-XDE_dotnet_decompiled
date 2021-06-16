using System;
using System.Windows.Controls;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001B RID: 27
	public interface IXdeTab : IXdePluginComponent
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600009B RID: 155
		string Caption { get; }

		// Token: 0x0600009C RID: 156
		UserControl CreateTab();
	}
}
