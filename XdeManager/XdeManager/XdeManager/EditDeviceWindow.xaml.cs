using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Xde.DeviceManagement;
using XdeManager.ViewModel;

namespace XdeManager
{
	// Token: 0x02000003 RID: 3
	public partial class EditDeviceWindow : Window
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020ED File Offset: 0x000002ED
		public EditDeviceWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020FB File Offset: 0x000002FB
		public EditDeviceWindow(Window owner, XdeDevice device, bool createNewDevice)
		{
			base.Owner = owner;
			base.DataContext = new EditDeviceViewModel(owner, device, createNewDevice);
			this.InitializeComponent();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000211E File Offset: 0x0000031E
		private bool IsValid(DependencyObject obj)
		{
			return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(new Func<DependencyObject, bool>(this.IsValid));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002146 File Offset: 0x00000346
		private void DownloadImageButton_Click(object sender, RoutedEventArgs e)
		{
			this.Model.ShowDownloadDialog();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002153 File Offset: 0x00000353
		private void RefreshDownloadImageButton_Click(object sender, RoutedEventArgs e)
		{
			this.Model.RefreshDownloadedImage();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002160 File Offset: 0x00000360
		private void DownloadLatestImageButton_Click(object sender, RoutedEventArgs e)
		{
			this.Model.DownloadLatestFromSource();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000216D File Offset: 0x0000036D
		private EditDeviceViewModel Model
		{
			get
			{
				return (EditDeviceViewModel)base.DataContext;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000217A File Offset: 0x0000037A
		protected override void OnClosing(CancelEventArgs e)
		{
			if (!this.Model.TryClosing())
			{
				e.Cancel = true;
				return;
			}
			base.OnClosing(e);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002198 File Offset: 0x00000398
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (!this.IsValid(this))
			{
				return;
			}
			if (this.Model.Save())
			{
				base.DialogResult = new bool?(true);
				base.Close();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021C3 File Offset: 0x000003C3
		private void ResetDiffDiskCheckpoints_Click(object sender, RoutedEventArgs e)
		{
			this.Model.ResetDiffDiskCheckpoints();
		}
	}
}
