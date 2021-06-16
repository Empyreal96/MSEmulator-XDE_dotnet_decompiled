using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager.ViewModel
{
	// Token: 0x02000012 RID: 18
	public class OptionsViewModel : ViewModelBase, IDataErrorInfo
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x0000414C File Offset: 0x0000234C
		public OptionsViewModel()
		{
			this.BrowseForDownloadRootCommand = new RelayCommand(delegate(object _)
			{
				this.BrowseForDownloadRoot();
			}, (object _) => true);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000041A5 File Offset: 0x000023A5
		public ICommand BrowseForDownloadRootCommand { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000041AD File Offset: 0x000023AD
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000041B5 File Offset: 0x000023B5
		public string DownloadRoot
		{
			get
			{
				return this.downloadRoot;
			}
			set
			{
				if (value != this.downloadRoot)
				{
					this.downloadRoot = value;
					this.OnPropertyChanged("DownloadRoot");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000041D7 File Offset: 0x000023D7
		string IDataErrorInfo.Error
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000063 RID: 99
		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				string result = null;
				if (columnName == "DownloadRoot" && string.IsNullOrEmpty(this.DownloadRoot))
				{
					result = "Download root can't be null";
				}
				return result;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004210 File Offset: 0x00002410
		public bool Save()
		{
			if (!Directory.Exists(this.downloadRoot))
			{
				try
				{
					Directory.CreateDirectory(this.downloadRoot);
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show("Failed to create download root:\r\n\r\n" + ex.Message, "Microsoft Emulator Mananger", MessageBoxButton.OK, MessageBoxImage.Hand);
					return false;
				}
			}
			XdeManagerSettings.Current.DownloadRoot = this.downloadRoot;
			XdeManagerSettings.Current.Save();
			return true;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004288 File Offset: 0x00002488
		private void BrowseForDownloadRoot()
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.SelectedPath = this.DownloadRoot;
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					this.DownloadRoot = folderBrowserDialog.SelectedPath;
				}
			}
		}

		// Token: 0x04000043 RID: 67
		private string downloadRoot = XdeManagerSettings.Current.DownloadRoot;
	}
}
