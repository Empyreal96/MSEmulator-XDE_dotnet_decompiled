using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Services.Store;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000007 RID: 7
	public class DownloadableImageInfo : INotifyPropertyChanged
	{
		// Token: 0x06000036 RID: 54 RVA: 0x000026EF File Offset: 0x000008EF
		private DownloadableImageInfo(StoreContext storeContext, StoreProduct product, bool downloaded)
		{
			this.storeContext = storeContext;
			this.StoreProduct = product;
			this.Downloaded = downloaded;
			Task.Run(delegate()
			{
				this.RefreshProperties();
			});
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000271E File Offset: 0x0000091E
		public bool CanCancel
		{
			get
			{
				return this.storeQueueItem != null && !this.cancelRequested;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002733 File Offset: 0x00000933
		public bool Installing
		{
			get
			{
				return this.downloadTaskRunning || this.storeQueueItem != null;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002748 File Offset: 0x00000948
		public bool CanInstall
		{
			get
			{
				return !this.Downloaded && !this.Installing;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000275D File Offset: 0x0000095D
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002765 File Offset: 0x00000965
		public IDownloadableItemStatus DownloadItemStatus
		{
			get
			{
				return this.itemStatus;
			}
			set
			{
				this.itemStatus = value;
				if (this.itemStatus != null && this.itemStatus.PackageInstallState == 1)
				{
					this.Downloaded = true;
					XdeDeviceFactory.FireDeviceListChanged();
				}
				this.OnPropertyChanged("DownloadItemStatus");
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000279B File Offset: 0x0000099B
		public string Name
		{
			get
			{
				return this.StoreProduct.Title;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000027A8 File Offset: 0x000009A8
		// (set) Token: 0x0600003E RID: 62 RVA: 0x000027B0 File Offset: 0x000009B0
		public int PackageSize { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000027B9 File Offset: 0x000009B9
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000027C1 File Offset: 0x000009C1
		public bool Downloaded
		{
			get
			{
				return this.downloaded;
			}
			private set
			{
				if (this.downloaded != value)
				{
					this.downloaded = value;
					this.OnPropertyChanged("Downloaded");
					this.OnPropertyChanged("CanInstall");
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000027E9 File Offset: 0x000009E9
		public string ProductId
		{
			get
			{
				return this.StoreProduct.InAppOfferToken;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000027F6 File Offset: 0x000009F6
		public string StoreId
		{
			get
			{
				return this.StoreProduct.StoreId;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002803 File Offset: 0x00000A03
		public StoreProduct StoreProduct { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000044 RID: 68 RVA: 0x0000280C File Offset: 0x00000A0C
		// (remove) Token: 0x06000045 RID: 69 RVA: 0x00002844 File Offset: 0x00000A44
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000046 RID: 70 RVA: 0x0000287C File Offset: 0x00000A7C
		public static async Task<IEnumerable<DownloadableImageInfo>> LoadDownloadableImageInfos(StoreContext storeContext)
		{
			List<DownloadableImageInfo> infos = new List<DownloadableImageInfo>();
			StoreProductQueryResult storeProductQueryResult = await storeContext.GetAssociatedStoreProductsAsync(new string[]
			{
				"Durable"
			});
			foreach (KeyValuePair<string, StoreProduct> pair in storeProductQueryResult.Products)
			{
				bool flag = await pair.Value.GetIsAnySkuInstalledAsync();
				infos.Add(new DownloadableImageInfo(storeContext, pair.Value, flag));
				pair = default(KeyValuePair<string, StoreProduct>);
			}
			IEnumerator<KeyValuePair<string, StoreProduct>> enumerator = null;
			return infos;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000028C4 File Offset: 0x00000AC4
		public async void Install()
		{
			this.cancelRequested = false;
			this.downloadTaskRunning = true;
			this.OnPropertyChanged("Installing");
			this.OnPropertyChanged("CanInstall");
			if (!this.StoreProduct.IsInUserCollection)
			{
				StorePurchaseResult storePurchaseResult = await this.storeContext.RequestPurchaseAsync(this.StoreId);
				if (storePurchaseResult.ExtendedError != null)
				{
					this.SetGenericErrorMessageForInstall(storePurchaseResult.ExtendedError);
					return;
				}
				switch (storePurchaseResult.Status)
				{
				case 2:
					this.SetGenericErrorMessageForInstall("Image was not purchased.");
					return;
				case 3:
					this.SetGenericErrorMessageForInstall("Network error while acquiring image.");
					return;
				case 4:
					this.SetGenericErrorMessageForInstall("Server error while purchasing image.");
					return;
				}
			}
			await Task.Run(async delegate()
			{
				IAsyncOperationWithProgress<StorePackageUpdateResult, StorePackageUpdateStatus> downloadTask = this.storeContext.DownloadAndInstallStorePackagesAsync(new string[]
				{
					this.StoreId
				});
				downloadTask.put_Progress(delegate(IAsyncOperationWithProgress<StorePackageUpdateResult, StorePackageUpdateStatus> op, StorePackageUpdateStatus status)
				{
					this.DownloadItemStatus = new InstallItemStatus(0, status, null);
				});
				int num = 0;
				while (num < 5000 && downloadTask.Status == null)
				{
					this.RefreshProperties();
					if (this.storeQueueItem != null)
					{
						break;
					}
					Thread.Sleep(250);
					num += 250;
				}
				StoreQueueItem storeQueueItem = this.storeQueueItem;
				await WindowsRuntimeSystemExtensions.AsTask<StorePackageUpdateResult, StorePackageUpdateStatus>(downloadTask);
				this.NotifyInstallFinished();
				StorePackageUpdateResult results = downloadTask.GetResults();
				StoreQueueItem storeQueueItem2 = results.StoreQueueItems.FirstOrDefault<StoreQueueItem>();
				if (storeQueueItem2 != null)
				{
					StoreQueueItemStatus currentStatus = storeQueueItem2.GetCurrentStatus();
					this.DownloadItemStatus = new StoreQueueItemStatusWrapper(currentStatus);
					if (currentStatus.PackageInstallState == 3)
					{
						WindowsRuntimeSystemExtensions.AsTask(storeQueueItem2.CancelInstallAsync()).Wait();
					}
				}
				else
				{
					StoreQueueItemState installState = downloadTask.Status;
					Exception ex = downloadTask.ErrorCode;
					if (results.OverallState == 5)
					{
						installState = 3;
						if (ex == null)
						{
							ex = new Exception("Unknown error while trying to acquire image.");
						}
					}
					StorePackageUpdateStatus updateStatus;
					if (results.StorePackageUpdateStatuses != null && results.StorePackageUpdateStatuses.Count > 0)
					{
						updateStatus = results.StorePackageUpdateStatuses.First<StorePackageUpdateStatus>();
					}
					else
					{
						StorePackageUpdateStatus storePackageUpdateStatus = default(StorePackageUpdateStatus);
						storePackageUpdateStatus.PackageUpdateState = results.OverallState;
						updateStatus = storePackageUpdateStatus;
					}
					this.DownloadItemStatus = new InstallItemStatus(installState, updateStatus, ex);
				}
			});
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002900 File Offset: 0x00000B00
		public async void CancelInstall()
		{
			StoreQueueItem storeQueueItem = this.storeQueueItem;
			if (storeQueueItem != null)
			{
				this.cancelRequested = true;
				this.OnPropertyChanged("CanCancel");
				await storeQueueItem.CancelInstallAsync();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002939 File Offset: 0x00000B39
		private void SetGenericErrorMessageForInstall(string message)
		{
			this.SetGenericErrorMessageForInstall(new Exception(message));
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002948 File Offset: 0x00000B48
		private void SetGenericErrorMessageForInstall(Exception error)
		{
			this.NotifyInstallFinished();
			StorePackageUpdateStatus storePackageUpdateStatus = default(StorePackageUpdateStatus);
			storePackageUpdateStatus.PackageUpdateState = 5;
			StorePackageUpdateStatus updateStatus = storePackageUpdateStatus;
			this.DownloadItemStatus = new InstallItemStatus(3, updateStatus, error);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000297B File Offset: 0x00000B7B
		private void NotifyInstallFinished()
		{
			this.downloadTaskRunning = false;
			this.storeQueueItem = null;
			this.cancelRequested = false;
			this.OnPropertyChanged("Installing");
			this.OnPropertyChanged("CanCancel");
			this.OnPropertyChanged("CanInstall");
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000029B4 File Offset: 0x00000BB4
		private async void RefreshProperties()
		{
			bool flag = await this.StoreProduct.GetIsAnySkuInstalledAsync();
			this.Downloaded = flag;
			if (this.storeQueueItem == null)
			{
				this.storeQueueItem = (await this.storeContext.GetStoreQueueItemsAsync(new string[]
				{
					this.StoreProduct.StoreId
				})).FirstOrDefault<StoreQueueItem>();
				if (this.storeQueueItem != null)
				{
					if (this.storeQueueItem.GetCurrentStatus().PackageInstallState != null)
					{
						await this.storeQueueItem.CancelInstallAsync();
						this.storeQueueItem = null;
					}
					else
					{
						if (!this.downloadTaskRunning)
						{
							StoreQueueItem @object = this.storeQueueItem;
							WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<StoreQueueItem, object>>(new Func<TypedEventHandler<StoreQueueItem, object>, EventRegistrationToken>(@object.add_StatusChanged), new Action<EventRegistrationToken>(@object.remove_StatusChanged), new TypedEventHandler<StoreQueueItem, object>(this.StoreQueueItem_StatusChanged));
							@object = this.storeQueueItem;
							WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<StoreQueueItem, StoreQueueItemCompletedEventArgs>>(new Func<TypedEventHandler<StoreQueueItem, StoreQueueItemCompletedEventArgs>, EventRegistrationToken>(@object.add_Completed), new Action<EventRegistrationToken>(@object.remove_Completed), new TypedEventHandler<StoreQueueItem, StoreQueueItemCompletedEventArgs>(this.StoreQueueItem_Completed));
						}
						this.OnPropertyChanged("CanCancel");
						this.OnPropertyChanged("Installing");
					}
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000029ED File Offset: 0x00000BED
		private void StoreQueueItem_Completed(StoreQueueItem sender, StoreQueueItemCompletedEventArgs args)
		{
			this.NotifyInstallFinished();
			this.DownloadItemStatus = new StoreQueueItemStatusWrapper(args.Status);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002A06 File Offset: 0x00000C06
		private void StoreQueueItem_StatusChanged(StoreQueueItem sender, object args)
		{
			this.DownloadItemStatus = new StoreQueueItemStatusWrapper(sender.GetCurrentStatus());
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002A19 File Offset: 0x00000C19
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04000016 RID: 22
		private StoreContext storeContext;

		// Token: 0x04000017 RID: 23
		private StoreQueueItem storeQueueItem;

		// Token: 0x04000018 RID: 24
		private bool cancelRequested;

		// Token: 0x04000019 RID: 25
		private bool downloaded;

		// Token: 0x0400001A RID: 26
		private IDownloadableItemStatus itemStatus;

		// Token: 0x0400001B RID: 27
		private bool downloadTaskRunning;
	}
}
