using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Tools.WindowsDevicePortal;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xde.Base;
using Microsoft.Xde.Client.Controls;
using Microsoft.Xde.Client.Properties;
using Microsoft.Xde.Client.XdeTools;
using Microsoft.Xde.Common;
using Microsoft.Xde.Communication;
using Microsoft.Xde.DeviceManagement;
using Microsoft.Xde.Hcs;
using Microsoft.Xde.Interface;
using Microsoft.Xde.Telemetry;
using Microsoft.Xde.Wmi;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200002E RID: 46
	[Export(typeof(IXdeControllerState))]
	[Export(typeof(IXdeCommands))]
	public sealed class XdeController : IXdeAutomation, IXdeCommands, IDisposable, IXdeControllerState, INotifyPropertyChanged
	{
		// Token: 0x0600030F RID: 783 RVA: 0x0000B8F0 File Offset: 0x00009AF0
		public XdeController()
		{
			this.ArgsProcessor = new XdeArgsProcessor();
			this.InitUiFactory();
			this.SkuFactory = new SkuFactory();
			this.WindowsCompatCheck = new WindowsCompatCheck();
			this.appLifetimeDisposables.Add(this.connectedEvent);
			this.snapshotControl = new SnapshotControl(this);
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000310 RID: 784 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		// (remove) Token: 0x06000311 RID: 785 RVA: 0x0000B9E0 File Offset: 0x00009BE0
		public event EventHandler<TaskDialogArgs> ShowingTaskDialog;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000312 RID: 786 RVA: 0x0000BA18 File Offset: 0x00009C18
		// (remove) Token: 0x06000313 RID: 787 RVA: 0x0000BA50 File Offset: 0x00009C50
		public event EventHandler SkinChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000314 RID: 788 RVA: 0x0000BA88 File Offset: 0x00009C88
		// (remove) Token: 0x06000315 RID: 789 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		public event EventHandler<VirtualMachineStartupEventArgs> VirtualMachineStarting;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000316 RID: 790 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		// (remove) Token: 0x06000317 RID: 791 RVA: 0x0000BB30 File Offset: 0x00009D30
		public event EventHandler VirtualMachineShuttingDown;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000318 RID: 792 RVA: 0x0000BB68 File Offset: 0x00009D68
		// (remove) Token: 0x06000319 RID: 793 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		public event EventHandler<EnabledStateChangedEventArgs> VmStateChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600031A RID: 794 RVA: 0x0000BBD8 File Offset: 0x00009DD8
		// (remove) Token: 0x0600031B RID: 795 RVA: 0x0000BC10 File Offset: 0x00009E10
		public event EventHandler PipeReady;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600031C RID: 796 RVA: 0x0000BC48 File Offset: 0x00009E48
		// (remove) Token: 0x0600031D RID: 797 RVA: 0x0000BC80 File Offset: 0x00009E80
		public event EventHandler XdeReboot;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600031E RID: 798 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		// (remove) Token: 0x0600031F RID: 799 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		public event EventHandler XdeExit;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000320 RID: 800 RVA: 0x0000BD28 File Offset: 0x00009F28
		// (remove) Token: 0x06000321 RID: 801 RVA: 0x0000BD60 File Offset: 0x00009F60
		public event EventHandler ShellReady;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000322 RID: 802 RVA: 0x0000BD98 File Offset: 0x00009F98
		// (remove) Token: 0x06000323 RID: 803 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		public event EventHandler AudioPipeReady;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000324 RID: 804 RVA: 0x0000BE08 File Offset: 0x0000A008
		// (remove) Token: 0x06000325 RID: 805 RVA: 0x0000BE40 File Offset: 0x0000A040
		public event EventHandler MicrophonePipeReady;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000326 RID: 806 RVA: 0x0000BE78 File Offset: 0x0000A078
		// (remove) Token: 0x06000327 RID: 807 RVA: 0x0000BEB0 File Offset: 0x0000A0B0
		public event EventHandler MicrophoneStarted;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000328 RID: 808 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		// (remove) Token: 0x06000329 RID: 809 RVA: 0x0000BF20 File Offset: 0x0000A120
		public event EventHandler MicrophoneStoped;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600032A RID: 810 RVA: 0x0000BF58 File Offset: 0x0000A158
		// (remove) Token: 0x0600032B RID: 811 RVA: 0x0000BF90 File Offset: 0x0000A190
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000BFC5 File Offset: 0x0000A1C5
		IXdeVirtualMachine IXdeControllerState.CurrentVirtualMachine
		{
			get
			{
				return this.virtualMachine;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000BFCD File Offset: 0x0000A1CD
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		public IXdeArgsProcessor ArgsProcessor { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000BFDE File Offset: 0x0000A1DE
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000BFE6 File Offset: 0x0000A1E6
		public IXdeView View { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000BFEF File Offset: 0x0000A1EF
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000BFF7 File Offset: 0x0000A1F7
		public IXdeUiFactory UiFactory { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000C000 File Offset: 0x0000A200
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000C008 File Offset: 0x0000A208
		public IXdeSkuFactory SkuFactory { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000C011 File Offset: 0x0000A211
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000C019 File Offset: 0x0000A219
		[Import]
		public IXdeInputSettings InputSettings { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000C022 File Offset: 0x0000A222
		[Export(typeof(IXdeMinUiFactory))]
		public IXdeMinUiFactory CurrentMinUiFactory
		{
			get
			{
				return (IXdeMinUiFactory)this.UiFactory;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000C02F File Offset: 0x0000A22F
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0000C037 File Offset: 0x0000A237
		[Import(typeof(IXdeAutomationSimpleCommandsPipe))]
		public IXdeAutomationSimpleCommandsPipe SimpleCommandsPipe
		{
			get
			{
				return this.simpleCommandsPipe;
			}
			set
			{
				if (this.simpleCommandsPipe != value)
				{
					this.simpleCommandsPipe = value;
					this.simpleCommandsPipe.PropertyChanged += this.SimpleCommandsPipe_PropertyChanged;
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000C060 File Offset: 0x0000A260
		public IXdeSkin Skin
		{
			get
			{
				return this.skin;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000C068 File Offset: 0x0000A268
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000C070 File Offset: 0x0000A270
		[Import]
		public IXdeConnectionAddressInfo AddressInfo
		{
			get
			{
				return this.addressInfo;
			}
			set
			{
				this.addressInfo = value;
				this.addressInfo.Ready += this.AddressInfo_Ready;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000C090 File Offset: 0x0000A290
		// (set) Token: 0x0600033E RID: 830 RVA: 0x0000C098 File Offset: 0x0000A298
		[Import]
		public IXdeConnectionManager ConnectionManager
		{
			get
			{
				return this.connectionManager;
			}
			set
			{
				if (this.connectionManager != value)
				{
					this.connectionManager = value;
					if (this.connectionManager != null)
					{
						this.connectionManager.ConnectionsFailed += this.ConnectionManager_ConnectionsFailed;
						this.connectionManager.ShellReady += this.ShellReadyPipe_ShellReadyEvent;
					}
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000C0EB File Offset: 0x0000A2EB
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000C0F4 File Offset: 0x0000A2F4
		[Import]
		public IXdeAutomationGuestNotificationsPipe GuestNotificationsPipe
		{
			get
			{
				return this.guestNotificationsPipe;
			}
			set
			{
				if (this.guestNotificationsPipe != value)
				{
					this.guestNotificationsPipe = value;
					this.guestNotificationsPipe.MicrophoneStarted += this.GuestNotificationsPipe_MicrophoneStarted;
					this.guestNotificationsPipe.MicrophoneStopped += this.GuestNotificationsPipe_MicrophoneStopped;
					this.guestNotificationsPipe.GuestUpdated += delegate(object _, GuestUpdatedEventArgs e)
					{
					};
					this.guestNotificationsPipe.PropertyChanged += this.GuestNotificationsPipe_PropertyChanged;
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000C180 File Offset: 0x0000A380
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000C188 File Offset: 0x0000A388
		[Import]
		public IXdeAutomationAudioPipe AudioPipe
		{
			get
			{
				return this.audioPipe;
			}
			set
			{
				if (this.audioPipe != value)
				{
					this.audioPipe = value;
					this.audioPipe.PropertyChanged += this.AudioPipe_PropertyChanged;
				}
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000C1B1 File Offset: 0x0000A3B1
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000C1B9 File Offset: 0x0000A3B9
		[ImportMany]
		public IEnumerable<IXdeTelemetryListener> TelemetryListeners { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000C1C2 File Offset: 0x0000A3C2
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000C1CA File Offset: 0x0000A3CA
		public IXdeMicrophonePipe MicrophonePipe { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000C1D3 File Offset: 0x0000A3D3
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000C1DB File Offset: 0x0000A3DB
		public XdeServerSession ServerSession { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000C1EC File Offset: 0x0000A3EC
		[Import]
		public IXdeSkinFactory SkinFactory { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000C1F5 File Offset: 0x0000A3F5
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000C1FD File Offset: 0x0000A3FD
		[Import(AllowDefault = true)]
		public IXdeNetworkThrottlingConfig ThrottlingConfig { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000C206 File Offset: 0x0000A406
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000C20E File Offset: 0x0000A40E
		public IWindowsCompatCheck WindowsCompatCheck { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000C217 File Offset: 0x0000A417
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000C224 File Offset: 0x0000A424
		public DisplayOrientation DisplayOrientation
		{
			get
			{
				return this.skin.Orientation;
			}
			set
			{
				this.InvokeOnView(new MethodInvoker(delegate
				{
					this.skin.Orientation = value;
				}));
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000C257 File Offset: 0x0000A457
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000C274 File Offset: 0x0000A474
		public int ScreenZoomPercent
		{
			get
			{
				return Convert.ToInt32(this.skin.DisplayScale * 100.0);
			}
			set
			{
				float scale = (float)value / 100f;
				this.InvokeOnView(new MethodInvoker(delegate
				{
					this.skin.DisplayScale = (double)scale;
				}));
				Logger.Instance.Log("ZoomSet", Logger.Level.Info, new
				{
					PartA_iKey = "A-MSTelDefault",
					zoom = value
				});
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000C2C9 File Offset: 0x0000A4C9
		public Rectangle DisplayWindowBounds
		{
			get
			{
				return this.View.DisplayDesktopBounds;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000C2D6 File Offset: 0x0000A4D6
		public Rectangle MainWindowBounds
		{
			get
			{
				return this.View.DesktopBounds;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000C2E3 File Offset: 0x0000A4E3
		public VirtualMachineEnabledState VmState
		{
			get
			{
				if (this.virtualMachine == null)
				{
					return VirtualMachineEnabledState.Unknown;
				}
				return this.virtualMachine.EnabledState;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000C2FA File Offset: 0x0000A4FA
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000C302 File Offset: 0x0000A502
		public IVirtualMachineShutdown VirtualMachineShutdown { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000C30B File Offset: 0x0000A50B
		public bool IsShellReady
		{
			get
			{
				return this.shellReady;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000C313 File Offset: 0x0000A513
		public string ScreenText
		{
			get
			{
				if (this.View == null)
				{
					return string.Empty;
				}
				return this.View.ScreenText;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000C32E File Offset: 0x0000A52E
		public VmUserSettings VmUserSettings
		{
			get
			{
				return this.settingsVM;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000C336 File Offset: 0x0000A536
		IXdeAutomationMicrophonePipe IXdeAutomation.MicrophonePipe
		{
			get
			{
				return this.MicrophonePipe;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000C33E File Offset: 0x0000A53E
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000C34B File Offset: 0x0000A54B
		bool IXdeAutomation.IsNetworkSimulationEnabled
		{
			get
			{
				return this.ThrottlingConfig.IsNetworkSimulationEnabled;
			}
			set
			{
				this.ThrottlingConfig.IsNetworkSimulationEnabled = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000C359 File Offset: 0x0000A559
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000C366 File Offset: 0x0000A566
		NetworkThrottlingSpeed IXdeAutomation.NetThrottlingSpeed
		{
			get
			{
				return this.ThrottlingConfig.NetThrottlingSpeed;
			}
			set
			{
				this.ThrottlingConfig.NetThrottlingSpeed = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000C374 File Offset: 0x0000A574
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0000C381 File Offset: 0x0000A581
		NetworkThrottlingSignalStrength IXdeAutomation.SignalStrength
		{
			get
			{
				return this.ThrottlingConfig.SignalStrength;
			}
			set
			{
				this.ThrottlingConfig.SignalStrength = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000C38F File Offset: 0x0000A58F
		[Export(typeof(IXdeSnapshotControl))]
		IXdeSnapshotControl IXdeAutomation.SnapshotControl
		{
			get
			{
				return this.snapshotControl;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000C397 File Offset: 0x0000A597
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0000C39F File Offset: 0x0000A59F
		[Import(typeof(IXdeSensorsConfig))]
		public IXdeSensorsConfig SensorsConfig
		{
			get
			{
				return this.sensorsConfig;
			}
			set
			{
				if (this.sensorsConfig != value)
				{
					this.sensorsConfig = value;
					this.sensorsConfig.PropertyChanged += this.SensorsConfig_PropertyChanged;
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		public string BrandingName
		{
			get
			{
				return this.sku.Branding.Name;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000C3DA File Offset: 0x0000A5DA
		public string[] FeatureNames
		{
			get
			{
				return this.sku.Features.Select((IXdeFeature feature, int index) => feature.Name).ToArray<string>();
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000C410 File Offset: 0x0000A610
		public string[] ToolsTabIds
		{
			get
			{
				if (XdeToolsForm.CurrentInstance != null)
				{
					return XdeToolsForm.CurrentInstance.XdeToolsControl.GetTabIds();
				}
				return new string[0];
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000C42F File Offset: 0x0000A62F
		public XdeToolbarButtonAutomation[] ToolbarButtons
		{
			get
			{
				return this.View.Toolbar.Buttons;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000C441 File Offset: 0x0000A641
		// (set) Token: 0x0600036A RID: 874 RVA: 0x0000C45F File Offset: 0x0000A65F
		public string ActiveToolsTabId
		{
			get
			{
				if (XdeToolsForm.CurrentInstance != null)
				{
					return XdeToolsForm.CurrentInstance.XdeToolsControl.SelectedTabId;
				}
				return string.Empty;
			}
			set
			{
				if (XdeToolsForm.CurrentInstance != null)
				{
					XdeToolsForm.CurrentInstance.XdeToolsControl.SelectedTabId = value;
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000C478 File Offset: 0x0000A678
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000C480 File Offset: 0x0000A680
		public bool IsShuttingDown
		{
			get
			{
				return this.xdeShutdownStarted;
			}
			private set
			{
				this.xdeShutdownStarted = value;
				this.UiFactory.IsShuttingDown = value;
				Logger.Instance.IsXdeShuttingDown = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000C4A0 File Offset: 0x0000A6A0
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		public bool DeleteCheckpointsAfterReboot { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000C4B1 File Offset: 0x0000A6B1
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000C4B9 File Offset: 0x0000A6B9
		public bool IsGuestOSViewDisplayed
		{
			get
			{
				return this.connectedRdpToVm;
			}
			private set
			{
				if (this.connectedRdpToVm != value)
				{
					this.connectedRdpToVm = value;
					this.OnPropertyChanged("IsGuestOSViewDisplayed");
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000C4D6 File Offset: 0x0000A6D6
		private string VirtualMachineName
		{
			get
			{
				return this.ArgsProcessor.VirtualMachineName;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000C4E3 File Offset: 0x0000A6E3
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000C4EB File Offset: 0x0000A6EB
		private bool AttemptLoadSnapshot { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000C4F4 File Offset: 0x0000A6F4
		private bool UsingSnapshotOption
		{
			get
			{
				return this.ArgsProcessor.BootToSnapshot;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C501 File Offset: 0x0000A701
		public void ShutdownXde()
		{
			if (this.IsShuttingDown)
			{
				return;
			}
			this.InvokeOnView(new MethodInvoker(delegate
			{
				this.View.IndicateShutdown();
			}));
			this.IsShuttingDown = true;
			new Thread(new ThreadStart(this.ShutdownXdeProc)).Start();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C53B File Offset: 0x0000A73B
		public void ShowErrorMessageForException(string messageFormat, Exception e, string telemetryIdentifier)
		{
			this.UiFactory.ShowErrorMessageForException(this.GetModalWindowOwner(), messageFormat, e, telemetryIdentifier);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C554 File Offset: 0x0000A754
		public XdeReturnCode Run(string[] args)
		{
			this.ArgsProcessor.ParseArgs(args);
			this.ArgsProcessor.LoadRegistryOverrides();
			this.AddSensitiveStrings();
			this.UiFactory.SilentMode = this.ArgsProcessor.SilentUi;
			this.AttemptLoadSnapshot = this.UsingSnapshotOption;
			try
			{
				Logger.Instance.Log("XdeStarted", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					addUserToHyperVAdmins = !string.IsNullOrWhiteSpace(this.ArgsProcessor.AddUserToHyperVAdmins),
					addUserToPerformanceLogUsersGroup = !string.IsNullOrWhiteSpace(this.ArgsProcessor.AddUserToPerformanceLogUsersGroup),
					automateFeatures = this.ArgsProcessor.AutomateFeatures,
					bootLanguage = this.ArgsProcessor.BootLanguage,
					bootToSnapshot = this.ArgsProcessor.BootToSnapshot,
					com1PipeName = !string.IsNullOrWhiteSpace(this.ArgsProcessor.Com1PipeName),
					com2PipeName = !string.IsNullOrWhiteSpace(this.ArgsProcessor.Com2PipeName),
					diffDiskVhd = !string.IsNullOrWhiteSpace(this.ArgsProcessor.DiffDiskVhd),
					displayName = !string.IsNullOrWhiteSpace(this.ArgsProcessor.DisplayName),
					fastShutdown = this.ArgsProcessor.FastShutdown,
					originalVideoResolution = this.ArgsProcessor.OriginalVideoResolution,
					screenDiagonalSize = this.ArgsProcessor.ScreenDiagonalSize,
					language = this.ArgsProcessor.Language,
					memSize = this.ArgsProcessor.MemSize,
					natDisabled = DefaultSettings.NATDisabled,
					noStart = this.ArgsProcessor.NoStart,
					gpuDisabled = DefaultSettings.GpuDisabledForXde,
					sensorsEnabled = this.ArgsProcessor.SensorsEnabled,
					showName = this.ArgsProcessor.ShowName,
					showUsage = this.ArgsProcessor.ShowUsage,
					silentSnapshot = this.ArgsProcessor.SilentSnapshot,
					silentUi = this.ArgsProcessor.SilentUi,
					sku = this.ArgsProcessor.Sku,
					startedBy = this.StartedByWhatProgram(),
					version = Globals.XdeVersion.ToString(),
					versionLong = Globals.XdeVersionLong,
					vhdPath = !string.IsNullOrWhiteSpace(this.ArgsProcessor.VhdPath),
					videoResolution = this.ArgsProcessor.VideoResolution,
					virtualMachineName = !string.IsNullOrWhiteSpace(this.ArgsProcessor.VirtualMachineName),
					waitForClientConnection = this.ArgsProcessor.WaitForClientConnection
				});
				Logger.Instance.LogSensorsEnabled("CommandLine", this.ArgsProcessor.SensorsEnabled);
				if (this.ArgsProcessor.ShowUsage)
				{
					this.SetRequestedLanguage();
					this.UiFactory.ShowUsage();
					Logger.Instance.Log("UsageShown", Logger.Level.Local);
					return this.exitCode = XdeReturnCode.ShowUsage;
				}
				if (!this.ValidateArguments())
				{
					return this.exitCode = XdeReturnCode.InvalidArguments;
				}
				string xdeOwnershipMutexName = Utility.GetXdeOwnershipMutexName(this.ArgsProcessor.VirtualMachineName);
				bool flag;
				using (new Mutex(true, xdeOwnershipMutexName, ref flag))
				{
					if (!flag)
					{
						this.BringOtherXdeToFront();
						return this.exitCode = XdeReturnCode.InstanceAlreadyRunning;
					}
					this.networkManager = new XdeNetworkManager();
					this.appLifetimeDisposables.Add(this.networkManager);
					if (!this.LoadSku())
					{
						return this.exitCode = XdeReturnCode.InvalidArguments;
					}
					this.LoadTelemetryListeners();
					DefaultSettings.NATDisabled = (this.sku.Options.NATDisabled || !NetNat.IsSupported());
					this.AddUserToSecurityGroupIfRequested(this.ArgsProcessor.AddUserToPerformanceLogUsersGroup, "Performance Log Users");
					if (!this.CheckWindowsCompatibility())
					{
						return this.exitCode = XdeReturnCode.WrongWindows;
					}
					if (!this.CheckHyperVInstalled())
					{
						return this.exitCode = XdeReturnCode.HyperVNotInstalled;
					}
					if (!this.CheckHypervisorRunning())
					{
						return this.exitCode = XdeReturnCode.HypervisorNotRunning;
					}
					if (!this.CheckHyperVManagementServiceRunning())
					{
						return this.exitCode = XdeReturnCode.HyperVNotInstalled;
					}
					if (!this.CanAccessVmApi())
					{
						return this.exitCode = XdeReturnCode.CantAccessHyperVApi;
					}
					if (this.networkManager != null)
					{
						this.networkManager.PreInitialize();
						if (!this.networkManager.TryAquireNecessaryPermissions())
						{
							return this.exitCode = XdeReturnCode.CouldntAquireNetworkModifyPermissions;
						}
					}
					if (!this.CheckEula())
					{
						return this.exitCode = XdeReturnCode.EulaNotAccepted;
					}
					if (!this.CheckHostSettingModified())
					{
						return this.exitCode = XdeReturnCode.HostSettingsModified;
					}
					this.AddUserToSecurityGroupIfRequested(this.ArgsProcessor.AddUserToHyperVAdmins, "Hyper-V Administrators");
					Mutex mutex3;
					Mutex mutex2 = mutex3 = new Mutex(false, "Microsoft.WindowsPhone.XDE.Resources." + this.ArgsProcessor.VirtualMachineName);
					try
					{
						try
						{
							mutex2.WaitOne();
						}
						catch (AbandonedMutexException)
						{
						}
						if (!this.LoadUserSettings())
						{
							return this.exitCode = XdeReturnCode.CouldntLoadSettings;
						}
						if (!this.ArgsProcessor.NoStart && !this.ArgsProcessor.SilentSnapshot)
						{
							this.WireViewEvents();
							if (!this.LoadSkin(this.ArgsProcessor.VideoResolution, this.ArgsProcessor.SensorsEnabled))
							{
								return this.exitCode = XdeReturnCode.CouldntLoadSkin;
							}
							this.skin.DisplayScale = (double)this.settingsVM.Zoom / 100.0;
							TimeSpan timeSpan = DateTime.Now - App.ProgramStartedAt;
							Logger.Instance.LogTimeTaken("UIShown", (uint)timeSpan.TotalMilliseconds);
							this.LogFeaturesAndPipeNames();
							this.View.Run();
						}
						else
						{
							this.StartInitializationThread();
							this.threadInit.Join();
							if (this.ArgsProcessor.SilentSnapshot)
							{
								this.xdeDoneEvent.WaitOne();
							}
						}
						mutex2.ReleaseMutex();
					}
					finally
					{
						if (mutex3 != null)
						{
							((IDisposable)mutex3).Dispose();
						}
					}
				}
			}
			catch (Exception e)
			{
				Logger.Instance.LogException("Unhandled", e);
				if (this.exitCode == XdeReturnCode.Success)
				{
					this.exitCode = XdeReturnCode.GenericError;
				}
				throw;
			}
			finally
			{
				Logger.Instance.Log("XdeStopped", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					exitCode = this.exitCode,
					exitCodeString = this.exitCode.ToString()
				});
			}
			return this.exitCode;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000CB58 File Offset: 0x0000AD58
		private void LogFeaturesAndPipeNames()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (IXdeFeature xdeFeature in this.sku.Features)
			{
				list.Add(xdeFeature.Name);
			}
			list2.Add(XdeShellReadyPipe.PipeName);
			foreach (IXdeConnectionController xdeConnectionController in this.sku.ConnectionControllers)
			{
				list2.Add(xdeConnectionController.Name);
			}
			Logger.Instance.Log("ComponentsInSku", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				featureNames = string.Join(",", list.ToArray()),
				pipeNames = string.Join(",", list2.ToArray())
			});
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000CC50 File Offset: 0x0000AE50
		public void ClickToolbarButton(string buttonName)
		{
			this.View.Toolbar.ClickButton(buttonName);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000CC64 File Offset: 0x0000AE64
		void IXdeCommands.ShowZoomUi()
		{
			Logger.Instance.LogButtonClicked("Zoom", "Toolbar");
			XdeUiFactory.ZoomUiResult zoomUiResult = this.UiFactory.DisplayZoomUi(this.GetModalWindowOwner(), this.ScreenZoomPercent);
			if (zoomUiResult.Result == DialogResult.OK)
			{
				this.ScreenZoomPercent = zoomUiResult.Scale;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000CCB2 File Offset: 0x0000AEB2
		void IXdeCommands.Minimize()
		{
			Logger.Instance.LogButtonClicked("Minimize", "Toolbar");
			this.View.Minimize();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000CCD3 File Offset: 0x0000AED3
		void IXdeCommands.Close()
		{
			this.Close();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000CCDB File Offset: 0x0000AEDB
		public void BringUiToFront()
		{
			Logger.Instance.Log("BringToFrontExecuted", Logger.Level.Local);
			this.InvokeOnView(new MethodInvoker(delegate
			{
				this.View.BringAppToFront();
			}));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000CCFF File Offset: 0x0000AEFF
		public void CaptureImage(string fileName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000CD06 File Offset: 0x0000AF06
		void IXdeCommands.FitToScreen()
		{
			this.FitToScreen();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000CD0E File Offset: 0x0000AF0E
		void IXdeCommands.LaunchXdeTools()
		{
			this.LaunchXdeTools();
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000CD16 File Offset: 0x0000AF16
		void IXdeCommands.RotateClockwise()
		{
			this.RotateClockwise();
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000CD1E File Offset: 0x0000AF1E
		void IXdeCommands.RotateCounterClockwise()
		{
			this.RotateCounterClockwise();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000CD26 File Offset: 0x0000AF26
		void IXdeCommands.DisplayHelp()
		{
			this.DisplayHelp();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000CD2E File Offset: 0x0000AF2E
		IEnumerable<ISkinButtonInfo> IXdeAutomation.GetSkinButtonInfos()
		{
			return this.skin.Buttons;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000CD3B File Offset: 0x0000AF3B
		IEnumerable<string> IXdeAutomation.GetSkinResources()
		{
			return this.skin.BitmapFileNames;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000CD48 File Offset: 0x0000AF48
		void IXdeAutomation.OnClientConnected()
		{
			this.connectedEvent.Set();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000CD58 File Offset: 0x0000AF58
		void IXdeAutomation.GetEndPoint(out string hostIP, out string deviceIP)
		{
			hostIP = null;
			deviceIP = null;
			if (this.virtualMachine != null)
			{
				try
				{
					hostIP = this.AddressInfo.HostIpAddress;
					deviceIP = this.AddressInfo.GuestIpAddress;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000CDA4 File Offset: 0x0000AFA4
		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (this.connectedEvent != null)
				{
					this.connectedEvent.Set();
				}
				if (this.virtualMachine != null)
				{
					this.virtualMachine.EnableStateChanged -= this.VirtualMachine_EnableStateChanged;
				}
				if (this.xdeDoneEvent != null)
				{
					this.xdeDoneEvent.Set();
				}
				this.appLifetimeDisposables.ForEach(delegate(IDisposable d)
				{
					d.Dispose();
				});
				this.DisposeGuestLifetimeObjects();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000CE40 File Offset: 0x0000B040
		internal void InternalResumeSnapshot(IXdeVirtualMachineSettings snapshot)
		{
			this.restoringSnapshot = true;
			try
			{
				this.ResetAllConnections();
				this.virtualMachine.Stop();
				this.virtualMachine.ApplySnapshot(snapshot);
				this.virtualMachine.Start();
				this.View.Invoke(new MethodInvoker(delegate
				{
					this.LoadSku();
				}));
				this.ConnectViewToVm();
				this.takingSnapshotWhenReady = false;
				this.ConnectionManager.DoShellReady();
			}
			finally
			{
				this.restoringSnapshot = false;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
		private static void Retry(int maxRetries, int timeBetweenRetries, Action action)
		{
			for (int i = 0; i <= maxRetries; i++)
			{
				try
				{
					action();
					break;
				}
				catch (Exception ex)
				{
					if (i >= maxRetries)
					{
						throw ex;
					}
					Thread.Sleep(timeBetweenRetries);
				}
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000CF0C File Offset: 0x0000B10C
		private static bool AreSettingsEquivalent(IXdeVirtualMachineSettings settings1, IXdeVirtualMachineSettings settings2)
		{
			return settings1.RamSize == settings2.RamSize && settings1.Com1NamedPipe == settings2.Com1NamedPipe && settings1.Com2NamedPipe == settings2.Com2NamedPipe;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000CF44 File Offset: 0x0000B144
		private static bool IsVhdSnapshotOfBaseVhd(string vhdToCheck, string baseVhd)
		{
			string baseParent = VhdUtils.GetVhdParentPath(baseVhd);
			if (baseParent == null)
			{
				baseParent = baseVhd;
			}
			return VhdUtils.GetParentVhdPaths(vhdToCheck).FirstOrDefault((string p) => StringComparer.OrdinalIgnoreCase.Equals(p, baseParent)) != null;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000CF8C File Offset: 0x0000B18C
		private static CultureInfo ParseLanguage(string language)
		{
			int culture;
			CultureInfo result;
			if (int.TryParse(language, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out culture))
			{
				result = new CultureInfo(culture);
			}
			else
			{
				result = new CultureInfo(language);
			}
			return result;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
				propertyChanged(this, e);
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
		private bool CheckEula()
		{
			string text = Path.Combine(Path.GetDirectoryName(this.ArgsProcessor.VhdPath), "eula.rtf");
			if (File.Exists(text))
			{
				using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\XDE\\eula"))
				{
					string vhdPath = this.ArgsProcessor.VhdPath;
					object value = registryKey.GetValue(vhdPath);
					if (value is int && (int)value != 0)
					{
						return true;
					}
					bool? flag = new EulaWindow(this.ArgsProcessor.DisplayName, text).ShowDialog();
					bool flag2 = true;
					if (flag.GetValueOrDefault() == flag2 & flag != null)
					{
						registryKey.SetValue(vhdPath, 1);
						return true;
					}
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000D0BC File Offset: 0x0000B2BC
		private void LoadTelemetryListeners()
		{
			if (this.TelemetryListeners != null)
			{
				foreach (IXdeTelemetryListener item in this.TelemetryListeners)
				{
					Logger.Instance.Listeners.Add(item);
				}
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000D11C File Offset: 0x0000B31C
		private void InitUiFactory()
		{
			this.UiFactory = new XdeUiFactory();
			this.UiFactory.ShowingTaskDialog += this.UiFactory_ShowTaskDialog;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000D140 File Offset: 0x0000B340
		private void AddressInfo_Ready(object sender, EventArgs e)
		{
			EventHandler pipeReady = this.PipeReady;
			if (pipeReady == null)
			{
				return;
			}
			pipeReady(this, EventArgs.Empty);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000D158 File Offset: 0x0000B358
		private async void RestartGuestShell()
		{
			DevicePortal devicePortal = new DevicePortal("http://" + this.addressInfo.GuestIpAddress);
			try
			{
				Task<DevicePortal.RunningProcesses> runningProcessesAsync = devicePortal.GetRunningProcessesAsync();
				runningProcessesAsync.Wait();
				DevicePortal.DeviceProcessInfo deviceProcessInfo = runningProcessesAsync.Result.Processes.FirstOrDefault((DevicePortal.DeviceProcessInfo p) => StringComparer.OrdinalIgnoreCase.Equals(p.Name, "ModernUXShellApp.exe"));
				if (deviceProcessInfo != null)
				{
					await devicePortal.DeleteAsync(string.Format("/api/taskmanager/process?pid={0}", deviceProcessInfo.ProcessId), null);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000D191 File Offset: 0x0000B391
		private void ConnectionManager_ConnectionsFailed(object sender, MessageEventArgs e)
		{
			this.ShowFailedToConnectToGuestMessage(e.Message);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000D19F File Offset: 0x0000B39F
		private void DisplayHelp()
		{
			Logger.Instance.LogButtonClicked("Help", "Toolbar");
			this.UiFactory.DisplayHelp();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		private void LaunchXdeTools()
		{
			if (!this.View.Toolbar.IsToolsButtonEnabled)
			{
				return;
			}
			Logger.Instance.LogButtonClicked("XdeTools", "Toolbar");
			Point possibleToolsLeftCornerLocation = new Point(this.View.Toolbar.DesktopBounds.Right + 5, this.View.Toolbar.DesktopBounds.Top);
			Point possibleToolsRightCornerLocation = new Point(this.View.DesktopBounds.Left - 5, this.View.DesktopBounds.Top);
			this.InvokeOnView(new MethodInvoker(delegate
			{
				this.UiFactory.DisplayToolsUi(this.GetModalWindowOwner(), this.View.Location, possibleToolsLeftCornerLocation, possibleToolsRightCornerLocation);
			}));
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000D283 File Offset: 0x0000B483
		private void Close()
		{
			Logger.Instance.LogButtonClicked("Close", "Toolbar");
			this.Close(XdeReturnCode.Success);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		private void FitToScreen()
		{
			Logger.Instance.LogButtonClicked("FitToScreen", "Toolbar");
			this.View.CenterToScreen();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		private void RotateClockwise()
		{
			Logger.Instance.LogButtonClicked("RotateRight", "Toolbar");
			DisplayOrientation current = this.skin.Orientation;
			DisplayOrientation current2 = current;
			current = current2 - 1;
			if (current < DisplayOrientation.Portrait)
			{
				current = DisplayOrientation.LandscapeRight;
			}
			this.InvokeOnView(new MethodInvoker(delegate
			{
				this.DisplayOrientation = current;
			}));
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000D338 File Offset: 0x0000B538
		private void RotateCounterClockwise()
		{
			Logger.Instance.LogButtonClicked("RotateLeft", "Toolbar");
			DisplayOrientation current = this.skin.Orientation;
			DisplayOrientation current2 = current;
			current = current2 + 1;
			if (current > DisplayOrientation.LandscapeRight)
			{
				current = DisplayOrientation.Portrait;
			}
			this.InvokeOnView(new MethodInvoker(delegate
			{
				this.DisplayOrientation = current;
			}));
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000D3A9 File Offset: 0x0000B5A9
		private IWin32Window GetModalWindowOwner()
		{
			if (this.View == null)
			{
				return null;
			}
			return this.View.TopWindow;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		private void WaitForThreadInitExit()
		{
			if (this.threadInit != null)
			{
				this.threadInit.Join();
				this.threadInit = null;
			}
			if (this.threadReconnect != null)
			{
				this.threadReconnect.Join();
				this.threadReconnect = null;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000D3F6 File Offset: 0x0000B5F6
		private void StartInitializationThread()
		{
			if (this.threadInit == null)
			{
				this.threadInit = new Thread(new ThreadStart(this.InitializeThreadProc));
				this.threadInit.Start();
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000D424 File Offset: 0x0000B624
		private bool CheckWindowsCompatibility()
		{
			bool flag = this.WindowsCompatCheck.Is64BitWindows();
			bool flag2 = this.WindowsCompatCheck.IsWindows10OrBetter();
			bool flag3 = this.WindowsCompatCheck.IsHyperVFeatureOptionAvailable();
			StringBuilder stringBuilder = new StringBuilder();
			if (!flag || !flag2 || !flag3)
			{
				stringBuilder.AppendLine(Resources.Win10_64Bit);
			}
			bool flag4 = false;
			if (flag2)
			{
				flag4 = this.WindowsCompatCheck.IsSlatPresent();
				if (!flag4)
				{
					stringBuilder.AppendLine(Resources.SLAT);
				}
			}
			if (stringBuilder.Length != 0)
			{
				string message = StringUtilities.CurrentCultureFormat(Resources.Windows8RequiredText, new object[]
				{
					stringBuilder.ToString()
				});
				this.UiFactory.ShowErrorMessage(this.GetModalWindowOwner(), Resources.CantStartEmulator, message);
				Logger.Instance.LogError("SystemNotCompatible", new
				{
					win64 = flag,
					win10 = flag2,
					hyperVAvailable = flag3,
					slatFound = flag4
				});
				return false;
			}
			return true;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000D4EA File Offset: 0x0000B6EA
		private bool CheckHyperVInstalled()
		{
			if (!this.WindowsCompatCheck.IsHyperVFeatureInstalled())
			{
				this.UiFactory.ShowErrorMessage(this.GetModalWindowOwner(), Resources.HyperVNotInstalledInstruction, Resources.HyperVNotInstalledText);
				Logger.Instance.LogError("HyperVNotEnabled");
				return false;
			}
			return true;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000D526 File Offset: 0x0000B726
		private bool CheckHypervisorRunning()
		{
			if (!this.WindowsCompatCheck.IsHypervisorPresent())
			{
				this.UiFactory.ShowErrorMessage(this.GetModalWindowOwner(), Resources.CantStartEmulator, Resources.HypervisorNotFoundText);
				Logger.Instance.LogError("HyperVNotRunning");
				return false;
			}
			return true;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000D562 File Offset: 0x0000B762
		private bool CheckHyperVManagementServiceRunning()
		{
			if (!this.WindowsCompatCheck.IsHyperVManagementServiceRunning())
			{
				this.UiFactory.ShowErrorMessage(this.GetModalWindowOwner(), Resources.VmmsIsntRunningInstruction, Resources.VmmsIsntRunningText);
				Logger.Instance.LogError("HyperVManagementServiceNotRunning");
				return false;
			}
			return true;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000D5A0 File Offset: 0x0000B7A0
		private bool CheckHostSettingModified()
		{
			if (RegistryHelper.RestartPending)
			{
				using (TaskDialog dlg = this.UiFactory.CreatedLinkEnabledTaskDialog())
				{
					bool reboot = false;
					dlg.Caption = Resources.ApplicationName;
					dlg.InstructionText = string.Format(Resources.HostSettingChanged, Resources.ApplicationName);
					dlg.Text = Resources.RestartWarningText;
					TaskDialogButton taskDialogButton = new TaskDialogButton("buttonNow", Resources.ButtonText_RestartNow);
					taskDialogButton.Click += delegate(object sender, EventArgs args)
					{
						Logger.Instance.LogDialogRespondedTo("Reboot", 1U);
						reboot = true;
						dlg.Close();
					};
					TaskDialogButton taskDialogButton2 = new TaskDialogButton("buttonLater", Resources.ButtonText_RestartLater);
					taskDialogButton2.Default = true;
					taskDialogButton2.Click += delegate(object sender, EventArgs args)
					{
						Logger.Instance.LogDialogRespondedTo("Reboot", 8U);
						dlg.Close();
					};
					dlg.Controls.Add(taskDialogButton);
					dlg.Controls.Add(taskDialogButton2);
					dlg.Show();
					if (reboot)
					{
						this.RebootHost(30);
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D6F4 File Offset: 0x0000B8F4
		private void RebootHost(int delay = 30)
		{
			string arguments = string.Format("/r /t {0} /d p:4:1", delay);
			Process.Start("shutdown", arguments);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000D720 File Offset: 0x0000B920
		private string GetViewTitle()
		{
			if (!string.IsNullOrEmpty(this.ArgsProcessor.DisplayName))
			{
				return this.ArgsProcessor.DisplayName;
			}
			if (this.sku != null && this.sku.Branding != null)
			{
				return StringUtilities.CurrentCultureFormat(Resources.AppTitleFormat, new object[]
				{
					this.sku.Branding.DisplayName,
					this.ArgsProcessor.VirtualMachineName
				});
			}
			return this.ArgsProcessor.VirtualMachineName;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D79D File Offset: 0x0000B99D
		private void InitializeView()
		{
			if (this.View == null)
			{
				this.View = new EmulatorWindow();
				this.UiFactory.DefaultOwner = this.View.TopWindow;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
		private bool LoadUserSettings()
		{
			bool result;
			try
			{
				VmUserSettings vmUserSettings;
				if (!XdeUserSettings.Default.TryGetValue(this.ArgsProcessor.VirtualMachineName, out vmUserSettings))
				{
					vmUserSettings = new VmUserSettings();
					vmUserSettings.Zoom = 60;
					vmUserSettings.Name = this.ArgsProcessor.VirtualMachineName;
					vmUserSettings.ScreenLocation = XdeController.UnspecifiedLocation;
				}
				this.settingsVM = vmUserSettings;
				result = true;
			}
			catch (Exception e)
			{
				this.UiFactory.ShowErrorMessage(this.GetModalWindowOwner(), null, Resources.UserSettingsCorrupted);
				Logger.Instance.LogException("UserSettingsLoaded", e);
				this.HandleConfigException(e);
				result = false;
			}
			return result;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D868 File Offset: 0x0000BA68
		private void HandleConfigException(Exception e)
		{
			string text = null;
			ConfigurationErrorsException ex = e as ConfigurationErrorsException;
			if (ex == null)
			{
				return;
			}
			ConfigurationErrorsException ex2 = ex.InnerException as ConfigurationErrorsException;
			if (ex2 != null)
			{
				text = ex2.Filename;
			}
			try
			{
				if (!string.IsNullOrEmpty(text))
				{
					File.Delete(text);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		private bool SetRequestedLanguage()
		{
			if (!string.IsNullOrEmpty(this.ArgsProcessor.Language))
			{
				try
				{
					this.cultureInfo = XdeController.ParseLanguage(this.ArgsProcessor.Language);
				}
				catch (Exception e)
				{
					this.ShowErrorMessage(Resources.InvalidLanguageSpecified);
					Logger.Instance.LogException("RequestedLanguageSet", e);
					return false;
				}
				this.SetCultureForCurrentThread();
				return true;
			}
			return true;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D930 File Offset: 0x0000BB30
		private bool SetRequestedBootLanguage()
		{
			if (!string.IsNullOrEmpty(this.ArgsProcessor.BootLanguage))
			{
				CultureInfo cultureInfo;
				try
				{
					cultureInfo = XdeController.ParseLanguage(this.ArgsProcessor.BootLanguage);
				}
				catch (Exception e)
				{
					this.ShowErrorMessage(Resources.InvalidBootLanguageSpecified);
					Logger.Instance.LogException("RequestedBootLanguageSet", e);
					return false;
				}
				this.bootLanguage = cultureInfo.Name;
				return true;
			}
			return true;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
		private bool LoadSku()
		{
			try
			{
				XdeWmiVirtualServices xdeWmiVirtualServices = new XdeWmiVirtualServices();
				XdeHcsVirtualServices xdeHcsVirtualServices = new XdeHcsVirtualServices();
				this.virtualServices = xdeWmiVirtualServices;
				XdeSku xdeSku = null;
				XdeInstallation appInstallXdeInstallation = XdeInstallation.GetAppInstallXdeInstallation();
				if (appInstallXdeInstallation != null)
				{
					xdeSku = appInstallXdeInstallation.Skus.FirstOrDefault((XdeSku s) => StringComparer.OrdinalIgnoreCase.Equals(s.Name, this.ArgsProcessor.Sku));
				}
				this.sku = this.SkuFactory.LoadSkuFromName((xdeSku != null) ? xdeSku.Path : this.ArgsProcessor.Sku, new object[]
				{
					this,
					this.ArgsProcessor,
					this.View,
					this.networkManager,
					xdeWmiVirtualServices,
					xdeHcsVirtualServices
				});
				this.UpdateBrandingInfo();
				if (!this.ValidateDisplayArguments())
				{
					return false;
				}
				this.skuGpuAssignmentMode = this.sku.Options.GpuAssignmentMode;
				if (this.sku.Options.UseHCSIfAvailable && this.WindowsCompatCheck.IsBuildEqualOrGreater(MinBuildVersion.RS5) && !this.ArgsProcessor.UseWmi)
				{
					this.virtualServices = xdeHcsVirtualServices;
				}
				Logger.Instance.Log("VirtualServicesSelected", Logger.Level.Info, new
				{
					PartA_iKey = "A-MSTelDefault",
					type = this.virtualServices.GetType().Name
				});
				this.InitInputMode();
				this.InitValidSensorsOnSensorsConfig();
			}
			catch (Exception ex)
			{
				Logger.Instance.LogException("SkuLoaded", ex);
				string message = StringUtilities.CurrentCultureFormat(Resources.UnableToLoadSku, new object[]
				{
					this.ArgsProcessor.Sku,
					ex.Message
				});
				this.ShowErrorMessage(message);
				return false;
			}
			return true;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000DB44 File Offset: 0x0000BD44
		private SkinDisplay GetRequestedDisplayInformation()
		{
			return this.SkinFactory.GetSkinDisplayInformation(this.ArgsProcessor.VideoResolution);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		private Size GetRequestedScreenResolution()
		{
			SkinDisplay requestedDisplayInformation = this.GetRequestedDisplayInformation();
			return new Size(requestedDisplayInformation.TotalDisplayWidth, requestedDisplayInformation.DisplayHeight);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000DB81 File Offset: 0x0000BD81
		private int GetRequestedVRAMforVGPU()
		{
			return this.GetRequestedDisplayInformation().VRAMforVGPU;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000DB90 File Offset: 0x0000BD90
		private string GetEffectiveRequestedDiagonalScreenSize()
		{
			string text = this.ArgsProcessor.ScreenDiagonalSize;
			if (string.IsNullOrEmpty(text))
			{
				text = this.GetRequestedDisplayInformation().DefaultDiagonalSize;
			}
			return text;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000DBC0 File Offset: 0x0000BDC0
		private bool SetRequestedBootScreenSize()
		{
			string effectiveRequestedDiagonalScreenSize = this.GetEffectiveRequestedDiagonalScreenSize();
			bool flag = false;
			float num;
			if (float.TryParse(effectiveRequestedDiagonalScreenSize, NumberStyles.Any, CultureInfo.InvariantCulture, out num) && num > 0f)
			{
				float newDiag = MathUtils.ConvertFromInchesToCentimeters(num);
				SkinDisplay requestedDisplayInformation = this.GetRequestedDisplayInformation();
				Size size = new Size(requestedDisplayInformation.DisplayWidth, requestedDisplayInformation.DisplayHeight);
				SizeF triangleSidesUsingNewDiagAndOldSides = MathUtils.GetTriangleSidesUsingNewDiagAndOldSides(newDiag, new SizeF((float)size.Width, (float)size.Height));
				if (triangleSidesUsingNewDiagAndOldSides.Width < 409.5f && triangleSidesUsingNewDiagAndOldSides.Height < 409.5f)
				{
					this.bootScreenSize = new Size((int)Math.Round((double)triangleSidesUsingNewDiagAndOldSides.Width, 0), (int)Math.Round((double)triangleSidesUsingNewDiagAndOldSides.Height, 0));
					flag = true;
				}
			}
			if (!flag)
			{
				this.ShowErrorMessage(Resources.InvalidScreenSizeSpecified);
				Logger.Instance.LogCommandLineParameterInvalid("ScreenSize", effectiveRequestedDiagonalScreenSize);
			}
			return flag;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000DC9A File Offset: 0x0000BE9A
		private void SetCultureForCurrentThread()
		{
			if (this.cultureInfo != null)
			{
				Thread.CurrentThread.CurrentUICulture = this.cultureInfo;
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		private void ShowErrorMessage(string message)
		{
			this.UiFactory.ShowErrorMessage(this.GetModalWindowOwner(), message);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
		private bool TryWriteVhdBootSettings(string vhdPath)
		{
			bool result;
			try
			{
				this.WriteVhdBootSettings(vhdPath);
				result = true;
			}
			catch (Exception ex) when ((ex is Win32Exception && ((Win32Exception)ex).NativeErrorCode == -2132344830) || ex is ObjectNotFoundException)
			{
				string text = string.Concat(new string[]
				{
					"The emulator failed to write boot settings to the VHD:\r\n\r\n",
					ex.Message,
					"\r\n\r\nYou can try to continue booting, but some functionality may be degraded. If you choose to exit you can reset the device's diff disk in a command prompt:\r\n\r\nxdeconfig reset \"",
					this.ArgsProcessor.VirtualMachineName,
					"\""
				});
				if (this.CurrentMinUiFactory.ShowTaskDialog("Click \"OK\" to continue booting anyway or \"Cancel\" to exit the emulator", text, TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Cancel, TaskDialogResult.Cancel, TaskDialogStandardIcon.Warning) == TaskDialogResult.Cancel)
				{
					this.exitCode = XdeReturnCode.FailedToWriteBootSettings;
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (Exception ex2) when (ex2 is UnauthorizedAccessException || (ex2 is Win32Exception && ((Win32Exception)ex2).NativeErrorCode == 1314))
			{
				this.CurrentMinUiFactory.ShowElevationDialog(Resources.RetryRunningAsAdmin, Resources.NoPermissionsToMountVirtualDisk, true);
				this.exitCode = XdeReturnCode.CouldntMountVhd;
				result = false;
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000DE10 File Offset: 0x0000C010
		private string CreateHashForBootSettings(SkinDisplay displayInfo)
		{
			return "sku=" + this.ArgsProcessor.Sku + ",skin=" + displayInfo.Alias;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000DE34 File Offset: 0x0000C034
		private void WriteVhdBootSettings(string vhdPath)
		{
			DateTime now = DateTime.Now;
			using (WindowsImageVhd windowsImageVhd = WindowsImageVhd.OpenVhd(vhdPath, FileAccess.ReadWrite))
			{
				bool flag = windowsImageVhd.OSFileSystem.FileExists("\\windows\\system32\\Drivers\\UMDF\\XdeExternMon.dll");
				if (this.skin.Display.SupportsExternalDisplay && !flag)
				{
					this.skin.Display.ExternalDisplayHeight = (this.skin.Display.ExternalDisplayWidth = 0);
				}
				string path = Path.Combine(this.sku.SkuDirectory, "ACPITABL.dat");
				if (File.Exists(path))
				{
					using (Stream stream = windowsImageVhd.OSFileSystem.OpenFile("\\windows\\system32\\ACPITABL.dat", FileMode.Truncate))
					{
						byte[] array = File.ReadAllBytes(path);
						stream.Write(array, 0, array.Length);
					}
				}
				windowsImageVhd.EnableKernelDebugger(this.virtualMachine);
				this.virtualMachine.WriteSettingsToVhd(windowsImageVhd);
				DateTime now2 = DateTime.Now;
				Logger.Instance.LogTimeTaken("WriteSettingsToVhdTimeTaken", (uint)(now2 - now).TotalMilliseconds);
				SkinDisplay requestedDisplayInformation = this.GetRequestedDisplayInformation();
				bool flag2 = true;
				string text = this.CreateHashForBootSettings(requestedDisplayInformation);
				if (windowsImageVhd.OSFileSystem.FileExists("\\xde.boot.settings"))
				{
					using (Stream stream2 = windowsImageVhd.OSFileSystem.OpenFile("\\xde.boot.settings", FileMode.Open))
					{
						using (StreamReader streamReader = new StreamReader(stream2))
						{
							if (streamReader.ReadToEnd() == text)
							{
								Logger.Instance.Log("SkippingRegBootSettingsAlreadyWritten", Logger.Level.Local);
								flag2 = false;
							}
						}
					}
				}
				if (flag2)
				{
					Logger.Instance.Log("WritingRegBootSettings", Logger.Level.Local);
					windowsImageVhd.DisableStateSeparation(this.ArgsProcessor.DisableStateSep);
					string s = string.Format(Resources.GenericSettings_pkg, Logger.Instance.IsUserOptedIn ? 3 : 1);
					windowsImageVhd.ApplyPackage(new StringReader(s));
					string s2 = string.Format(Resources.NetworkSettings_pkg, 1, "0.0.0.0", DefaultSettings.NATDisabled ? 1 : 0);
					windowsImageVhd.ApplyPackage(new StringReader(s2));
					bool ignoreScaleDirectives = flag;
					windowsImageVhd.ApplySku(this.sku, requestedDisplayInformation, ignoreScaleDirectives);
					if (this.ArgsProcessor.ServiceDebugEnabled)
					{
						windowsImageVhd.ApplyPackage(new StringReader(Resources.Debug_pkg));
					}
					int num;
					int num2;
					if (requestedDisplayInformation.ChromeCount > 1)
					{
						num = 1;
						num2 = requestedDisplayInformation.InitialActiveChromeCount;
					}
					else
					{
						num = 0;
						num2 = 1;
					}
					string s3 = string.Format(Resources.GraphicsHyperV_pkg, new object[]
					{
						requestedDisplayInformation.TotalDisplayWidth,
						requestedDisplayInformation.DisplayHeight,
						requestedDisplayInformation.VideoMode,
						requestedDisplayInformation.DisplayCount,
						requestedDisplayInformation.DisplayWidth,
						requestedDisplayInformation.DisplayScaleFactor,
						num,
						num2,
						this.bootScreenSize.Width,
						this.bootScreenSize.Height
					});
					windowsImageVhd.ApplyPackage(new StringReader(s3));
					XdeSensors xdeSensors = this.bootSensors & this.sensorsConfig.ValidSensors;
					if (xdeSensors != XdeSensors.Default)
					{
						string vhdPackageContents = this.sensorsConfig.GetVhdPackageContents(xdeSensors);
						windowsImageVhd.ApplyPackage(new StringReader(vhdPackageContents));
					}
					using (Stream stream3 = windowsImageVhd.OSFileSystem.OpenFile("\\xde.boot.settings", FileMode.Create))
					{
						using (StreamWriter streamWriter = new StreamWriter(stream3))
						{
							streamWriter.Write(text);
						}
					}
					Logger.Instance.LogTimeTaken("ApplyPackagesToVhdTimeTaken", (uint)(DateTime.Now - now2).TotalMilliseconds);
				}
			}
			Logger.Instance.LogTimeTaken("WriteVhdBootSettingsTimeTaken", (uint)(DateTime.Now - now).TotalMilliseconds);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		private bool CreateDiffDiskIfNeeded()
		{
			if (string.IsNullOrEmpty(this.ArgsProcessor.DiffDiskVhd))
			{
				return true;
			}
			if (this.ArgsProcessor.ReuseExistingDiffDisk && File.Exists(this.ArgsProcessor.DiffDiskVhd))
			{
				if (VhdUtils.IsVhdChildOfVhd(this.ArgsProcessor.VhdPath, this.ArgsProcessor.DiffDiskVhd))
				{
					return true;
				}
				File.Delete(this.ArgsProcessor.DiffDiskVhd);
			}
			bool flag = false;
			int num = 1;
			while (!flag)
			{
				try
				{
					VhdUtils.CreateDiffDisk(this.ArgsProcessor.VhdPath, this.ArgsProcessor.DiffDiskVhd);
					flag = true;
				}
				catch (IOException e)
				{
					if (num == 15)
					{
						this.ProcessFailedDiffDiskCreationException(e);
						return false;
					}
					Thread.Sleep(1000);
				}
				catch (Exception e2)
				{
					this.ProcessFailedDiffDiskCreationException(e2);
					return false;
				}
				num++;
			}
			this.RemovePreviousDriveFromVirtualMachine();
			return true;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000E38C File Offset: 0x0000C58C
		private void ProcessFailedDiffDiskCreationException(Exception e)
		{
			this.ShowErrorMessageForException(Resources.FailedToCreateDiffVhdFormat, e, "DiffDiskCreated");
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000E39F File Offset: 0x0000C59F
		private void SensorsConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "EnabledStates" && this.skin != null)
			{
				this.InvokeOnView(new MethodInvoker(delegate
				{
					this.skin.Sensors = this.sensorsConfig.EnabledStates;
				}));
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		private void AudioPipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected" && this.AudioPipe.IsConnected)
			{
				EventHandler audioPipeReady = this.AudioPipeReady;
				if (audioPipeReady != null)
				{
					audioPipeReady(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000E412 File Offset: 0x0000C612
		private void GuestNotificationsPipe_MicrophoneStarted(object sender, EventArgs e)
		{
			if (this.MicrophoneStarted != null)
			{
				this.MicrophoneStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000E42D File Offset: 0x0000C62D
		private void GuestNotificationsPipe_MicrophoneStopped(object sender, EventArgs e)
		{
			if (this.MicrophoneStoped != null)
			{
				this.MicrophoneStoped(this, EventArgs.Empty);
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000E448 File Offset: 0x0000C648
		private void GuestNotificationsPipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected" && this.guestNotificationsPipe.IsConnected && this.MicrophonePipeReady != null)
			{
				this.MicrophonePipeReady(this, EventArgs.Empty);
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000E484 File Offset: 0x0000C684
		private void ShellReadyPipe_ShellReadyEvent(object sender, EventArgs e)
		{
			uint timeTakenMilliseconds = (uint)(DateTime.Now - this.startedVM).TotalMilliseconds;
			Logger.Instance.Log("SystemReady", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				usingSnapshot = this.usingSnapshot,
				timeTakenMilliseconds = timeTakenMilliseconds
			});
			this.threadSnapshot = new Thread(new ThreadStart(this.HandleShellReadyProc));
			this.threadSnapshot.Start();
			this.StopPipeConnectTimer();
			this.shellReady = true;
			this.FireShellReady();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000E501 File Offset: 0x0000C701
		private void WaitForSnapshotThreadExit()
		{
			if (this.threadSnapshot != null)
			{
				this.threadSnapshot.Join();
				this.threadSnapshot = null;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000E51D File Offset: 0x0000C71D
		private void FireShellReady()
		{
			if (this.ShellReady != null)
			{
				this.ShellReady(this, EventArgs.Empty);
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000E538 File Offset: 0x0000C738
		private void HandleShellReadyProc()
		{
			this.SetCultureForCurrentThread();
			if (this.takingSnapshotWhenReady)
			{
				this.takingSnapshotWhenReady = false;
				Thread.Sleep(10000);
				string snapshotName = "Default Emulator Checkpoint";
				try
				{
					Logger.Instance.Log("SnapshotStarted", Logger.Level.Local);
					bool isGuestOSViewDisplayed = this.IsGuestOSViewDisplayed;
					this.restoringSnapshot = true;
					try
					{
						this.virtualMachine.CreateSnapshot(snapshotName);
					}
					finally
					{
						this.restoringSnapshot = false;
						if (isGuestOSViewDisplayed)
						{
							this.ConnectViewToVm();
						}
					}
					Logger.Instance.Log("SnapshotSucceeded", Logger.Level.Local);
				}
				catch (Exception e)
				{
					Logger.Instance.LogException("SnapshotCreated", e);
				}
				if (this.ArgsProcessor.SilentSnapshot)
				{
					Logger.Instance.Log("CloseAfterSilentSnapshot", Logger.Level.Local);
					this.Close(XdeReturnCode.Success);
					return;
				}
			}
			this.InitNonShellReadyPipes();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000E618 File Offset: 0x0000C818
		private void InitNonShellReadyPipes()
		{
			this.connectionManager.DoConnections();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000E625 File Offset: 0x0000C825
		private void InitServerSession()
		{
			if (this.ServerSession == null)
			{
				this.ServerSession = this.StartConnectionServer();
				this.appLifetimeDisposables.Add(this.ServerSession);
			}
			if (this.ArgsProcessor.WaitForClientConnection)
			{
				this.connectedEvent.WaitOne();
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000E665 File Offset: 0x0000C865
		private void UiFactory_ShowTaskDialog(object sender, TaskDialogArgs e)
		{
			if (this.ShowingTaskDialog != null)
			{
				this.ShowingTaskDialog(this, e);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000E67C File Offset: 0x0000C87C
		private void OnSkinChanged()
		{
			if (this.SkinChanged != null)
			{
				this.SkinChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000E697 File Offset: 0x0000C897
		private void OnVmStateChanged(VirtualMachineEnabledState state)
		{
			if (this.VmStateChanged != null)
			{
				this.VmStateChanged(this, new EnabledStateChangedEventArgs(state));
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		private bool ValidateArguments()
		{
			if (!this.SetRequestedLanguage())
			{
				return false;
			}
			if (!this.SetRequestedBootLanguage())
			{
				return false;
			}
			this.InitializeView();
			string vhdPath = this.ArgsProcessor.VhdPath;
			if (!string.IsNullOrEmpty(vhdPath) && !File.Exists(vhdPath))
			{
				string message = StringUtilities.CurrentCultureFormat(Resources.CantFindVhdFormat, new object[]
				{
					vhdPath
				});
				this.ShowErrorMessage(message);
				Logger.Instance.LogCommandLineParameterInvalid("FindVhd", string.Empty);
				return false;
			}
			if (string.IsNullOrEmpty(this.ArgsProcessor.VhdPath) && !string.IsNullOrEmpty(this.ArgsProcessor.DiffDiskVhd))
			{
				this.ShowErrorMessage(Resources.DiffDiskVhdRequiresVhdPath);
				Logger.Instance.LogCommandLineParameterInvalid("DiffDiskVhdPathRequired", string.Empty);
				return false;
			}
			if (this.ArgsProcessor.MemSize != null)
			{
				int value = this.ArgsProcessor.MemSize.Value;
				if (value < 8 || value > 2097152 || value % 2 != 0)
				{
					this.ShowErrorMessage(Resources.InvalidMemorySize);
					Logger.Instance.LogCommandLineParameterInvalid("MemorySize", value.ToString());
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E7CC File Offset: 0x0000C9CC
		private bool ValidateDisplayArguments()
		{
			if (!this.SkinFactory.IsValidResolutionName(this.ArgsProcessor.VideoResolution))
			{
				string message = StringUtilities.CurrentCultureFormat(Resources.InvalidVideoParamFormat, new object[]
				{
					this.ArgsProcessor.VideoResolution
				});
				this.ShowErrorMessage(message);
				Logger.Instance.LogCommandLineParameterInvalid("VideoParam", this.ArgsProcessor.VideoResolution);
				return false;
			}
			return this.SetRequestedBootScreenSize();
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E840 File Offset: 0x0000CA40
		private void InitValidSensorsOnSensorsConfig()
		{
			XdeSensors xdeSensors = (XdeSensors)Enum.Parse(typeof(XdeSensors), this.sku.Options.ValidSensors);
			XdeSensors validSensorsForDisplay = this.GetValidSensorsForDisplay();
			if (!this.ArgsProcessor.SensorsEnabled.HasFlag(XdeSensors.DesktopGPU))
			{
				this.preStartVGPUStatus = VGPUStatus.DisabledByCommandLine;
			}
			else if (!xdeSensors.HasFlag(XdeSensors.DesktopGPU) || !validSensorsForDisplay.HasFlag(XdeSensors.DesktopGPU) || this.sku.Options.GpuAssignmentMode == GpuAssignmentMode.None)
			{
				this.preStartVGPUStatus = VGPUStatus.DisabledBySku;
			}
			else if (!this.virtualServices.IsGpuSupported)
			{
				this.preStartVGPUStatus = VGPUStatus.NoCompatibleHostHardwareFound;
			}
			else if (DefaultSettings.GpuDisabledForXde)
			{
				this.preStartVGPUStatus = VGPUStatus.DisabledByRegistry;
			}
			XdeSensors xdeSensors2 = xdeSensors & validSensorsForDisplay;
			bool gpuDisabledForXde = DefaultSettings.GpuDisabledForXde;
			if (!this.virtualServices.IsGpuSupported || gpuDisabledForXde)
			{
				xdeSensors2 &= ~XdeSensors.DesktopGPU;
			}
			this.sensorsConfig.ValidSensors = xdeSensors2;
			this.sensorsConfig.RequiredSensors = this.GetRequiredSensorsForDisplay();
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000E954 File Offset: 0x0000CB54
		private void InitInputMode()
		{
			TouchMode inputMode = this.sku.Options.InputMode;
			bool hostCursorDisabledInMouseMode = this.sku.Options.HostCursorDisabledInMouseMode;
			Logger.Instance.Log("InputModeInitialized", Logger.Level.Info, new
			{
				PartA_iKey = "A-MSTelDefault",
				inputMode = inputMode,
				hostCursorDisabledInMouseMode = hostCursorDisabledInMouseMode
			});
			this.InputSettings.TouchMode = inputMode;
			this.InputSettings.HostCursorDisabledInMouseMode = hostCursorDisabledInMouseMode;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000E9B7 File Offset: 0x0000CBB7
		private XdeSensors GetValidSensorsForDisplay()
		{
			return this.GetRequestedDisplayInformation().ValidSensors;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		private XdeSensors GetRequiredSensorsForDisplay()
		{
			return this.GetRequestedDisplayInformation().RequiredSensors;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
		private bool LoadSkin(string videoResolution, XdeSensors sensors)
		{
			try
			{
				this.skin = this.SkinFactory.LoadSkinFromResolution(videoResolution, sensors);
			}
			catch (Exception e)
			{
				this.ShowErrorMessageForException(Resources.UnableToLoadSkin, e, "SkinLoaded");
				return false;
			}
			Logger.Instance.Log("SkinLoaded", Logger.Level.Info, new
			{
				PartA_iKey = "A-MSTelDefault",
				displayWidth = this.skin.Display.TotalDisplayWidth,
				displayHeight = this.skin.Display.DisplayHeight
			});
			if (this.ArgsProcessor.ShowName)
			{
				this.skin.InformationText = this.VirtualMachineName;
			}
			this.OnSkinChanged();
			return true;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000EA80 File Offset: 0x0000CC80
		private bool CanAccessVmApi()
		{
			if (!this.virtualServices.CanAccessApi)
			{
				this.UiFactory.ShowElevationRequiredForHyperV();
				return false;
			}
			return true;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000EAA0 File Offset: 0x0000CCA0
		private string GetEffectiveVhd()
		{
			string result = this.ArgsProcessor.VhdPath;
			if (!string.IsNullOrEmpty(this.ArgsProcessor.DiffDiskVhd))
			{
				result = this.ArgsProcessor.DiffDiskVhd;
			}
			return result;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000EAD8 File Offset: 0x0000CCD8
		private bool FindOrCreateVirtualMachine()
		{
			string virtualMachineName = this.VirtualMachineName;
			string effectiveVhd = this.GetEffectiveVhd();
			bool generation = StringUtilities.EqualsNoCase(Path.GetExtension(effectiveVhd), ".VHDX");
			SettingsOptions options = this.AttemptLoadSnapshot ? (SettingsOptions.ReadOnly | SettingsOptions.LoadSettingsAsync) : SettingsOptions.None;
			try
			{
				this.virtualMachine = this.virtualServices.GetVirtualMachine(virtualMachineName, options);
			}
			catch (Exception ex)
			{
				this.ShowErrorMessage(ex.Message);
				Logger.Instance.LogException("VMFound", ex);
				return false;
			}
			if (this.virtualMachine == null && effectiveVhd == null)
			{
				string message = StringUtilities.CurrentCultureFormat(Resources.CantFindVMFormat, new object[]
				{
					virtualMachineName
				});
				this.ShowErrorMessage(message);
				Logger.Instance.LogError("VMFound");
				return false;
			}
			if (this.virtualMachine != null)
			{
				this.appLifetimeDisposables.Add(this.virtualMachine);
				this.snapshotControl.Initialize(this.virtualMachine);
			}
			bool result;
			for (;;)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				try
				{
					try
					{
						if (this.virtualMachine == null)
						{
							this.virtualMachine = this.virtualServices.CreateVirtualMachine(virtualMachineName, generation);
							flag = true;
							this.appLifetimeDisposables.Add(this.virtualMachine);
							this.snapshotControl.Initialize(this.virtualMachine);
						}
					}
					catch (Exception e)
					{
						this.ShowErrorMessageForException(Resources.FailedToCreateVMFormat, e, "VMCreated");
						result = false;
						break;
					}
					if (!this.InitializeRequestedVmSettings(flag))
					{
						result = false;
					}
					else
					{
						if (!this.InitSnapshotIfNeeded(effectiveVhd, flag))
						{
							flag3 = true;
							continue;
						}
						if (!this.InitializeDiffAndVhd())
						{
							result = false;
						}
						else
						{
							this.virtualMachine.EnableStateChanged += this.VirtualMachine_EnableStateChanged;
							flag2 = true;
							result = true;
						}
					}
				}
				finally
				{
					if (!flag2 && (flag || flag3) && this.virtualMachine != null)
					{
						try
						{
							this.virtualMachine.DeleteVirtualMachine();
							this.DisposeVirtualMachine();
						}
						catch (Exception)
						{
						}
					}
				}
				break;
			}
			return result;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000ECC4 File Offset: 0x0000CEC4
		private bool InitializeDiffAndVhd()
		{
			if (!this.usingSnapshot)
			{
				this.EnsureVirtualMachineStopped();
				if (!this.CreateDiffDiskIfNeeded())
				{
					return false;
				}
				if (!this.SetVhdPathIfNeeded(this.GetEffectiveVhd()))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		private bool InitializeRequestedVmSettings(bool newVm)
		{
			bool result;
			try
			{
				if (!string.IsNullOrEmpty(this.ArgsProcessor.Com1PipeName))
				{
					this.virtualMachine.CurrentSettings.Com1NamedPipe = this.ArgsProcessor.Com1PipeName;
				}
				if (!string.IsNullOrEmpty(this.ArgsProcessor.Com2PipeName))
				{
					this.virtualMachine.CurrentSettings.Com2NamedPipe = this.ArgsProcessor.Com2PipeName;
				}
				int? num = null;
				if (this.ArgsProcessor.MemSize != null)
				{
					num = new int?(this.ArgsProcessor.MemSize.Value);
				}
				else if (newVm)
				{
					num = new int?(this.sku.Options.DefaultMemSize);
				}
				if (num != null)
				{
					this.virtualMachine.CurrentSettings.RamSize = num.Value;
				}
				this.virtualMachine.CurrentSettings.NumProcessors = Math.Min(this.sku.Options.ProcessorCount, Environment.ProcessorCount);
				this.InitializeVmResolutionSettings();
				result = true;
			}
			catch (Exception e)
			{
				this.ShowErrorMessageForException(Resources.FailedToSetVmProperties, e, "VmPropertiesSet");
				result = false;
			}
			return result;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000EE34 File Offset: 0x0000D034
		private void InitializeVmResolutionSettings()
		{
			if (this.virtualMachine.SnapshotSettings.Count == 0)
			{
				VGPUStatus vgpustatus = this.preStartVGPUStatus;
				if (vgpustatus == VGPUStatus.Enabled && this.previouslyEnabledSensors != null && !this.previouslyEnabledSensors.Value.HasFlag(XdeSensors.DesktopGPU))
				{
					vgpustatus = VGPUStatus.DisabledByUser;
				}
				this.virtualMachine.CurrentSettings.EnsureHasDisplayAdapter(vgpustatus, this.GetRequestedScreenResolution(), this.GetRequestedVRAMforVGPU(), this.skuGpuAssignmentMode);
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		private string GetFixedSnapshotName()
		{
			return this.snapshotControl.DefaultSnapshot;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		private bool InitSnapshotIfNeeded(string vhd, bool createdNewVm)
		{
			bool result;
			try
			{
				if (this.UsingSnapshotOption)
				{
					this.takingSnapshotWhenReady = true;
					this.usingSnapshot = false;
					if (!createdNewVm && this.AttemptLoadSnapshot)
					{
						string fixedSnapshotName = this.GetFixedSnapshotName();
						IXdeVirtualMachineSettings xdeVirtualMachineSettings = this.virtualMachine.FindSnapshotSettings(fixedSnapshotName);
						if (xdeVirtualMachineSettings != null)
						{
							if (!File.Exists(vhd))
							{
								vhd = null;
							}
							bool flag = XdeController.AreSettingsEquivalent(xdeVirtualMachineSettings, this.virtualMachine.CurrentSettings);
							bool flag2 = vhd == null || StringUtilities.EqualsNoCase(xdeVirtualMachineSettings.VhdPath, vhd) || XdeController.IsVhdSnapshotOfBaseVhd(xdeVirtualMachineSettings.VhdPath, vhd);
							if (flag && flag2 && !this.ArgsProcessor.SilentSnapshot)
							{
								this.EnsureVirtualMachineStopped();
								try
								{
									this.virtualMachine.ApplySnapshot(xdeVirtualMachineSettings);
									this.usingSnapshot = true;
									this.takingSnapshotWhenReady = false;
								}
								catch (Exception e)
								{
									Logger.Instance.LogException("SnapshotApplied", e);
								}
								if (this.usingSnapshot)
								{
									this.currentSnapshotSettings = xdeVirtualMachineSettings;
									this.appLifetimeDisposables.Add(this.currentSnapshotSettings);
								}
							}
							if (!this.usingSnapshot)
							{
								Logger.Instance.Log("SnapshotRemovedAfterSettingsDidntMatch", Logger.Level.Info, new
								{
									PartA_iKey = "A-MSTelDefault",
									settingsEquivalent = flag,
									vhdEquivalent = flag2,
									sdcardVhdEquivalent = true,
									silentSnapshot = this.ArgsProcessor.SilentSnapshot
								});
								this.BlowAwaySnapshot(xdeVirtualMachineSettings, InvalidSettingsReason.SnapshotSettingsMismatchWithNewSettings);
								if (this.virtualMachine == null)
								{
									return false;
								}
							}
						}
					}
				}
				result = true;
			}
			catch (Exception e2)
			{
				Logger.Instance.LogException("SnapshotsInitialized", e2);
				result = false;
			}
			return result;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000F058 File Offset: 0x0000D258
		private void BlowAwaySnapshot(IXdeVirtualMachineSettings snapshotSettings, InvalidSettingsReason reason)
		{
			Logger.Instance.Log("SnapshotDeleted", Logger.Level.Info, new
			{
				PartA_iKey = "A-MSTelDefault",
				reason = reason
			});
			try
			{
				snapshotSettings.Delete();
				this.virtualMachine.RevertToStoppedState();
			}
			catch (Exception)
			{
				try
				{
					this.virtualMachine.DeleteVirtualMachine();
					this.DisposeVirtualMachine();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000F0C8 File Offset: 0x0000D2C8
		private bool SetVhdPathIfNeeded(string vhd)
		{
			bool result;
			try
			{
				if (!string.IsNullOrEmpty(vhd) && !StringComparer.OrdinalIgnoreCase.Equals(this.virtualMachine.CurrentSettings.VhdPath, vhd))
				{
					this.virtualMachine.CurrentSettings.VhdPath = vhd;
				}
				result = true;
			}
			catch (Exception e)
			{
				this.ShowErrorMessageForException(Resources.FailedToSetVhd, e, "VhdPathSet");
				result = false;
			}
			return result;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000F138 File Offset: 0x0000D338
		private void RemovePreviousDriveFromVirtualMachine()
		{
			if (this.virtualMachine != null)
			{
				this.virtualMachine.CurrentSettings.VhdPath = null;
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000F154 File Offset: 0x0000D354
		private void VirtualMachine_EnableStateChanged(object sender, EnabledStateChangedEventArgs e)
		{
			VirtualMachineEnabledState enabledState = e.EnabledState;
			VirtualMachineEnabledState virtualMachineEnabledState = this.lastState;
			if (enabledState == virtualMachineEnabledState)
			{
				return;
			}
			if (this.IsShuttingDown || this.disposed)
			{
				return;
			}
			this.lastState = enabledState;
			this.OnVmStateChanged(e.EnabledState);
			Logger.Instance.Log("VirtualMachineStateChanged", Logger.Level.Info, new
			{
				PartA_iKey = "A-MSTelDefault",
				enabledState = enabledState.ToString()
			});
			if (enabledState == VirtualMachineEnabledState.Shutdown && virtualMachineEnabledState == VirtualMachineEnabledState.Enabled)
			{
				this.ResetAllConnections();
				return;
			}
			if (enabledState == VirtualMachineEnabledState.Disabled && this.DeleteCheckpointsAfterReboot)
			{
				this.HandleIntentionalShutdown();
				this.OnVmReboot();
				return;
			}
			if (enabledState == VirtualMachineEnabledState.Enabled && virtualMachineEnabledState == VirtualMachineEnabledState.Shutdown)
			{
				this.OnVmReboot();
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000F1F8 File Offset: 0x0000D3F8
		private void HandleIntentionalShutdown()
		{
			this.EnsureVirtualMachineStopped();
			this.DeleteAllCheckpoints();
			this.usingSnapshot = false;
			this.takingSnapshotWhenReady = this.UsingSnapshotOption;
			this.previouslyEnabledSensors = new XdeSensors?(this.sensorsConfig.EnabledStates);
			this.InitializeVmResolutionSettings();
			this.EnsureVirtualMachineRunning(false);
			this.DeleteCheckpointsAfterReboot = false;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000F250 File Offset: 0x0000D450
		private void DeleteAllCheckpoints()
		{
			foreach (IXdeVirtualMachineSettings xdeVirtualMachineSettings in this.virtualMachine.SnapshotSettings)
			{
				xdeVirtualMachineSettings.Delete();
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
		private void InitializePipeConnectTimer()
		{
			if (this.ArgsProcessor.PipeTimeout > 0)
			{
				if (this.giveupOnPipesTimer == null)
				{
					this.giveupOnPipesTimer = new System.Timers.Timer();
					this.giveupOnPipesTimer.Interval = (double)this.ArgsProcessor.PipeTimeout;
					this.giveupOnPipesTimer.Elapsed += new ElapsedEventHandler(this.GiveupOnPipeConnectTimerCallback);
					this.appLifetimeDisposables.Add(this.giveupOnPipesTimer);
				}
				this.giveupOnPipesTimer.Start();
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000F318 File Offset: 0x0000D518
		private void StopPipeConnectTimer()
		{
			if (this.giveupOnPipesTimer != null && this.giveupOnPipesTimer.Enabled)
			{
				this.giveupOnPipesTimer.Stop();
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000F33C File Offset: 0x0000D53C
		private void GiveupOnPipeConnectTimerCallback(object sender, EventArgs e)
		{
			this.giveupOnPipesTimer.Stop();
			if (!this.IsShuttingDown && !this.IsShellReady)
			{
				this.ShowFailedToConnectToGuestMessage(new ExEventArgs(new Exception(Resources.GuestNeverResponded)));
				if (this.ArgsProcessor.SilentSnapshot)
				{
					this.Close(XdeReturnCode.FailedToConnect);
				}
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000F390 File Offset: 0x0000D590
		private void WireViewEvents()
		{
			this.View.Shown += this.View_Shown;
			this.View.Closing += this.View_Closing;
			this.View.Load += this.View_Load;
			this.View.RdpDisconnected += this.View_RdpDisconnected;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000F3FC File Offset: 0x0000D5FC
		private void View_RdpDisconnected(object sender, EventArgs e)
		{
			this.IsGuestOSViewDisplayed = false;
			if (!this.IsShuttingDown && !this.restoringSnapshot && !this.rebooting && !this.DeleteCheckpointsAfterReboot)
			{
				this.View.IndicateGuestVideoDisconnected();
			}
			if (this.restoringSnapshot)
			{
				this.View.IndicateRestoring();
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000F44E File Offset: 0x0000D64E
		private XdeServerSession StartConnectionServer()
		{
			return XdeServerSession.ServiceHostFactory(this.VirtualMachineName, this);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000F45C File Offset: 0x0000D65C
		private void View_Load(object sender, EventArgs e)
		{
			Screen[] allScreens = Screen.AllScreens;
			for (int i = 0; i < allScreens.Length; i++)
			{
				if (allScreens[i].Bounds.Contains(this.settingsVM.ScreenLocation))
				{
					this.View.ScreenLocation = this.settingsVM.ScreenLocation;
					return;
				}
			}
			this.FitToScreen();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000F4B7 File Offset: 0x0000D6B7
		private void View_Closing(object sender, CancelEventArgs e)
		{
			if (!this.IsShuttingDown)
			{
				this.Close();
				e.Cancel = true;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		private void ShutdownXdeProc()
		{
			this.SetCultureForCurrentThread();
			try
			{
				if (this.XdeExit != null)
				{
					this.XdeExit(this, EventArgs.Empty);
				}
				this.WaitForThreadInitExit();
				if (this.View != null && !this.ArgsProcessor.SilentSnapshot)
				{
					this.SaveUserSettings();
				}
				if (this.virtualMachine != null && this.virtualMachine.EnabledState == VirtualMachineEnabledState.Enabled)
				{
					if (this.VirtualMachineShutdown == null)
					{
						this.VirtualMachineShutdown = new VirtualMachineShutdown();
					}
					if (this.VirtualMachineShuttingDown != null)
					{
						this.VirtualMachineShuttingDown(this, EventArgs.Empty);
					}
					this.VirtualMachineShutdown.ShutdownVirtualMachine(this.virtualMachine, this.SimpleCommandsPipe, this.ArgsProcessor.FastShutdown ? 0 : 120000);
				}
				this.RemoveSnapshotIfNeeded();
			}
			catch (Exception)
			{
			}
			this.CloseViewAsync();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000F5AC File Offset: 0x0000D7AC
		private void RemoveSnapshotIfNeeded()
		{
			if (this.usingSnapshot && this.errorShownForConnect && this.virtualMachine != null)
			{
				string fixedSnapshotName = this.GetFixedSnapshotName();
				IXdeVirtualMachineSettings xdeVirtualMachineSettings = this.virtualMachine.FindSnapshotSettings(fixedSnapshotName);
				if (xdeVirtualMachineSettings != null)
				{
					Logger.Instance.Log("SnapshotRemovedAfterFailedConnect", Logger.Level.Info);
					xdeVirtualMachineSettings.Delete();
				}
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000F600 File Offset: 0x0000D800
		private void SaveUserSettings()
		{
			double dpi = this.View.Dpi;
			this.settingsVM.Zoom = (int)(this.skin.DisplayScale * (1.0 / dpi) * 100.0);
			this.settingsVM.ScreenLocation = this.View.ScreenLocation;
			this.settingsVM.DisplayOrientation = this.DisplayOrientation;
			XdeUserSettings.Default.AddVmSetting(this.settingsVM);
			XdeUserSettings.Default.Save();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000F688 File Offset: 0x0000D888
		private void InitializeThreadProc()
		{
			try
			{
				this.SetCultureForCurrentThread();
				this.InitServerSession();
				if (this.networkManager != null && !this.networkManager.InitializeNetworkConfig())
				{
					this.Close(XdeReturnCode.CouldntInitNetworkConfig);
				}
				else
				{
					while (this.virtualMachine == null)
					{
						this.bootSensors = this.ArgsProcessor.SensorsEnabled;
						if (!this.FindOrCreateVirtualMachine())
						{
							this.Close(XdeReturnCode.CouldntFindOrCreateVm);
							return;
						}
						if (this.ArgsProcessor.NoStart)
						{
							return;
						}
						if (!this.EnsureVirtualMachineRunning(true))
						{
							if (this.exitCode != XdeReturnCode.Success)
							{
								this.Close(this.exitCode);
								return;
							}
							if (!this.AttemptLoadSnapshot)
							{
								this.Close(XdeReturnCode.CouldntStartVm);
								return;
							}
							this.CleanupFromFailedSnapshot(InvalidSettingsReason.SnapshotFailedToStart);
						}
						else if (this.AttemptLoadSnapshot)
						{
							this.virtualMachine.CurrentSettings.WaitForLoadedSettings();
							InvalidSettingsReason invalidSettingsReason = this.virtualMachine.CurrentSettings.InvalidSettingsReason;
							if (invalidSettingsReason == InvalidSettingsReason.NotInvalid && this.currentSnapshotSettings != null)
							{
								this.currentSnapshotSettings.WaitForLoadedSettings();
								invalidSettingsReason = this.currentSnapshotSettings.InvalidSettingsReason;
							}
							if (invalidSettingsReason != InvalidSettingsReason.NotInvalid)
							{
								this.CleanupFromFailedSnapshot(invalidSettingsReason);
							}
						}
					}
					this.ConnectionManager.DoShellReady();
				}
			}
			catch (Exception e)
			{
				this.ShowErrorMessageForException(Resources.UnableToConnectToDevice, e, "InitializeThreadProc");
				this.Close(XdeReturnCode.GenericError);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000F7E8 File Offset: 0x0000D9E8
		private void CleanupFromFailedSnapshot(InvalidSettingsReason reason)
		{
			this.EnsureVirtualMachineStopped();
			if (this.usingSnapshot)
			{
				string fixedSnapshotName = this.GetFixedSnapshotName();
				IXdeVirtualMachineSettings xdeVirtualMachineSettings = this.virtualMachine.FindSnapshotSettings(fixedSnapshotName);
				if (xdeVirtualMachineSettings != null)
				{
					this.BlowAwaySnapshot(xdeVirtualMachineSettings, reason);
				}
				this.usingSnapshot = false;
			}
			this.AttemptLoadSnapshot = false;
			this.DisposeVirtualMachine();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000F837 File Offset: 0x0000DA37
		private void DisposeVirtualMachine()
		{
			this.DisposeSnapshotSettings();
			if (this.virtualMachine != null)
			{
				this.appLifetimeDisposables.Remove(this.virtualMachine);
				this.virtualMachine.Dispose();
				this.virtualMachine = null;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000F86B File Offset: 0x0000DA6B
		private void DisposeSnapshotSettings()
		{
			if (this.currentSnapshotSettings != null)
			{
				this.appLifetimeDisposables.Remove(this.currentSnapshotSettings);
				this.currentSnapshotSettings.Dispose();
				this.currentSnapshotSettings = null;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000F899 File Offset: 0x0000DA99
		private void ReconnectThreadProc()
		{
			this.ConnectionManager.DoShellReady();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000F8A6 File Offset: 0x0000DAA6
		private void ConnectViewToVm()
		{
			if (!this.ArgsProcessor.NoStart)
			{
				this.InvokeOnView(new MethodInvoker(delegate
				{
					if (this.IsGuestOSViewDisplayed)
					{
						return;
					}
					this.IsGuestOSViewDisplayed = true;
					this.View.ConnectToVirtualMachineGuid(this.VirtualMachineName, this.virtualMachine.Guid);
				}));
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000F8C7 File Offset: 0x0000DAC7
		private void View_Shown(object sender, EventArgs e)
		{
			this.StartInitializationThread();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000F8D0 File Offset: 0x0000DAD0
		private bool EnsureVirtualMachineStopped()
		{
			try
			{
				XdeController.Retry(10, 1000, delegate
				{
					if (this.virtualMachine.EnabledState != VirtualMachineEnabledState.Disabled)
					{
						this.virtualMachine.Stop();
					}
				});
			}
			catch (Exception e)
			{
				this.ShowErrorMessageForException(Resources.FailedToStopFormat, e, "VmStopped");
				return false;
			}
			return true;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000F920 File Offset: 0x0000DB20
		private bool EnsureVirtualMachineRunning(bool initialBoot)
		{
			bool result;
			try
			{
				if (!this.ArgsProcessor.BootToSnapshot && this.virtualMachine.EnabledState != VirtualMachineEnabledState.Disabled)
				{
					this.virtualMachine.Stop();
				}
				if (this.VirtualMachineStarting != null)
				{
					this.VirtualMachineStarting(this, new VirtualMachineStartupEventArgs(this.usingSnapshot));
				}
				if (!this.usingSnapshot)
				{
					if (initialBoot && !this.sku.Options.WriteVhdBootSettingsDisabled)
					{
						bool wroteVhdBootSettings = false;
						XdeController.Retry(10, 1000, delegate
						{
							wroteVhdBootSettings = this.TryWriteVhdBootSettings(this.virtualMachine.CurrentSettings.VhdPath);
						});
						if (!wroteVhdBootSettings)
						{
							return false;
						}
					}
					this.InvokeOnView(new MethodInvoker(delegate
					{
						this.View.IndicateFullBoot();
					}));
				}
				bool bootingToTakeSnapshot = this.takingSnapshotWhenReady;
				this.virtualMachine.Start(bootingToTakeSnapshot);
				Logger.Instance.Log("VmStarted", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					vmId = this.virtualMachine.Guid
				});
				this.ConnectViewToVmIfNeeded();
				result = true;
			}
			catch (Exception ex)
			{
				string text;
				if ((ex.Data.Contains("ReturnCode") ? ((WmiUtils.ReturnCode)ex.Data["ReturnCode"]) : WmiUtils.ReturnCode.Unknown) == WmiUtils.ReturnCode.OutOfMemory)
				{
					this.AttemptLoadSnapshot = false;
					text = StringUtilities.CurrentCultureFormat(Resources.OutOfMemoryOnStartupFormat, new object[]
					{
						this.virtualMachine.CurrentSettings.RamSize
					});
				}
				else
				{
					text = ex.Message;
					if (ex.InnerException != null)
					{
						text = text + "\r\n\r\n" + ex.InnerException.Message;
					}
				}
				if (!this.AttemptLoadSnapshot)
				{
					Logger.Instance.LogException("VmStarted", ex);
					string message = StringUtilities.CurrentCultureFormat(Resources.FailedToStartFormat, new object[]
					{
						text
					});
					this.ShowErrorMessage(message);
				}
				else
				{
					Logger.Instance.LogException("VmStartedWithSnapshot", ex);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000FB14 File Offset: 0x0000DD14
		private void ShowFailedToConnectToGuestMessage(string errorDetails)
		{
			bool flag = false;
			if (this.disposed)
			{
				return;
			}
			object obj = this.errorShownForConnectLock;
			lock (obj)
			{
				if (!this.errorShownForConnect)
				{
					this.errorShownForConnect = true;
					flag = true;
				}
			}
			if (flag)
			{
				string message = StringUtilities.CurrentCultureFormat(Resources.UnableToConnectToGuestFormat, new object[]
				{
					errorDetails
				});
				Logger.Instance.LogError("ConnectToGuestFailed", new
				{
					errorDetails = Logger.Instance.ReplaceSensitiveStrings(errorDetails)
				});
				this.ShowErrorMessage(message);
			}
			this.ConnectViewToVmIfNeeded();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000FBB0 File Offset: 0x0000DDB0
		private void ShowFailedToConnectToGuestMessage(ExEventArgs args)
		{
			this.ShowFailedToConnectToGuestMessage(args.ExceptionData.Message);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		private void SimpleCommandsPipe_PropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName != "IsConnected" || !this.SimpleCommandsPipe.IsConnected)
			{
				return;
			}
			Logger.Instance.Log("ConnectedToGuest", Logger.Level.Info);
			this.GetResolutionFromGuest();
			this.gotRes = true;
			this.ConnectViewToVmIfNeeded();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000FC14 File Offset: 0x0000DE14
		private void ConnectViewToVmIfNeeded()
		{
			bool flag = this.gotRes;
			if ((flag || (this.sku != null && this.sku.Options.ShowGuestDisplayASAP)) && !this.IsGuestOSViewDisplayed && !this.ArgsProcessor.SilentSnapshot)
			{
				this.ConnectViewToVm();
			}
			if (flag && this.firstReadyForInteraction)
			{
				this.firstReadyForInteraction = false;
				uint timeTakenMilliseconds = (uint)(DateTime.Now - App.ProgramStartedAt).TotalMilliseconds;
				Logger.Instance.Log("ViewReadyForInteraction", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					usingSnapshot = this.usingSnapshot,
					timeTakenMilliseconds = timeTakenMilliseconds
				});
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		private void OnVmReboot()
		{
			this.rebooting = true;
			this.usingSnapshot = false;
			this.ServerSession.OnVmReboot();
			this.startedVM = DateTime.Now;
			EventHandler xdeReboot = this.XdeReboot;
			if (xdeReboot != null)
			{
				xdeReboot(this, EventArgs.Empty);
			}
			this.ResetAllConnections();
			this.IsShuttingDown = false;
			this.InvokeOnView(new MethodInvoker(delegate
			{
				this.LoadSku();
				this.View.IndicateFullBoot();
				this.ConnectViewToVmIfNeeded();
			}));
			this.threadReconnect = new Thread(new ThreadStart(this.ReconnectThreadProc));
			this.threadReconnect.Start();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000FD40 File Offset: 0x0000DF40
		private void UpdateBrandingInfo()
		{
			if (this.sku != null)
			{
				this.View.Icon = this.sku.Branding.Icon;
				this.UiFactory.Sku = this.sku;
			}
			this.View.Text = this.GetViewTitle();
			this.View.ShowDisplayName = this.ArgsProcessor.ShowName;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
		private void ResetAllConnections()
		{
			this.View.BeginInvoke(new MethodInvoker(delegate
			{
				this.DisposeGuestLifetimeObjects();
				this.InitUiFactory();
			}));
			this.shellReady = false;
			this.gotRes = false;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000FDD0 File Offset: 0x0000DFD0
		private void DisposeObject(IDisposable disposableObject)
		{
			try
			{
				if (disposableObject != null)
				{
					disposableObject.Dispose();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		private void DisposePipeObjects()
		{
			this.WaitForSnapshotThreadExit();
			this.DisposeObject(this.ConnectionManager);
			this.DisposeObject(this.AddressInfo);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000FE1C File Offset: 0x0000E01C
		private void DisposeGuestLifetimeObjects()
		{
			this.UiFactory.IsShuttingDown = true;
			this.DisposePipeObjects();
			this.DisposeSku();
			this.DisposeObject(this.UiFactory);
			this.UiFactory = null;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000FE49 File Offset: 0x0000E049
		private void DisposeSku()
		{
			if (this.sku != null)
			{
				this.sku.Dispose();
				this.sku = null;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000FE68 File Offset: 0x0000E068
		private void GetResolutionFromGuest()
		{
			Size currentResolution = new Size(this.skin.Display.TotalDisplayWidth, this.skin.Display.DisplayHeight);
			try
			{
				currentResolution = this.virtualMachine.GetCurrentResolution();
				Logger.Instance.Log("GuestResolutionIndicated", Logger.Level.Info, new
				{
					PartA_iKey = "A-MSTelDefault",
					width = currentResolution.Width,
					height = currentResolution.Height
				});
			}
			catch (Exception e)
			{
				Logger.Instance.LogException("GuestResolution", e);
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		private void BringOtherXdeToFront()
		{
			IntPtr intPtr = NativeMethods.FindWindowW(null, this.View.Text);
			if (intPtr != IntPtr.Zero)
			{
				NativeMethods.ShowWindow(intPtr, 1);
				NativeMethods.SetForegroundWindow(intPtr);
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000FF33 File Offset: 0x0000E133
		private void InvokeOnView(Delegate method)
		{
			if (!this.IsShuttingDown && this.View != null && !this.ArgsProcessor.SilentSnapshot)
			{
				this.View.Invoke(method);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000FF5F File Offset: 0x0000E15F
		private void CloseViewAsync()
		{
			if (this.View != null)
			{
				this.View.AsynchronousClose();
			}
			this.xdeDoneEvent.Set();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000FF80 File Offset: 0x0000E180
		private void Close(XdeReturnCode returnCode)
		{
			this.exitCode = returnCode;
			this.ShutdownXde();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000FF90 File Offset: 0x0000E190
		private void AddUserToSecurityGroupIfRequested(string userName, string groupName)
		{
			if (string.IsNullOrEmpty(userName))
			{
				return;
			}
			int num = NativeMethods.NetLocalGroupAddMembers(null, groupName, 3, new string[]
			{
				userName
			}, 1);
			if (num == 0)
			{
				Logger.Instance.Log("UserAddedToSecurityGroup", Logger.Level.Info);
				return;
			}
			if (num == 1378)
			{
				Logger.Instance.LogAddUserToSecurityGroupIfRequestedError(num);
				return;
			}
			Logger.Instance.LogAddUserToSecurityGroupIfRequestedError(num);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000FFF0 File Offset: 0x0000E1F0
		private string StartedByWhatProgram()
		{
			try
			{
				Process process = NativeMethods.FindParentProcess();
				if (process != null)
				{
					return process.ProcessName;
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00010028 File Offset: 0x0000E228
		private void AddSensitiveStrings()
		{
			string text = null;
			try
			{
				string fullName = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
				Logger.Instance.AddSensitive(Directory.GetParent(fullName).ToString(), "<SpecialFolder>");
				Logger.Instance.AddSensitive(Environment.UserName, "<UserName>");
				text = WindowsIdentity.GetCurrent().Name.Split(new char[]
				{
					'\\'
				}).Last<string>();
				Logger.Instance.AddSensitive(text, "<CurrentUserName>");
			}
			catch
			{
			}
			Logger.Instance.AddSensitive(this.ArgsProcessor.VirtualMachineName, "<VirtualMachineName>");
			if (string.IsNullOrEmpty(text))
			{
				Logger.Instance.AddSensitive(this.ArgsProcessor.DisplayName, "<DisplayName>");
			}
			Logger.Instance.AddSensitive(this.ArgsProcessor.DiffDiskVhd, "<DiffDiskVhd>");
			Logger.Instance.AddSensitive(this.ArgsProcessor.VhdPath, "<VhdPath>");
			Logger.Instance.AddSensitive(this.ArgsProcessor.AddUserToHyperVAdmins, "<HyperVAdminsUserName>");
			Logger.Instance.AddSensitive(this.ArgsProcessor.AddUserToPerformanceLogUsersGroup, "<PerformanceLogUsersGroupUserName>");
			Logger.Instance.AddSensitive(this.ArgsProcessor.Com1PipeName, "<Com1PipeName>");
			Logger.Instance.AddSensitive(this.ArgsProcessor.Com2PipeName, "<Com2PipeName>");
		}

		// Token: 0x04000125 RID: 293
		public const float MinScale = 0.1f;

		// Token: 0x04000126 RID: 294
		public const float MaxScale = 1f;

		// Token: 0x04000127 RID: 295
		public const float DefaultScale = 0.6f;

		// Token: 0x04000128 RID: 296
		private const string ButtonLocation = "Toolbar";

		// Token: 0x04000129 RID: 297
		private static readonly Size IntroScreenSize = new Size(1024, 768);

		// Token: 0x0400012A RID: 298
		private static readonly Point UnspecifiedLocation = new Point(int.MaxValue, int.MaxValue);

		// Token: 0x0400012B RID: 299
		private static TimeSpan bootFinishedTimeout = new TimeSpan(0, 1, 0);

		// Token: 0x0400012C RID: 300
		private IXdeVirtualMachine virtualMachine;

		// Token: 0x0400012D RID: 301
		private IXdeVirtualMachineSettings currentSnapshotSettings;

		// Token: 0x0400012E RID: 302
		private IXdeSkin skin;

		// Token: 0x0400012F RID: 303
		private bool disposed;

		// Token: 0x04000130 RID: 304
		private bool rebooting;

		// Token: 0x04000131 RID: 305
		private DateTime startedVM = Process.GetCurrentProcess().StartTime;

		// Token: 0x04000132 RID: 306
		private VmUserSettings settingsVM;

		// Token: 0x04000133 RID: 307
		private bool xdeShutdownStarted;

		// Token: 0x04000134 RID: 308
		private Thread threadInit;

		// Token: 0x04000135 RID: 309
		private Thread threadReconnect;

		// Token: 0x04000136 RID: 310
		private Thread threadSnapshot;

		// Token: 0x04000137 RID: 311
		private bool usingSnapshot;

		// Token: 0x04000138 RID: 312
		private bool takingSnapshotWhenReady;

		// Token: 0x04000139 RID: 313
		private ManualResetEvent connectedEvent = new ManualResetEvent(false);

		// Token: 0x0400013A RID: 314
		private bool shellReady;

		// Token: 0x0400013B RID: 315
		private bool errorShownForConnect;

		// Token: 0x0400013C RID: 316
		private object errorShownForConnectLock = new object();

		// Token: 0x0400013D RID: 317
		private System.Timers.Timer giveupOnPipesTimer;

		// Token: 0x0400013E RID: 318
		private bool connectedRdpToVm;

		// Token: 0x0400013F RID: 319
		private bool gotRes;

		// Token: 0x04000140 RID: 320
		private XdeReturnCode exitCode;

		// Token: 0x04000141 RID: 321
		private ManualResetEvent xdeDoneEvent = new ManualResetEvent(false);

		// Token: 0x04000142 RID: 322
		private List<IDisposable> appLifetimeDisposables = new List<IDisposable>();

		// Token: 0x04000143 RID: 323
		private CultureInfo cultureInfo;

		// Token: 0x04000144 RID: 324
		private string bootLanguage;

		// Token: 0x04000145 RID: 325
		private Size bootScreenSize;

		// Token: 0x04000146 RID: 326
		private SnapshotControl snapshotControl;

		// Token: 0x04000147 RID: 327
		private IXdeNetworkManager networkManager;

		// Token: 0x04000148 RID: 328
		private bool restoringSnapshot;

		// Token: 0x04000149 RID: 329
		private IXdeSensorsConfig sensorsConfig;

		// Token: 0x0400014A RID: 330
		private IXdeConnectionManager connectionManager;

		// Token: 0x0400014B RID: 331
		private IXdeSku sku;

		// Token: 0x0400014C RID: 332
		private IXdeAutomationAudioPipe audioPipe;

		// Token: 0x0400014D RID: 333
		private IXdeAutomationSimpleCommandsPipe simpleCommandsPipe;

		// Token: 0x0400014E RID: 334
		private IXdeAutomationGuestNotificationsPipe guestNotificationsPipe;

		// Token: 0x0400014F RID: 335
		private VirtualMachineEnabledState lastState;

		// Token: 0x04000150 RID: 336
		private XdeSensors bootSensors = XdeSensors.Default;

		// Token: 0x04000151 RID: 337
		private IXdeConnectionAddressInfo addressInfo;

		// Token: 0x04000152 RID: 338
		private IXdeVirtualServices virtualServices;

		// Token: 0x04000153 RID: 339
		private bool firstReadyForInteraction = true;

		// Token: 0x04000154 RID: 340
		private VGPUStatus preStartVGPUStatus = VGPUStatus.Enabled;

		// Token: 0x04000155 RID: 341
		private XdeSensors? previouslyEnabledSensors;

		// Token: 0x04000156 RID: 342
		private GpuAssignmentMode skuGpuAssignmentMode = GpuAssignmentMode.Default;
	}
}
