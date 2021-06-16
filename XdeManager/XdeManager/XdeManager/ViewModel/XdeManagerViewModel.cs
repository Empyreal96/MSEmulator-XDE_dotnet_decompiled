using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;

namespace XdeManager.ViewModel
{
	// Token: 0x02000016 RID: 22
	public class XdeManagerViewModel : ViewModelBase
	{
		// Token: 0x0600011E RID: 286 RVA: 0x000049D4 File Offset: 0x00002BD4
		public XdeManagerViewModel(Window owner, EnumerateDevices devicesEnum)
		{
			this.devicesEnum = devicesEnum;
			this.Owner = owner;
			this.NewDeviceCommand = new RelayCommand(new Action<object>(this.NewDevice), (object _) => XdeAppUtils.IsInternalVersion);
			this.DownloadEmulatorImagesCommand = new RelayCommand(new Action<object>(this.ShowDownloadEmulatorImages), (object _) => true);
			this.RefreshCommand = new RelayCommand(delegate(object _)
			{
				this.RefreshDevices();
			}, new Predicate<object>(this.CanRefresh));
			this.OptionsCommand = new RelayCommand(new Action<object>(this.ShowOptions), (object _) => true);
			this.CleanCoreconCommand = new RelayCommand(new Action<object>(this.CleanCorecon), (object _) => true);
			this.SendFeedbackProblemCommand = new RelayCommand(delegate(object _)
			{
				this.SendFeedback(true);
			}, (object _) => true);
			this.SendFeedbackSuggestionCommand = new RelayCommand(delegate(object _)
			{
				this.SendFeedback(false);
			}, (object _) => true);
			this.Devices = new ObservableCollection<XdeDeviceViewModel>();
			XdeDeviceFactory.DeviceListChanged = (EventHandler)Delegate.Combine(XdeDeviceFactory.DeviceListChanged, new EventHandler(delegate(object sender, EventArgs args)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					this.RefreshDevices();
				});
			}));
			this.RefreshDevices();
			this.SelectedDevice = this.Devices.FirstOrDefault<XdeDeviceViewModel>();
			this.Owner.Activated += this.Owner_Activated;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00004BBC File Offset: 0x00002DBC
		public ICommand NewDeviceCommand { get; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00004BC4 File Offset: 0x00002DC4
		public ICommand DownloadEmulatorImagesCommand { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00004BCC File Offset: 0x00002DCC
		public ICommand RefreshCommand { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public ICommand OptionsCommand { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00004BDC File Offset: 0x00002DDC
		public ICommand CleanCoreconCommand { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00004BE4 File Offset: 0x00002DE4
		public ICommand SendFeedbackProblemCommand { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00004BEC File Offset: 0x00002DEC
		public ICommand SendFeedbackSuggestionCommand { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00004BF4 File Offset: 0x00002DF4
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00004BFC File Offset: 0x00002DFC
		public XdeDeviceViewModel SelectedDevice
		{
			get
			{
				return this.selectedDevice;
			}
			set
			{
				if (this.selectedDevice != value)
				{
					XdeDeviceViewModel xdeDeviceViewModel = this.selectedDevice;
					this.selectedDevice = value;
					this.OnPropertyChanged("SelectedDevice");
					if (xdeDeviceViewModel != null)
					{
						xdeDeviceViewModel.SelectionChanged();
					}
					if (value != null)
					{
						value.SelectionChanged();
					}
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00004C40 File Offset: 0x00002E40
		public Window Owner { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00004C48 File Offset: 0x00002E48
		public ObservableCollection<XdeDeviceViewModel> Devices { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00004C50 File Offset: 0x00002E50
		public string VersionText
		{
			get
			{
				PackageInfo packageInfo = PackageInfo.Current;
				if (packageInfo == null)
				{
					return string.Empty;
				}
				return string.Format("v{0}", packageInfo.Version);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004C7C File Offset: 0x00002E7C
		public string Title
		{
			get
			{
				if (UacSecurity.IsAdmin())
				{
					return "Microsoft Emulator Manager (Administrator)";
				}
				return "Microsoft Emulator Manager";
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004C90 File Offset: 0x00002E90
		public void RefreshDevices()
		{
			XdeDeviceViewModel xdeDeviceViewModel = this.selectedDevice;
			this.Devices.Clear();
			foreach (XdeDevice device in this.devicesEnum())
			{
				XdeDeviceViewModel xdeDeviceViewModel2 = new XdeDeviceViewModel(this, device);
				this.Devices.Add(xdeDeviceViewModel2);
				if (xdeDeviceViewModel != null && xdeDeviceViewModel.ID == xdeDeviceViewModel2.ID)
				{
					this.SelectedDevice = xdeDeviceViewModel2;
				}
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004D20 File Offset: 0x00002F20
		private void ShowDownloadEmulatorImages(object obj)
		{
			bool? flag = new DownloadEmulatorsWindow(this.Owner).ShowDialog();
			if (flag != null && flag.Value)
			{
				this.RefreshDevices();
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004D56 File Offset: 0x00002F56
		private void Owner_Activated(object sender, EventArgs e)
		{
			if (!this.firstActivation)
			{
				return;
			}
			this.firstActivation = false;
			if (this.Devices.Count == 0)
			{
				this.ShowDownloadEmulatorImages(null);
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004D7C File Offset: 0x00002F7C
		private void NewDevice(object obj)
		{
			bool? flag = new EditDeviceWindow(this.Owner, null, true).ShowDialog();
			if (flag != null && flag.Value)
			{
				this.RefreshDevices();
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004DB4 File Offset: 0x00002FB4
		private bool CanRefresh(object _)
		{
			return true;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004DB7 File Offset: 0x00002FB7
		private void ShowOptions(object _)
		{
			new OptionsWindow
			{
				Owner = this.Owner
			}.ShowDialog();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00004DD0 File Offset: 0x00002FD0
		private void CleanCorecon(object _)
		{
			if (MessageBox.Show("If the Visual Studio target device dropdown doesn't have any entries, the device list might be corrupted. Would you like to try to repair the list? Make sure you close any running instances of Visual Studio first.", "Clean Visual Studio Device Dropdown", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
			{
				return;
			}
			try
			{
				XdeAppUtils.CleanCoreconDatabase();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to delete all the Visual Studio device list files:\r\n\r\n" + ex.Message + "\r\n\r\nMake sure you close all instances of Visual Studio before you try again.", "Microsoft Emulator Manager", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004E34 File Offset: 0x00003034
		private void SendFeedback(bool problem)
		{
			int num = problem ? 2 : 1;
			Process.Start(new ProcessStartInfo(string.Format("Feedback-Hub://?referrer=microsoftEmulatorManagerWindow&tabID=2&feedbackType={0}&AppId={1}!App&newFeedback=true", num, "Microsoft.MicrosoftEmulator_8wekyb3d8bbwe")));
		}

		// Token: 0x04000053 RID: 83
		private EnumerateDevices devicesEnum;

		// Token: 0x04000054 RID: 84
		private XdeDeviceViewModel selectedDevice;

		// Token: 0x04000055 RID: 85
		private bool firstActivation = true;
	}
}
