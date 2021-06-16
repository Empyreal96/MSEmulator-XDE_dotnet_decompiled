using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Xde.DeviceManagement;
using XdeManager.ViewModel;

namespace XdeManager
{
	// Token: 0x02000005 RID: 5
	public partial class LaunchXdeWindow : Window
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000024E6 File Offset: 0x000006E6
		public LaunchXdeWindow() : this(null)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024EF File Offset: 0x000006EF
		public LaunchXdeWindow(XdeDevice device)
		{
			if (device != null)
			{
				this.viewModel = new LaunchXdeViewModel(device);
				base.DataContext = this.viewModel;
			}
			this.InitializeComponent();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002518 File Offset: 0x00000718
		public string AddtionalCommandLineArgs
		{
			get
			{
				if (this.viewModel == null)
				{
					return null;
				}
				return this.viewModel.AdditionalCommandLineArgs;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000252F File Offset: 0x0000072F
		private void LaunchButton_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
			base.Close();
		}

		// Token: 0x0400000E RID: 14
		private LaunchXdeViewModel viewModel;
	}
}
