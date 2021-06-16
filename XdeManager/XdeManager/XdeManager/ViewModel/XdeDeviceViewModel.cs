using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager.ViewModel
{
	// Token: 0x02000014 RID: 20
	public class XdeDeviceViewModel : ViewModelBase
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x000043DC File Offset: 0x000025DC
		public XdeDeviceViewModel(XdeManagerViewModel parent, XdeDevice device)
		{
			this.parent = parent;
			this.device = device;
			this.device.PropertyChanged += this.Device_PropertyChanged;
			this.EditCommand = new RelayCommand(new Action<object>(this.EditDevice), new Predicate<object>(this.CanEdit));
			this.StartCommand = new RelayCommand(new Action<object>(this.StartDevice), new Predicate<object>(this.CanStart));
			this.StartKernelDbgCommand = new RelayCommand(new Action<object>(this.StartKernelDbg), new Predicate<object>(this.CanStartKernelDbg));
			this.MakeVisibleInVisualStudioCommand = new RelayCommand(new Action<object>(this.MakeVisibleInVisualStudio), (object _) => !this.device.VisibleToVisualStudio);
			this.CloneCommand = new RelayCommand(new Action<object>(this.CloneDevice), new Predicate<object>(this.CanClone));
			this.DeleteCommand = new RelayCommand(new Action<object>(this.DeleteDevice));
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000044DA File Offset: 0x000026DA
		public Guid ID
		{
			get
			{
				return this.device.ID;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000044E7 File Offset: 0x000026E7
		public string Name
		{
			get
			{
				return this.device.Name;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000044F4 File Offset: 0x000026F4
		public string VhdPath
		{
			get
			{
				return this.device.Vhd;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004501 File Offset: 0x00002701
		public Visibility AddressVisibility
		{
			get
			{
				if (!this.device.IsXdeUIRunning)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004513 File Offset: 0x00002713
		public Visibility CommandsVisibility
		{
			get
			{
				if (this.parent.SelectedDevice != this)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004526 File Offset: 0x00002726
		public Visibility InternalVersionVisibility
		{
			get
			{
				if (!XdeAppUtils.IsInternalVersion)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004532 File Offset: 0x00002732
		public Visibility MakeVisibleInVisualStudioVisibility
		{
			get
			{
				if (this.device.VisibleToVisualStudio)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004544 File Offset: 0x00002744
		public string AddressInfo
		{
			get
			{
				if (!this.device.IsXdeUIRunning)
				{
					return null;
				}
				string text = this.device.IpAddress;
				if (string.IsNullOrEmpty(text))
				{
					text = "IP address not yet available";
				}
				return text;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000457B File Offset: 0x0000277B
		public SolidColorBrush HeadingBrush
		{
			get
			{
				if (!this.device.IsXdeUIRunning)
				{
					return XdeDeviceViewModel.StoppedBrush;
				}
				if (string.IsNullOrEmpty(this.device.IpAddress))
				{
					return XdeDeviceViewModel.StartingBrush;
				}
				return XdeDeviceViewModel.StartedBrush;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000045B0 File Offset: 0x000027B0
		public string VhdProperties
		{
			get
			{
				if (!this.device.IsVhdValidForLaunching)
				{
					return "The VHD path isn't set correctly. Edit the device to download a VHD or set it to an existing path.";
				}
				string text = this.device.Vhd;
				DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(this.device.Vhd);
				if (downloadedVhdInfo != null)
				{
					if (!string.IsNullOrEmpty(downloadedVhdInfo.BuildVersion) && !string.IsNullOrEmpty(downloadedVhdInfo.Branch))
					{
						text = string.Concat(new string[]
						{
							text,
							"\r\n",
							downloadedVhdInfo.BuildVersion,
							".",
							downloadedVhdInfo.Branch
						});
						SkuBuildInfo skuBuildInfoForSku = SkuBuildInfo.GetSkuBuildInfoForSku(this.device.Sku);
						if (skuBuildInfoForSku != null)
						{
							ImageInfo imageInfo = skuBuildInfoForSku.FindInfoForVhdFileName(this.device.Vhd);
							if (imageInfo != null)
							{
								text = text + " - " + imageInfo.Name;
							}
						}
					}
					else
					{
						text = text + "\r\n" + downloadedVhdInfo.UapVersion;
					}
				}
				return text;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004690 File Offset: 0x00002890
		public string DeviceProperties
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				XdeInstallation xdeInstallation = XdeInstallation.LoadFromPath(this.device.XdeLocation);
				if (xdeInstallation != null)
				{
					XdeSku xdeSku = xdeInstallation.Skus.FirstOrDefault((XdeSku s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, this.device.Sku));
					if (xdeSku != null)
					{
						XdeDeviceViewModel.AddTextToProps(stringBuilder, EditDeviceViewModel.GetSkuProperties(this.device, xdeSku));
						XdeSkin xdeSkin = xdeSku.FindSkin(this.device.Skin);
						if (xdeSkin != null)
						{
							XdeDeviceViewModel.AddTextToProps(stringBuilder, EditDeviceViewModel.GetSkinProperties(xdeSkin));
						}
						else
						{
							XdeDeviceViewModel.AddTextToProps(stringBuilder, "The skin is invalid. Edit the device to correct the problem.");
						}
					}
					else
					{
						XdeDeviceViewModel.AddTextToProps(stringBuilder, "The current sku is invalid. Edit the device to correct the problem.");
					}
				}
				else
				{
					XdeDeviceViewModel.AddTextToProps(stringBuilder, "Couldn't find the emulator installation for the device. Edit the device to correct the problem.");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00004732 File Offset: 0x00002932
		public ICommand EditCommand { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000473A File Offset: 0x0000293A
		public ICommand StartCommand { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004742 File Offset: 0x00002942
		public ICommand CloneCommand { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000474A File Offset: 0x0000294A
		public ICommand DeleteCommand { get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004752 File Offset: 0x00002952
		public ICommand StartKernelDbgCommand { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000475A File Offset: 0x0000295A
		public ICommand MakeVisibleInVisualStudioCommand { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004762 File Offset: 0x00002962
		public bool CanDelete
		{
			get
			{
				return this.device.CanDelete;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000476F File Offset: 0x0000296F
		internal void SelectionChanged()
		{
			this.OnPropertyChanged("CommandsVisibility");
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000477C File Offset: 0x0000297C
		protected override void OnDispose()
		{
			base.OnDispose();
			this.device.Dispose();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000478F File Offset: 0x0000298F
		private static void AddTextToProps(StringBuilder builder, string text)
		{
			if (builder.Length != 0)
			{
				builder.AppendLine();
			}
			builder.Append(text);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000047A8 File Offset: 0x000029A8
		private void Device_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged("DeviceProperties");
			this.OnPropertyChanged("HeadingBrush");
			this.OnPropertyChanged("AddressVisibility");
			this.OnPropertyChanged("AddressInfo");
			((RelayCommand)this.StartCommand).UpdateCanExecute();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000047E6 File Offset: 0x000029E6
		private bool CanEdit(object obj)
		{
			return true;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000047E9 File Offset: 0x000029E9
		private bool CanStart(object obj)
		{
			return this.device.IsVhdValidForLaunching && !this.device.IsXdeUIRunning;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004808 File Offset: 0x00002A08
		private bool CanStartKernelDbg(object obj)
		{
			return this.device.CanKernelDebug;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004815 File Offset: 0x00002A15
		private bool CanClone(object obj)
		{
			return true;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004818 File Offset: 0x00002A18
		private bool ShowDeviceEditor(bool clone)
		{
			return new EditDeviceWindow(this.parent.Owner, this.device, clone).ShowDialog().Value;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004849 File Offset: 0x00002A49
		private void EditDevice(object obj)
		{
			this.ShowDeviceEditor(false);
			this.parent.RefreshDevices();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000485E File Offset: 0x00002A5E
		private void CloneDevice(object obj)
		{
			this.ShowDeviceEditor(true);
			this.parent.RefreshDevices();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004874 File Offset: 0x00002A74
		private async void DeleteDevice(object obj)
		{
			IEnumerable<XdeDevice> enumerable = from d in this.device.RelatedDevices
			where d.IsXdeUIRunning
			select d;
			if (enumerable.Count<XdeDevice>() > 0)
			{
				string text = "The following device(s) must be stopped before this device can be deleted:\r\n";
				foreach (XdeDevice xdeDevice in enumerable)
				{
					text = text + "\r\n" + xdeDevice.Name;
				}
				MessageBox.Show(text, "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			else
			{
				string text2 = "Delete the following device(s)?\r\n";
				foreach (XdeDevice xdeDevice2 in this.device.RelatedDevices)
				{
					text2 = text2 + "\r\n" + xdeDevice2.Name;
				}
				if (this.device.AppShutsDownForDelete)
				{
					text2 += "\r\n\r\nWarning: If you delete this device Windows will forceably shut down this app and all emulator instances. You should shut down any running emulators before deleting this device.";
				}
				if (MessageBox.Show(text2, "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
				{
					try
					{
						Mouse.OverrideCursor = Cursors.Wait;
						await this.device.Delete();
						Mouse.OverrideCursor = null;
					}
					catch (Exception ex)
					{
						Mouse.OverrideCursor = null;
						MessageBox.Show("Failed to delete device(s): " + ex.Message, "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Hand);
						return;
					}
					this.parent.RefreshDevices();
				}
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000048AD File Offset: 0x00002AAD
		private void StartDevice(object obj)
		{
			this.device.Start(false);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000048BB File Offset: 0x00002ABB
		private void StartKernelDbg(object obj)
		{
			this.device.StartKernelDebugger();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000048CC File Offset: 0x00002ACC
		private void MakeVisibleInVisualStudio(object obj)
		{
			try
			{
				this.device.VisibleToVisualStudio = true;
			}
			catch (Exception ex)
			{
				if (ex is UnauthorizedAccessException && !UacSecurity.IsAdmin())
				{
					if (MessageBox.Show("In order to add Visual Studio integration with this device, this app must run elevated. Relaunch app as admin?", "Elevation Required", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
					{
						UacSecurity.RestartElevated(string.Empty, false);
						this.parent.Owner.Close();
					}
					return;
				}
				MessageBox.Show("Failed to write Visual Studio integration file:\r\n\r\n" + ex.Message, "Microsoft Emulator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			MessageBox.Show("Restart Visual Studio in order for the device to appear in the Visual Studio target dropdown.", "Microsoft Emulator", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			this.OnPropertyChanged("MakeVisibleInVisualStudioVisibility");
		}

		// Token: 0x04000048 RID: 72
		private XdeDevice device;

		// Token: 0x04000049 RID: 73
		private XdeManagerViewModel parent;

		// Token: 0x0400004A RID: 74
		private static SolidColorBrush StoppedBrush = new SolidColorBrush(Colors.Black);

		// Token: 0x0400004B RID: 75
		private static SolidColorBrush StartingBrush = new SolidColorBrush(Colors.Blue);

		// Token: 0x0400004C RID: 76
		private static SolidColorBrush StartedBrush = new SolidColorBrush(Colors.Green);
	}
}
