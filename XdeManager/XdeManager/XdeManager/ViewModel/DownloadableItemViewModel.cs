using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;
using Windows.Services.Store;

namespace XdeManager.ViewModel
{
	// Token: 0x0200000D RID: 13
	public class DownloadableItemViewModel : ViewModelBase
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002D56 File Offset: 0x00000F56
		public DownloadableItemViewModel()
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002D69 File Offset: 0x00000F69
		public DownloadableItemViewModel(DownloadEmulatorsWindowViewModel parent, DownloadableImageInfo info)
		{
			this.parent = parent;
			this.info = info;
			this.info.PropertyChanged += this.Info_PropertyChanged;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002DA1 File Offset: 0x00000FA1
		public string Name
		{
			get
			{
				return this.info.Name;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002DAE File Offset: 0x00000FAE
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002DB6 File Offset: 0x00000FB6
		public string Version { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002DBF File Offset: 0x00000FBF
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002DC7 File Offset: 0x00000FC7
		public string PackageSize { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public bool Installed
		{
			get
			{
				return this.info.Downloaded;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002DDD File Offset: 0x00000FDD
		public bool Installing
		{
			get
			{
				return this.info.Installing;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002DEA File Offset: 0x00000FEA
		public bool CanCancel
		{
			get
			{
				return this.info.CanCancel;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002DF7 File Offset: 0x00000FF7
		public bool CanInstall
		{
			get
			{
				return this.info.CanInstall;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002E04 File Offset: 0x00001004
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002E0C File Offset: 0x0000100C
		public bool ShowingError
		{
			get
			{
				return this.showingError;
			}
			set
			{
				if (this.showingError != value)
				{
					this.showingError = value;
					this.OnPropertyChanged("ShowingError");
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002E29 File Offset: 0x00001029
		public RelayCommand InstallCommand
		{
			get
			{
				return new RelayCommand(delegate(object _)
				{
					this.InstallItem();
				});
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002E3C File Offset: 0x0000103C
		public RelayCommand CancelCommand
		{
			get
			{
				return new RelayCommand(delegate(object _)
				{
					this.CancelInstall();
				});
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002E4F File Offset: 0x0000104F
		public RelayCommand NavigateToStoreCommand
		{
			get
			{
				return new RelayCommand(delegate(object _)
				{
					this.NavigateToStore();
				});
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002E62 File Offset: 0x00001062
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00002E6A File Offset: 0x0000106A
		public double InstallPercent
		{
			get
			{
				return this.installPercent;
			}
			private set
			{
				if (value != this.installPercent)
				{
					this.installPercent = value;
					this.OnPropertyChanged("InstallPercent");
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002E87 File Offset: 0x00001087
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002E8F File Offset: 0x0000108F
		public string Status
		{
			get
			{
				return this.status;
			}
			private set
			{
				if (this.status != value)
				{
					this.status = value;
					this.OnPropertyChanged("Status");
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002EB1 File Offset: 0x000010B1
		public Visibility CommandsVisibility
		{
			get
			{
				if (this.parent.SelectedItem != this)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002EC4 File Offset: 0x000010C4
		internal void SelectionChanged()
		{
			this.OnPropertyChanged("CommandsVisibility");
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002ED1 File Offset: 0x000010D1
		private void CancelInstall()
		{
			this.Status = "Canceling...";
			this.info.CancelInstall();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002EE9 File Offset: 0x000010E9
		private void NavigateToStore()
		{
			Process.Start(new ProcessStartInfo("ms-windows-store://pdp/?productid=" + this.info.StoreId));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002F0C File Offset: 0x0000110C
		private void Info_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DownloadItemStatus")
			{
				string status = string.Empty;
				double percent = 0.0;
				IDownloadableItemStatus downloadItemStatus = this.info.DownloadItemStatus;
				bool installing = this.info.Installing;
				bool showingError = false;
				if (downloadItemStatus != null)
				{
					StorePackageUpdateStatus updateStatus = downloadItemStatus.UpdateStatus;
					if (installing)
					{
						if (updateStatus.PackageDownloadSizeInBytes != 0UL)
						{
							double num = updateStatus.PackageBytesDownloaded / 1073741824.0;
							double num2 = updateStatus.PackageDownloadSizeInBytes / 1073741824.0;
							status = StringUtilities.CurrentCultureFormat("Downloaded {0:F2} GB of {1:F2} GB", new object[]
							{
								num,
								num2
							});
							percent = updateStatus.PackageDownloadProgress;
						}
						else
						{
							status = "Starting download...";
						}
					}
					else if (downloadItemStatus.PackageInstallState == 2)
					{
						status = "Installation canceled.";
					}
					else if (downloadItemStatus.ExtendedError != null)
					{
						status = downloadItemStatus.ExtendedError.Message;
						showingError = true;
					}
					else
					{
						status = downloadItemStatus.PackageInstallState.ToString();
					}
				}
				Application.Current.Dispatcher.Invoke(delegate()
				{
					this.Status = status;
					this.InstallPercent = percent;
					this.ShowingError = showingError;
				});
				return;
			}
			if (e.PropertyName == "CanInstall")
			{
				this.OnPropertyChanged("CanInstall");
				return;
			}
			if (e.PropertyName == "CanCancel")
			{
				this.OnPropertyChanged("CanCancel");
				return;
			}
			if (e.PropertyName == "Installing")
			{
				this.OnPropertyChanged("Installing");
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000030C1 File Offset: 0x000012C1
		private void UpdateInstallProps()
		{
			this.OnPropertyChanged("Installing");
			this.OnPropertyChanged("Installed");
			this.OnPropertyChanged("CanInstall");
			this.OnPropertyChanged("CanCancel");
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000030EF File Offset: 0x000012EF
		private void InstallItem()
		{
			Application.Current.Dispatcher.Invoke(delegate()
			{
				this.ShowingError = false;
				this.Status = "Attempting installation...";
			});
			this.info.Install();
		}

		// Token: 0x04000026 RID: 38
		private readonly DownloadEmulatorsWindowViewModel parent;

		// Token: 0x04000027 RID: 39
		private readonly DownloadableImageInfo info;

		// Token: 0x04000028 RID: 40
		private double installPercent;

		// Token: 0x04000029 RID: 41
		private string status = string.Empty;

		// Token: 0x0400002A RID: 42
		private bool showingError;
	}
}
