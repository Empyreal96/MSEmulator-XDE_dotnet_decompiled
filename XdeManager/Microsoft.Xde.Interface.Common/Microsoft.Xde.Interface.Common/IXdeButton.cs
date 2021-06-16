using System;
using System.ComponentModel;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000008 RID: 8
	public interface IXdeButton : IXdeToolbarItem, IXdePluginComponent, INotifyPropertyChanged
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000016 RID: 22
		Bitmap Image { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000017 RID: 23
		// (set) Token: 0x06000018 RID: 24
		bool Toggled { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000019 RID: 25
		// (set) Token: 0x0600001A RID: 26
		bool Arrowed { get; set; }

		// Token: 0x0600001B RID: 27
		void OnClicked(object sender, EventArgs e);
	}
}
