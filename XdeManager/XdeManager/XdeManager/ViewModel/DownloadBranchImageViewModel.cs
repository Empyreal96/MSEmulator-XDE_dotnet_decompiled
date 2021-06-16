using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xde.DeviceManagement;
using XdeManager.Properties;

namespace XdeManager.ViewModel
{
	// Token: 0x0200000E RID: 14
	public class DownloadBranchImageViewModel : ViewModelBase, IDataErrorInfo
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00003144 File Offset: 0x00001344
		public DownloadBranchImageViewModel(Window owner, XdeDevice device)
		{
			this.owner = owner;
			this.device = device;
			ReadOnlyCollection<SkuBuildInfo> skuBuildInfos = SkuBuildInfo.GetSkuBuildInfos();
			this.skuBuildInfo = skuBuildInfos.FirstOrDefault((SkuBuildInfo s) => StringComparer.OrdinalIgnoreCase.Equals(s.SkuName, this.device.Sku));
			if (this.skuBuildInfo != null)
			{
				this.currentImageInfo = this.skuBuildInfo.FindInfoForVhdFileName(device.Vhd);
			}
			if (this.currentImageInfo == null)
			{
				this.currentImageInfo = this.skuBuildInfo.ImageInfos.FirstOrDefault<ImageInfo>();
			}
			this.DownloadFolder = Path.Combine(XdeManagerSettings.Current.DownloadRoot, this.device.Name);
			this.CurrentBranch = Settings.Default.LastUsedBranch;
			if (string.IsNullOrEmpty(this.CurrentBranch) && this.Branches.Count != 0)
			{
				this.CurrentBranch = this.Branches[0];
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000324C File Offset: 0x0000144C
		public ReadOnlyCollection<ImageInfo> ImageInfos
		{
			get
			{
				if (this.skuBuildInfo == null)
				{
					return null;
				}
				return this.skuBuildInfo.ImageInfos;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003263 File Offset: 0x00001463
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000326B File Offset: 0x0000146B
		public ImageInfo SelectedImageInfo
		{
			get
			{
				return this.currentImageInfo;
			}
			set
			{
				if (this.currentImageInfo != value)
				{
					this.currentImageInfo = value;
					this.RefreshImagePaths();
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003283 File Offset: 0x00001483
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000328B File Offset: 0x0000148B
		public string DownloadFolder
		{
			get
			{
				return this.downloadFolder;
			}
			set
			{
				if (this.downloadFolder != value)
				{
					this.downloadFolder = value;
					this.OnPropertyChanged("DownloadFolder");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000032AD File Offset: 0x000014AD
		public string DownloadedImagePath
		{
			get
			{
				return Path.Combine(this.DownloadFolder, Path.GetFileName(this.CurrentImagePath));
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000032C5 File Offset: 0x000014C5
		public ObservableCollection<string> FoundImagePaths { get; } = new ObservableCollection<string>();

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000032CD File Offset: 0x000014CD
		public ObservableCollection<string> Branches { get; } = new ObservableCollection<string>(Settings.Default.FavoriteBranches.Cast<string>());

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000032D5 File Offset: 0x000014D5
		public ObservableCollection<string> ImageTypes { get; } = new ObservableCollection<string>();

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000032DD File Offset: 0x000014DD
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000032E5 File Offset: 0x000014E5
		public string CurrentBranch
		{
			get
			{
				return this.currentBranch;
			}
			set
			{
				if (this.currentBranch != value)
				{
					this.currentBranch = value;
					Settings.Default.LastUsedBranch = value;
					this.OnPropertyChanged("CurrentBranch");
					this.RefreshImagePaths();
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003318 File Offset: 0x00001518
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003320 File Offset: 0x00001520
		public string CurrentImageType
		{
			get
			{
				return this.currentImageType;
			}
			set
			{
				if (this.currentImageType != value)
				{
					this.currentImageType = value;
					this.RefreshImagePaths();
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000333D File Offset: 0x0000153D
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00003345 File Offset: 0x00001545
		public string CurrentImagePath
		{
			get
			{
				return this.currentImagePath;
			}
			set
			{
				if (this.currentImagePath != value)
				{
					this.currentImagePath = value;
					this.OnPropertyChanged("CurrentImagePath");
				}
			}
		}

		// Token: 0x17000033 RID: 51
		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				string result = null;
				if (!(columnName == "DownloadFolder"))
				{
					if (columnName == "CurrentImagePath")
					{
						if (string.IsNullOrEmpty(this.CurrentImagePath) || !File.Exists(this.CurrentImagePath))
						{
							result = "Image path must exist";
						}
					}
				}
				else if (string.IsNullOrEmpty(this.DownloadFolder) || !DownloadBranchImageViewModel.IsValidPath(this.DownloadFolder))
				{
					result = "Download path must exist and be valid.";
				}
				return result;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000033D6 File Offset: 0x000015D6
		string IDataErrorInfo.Error
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000033DD File Offset: 0x000015DD
		protected override void OnDispose()
		{
			this.DisposeTokenSource();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000033E8 File Offset: 0x000015E8
		private static bool IsValidPath(string path)
		{
			if (!new Regex("^[a-zA-Z]:\\\\$").IsMatch(path.Substring(0, 3)))
			{
				return false;
			}
			string text = new string(Path.GetInvalidPathChars());
			text += ":/?*\"";
			return !new Regex("[" + Regex.Escape(text) + "]").IsMatch(path.Substring(3, path.Length - 3));
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000345A File Offset: 0x0000165A
		private void DisposeTokenSource()
		{
			if (this.cancelTokenSource != null)
			{
				this.cancelTokenSource.Dispose();
				this.cancelTokenSource = null;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003478 File Offset: 0x00001678
		private void RefreshImagePaths()
		{
			if (this.refreshTask != null)
			{
				this.cancelTokenSource.Cancel();
				this.refreshTask.Wait();
				this.DisposeTokenSource();
			}
			this.FoundImagePaths.Clear();
			if (string.IsNullOrEmpty(this.CurrentBranch))
			{
				return;
			}
			this.cancelTokenSource = new CancellationTokenSource();
			this.refreshTask = Task.Factory.StartNew(delegate()
			{
				this.RefreshImagesTask(this.cancelTokenSource.Token);
			});
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000034EC File Offset: 0x000016EC
		private void RefreshImagesTask(CancellationToken cancellationToken)
		{
			ImageInfo imageInfo = this.currentImageInfo;
			if (imageInfo == null || string.IsNullOrEmpty(imageInfo.Location))
			{
				return;
			}
			using (IEnumerator<string> enumerator = DownloadedVhdInfo.GetLatestVhdFileNamesFromBranch(this.CurrentBranch, imageInfo.Location, 20, cancellationToken).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string vhdxPath = enumerator.Current;
					this.owner.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate()
					{
						this.FoundImagePaths.Add(vhdxPath);
						if (this.FoundImagePaths.Count == 1)
						{
							this.CurrentImagePath = vhdxPath;
						}
					}));
				}
			}
		}

		// Token: 0x0400002D RID: 45
		private string downloadFolder;

		// Token: 0x0400002E RID: 46
		private string currentBranch;

		// Token: 0x0400002F RID: 47
		private string currentImagePath;

		// Token: 0x04000030 RID: 48
		private Window owner;

		// Token: 0x04000031 RID: 49
		private XdeDevice device;

		// Token: 0x04000032 RID: 50
		private SkuBuildInfo skuBuildInfo;

		// Token: 0x04000033 RID: 51
		private ImageInfo currentImageInfo;

		// Token: 0x04000034 RID: 52
		private string currentImageType;

		// Token: 0x04000035 RID: 53
		private Task refreshTask;

		// Token: 0x04000036 RID: 54
		private CancellationTokenSource cancelTokenSource;
	}
}
