using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Xde.DeviceManagement;
using XdeManager.ViewModel;

namespace XdeManager
{
	// Token: 0x02000004 RID: 4
	public partial class DownloadBranchImageWindow : Window
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022E9 File Offset: 0x000004E9
		public ICommand CopyCommand { get; }

		// Token: 0x06000013 RID: 19 RVA: 0x000022F1 File Offset: 0x000004F1
		public DownloadBranchImageWindow(XdeDevice device)
		{
			base.DataContext = new DownloadBranchImageViewModel(this, device);
			this.InitializeComponent();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000230C File Offset: 0x0000050C
		public DownloadBranchImageViewModel Model
		{
			get
			{
				return (DownloadBranchImageViewModel)base.DataContext;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002319 File Offset: 0x00000519
		private bool IsValid(DependencyObject obj)
		{
			return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(new Func<DependencyObject, bool>(this.IsValid));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002344 File Offset: 0x00000544
		private void CopyButton_Click(object sender, RoutedEventArgs args)
		{
			if (!this.IsValid(this))
			{
				return;
			}
			if (!Directory.Exists(this.Model.DownloadFolder))
			{
				try
				{
					Directory.CreateDirectory(this.Model.DownloadFolder);
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show("Failed to create download folder: " + ex.Message, "Download Failed", MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
			}
			try
			{
				if (EditDeviceViewModel.CopyVhdSkipIfSame(this.Model.CurrentImagePath, this.Model.DownloadedImagePath))
				{
					base.DialogResult = new bool?(true);
					base.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023F4 File Offset: 0x000005F4
		private void BrowseForDestFolderButton_Click(object sender, RoutedEventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.SelectedPath = this.Model.DownloadFolder;
				if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					this.Model.DownloadFolder = folderBrowserDialog.SelectedPath;
				}
			}
		}
	}
}
