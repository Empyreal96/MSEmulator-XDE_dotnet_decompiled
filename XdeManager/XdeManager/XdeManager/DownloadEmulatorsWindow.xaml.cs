using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using XdeManager.ViewModel;

namespace XdeManager
{
	// Token: 0x02000002 RID: 2
	public partial class DownloadEmulatorsWindow : Window
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public DownloadEmulatorsWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205D File Offset: 0x0000025D
		public DownloadEmulatorsWindow(Window owner)
		{
			base.Owner = owner;
			this.model = new DownloadEmulatorsWindowViewModel(this);
			base.DataContext = this.model;
			this.InitializeComponent();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002091 File Offset: 0x00000291
		protected override void OnContentRendered(EventArgs e)
		{
			if (this.first)
			{
				this.first = false;
				this.model.LoadItems();
			}
			base.OnContentRendered(e);
		}

		// Token: 0x04000001 RID: 1
		private DownloadEmulatorsWindowViewModel model;

		// Token: 0x04000002 RID: 2
		private bool first = true;
	}
}
