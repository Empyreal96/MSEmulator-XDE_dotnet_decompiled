using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;
using Windows.Services.Store;

namespace XdeManager.ViewModel
{
	// Token: 0x0200000C RID: 12
	public class DownloadEmulatorsWindowViewModel : ViewModelBase
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002C55 File Offset: 0x00000E55
		public DownloadEmulatorsWindowViewModel(Window owner)
		{
			this.owner = owner;
			this.mainStatus = "Looking for emulator images in the store...";
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002C7A File Offset: 0x00000E7A
		public ObservableCollection<DownloadableItemViewModel> Items { get; } = new ObservableCollection<DownloadableItemViewModel>();

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002C82 File Offset: 0x00000E82
		public StoreContext StoreContext
		{
			get
			{
				return this.storeContext;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002C8A File Offset: 0x00000E8A
		public bool HasItems
		{
			get
			{
				return this.Items.Count != 0;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002C9A File Offset: 0x00000E9A
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public string MainStatus
		{
			get
			{
				return this.mainStatus;
			}
			set
			{
				if (this.mainStatus != value)
				{
					this.mainStatus = value;
					this.OnPropertyChanged("MainStatus");
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002CC4 File Offset: 0x00000EC4
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002CCC File Offset: 0x00000ECC
		public DownloadableItemViewModel SelectedItem
		{
			get
			{
				return this.selectedItem;
			}
			set
			{
				if (this.selectedItem != value)
				{
					DownloadableItemViewModel downloadableItemViewModel = this.selectedItem;
					this.selectedItem = value;
					this.OnPropertyChanged("SelectedItem");
					if (downloadableItemViewModel != null)
					{
						downloadableItemViewModel.SelectionChanged();
					}
					if (value != null)
					{
						value.SelectionChanged();
					}
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002D10 File Offset: 0x00000F10
		public async void LoadItems()
		{
			IntPtr handle = new WindowInteropHelper(this.owner).Handle;
			this.storeContext = StoreUtils.GetDefaultContext(handle);
			using (IEnumerator<DownloadableImageInfo> enumerator = (from i in await DownloadableImageInfo.LoadDownloadableImageInfos(this.storeContext)
			where !i.Downloaded
			select i).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DownloadableImageInfo item = enumerator.Current;
					Application.Current.Dispatcher.Invoke(delegate()
					{
						this.Items.Add(new DownloadableItemViewModel(this, item));
						if (this.Items.Count == 1)
						{
							this.SelectedItem = this.Items[0];
							this.OnPropertyChanged("HasItems");
						}
					});
				}
			}
			if (this.Items.Count == 0)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					this.MainStatus = "No emulator images found in the store that haven't already been installed.";
				});
			}
		}

		// Token: 0x04000021 RID: 33
		private StoreContext storeContext;

		// Token: 0x04000022 RID: 34
		private DownloadableItemViewModel selectedItem;

		// Token: 0x04000023 RID: 35
		private string mainStatus;

		// Token: 0x04000024 RID: 36
		private Window owner;
	}
}
