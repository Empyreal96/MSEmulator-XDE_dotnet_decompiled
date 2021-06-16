using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager.ViewModel
{
	// Token: 0x0200000F RID: 15
	public class EditDeviceViewModel : ViewModelBase, IDataErrorInfo
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000035BC File Offset: 0x000017BC
		public EditDeviceViewModel(Window owner, XdeDevice device, bool createNewDevice)
		{
			this.Owner = owner;
			if (createNewDevice)
			{
				XdeDevice xdeDevice = XdeDeviceFactory.InitNewDevice();
				if (device != null)
				{
					xdeDevice.Name = device.Name + " (cloned)";
					xdeDevice.Vhd = device.Vhd;
					xdeDevice.UseDiffDisk = device.UseDiffDisk;
					xdeDevice.MemSize = device.MemSize;
					xdeDevice.Sku = device.Sku;
					xdeDevice.Skin = device.Skin;
					xdeDevice.UseCheckpoint = device.UseCheckpoint;
					xdeDevice.ShowDisplayName = device.ShowDisplayName;
				}
				else
				{
					XdeInstallation appInstallXdeInstallation = XdeInstallation.GetAppInstallXdeInstallation();
					XdeSku xdeSku = appInstallXdeInstallation.Skus.FirstOrDefault((XdeSku s) => s.Name == "sant");
					if (xdeSku != null)
					{
						xdeDevice = appInstallXdeInstallation.CreateDeviceForSku(xdeSku);
						this.createdDevice = true;
					}
					else
					{
						xdeDevice.MemSize = 4096;
					}
					xdeDevice.Name = "New device";
				}
				device = xdeDevice;
			}
			this.Title = (createNewDevice ? "Save New Emulator Device" : "Edit Emulator Device");
			this.BrowseForVhdCommand = new RelayCommand(new Action<object>(this.BrowseForVhd), new Predicate<object>(this.CanAlwaysExecute));
			this.Device = device;
			this.currentXdeInstall = device.GetXdeInstallation();
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000036FA File Offset: 0x000018FA
		public bool IsInternal
		{
			get
			{
				return XdeAppUtils.IsInternalVersion;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003701 File Offset: 0x00001901
		public Visibility IsInternalVisbility
		{
			get
			{
				if (!this.IsInternal)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000370E File Offset: 0x0000190E
		public bool CanEditNoGpu
		{
			get
			{
				return this.Device.CanModifyProperty("NoGpu");
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003720 File Offset: 0x00001920
		public bool CanEditSku
		{
			get
			{
				return this.Device.CanModifyProperty("Sku");
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003732 File Offset: 0x00001932
		public bool CanEditSkin
		{
			get
			{
				return this.Device.CanModifyProperty("Skin");
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003744 File Offset: 0x00001944
		public bool CanEditUseWmi
		{
			get
			{
				return this.Device.CanModifyProperty("UseWmi");
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003756 File Offset: 0x00001956
		public bool CanEditShowDisplayName
		{
			get
			{
				return this.Device.CanModifyProperty("ShowDisplayName");
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003768 File Offset: 0x00001968
		public bool CanEditDisableStateSep
		{
			get
			{
				return this.Device.CanModifyProperty("DisableStateSep");
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000377A File Offset: 0x0000197A
		public bool CanEditUseDiffDisk
		{
			get
			{
				return this.Device.CanModifyProperty("UseDiffDisk");
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000378C File Offset: 0x0000198C
		public bool CanEditMemSize
		{
			get
			{
				return this.Device.CanModifyProperty("MemSize");
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000379E File Offset: 0x0000199E
		public bool CanEditUseCheckpoint
		{
			get
			{
				return this.Device.CanModifyProperty("UseCheckpoint");
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000037B0 File Offset: 0x000019B0
		public bool CanEditVhd
		{
			get
			{
				return this.Device.CanModifyProperty("Vhd");
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000037C2 File Offset: 0x000019C2
		public bool CantEditVhd
		{
			get
			{
				return !this.CanEditVhd;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000037CD File Offset: 0x000019CD
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000037D9 File Offset: 0x000019D9
		public Visibility DownloadButtonsVisibility
		{
			get
			{
				if (!XdeAppUtils.IsInternalVersion || !this.Device.CanModifyProperty("Vhd"))
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000037F8 File Offset: 0x000019F8
		public void DownloadLatestFromSource()
		{
			DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(this.Device.Vhd);
			if (downloadedVhdInfo == null)
			{
				return;
			}
			string latestVhdFileNameFromSource = downloadedVhdInfo.GetLatestVhdFileNameFromSource();
			if (latestVhdFileNameFromSource == null)
			{
				MessageBox.Show("Unable to find the latest VHD on \\\\winbuilds\\release\\" + downloadedVhdInfo.Branch, "XDE Device Manager", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			EditDeviceViewModel.CopyVhdSkipIfSame(latestVhdFileNameFromSource, this.Vhd);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003850 File Offset: 0x00001A50
		public void ResetDiffDiskCheckpoints()
		{
			if (MessageBox.Show("Delete any existing diff disk and checkpoints? This will force a clean reboot the next time you start the device.", "XDE Device Manager", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				try
				{
					this.Device.DeleteVirtualMachine();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to reset device:\r\n\r\n" + ex.Message, "XDE Device Manager", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000038B0 File Offset: 0x00001AB0
		public bool TryClosing()
		{
			if (!this.Device.IsDirty)
			{
				return true;
			}
			MessageBoxResult messageBoxResult = MessageBox.Show("Save device properties before exiting?", "Device has been modified", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				return this.Save();
			}
			if (messageBoxResult == MessageBoxResult.No)
			{
				if (this.createdDevice)
				{
					this.Device.Delete();
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003908 File Offset: 0x00001B08
		public void ShowDownloadDialog()
		{
			DownloadBranchImageWindow downloadBranchImageWindow = new DownloadBranchImageWindow(this.Device)
			{
				Owner = this.Owner
			};
			downloadBranchImageWindow.ShowDialog();
			if (downloadBranchImageWindow.DialogResult != null && downloadBranchImageWindow.DialogResult.Value)
			{
				this.Vhd = downloadBranchImageWindow.Model.DownloadedImagePath;
				this.OnPropertyChanged("VhdSource");
				this.OnPropertyChanged("CanRefreshDownload");
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000397B File Offset: 0x00001B7B
		private Window Owner { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003983 File Offset: 0x00001B83
		public string Title { get; }

		// Token: 0x060000AD RID: 173 RVA: 0x0000398C File Offset: 0x00001B8C
		public static string GetSkuProperties(XdeDevice device, XdeSku sku)
		{
			if (sku == null)
			{
				return string.Empty;
			}
			bool flag = sku.UseHcsIfAvailable;
			if (device != null)
			{
				flag = (flag && !device.UseWmi);
			}
			string arg = flag ? "HCS (not visible in Hyper-V Manager)" : "WMI";
			string arg2;
			if (!flag)
			{
				arg2 = "Not available when using WMI";
			}
			else if (device != null && device.NoGpu)
			{
				arg2 = "Disabled on device settings.";
			}
			else
			{
				arg2 = (sku.UseGpu ? "True" : "Disabled by SKU.");
			}
			return string.Format("Processor count: {0}\r\nVM type: {1}\r\nUse GPU: {2}", sku.ProcessorCount, arg, arg2);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003A18 File Offset: 0x00001C18
		public bool Save()
		{
			bool result;
			try
			{
				this.Device.Save();
				result = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Concat(new string[]
				{
					"Failed to save ",
					this.Device.FileName,
					":\r\n\r\n",
					ex.Message,
					"\r\n\r\nYou may need to run as admin to write to this file."
				}), "XDE Device Manager", MessageBoxButton.OK, MessageBoxImage.Hand);
				result = false;
			}
			return result;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003A94 File Offset: 0x00001C94
		public XdeDevice Device { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003A9C File Offset: 0x00001C9C
		public ICommand BrowseForVhdCommand { get; }

		// Token: 0x060000B1 RID: 177 RVA: 0x00003AA4 File Offset: 0x00001CA4
		private bool CanAlwaysExecute(object obj)
		{
			return true;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003AA7 File Offset: 0x00001CA7
		private void Cancel(object _)
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003AA9 File Offset: 0x00001CA9
		private void Save(object _)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003AAC File Offset: 0x00001CAC
		private void BrowseForVhd(object obj)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "VHDX files (*.vhdx)|*.vhdx";
			bool? flag = openFileDialog.ShowDialog(this.Owner);
			bool flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				this.Vhd = openFileDialog.FileName;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003AF9 File Offset: 0x00001CF9
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003B06 File Offset: 0x00001D06
		public bool UseDiffDisk
		{
			get
			{
				return this.Device.UseDiffDisk;
			}
			set
			{
				if (value != this.Device.UseDiffDisk)
				{
					this.Device.UseDiffDisk = value;
					this.OnPropertyChanged("UseDiffDisk");
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003B2D File Offset: 0x00001D2D
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003B3A File Offset: 0x00001D3A
		public int MemSize
		{
			get
			{
				return this.Device.MemSize;
			}
			set
			{
				if (value != this.Device.MemSize)
				{
					this.Device.MemSize = value;
					this.OnPropertyChanged("MemSize");
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003B61 File Offset: 0x00001D61
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003B6E File Offset: 0x00001D6E
		public string Name
		{
			get
			{
				return this.Device.Name;
			}
			set
			{
				if (value != this.Device.Name)
				{
					this.Device.Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003B9C File Offset: 0x00001D9C
		public string SkuProperties
		{
			get
			{
				XdeSku currentSku = this.GetCurrentSku();
				return EditDeviceViewModel.GetSkuProperties(null, currentSku);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003BB7 File Offset: 0x00001DB7
		public string SkinProperties
		{
			get
			{
				return EditDeviceViewModel.GetSkinProperties(this.GetCurrentSkin());
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003BC4 File Offset: 0x00001DC4
		public static string GetSkinProperties(XdeSkin skin)
		{
			if (skin == null)
			{
				return string.Empty;
			}
			return string.Format("Displays: {0} @ {1}x{2}", skin.DisplayCount, skin.DisplayWidth, skin.DisplayHeight);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003BFA File Offset: 0x00001DFA
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003C08 File Offset: 0x00001E08
		public string Vhd
		{
			get
			{
				return this.Device.Vhd;
			}
			set
			{
				if (value != this.Device.Vhd)
				{
					this.Device.Vhd = value;
					this.OnPropertyChanged("Vhd");
					this.OnPropertyChanged("VhdSource");
					this.OnPropertyChanged("CanRefreshDownload");
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003C55 File Offset: 0x00001E55
		public bool CanRefreshDownload
		{
			get
			{
				return DownloadedVhdInfo.LoadInfoForDownloadedVhd(this.Device.Vhd) != null;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003C6C File Offset: 0x00001E6C
		public string VhdSource
		{
			get
			{
				if (string.IsNullOrEmpty(this.Device.Vhd))
				{
					return string.Empty;
				}
				DownloadedVhdInfo downloadedVhdInfo = DownloadedVhdInfo.LoadInfoForDownloadedVhd(this.Device.Vhd);
				if (downloadedVhdInfo != null)
				{
					return downloadedVhdInfo.Source;
				}
				return "Unknown";
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003CB1 File Offset: 0x00001EB1
		public bool CanTakeCheckpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003CB4 File Offset: 0x00001EB4
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003CC1 File Offset: 0x00001EC1
		public bool UseCheckpoint
		{
			get
			{
				return this.Device.UseCheckpoint;
			}
			set
			{
				if (value != this.Device.UseCheckpoint)
				{
					this.Device.UseCheckpoint = value;
					this.OnPropertyChanged("UseCheckpoint");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003CE8 File Offset: 0x00001EE8
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003CF5 File Offset: 0x00001EF5
		public bool ShowDisplayName
		{
			get
			{
				return this.Device.ShowDisplayName;
			}
			set
			{
				if (value != this.Device.ShowDisplayName)
				{
					this.Device.ShowDisplayName = value;
					this.OnPropertyChanged("ShowDisplayName");
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003D1C File Offset: 0x00001F1C
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003D29 File Offset: 0x00001F29
		public bool UseWmi
		{
			get
			{
				return this.Device.UseWmi;
			}
			set
			{
				if (value != this.Device.UseWmi)
				{
					this.Device.UseWmi = value;
					this.OnPropertyChanged("UseWmi");
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003D50 File Offset: 0x00001F50
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00003D5D File Offset: 0x00001F5D
		public bool NoGpu
		{
			get
			{
				return this.Device.NoGpu;
			}
			set
			{
				if (value != this.Device.NoGpu)
				{
					this.Device.NoGpu = value;
					this.OnPropertyChanged("NoGpu");
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00003D84 File Offset: 0x00001F84
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00003D91 File Offset: 0x00001F91
		public bool DisableStateSep
		{
			get
			{
				return this.Device.DisableStateSep;
			}
			set
			{
				if (value != this.Device.DisableStateSep)
				{
					this.Device.DisableStateSep = value;
					this.OnPropertyChanged("DisableStateSep");
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public IReadOnlyCollection<XdeSku> Skus
		{
			get
			{
				if (this.currentXdeInstall == null)
				{
					return new XdeSku[0];
				}
				return this.currentXdeInstall.Skus;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003DD4 File Offset: 0x00001FD4
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003DDC File Offset: 0x00001FDC
		public XdeSku CurrentSku
		{
			get
			{
				return this.GetCurrentSku();
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.Device.Sku = value.Name;
				if (this.GetCurrentSku() == null && this.currentXdeInstall != null && this.currentXdeInstall.Skus.Count != 0)
				{
					this.Device.Sku = this.currentXdeInstall.Skus.First<XdeSku>().Name;
				}
				this.OnPropertyChanged("CurrentSku");
				this.OnPropertyChanged("SkuProperties");
				this.OnPropertyChanged("Skins");
				this.OnPropertyChanged("MemSizes");
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003E6C File Offset: 0x0000206C
		public IEnumerable<int> MemSizes
		{
			get
			{
				XdeSku sku = this.GetCurrentSku();
				if (sku == null)
				{
					yield break;
				}
				for (int i = sku.MinMemSize; i <= sku.MaxMemSize; i += 1024)
				{
					yield return i;
				}
				yield break;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003E7C File Offset: 0x0000207C
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003E84 File Offset: 0x00002084
		public XdeSkin CurrentSkin
		{
			get
			{
				return this.GetCurrentSkin();
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.Device.Skin = value.Name;
				if (this.GetCurrentSkin() == null)
				{
					XdeSku currentSku = this.GetCurrentSku();
					if (currentSku != null && currentSku.Skins.Count != 0)
					{
						this.Device.Skin = currentSku.Skins.First<XdeSkin>().Name;
					}
				}
				this.OnPropertyChanged("CurrentSkin");
				this.OnPropertyChanged("SkinProperties");
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003EF8 File Offset: 0x000020F8
		public static bool CopyVhdSkipIfSame(string source, string vhd)
		{
			if (FileUtils.AreFilesSame(source, vhd))
			{
				MessageBox.Show("The destination file is up to date with the source:\r\n\r\nSource: " + source + "\r\n\r\nDestination: " + vhd, "Skipped Copying Files", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
			else
			{
				try
				{
					FileSystem.CopyFile(source, vhd, UIOption.AllDialogs);
				}
				catch (Exception)
				{
					return false;
				}
			}
			DownloadedVhdInfo.RecordDownloadedVhdInfo(source, vhd);
			return true;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003F5C File Offset: 0x0000215C
		public void RefreshDownloadedImage()
		{
			string vhdSource = this.VhdSource;
			string vhd = this.Vhd;
			EditDeviceViewModel.CopyVhdSkipIfSame(vhdSource, vhd);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003F7D File Offset: 0x0000217D
		private XdeSku GetCurrentSku()
		{
			if (this.currentXdeInstall == null)
			{
				return null;
			}
			return this.currentXdeInstall.Skus.FirstOrDefault((XdeSku s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, this.Device.Sku));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003FA8 File Offset: 0x000021A8
		private XdeSkin GetCurrentSkin()
		{
			XdeSku currentSku = this.GetCurrentSku();
			if (currentSku == null)
			{
				return null;
			}
			return currentSku.FindSkin(this.Device.Skin);
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003FD4 File Offset: 0x000021D4
		public IReadOnlyCollection<XdeSkin> Skins
		{
			get
			{
				if (this.currentXdeInstall == null)
				{
					return new XdeSkin[0];
				}
				XdeSku currentSku = this.GetCurrentSku();
				if (currentSku == null)
				{
					return new XdeSkin[0];
				}
				return currentSku.Skins;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004007 File Offset: 0x00002207
		string IDataErrorInfo.Error
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700005C RID: 92
		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				string result = null;
				if (!(columnName == "CurrentSku"))
				{
					if (!(columnName == "CurrentSkin"))
					{
						if (!(columnName == "Name"))
						{
							if (!(columnName == "MemSize"))
							{
								if (columnName == "Vhd")
								{
									if (string.IsNullOrEmpty(this.Vhd))
									{
										result = "Vhd can't be null";
									}
									if (!File.Exists(this.Vhd))
									{
										result = "Vhd must exist";
									}
								}
							}
							else if (this.MemSize < 1024)
							{
								result = "MemSize must be 1024 or greater";
							}
						}
						else if (string.IsNullOrEmpty(this.Name))
						{
							result = "Name can't be null";
						}
					}
					else if (this.CurrentSkin == null)
					{
						result = "Skin can't be null";
					}
				}
				else if (this.CurrentSku == null)
				{
					result = "Sku can't be null";
				}
				return result;
			}
		}

		// Token: 0x0400003A RID: 58
		private XdeInstallation currentXdeInstall;

		// Token: 0x0400003B RID: 59
		private bool createdDevice;
	}
}
