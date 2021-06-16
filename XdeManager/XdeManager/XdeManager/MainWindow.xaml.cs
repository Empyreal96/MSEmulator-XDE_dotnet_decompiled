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
	// Token: 0x02000009 RID: 9
	public partial class MainWindow : Window
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00002988 File Offset: 0x00000B88
		public MainWindow()
		{
			this.model = new XdeManagerViewModel(this, new EnumerateDevices(XdeDeviceFactory.GetDevices));
			base.DataContext = this.model;
			this.InitializeComponent();
			WindowPreferences windowPreferences = new WindowPreferences();
			base.Height = windowPreferences.WindowHeight;
			base.Width = windowPreferences.WindowWidth;
			base.Top = windowPreferences.WindowTop;
			base.Left = windowPreferences.WindowLeft;
			base.WindowState = windowPreferences.WindowState;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A08 File Offset: 0x00000C08
		protected override void OnClosing(CancelEventArgs e)
		{
			new WindowPreferences
			{
				WindowHeight = base.Height,
				WindowWidth = base.Width,
				WindowTop = base.Top,
				WindowLeft = base.Left,
				WindowState = base.WindowState
			}.Save();
			base.OnClosing(e);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A62 File Offset: 0x00000C62
		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x0400001B RID: 27
		private XdeManagerViewModel model;
	}
}
