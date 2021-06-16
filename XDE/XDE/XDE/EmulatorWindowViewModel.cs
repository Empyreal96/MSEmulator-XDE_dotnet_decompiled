using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xde.Base;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000017 RID: 23
	public class EmulatorWindowViewModel : ViewModelBase, IXdeToolbar
	{
		// Token: 0x0600010B RID: 267 RVA: 0x000060C8 File Offset: 0x000042C8
		public EmulatorWindowViewModel(EmulatorWindow window)
		{
			this.window = window;
			this.inputRepeatTimer = new System.Threading.Timer(new TimerCallback(this.InputRepeatTimerCallback));
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000613E File Offset: 0x0000433E
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00006165 File Offset: 0x00004365
		public string Message
		{
			get
			{
				if (this.chromeModels.Count <= 0)
				{
					return string.Empty;
				}
				return this.chromeModels[0].Message;
			}
			set
			{
				if (this.chromeModels.Count > 0)
				{
					this.chromeModels[0].Message = value;
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006187 File Offset: 0x00004387
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000618F File Offset: 0x0000438F
		public bool ShuttingDown
		{
			get
			{
				return this.shuttingDown;
			}
			set
			{
				if (this.shuttingDown != value)
				{
					this.shuttingDown = value;
					this.OnPropertyChanged("ShuttingDown");
					this.RefreshMessageVis();
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000061B2 File Offset: 0x000043B2
		public EmulatorWindow Window
		{
			get
			{
				return this.window;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000061BA File Offset: 0x000043BA
		public ReadOnlyCollection<EmulatorWindowViewModel.ChromeViewModel> ChromeModels
		{
			get
			{
				return this.chromeModels.AsReadOnly();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000061C7 File Offset: 0x000043C7
		public IEnumerable<EmulatorWindowViewModel.ChromeViewModel> VisibleChromeModels
		{
			get
			{
				if (this.skin != null)
				{
					int i = 0;
					while (i < this.skin.ActiveChromeCount && i < this.chromeModels.Count)
					{
						yield return this.chromeModels[i];
						int num = i;
						i = num + 1;
					}
				}
				yield break;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000061D7 File Offset: 0x000043D7
		// (set) Token: 0x06000114 RID: 276 RVA: 0x000061DF File Offset: 0x000043DF
		public IXdeControllerState ControllerState
		{
			get
			{
				return this.controllerState;
			}
			set
			{
				this.controllerState = value;
				if (this.controllerState.Skin != null)
				{
					this.UpdateSkin();
				}
				this.controllerState.SkinChanged += this.ControllerState_SkinChanged;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006212 File Offset: 0x00004412
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000621A File Offset: 0x0000441A
		public IXdeGuestDisplay GuestDisplay
		{
			get
			{
				return this.guestDisplay;
			}
			set
			{
				this.guestDisplay = value;
				this.RefreshMessageVis();
				this.guestDisplay.PropertyChanged += this.GuestDisplay_PropertyChanged;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006240 File Offset: 0x00004440
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00006248 File Offset: 0x00004448
		public IXdeAutomationInput InjectedInput
		{
			get
			{
				return this.injectedInput;
			}
			set
			{
				this.injectedInput = value;
				this.injectedInput.PropertyChanged += this.InjectedInput_PropertyChanged;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006268 File Offset: 0x00004468
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00006270 File Offset: 0x00004470
		public InputSettings InputSettings
		{
			get
			{
				return this.inputSettings;
			}
			set
			{
				this.inputSettings = value;
				this.inputSettings.PropertyChanged += this.InputSettings_PropertyChanged;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00006290 File Offset: 0x00004490
		public Visibility MultiTouchVisibility
		{
			get
			{
				if (this.inputSettings == null || this.inputSettings.TouchMode != TouchMode.MultiTouch)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000062AB File Offset: 0x000044AB
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000062B3 File Offset: 0x000044B3
		public IXdeDisplayOutput Output
		{
			get
			{
				return this.output;
			}
			set
			{
				this.output = value;
				this.OnPropertyChanged(null);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000062C3 File Offset: 0x000044C3
		// (set) Token: 0x0600011F RID: 287 RVA: 0x000062CB File Offset: 0x000044CB
		public IXdeSku Sku
		{
			get
			{
				return this.sku;
			}
			set
			{
				this.sku = value;
				this.sku.PropertyChanged += this.Sku_PropertyChanged;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000062EC File Offset: 0x000044EC
		public double CurrentDpi
		{
			get
			{
				return VisualTreeHelper.GetDpi(this.window).DpiScaleX;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000630C File Offset: 0x0000450C
		public double DisplayScale
		{
			get
			{
				if (this.skin != null)
				{
					return this.skin.DisplayScale / this.CurrentDpi;
				}
				return 0.0;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006332 File Offset: 0x00004532
		public double DeviceImageWidth
		{
			get
			{
				return (double)((this.DeviceImage != null) ? this.DeviceImage.PixelWidth : 0);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000634B File Offset: 0x0000454B
		public double DeviceImageHeight
		{
			get
			{
				return (double)((this.DeviceImage != null) ? this.DeviceImage.PixelHeight : 0);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006364 File Offset: 0x00004564
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000636C File Offset: 0x0000456C
		public Visibility FlyoutVisibility
		{
			get
			{
				return this.flyoutVisibility;
			}
			set
			{
				this.flyoutVisibility = value;
				this.OnPropertyChanged("FlyoutVisibility");
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00006380 File Offset: 0x00004580
		public ObservableCollection<ToolBarItemViewModel> ToolbarItems
		{
			get
			{
				return this.toolbarItems;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00006388 File Offset: 0x00004588
		public ObservableCollection<ToolBarItemViewModel> FlyoutItems
		{
			get
			{
				return this.flyoutItems;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00006390 File Offset: 0x00004590
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00006398 File Offset: 0x00004598
		public string WindowText
		{
			get
			{
				return this.windowText;
			}
			set
			{
				if (this.windowText != value)
				{
					this.windowText = value;
					this.OnPropertyChanged("WindowText");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000063BA File Offset: 0x000045BA
		public Visibility WindowTextVis
		{
			get
			{
				if (!this.ShowWindowText)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000063C7 File Offset: 0x000045C7
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000063CF File Offset: 0x000045CF
		public bool ShowWindowText
		{
			get
			{
				return this.showWindowText;
			}
			set
			{
				if (this.showWindowText != value)
				{
					this.showWindowText = value;
					this.OnPropertyChanged("WindowTextVis");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000063EC File Offset: 0x000045EC
		public IXdeToolbar Toolbar
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600012E RID: 302 RVA: 0x000063EF File Offset: 0x000045EF
		// (set) Token: 0x0600012F RID: 303 RVA: 0x000063F7 File Offset: 0x000045F7
		public BitmapSource DeviceImage
		{
			get
			{
				return this.deviceImage;
			}
			set
			{
				this.deviceImage = value;
				this.OnPropertyChanged(null);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006407 File Offset: 0x00004607
		public ImageSource ExternDisplay
		{
			get
			{
				if (this.output == null)
				{
					return null;
				}
				return this.output.ExternalMonitor;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000641E File Offset: 0x0000461E
		public double ActualExternWidth
		{
			get
			{
				return (double)this.ExternWidth * this.DisplayScale;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000642E File Offset: 0x0000462E
		public double ActualExternHeight
		{
			get
			{
				return (double)this.ExternHeight * this.DisplayScale;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000643E File Offset: 0x0000463E
		public double RightSizeRectCanvasTop
		{
			get
			{
				if (this.skin == null)
				{
					return 0.0;
				}
				if (this.skin.Orientation != DisplayOrientation.Portrait)
				{
					return 0.0;
				}
				return (double)this.rightResizeRectTop * this.DisplayScale;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00006478 File Offset: 0x00004678
		public double RightSizeRectCanvasRight
		{
			get
			{
				if (this.skin == null)
				{
					return 0.0;
				}
				if (this.skin.Orientation != DisplayOrientation.Portrait)
				{
					return -8.0;
				}
				return (double)this.rightResizeRectOffsetFromRight * this.DisplayScale + -8.0;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000064C6 File Offset: 0x000046C6
		public double RightSizeRectHeight
		{
			get
			{
				if (this.skin == null)
				{
					return 0.0;
				}
				if (this.skin.Orientation != DisplayOrientation.Portrait)
				{
					return this.ActualChromeHeight;
				}
				return (double)this.rightResizeRectLen * this.DisplayScale;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000064FC File Offset: 0x000046FC
		public double ActualChromeWidth
		{
			get
			{
				if (this.DeviceImage != null)
				{
					double num = ((this.skin.Orientation == DisplayOrientation.Portrait) ? (this.WidthForMonitorMode * (double)this.skin.ActiveChromeCount) : ((double)this.DeviceImage.PixelHeight)) * this.DisplayScale;
					if (!this.firstRender)
					{
						if (this.lastActualWidth > 0.0 && !this.dontMoveLeft)
						{
							double num2 = this.lastActualWidth - num;
							this.window.Left += num2;
						}
						this.lastActualWidth = num;
					}
					return num;
				}
				return 0.0;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006596 File Offset: 0x00004796
		public double CaptionHeight
		{
			get
			{
				return 35.0;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000065A1 File Offset: 0x000047A1
		public Visibility CaptionVisbility
		{
			get
			{
				if (!this.window.ShowDisplayName)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000065B3 File Offset: 0x000047B3
		public double TotalActualHeight
		{
			get
			{
				return this.ActualChromeHeight + ((this.CaptionVisbility == Visibility.Visible) ? this.CaptionHeight : 0.0);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000065D8 File Offset: 0x000047D8
		public double ActualChromeHeight
		{
			get
			{
				if (this.DeviceImage != null)
				{
					return ((this.skin.Orientation == DisplayOrientation.Portrait) ? ((double)this.DeviceImage.PixelHeight) : (this.WidthForMonitorMode * (double)this.skin.ActiveChromeCount)) * this.DisplayScale;
				}
				return 0.0;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000662C File Offset: 0x0000482C
		public double TouchCanvasWidth
		{
			get
			{
				return this.DeviceImageWidth;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006634 File Offset: 0x00004834
		public double TouchCanvasHeight
		{
			get
			{
				return this.DeviceImageHeight;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000663C File Offset: 0x0000483C
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00006644 File Offset: 0x00004844
		public double MultiX
		{
			get
			{
				return this.multiX;
			}
			set
			{
				this.multiX = value;
				this.OnPropertyChanged("MultiX");
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006658 File Offset: 0x00004858
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00006660 File Offset: 0x00004860
		public double MultiY
		{
			get
			{
				return this.multiY;
			}
			set
			{
				this.multiY = value;
				this.OnPropertyChanged("MultiY");
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00006674 File Offset: 0x00004874
		// (set) Token: 0x06000142 RID: 322 RVA: 0x0000667C File Offset: 0x0000487C
		public double MultiPoint1X
		{
			get
			{
				return this.multiPoint1X;
			}
			set
			{
				this.multiPoint1X = value;
				this.OnPropertyChanged("MultiPoint1X");
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00006690 File Offset: 0x00004890
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00006698 File Offset: 0x00004898
		public double MultiPoint1Y
		{
			get
			{
				return this.multiPoint1Y;
			}
			set
			{
				this.multiPoint1Y = value;
				this.OnPropertyChanged("MultiPoint1Y");
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000066AC File Offset: 0x000048AC
		public double RotateAngle
		{
			get
			{
				if (this.skin == null)
				{
					return 0.0;
				}
				double result;
				if (this.skin.Orientation == DisplayOrientation.Portrait)
				{
					result = 0.0;
				}
				else if (this.skin.Orientation == DisplayOrientation.LandscapeLeft)
				{
					result = -90.0;
				}
				else
				{
					result = 90.0;
				}
				return result;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00006709 File Offset: 0x00004909
		public double TranslateX
		{
			get
			{
				if (this.skin != null && this.skin.Orientation == DisplayOrientation.LandscapeRight)
				{
					return this.DeviceImageHeight;
				}
				return 0.0;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006731 File Offset: 0x00004931
		public double TranslateY
		{
			get
			{
				if (this.skin != null && this.skin.Orientation == DisplayOrientation.LandscapeLeft)
				{
					return this.DeviceImageWidth;
				}
				return 0.0;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00006759 File Offset: 0x00004959
		public double SingleDisplayTranslateX
		{
			get
			{
				if (this.skin != null && this.skin.Orientation == DisplayOrientation.Portrait && this.skin.MonitorMode == MonitorMode.RightOnly)
				{
					return -this.SingleDisplayHalfWidth;
				}
				return 0.0;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006790 File Offset: 0x00004990
		public double SingleDisplayTranslateY
		{
			get
			{
				if (this.skin != null)
				{
					if (this.skin.Orientation == DisplayOrientation.LandscapeRight)
					{
						if (this.skin.MonitorMode == MonitorMode.RightOnly)
						{
							return -this.SingleDisplayHalfWidth;
						}
					}
					else if (this.skin.Orientation == DisplayOrientation.LandscapeLeft && this.skin.MonitorMode == MonitorMode.LeftOnly)
					{
						return -this.SingleDisplayHalfWidth;
					}
				}
				return 0.0;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000067F6 File Offset: 0x000049F6
		Rectangle IXdeToolbar.DesktopBounds
		{
			get
			{
				return this.window.ToolbarDesktopBounds;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006804 File Offset: 0x00004A04
		bool IXdeToolbar.IsToolsButtonEnabled
		{
			get
			{
				ToolBarButtonViewModel toolBarButtonViewModel = (from i in this.toolbarItems
				where i.Name == "StockPlugin.ShowTools"
				select i).FirstOrDefault<ToolBarItemViewModel>() as ToolBarButtonViewModel;
				return toolBarButtonViewModel != null && toolBarButtonViewModel.Enabled;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006854 File Offset: 0x00004A54
		XdeToolbarButtonAutomation[] IXdeToolbar.Buttons
		{
			get
			{
				List<XdeToolbarButtonAutomation> list = new List<XdeToolbarButtonAutomation>();
				foreach (ToolBarItemViewModel toolBarItemViewModel in this.toolbarItems)
				{
					ToolBarButtonViewModel toolBarButtonViewModel = toolBarItemViewModel as ToolBarButtonViewModel;
					if (toolBarButtonViewModel != null)
					{
						list.Add(new XdeToolbarButtonAutomation
						{
							Name = toolBarButtonViewModel.Name,
							Arrowed = toolBarButtonViewModel.Arrowed,
							Enabled = toolBarButtonViewModel.Enabled,
							Toggled = toolBarButtonViewModel.Toggled
						});
					}
				}
				return list.ToArray();
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000068EC File Offset: 0x00004AEC
		// (set) Token: 0x0600014E RID: 334 RVA: 0x000068F4 File Offset: 0x00004AF4
		public Thickness FlyoutMargin
		{
			get
			{
				return this.flyoutMargin;
			}
			set
			{
				this.flyoutMargin = value;
				this.OnPropertyChanged("FlyoutMargin");
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006908 File Offset: 0x00004B08
		public double FlyoutColumns
		{
			get
			{
				return (double)((this.flyoutItems.Count >= 3) ? 3 : this.flyoutItems.Count);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00006927 File Offset: 0x00004B27
		public ObservableCollection<SkinButtonItemViewModel> SkinButtonItems
		{
			get
			{
				return this.skinButtonItems;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000692F File Offset: 0x00004B2F
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00006937 File Offset: 0x00004B37
		public Geometry ImageMask
		{
			get
			{
				return this.imageMask;
			}
			set
			{
				this.imageMask = value;
				this.OnPropertyChanged("ImageMask");
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000694B File Offset: 0x00004B4B
		public Visibility ExternalDisplayVisibility
		{
			get
			{
				if (this.skin == null || !this.skin.Display.SupportsExternalDisplay || !this.skin.ShowExternalDisplay)
				{
					return Visibility.Collapsed;
				}
				return Visibility.Visible;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00006977 File Offset: 0x00004B77
		public int ExternWidth
		{
			get
			{
				if (this.skin == null)
				{
					return 0;
				}
				return this.skin.Display.ExternalDisplayWidth;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006993 File Offset: 0x00004B93
		public int ExternHeight
		{
			get
			{
				if (this.skin == null)
				{
					return 0;
				}
				return this.skin.Display.ExternalDisplayHeight;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000069B0 File Offset: 0x00004BB0
		private double SingleDisplayHalfWidth
		{
			get
			{
				return (double)((this.skin != null) ? (this.skin.Display.DisplayPosX + this.skin.Display.DisplayWidth + this.skin.Display.DisplayGapWidth / 2) : 0);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000069FD File Offset: 0x00004BFD
		public TouchMode EffectiveTouchMode
		{
			get
			{
				if (this.InputSettings.TouchMode == TouchMode.Disabled)
				{
					return TouchMode.Disabled;
				}
				if (this.InjectedInput == null || !this.InjectedInput.IsConnected)
				{
					return TouchMode.Mouse;
				}
				return this.inputSettings.TouchMode;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006A30 File Offset: 0x00004C30
		private double WidthForMonitorMode
		{
			get
			{
				double result;
				if (this.skin.MonitorMode == MonitorMode.Both)
				{
					result = (double)this.DeviceImage.PixelWidth;
				}
				else if (this.skin.MonitorMode == MonitorMode.LeftOnly)
				{
					result = this.SingleDisplayHalfWidth;
				}
				else
				{
					result = (double)this.DeviceImage.PixelWidth - this.SingleDisplayHalfWidth;
				}
				return result;
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006A88 File Offset: 0x00004C88
		public static NativeMethods.POINTER_TYPE_INFO GetTouchPoint(double x, double y, uint id, TouchEventType contactType)
		{
			NativeMethods.POINTER_TYPE_INFO pointer_TYPE_INFO = default(NativeMethods.POINTER_TYPE_INFO);
			NativeMethods.POINTER_FLAGS pointerFlags = EmulatorWindowViewModel.ConvertContactType(contactType);
			pointer_TYPE_INFO.TouchPen.PenInfo.pointerInfo.pointerId = id;
			pointer_TYPE_INFO.type = NativeMethods.POINTER_INPUT_TYPE.Touch;
			pointer_TYPE_INFO.TouchPen.TouchInfo.pointerInfo.pointerType = pointer_TYPE_INFO.type;
			pointer_TYPE_INFO.TouchPen.TouchInfo.pointerInfo.pointerFlags = pointerFlags;
			pointer_TYPE_INFO.TouchPen.TouchInfo.pointerInfo.ptPixelLocation.X = (int)x;
			pointer_TYPE_INFO.TouchPen.TouchInfo.pointerInfo.ptPixelLocation.Y = (int)y;
			return pointer_TYPE_INFO;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006B31 File Offset: 0x00004D31
		public void OnDpiScaleChanged(double oldDpi, double newDpi)
		{
			this.dontMoveLeft = true;
			this.skin.DisplayScale = this.skin.DisplayScale * newDpi / oldDpi;
			this.dontMoveLeft = false;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006B5C File Offset: 0x00004D5C
		void IXdeToolbar.ClickButton(string buttonName)
		{
			((from i in this.toolbarItems
			where i.Name == buttonName
			select i).FirstOrDefault<ToolBarItemViewModel>() as ToolBarButtonViewModel).Execute(null);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006B9D File Offset: 0x00004D9D
		void IXdeToolbar.LoadFromSku(IXdeSku sku)
		{
			this.LoadItems(this.toolbarItems, sku.ToolbarItems, new Executed(this.OnMainToolbarButtonClicked));
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006BC0 File Offset: 0x00004DC0
		void IXdeToolbar.ShowToolbarFlyout(string buttonNameOrigin, IEnumerable<IXdeToolbarItem> items, ToolbarFlags toolbarFlags)
		{
			if (toolbarFlags.HasFlag(ToolbarFlags.HideIfAlreadyShowing) && (this.FlyoutVisibility == Visibility.Visible || this.hidFlyout))
			{
				this.FlyoutVisibility = Visibility.Collapsed;
				return;
			}
			this.hideFlyoutOnNextPress = toolbarFlags.HasFlag(ToolbarFlags.HideOnNextButtonPressed);
			this.LoadItems(this.flyoutItems, items, null);
			this.OnPropertyChanged("FlyoutColumns");
			this.FlyoutMargin = new Thickness(0.0, this.window.GetFlyoutLocationForButton(buttonNameOrigin).Y, 0.0, 0.0);
			this.FlyoutVisibility = Visibility.Visible;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006C69 File Offset: 0x00004E69
		public void OnRender()
		{
			if (this.firstRender)
			{
				this.OnDpiScaleChanged(1.0, this.CurrentDpi);
			}
			this.firstRender = false;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006C8F File Offset: 0x00004E8F
		private void InjectedInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected")
			{
				this.OnPropertyChanged("EffectiveTouchMode");
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006CB0 File Offset: 0x00004EB0
		private async Task ConsumeMouseDataAync()
		{
			try
			{
				for (;;)
				{
					TaskAwaiter<bool> taskAwaiter = this.mouseDataBuffer.OutputAvailableAsync<EmulatorWindowViewModel.MouseData>().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						break;
					}
					EmulatorWindowViewModel.MouseData mouseData = this.mouseDataBuffer.Receive<EmulatorWindowViewModel.MouseData>();
					System.Windows.Point loc = this.ScalePointForMonitors(mouseData.loc);
					TouchMode touchMode = mouseData.touchMode;
					if (touchMode == TouchMode.Mouse)
					{
						this.SendMouseEvent(mouseData.mouseButtons, loc);
					}
					switch (mouseData.mouseOp)
					{
					case EmulatorWindowViewModel.MouseOp.LeftDown:
						this.InputSettings.OnMouseDown(MouseButtons.Left, (int)loc.X, (int)loc.Y);
						break;
					case EmulatorWindowViewModel.MouseOp.LeftUp:
						this.InputSettings.OnMouseUp(MouseButtons.Left, (int)loc.X, (int)loc.Y);
						break;
					case EmulatorWindowViewModel.MouseOp.RightDown:
						this.InputSettings.OnMouseDown(MouseButtons.Right, (int)loc.X, (int)loc.Y);
						break;
					case EmulatorWindowViewModel.MouseOp.RightUp:
						this.InputSettings.OnMouseUp(MouseButtons.Right, (int)loc.X, (int)loc.Y);
						break;
					case EmulatorWindowViewModel.MouseOp.Move:
						this.InputSettings.OnMouseMove(mouseData.mouseButtons, (int)loc.X, (int)loc.Y);
						break;
					}
					bool flag = touchMode == TouchMode.MultiTouch || touchMode == TouchMode.SingleTouch;
					bool flag2 = touchMode == TouchMode.Pen;
					bool flag3 = false;
					if (flag2 || flag)
					{
						TouchEventType contactType;
						switch (mouseData.mouseOp)
						{
						case EmulatorWindowViewModel.MouseOp.LeftDown:
							goto IL_16F;
						case EmulatorWindowViewModel.MouseOp.LeftUp:
							contactType = TouchEventType.Up;
							break;
						case EmulatorWindowViewModel.MouseOp.RightDown:
						case EmulatorWindowViewModel.MouseOp.RightUp:
							contactType = TouchEventType.Down;
							flag3 = true;
							break;
						case EmulatorWindowViewModel.MouseOp.Move:
							contactType = (((mouseData.mouseButtons & MouseButtons.Left) == MouseButtons.Left) ? TouchEventType.Move : TouchEventType.Hover);
							break;
						default:
							goto IL_16F;
						}
						IL_194:
						if (!flag3)
						{
							NativeMethods.POINTER_TYPE_INFO[] inputPoints = new NativeMethods.POINTER_TYPE_INFO[]
							{
								flag ? EmulatorWindowViewModel.GetTouchPoint(loc.X, loc.Y, 1U, contactType) : this.GetPenPoint(loc.X, loc.Y, 1U, contactType)
							};
							this.SendTouchInfo(inputPoints);
							continue;
						}
						continue;
						IL_16F:
						contactType = TouchEventType.Down;
						goto IL_194;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006CF5 File Offset: 0x00004EF5
		public void OnDisplayMouseLeftButtonDown(MouseButtons mouseButtons, System.Windows.Point loc)
		{
			this.PostMouseOp(mouseButtons, loc, EmulatorWindowViewModel.MouseOp.LeftDown);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006D00 File Offset: 0x00004F00
		public void OnDisplayMouseLeftButtonUp(MouseButtons mouseButtons, System.Windows.Point loc)
		{
			this.PostMouseOp(mouseButtons, loc, EmulatorWindowViewModel.MouseOp.LeftUp);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006D0B File Offset: 0x00004F0B
		public void OnDisplayMouseRightButtonDown(MouseButtons mouseButtons, System.Windows.Point loc)
		{
			this.PostMouseOp(mouseButtons, loc, EmulatorWindowViewModel.MouseOp.RightDown);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006D16 File Offset: 0x00004F16
		public void OnDisplayMouseRightButtonUp(MouseButtons mouseButtons, System.Windows.Point loc)
		{
			this.PostMouseOp(mouseButtons, loc, EmulatorWindowViewModel.MouseOp.RightUp);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006D21 File Offset: 0x00004F21
		public void OnDisplayMouseMove(MouseButtons mouseButtons, System.Windows.Point loc)
		{
			this.PostMouseOp(mouseButtons, loc, EmulatorWindowViewModel.MouseOp.Move);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006D2C File Offset: 0x00004F2C
		public void ProcessTouchPoints(System.Windows.Point[] points, TouchEventType type, uint[] ids)
		{
			NativeMethods.POINTER_TYPE_INFO[] array = new NativeMethods.POINTER_TYPE_INFO[points.Length];
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = this.ScalePointForMonitors(points[i]);
			}
			for (int j = 0; j < points.Length; j++)
			{
				NativeMethods.POINTER_TYPE_INFO touchPoint = EmulatorWindowViewModel.GetTouchPoint(points[j].X, points[j].Y, ids[j], type);
				array[j] = touchPoint;
			}
			this.SendTouchInfo(array);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006DA4 File Offset: 0x00004FA4
		private void PostMouseOp(MouseButtons mouseButtons, System.Windows.Point loc, EmulatorWindowViewModel.MouseOp mouseOp)
		{
			EmulatorWindowViewModel.MouseData item = new EmulatorWindowViewModel.MouseData
			{
				loc = loc,
				mouseButtons = mouseButtons,
				mouseOp = mouseOp,
				touchMode = this.EffectiveTouchMode
			};
			this.mouseDataBuffer.SendAsync(item);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006DF0 File Offset: 0x00004FF0
		private void TurnOffRepeatTimer()
		{
			object obj = this.repeatInputPointsSync;
			lock (obj)
			{
				this.repeatInputPoints = null;
				this.inputRepeatTimer.Change(-1, -1);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006E40 File Offset: 0x00005040
		public void OnDisplayMouseLeave()
		{
			this.TurnOffRepeatTimer();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006E48 File Offset: 0x00005048
		public void OnDisplayMouseWheel(int delta)
		{
			if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && this.inputSettings.TouchMode == TouchMode.Pen)
			{
				int num = 100 * ((delta > 0) ? -1 : 1);
				int num2 = this.inputSettings.TouchPressure + num;
				num2 = MathUtils.Clamp<int>(num2, this.inputSettings.MinPressure, this.inputSettings.MaxPressure);
				this.inputSettings.TouchPressure = num2;
				return;
			}
			IXdeGuestDisplay xdeGuestDisplay = this.guestDisplay;
			if (xdeGuestDisplay == null)
			{
				return;
			}
			xdeGuestDisplay.GuestInput.SendMouseWheelEvent((ushort)delta);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006ED0 File Offset: 0x000050D0
		public void AttemptResize(ResizeCorner currentCorner, Vector diff)
		{
			if (currentCorner == ResizeCorner.TopLeft || currentCorner == ResizeCorner.Top || currentCorner == ResizeCorner.Left)
			{
				diff = -diff;
			}
			double num;
			if (currentCorner != ResizeCorner.BottomLeft && currentCorner != ResizeCorner.TopLeft && currentCorner != ResizeCorner.Top && currentCorner != ResizeCorner.Bottom)
			{
				double x = diff.X;
				if (x == 0.0)
				{
					return;
				}
				num = this.skin.DisplayScale * ((x + this.ActualChromeWidth) / this.ActualChromeWidth);
			}
			else
			{
				double y = diff.Y;
				if (y == 0.0)
				{
					return;
				}
				num = this.skin.DisplayScale * ((y + this.ActualChromeHeight) / this.ActualChromeHeight);
			}
			if (num < 0.10000000149011612 || num > 1.0)
			{
				return;
			}
			this.dontMoveLeft = true;
			this.skin.DisplayScale = num;
			this.dontMoveLeft = false;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006F98 File Offset: 0x00005198
		private static NativeMethods.POINTER_FLAGS ConvertContactType(TouchEventType contactType)
		{
			NativeMethods.POINTER_FLAGS result;
			switch (contactType)
			{
			case TouchEventType.Down:
				result = (NativeMethods.POINTER_FLAGS.InRange | NativeMethods.POINTER_FLAGS.InContact | NativeMethods.POINTER_FLAGS.Down);
				break;
			case TouchEventType.Hover:
				result = (NativeMethods.POINTER_FLAGS.InRange | NativeMethods.POINTER_FLAGS.Update);
				break;
			case TouchEventType.Move:
				result = (NativeMethods.POINTER_FLAGS.InRange | NativeMethods.POINTER_FLAGS.InContact | NativeMethods.POINTER_FLAGS.Update);
				break;
			case TouchEventType.Up:
				result = (NativeMethods.POINTER_FLAGS.Update | NativeMethods.POINTER_FLAGS.Up);
				break;
			default:
				result = NativeMethods.POINTER_FLAGS.None;
				break;
			}
			return result;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006FE0 File Offset: 0x000051E0
		private void SendMouseEvent(MouseButtons mouseButtons, System.Windows.Point loc)
		{
			this.window.Dispatcher.BeginInvoke(new Action(delegate()
			{
				IXdeGuestDisplay xdeGuestDisplay = this.guestDisplay;
				if (xdeGuestDisplay == null)
				{
					return;
				}
				xdeGuestDisplay.GuestInput.SendMouseEvent(mouseButtons, (uint)loc.X, (uint)loc.Y);
			}), Array.Empty<object>());
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000702C File Offset: 0x0000522C
		private NativeMethods.POINTER_TYPE_INFO GetPenPoint(double x, double y, uint id, TouchEventType contactType)
		{
			uint touchPressure = (uint)this.inputSettings.TouchPressure;
			NativeMethods.POINTER_TYPE_INFO pointer_TYPE_INFO = default(NativeMethods.POINTER_TYPE_INFO);
			NativeMethods.POINTER_FLAGS pointerFlags = EmulatorWindowViewModel.ConvertContactType(contactType);
			pointer_TYPE_INFO.TouchPen.PenInfo.pointerInfo.pointerId = id;
			pointer_TYPE_INFO.type = NativeMethods.POINTER_INPUT_TYPE.Pen;
			if (contactType == TouchEventType.Down || contactType == TouchEventType.Move)
			{
				pointer_TYPE_INFO.TouchPen.PenInfo.penMask = NativeMethods.PEN_MASK.Pressure;
				pointer_TYPE_INFO.TouchPen.PenInfo.pressure = touchPressure;
			}
			pointer_TYPE_INFO.TouchPen.PenInfo.pointerInfo.pointerType = pointer_TYPE_INFO.type;
			pointer_TYPE_INFO.TouchPen.PenInfo.pointerInfo.pointerFlags = pointerFlags;
			pointer_TYPE_INFO.TouchPen.PenInfo.pointerInfo.ptPixelLocation.X = (int)x;
			pointer_TYPE_INFO.TouchPen.PenInfo.pointerInfo.ptPixelLocation.Y = (int)y;
			return pointer_TYPE_INFO;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007110 File Offset: 0x00005310
		private void InputRepeatTimerCallback(object _)
		{
			NativeMethods.POINTER_TYPE_INFO[] array = null;
			object obj = this.repeatInputPointsSync;
			lock (obj)
			{
				array = this.repeatInputPoints;
			}
			if (array != null)
			{
				this.InjectedInput.SendTouchInfo(array);
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007164 File Offset: 0x00005364
		private void SendTouchInfo(NativeMethods.POINTER_TYPE_INFO[] inputPoints)
		{
			object obj = this.repeatInputPointsSync;
			int period;
			lock (obj)
			{
				this.repeatInputPoints = inputPoints;
				NativeMethods.POINTER_FLAGS pointerFlags = inputPoints[0].TouchPen.TouchInfo.pointerInfo.pointerFlags;
				if ((pointerFlags & NativeMethods.POINTER_FLAGS.InRange) == NativeMethods.POINTER_FLAGS.InRange)
				{
					bool flag2 = (pointerFlags & NativeMethods.POINTER_FLAGS.Down) == NativeMethods.POINTER_FLAGS.Down;
					bool flag3 = (pointerFlags & NativeMethods.POINTER_FLAGS.Up) == NativeMethods.POINTER_FLAGS.Up;
					if (flag2 || flag3)
					{
						this.InjectedInput.SendTouchInfo(inputPoints);
						NativeMethods.POINTER_FLAGS pointerFlags2 = EmulatorWindowViewModel.ConvertContactType(flag2 ? TouchEventType.Move : TouchEventType.Hover);
						for (int i = 0; i < inputPoints.Length; i++)
						{
							inputPoints[i].TouchPen.TouchInfo.pointerInfo.pointerFlags = pointerFlags2;
						}
					}
					period = 250;
				}
				else
				{
					period = -1;
				}
			}
			this.inputRepeatTimer.Change(0, period);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007254 File Offset: 0x00005454
		private System.Windows.Point ScalePointForMonitors(System.Windows.Point point)
		{
			if (this.skin.Display.DisplaysStackedVertically)
			{
				return new System.Windows.Point(point.X * 2.0, point.Y / 2.0);
			}
			return point;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007291 File Offset: 0x00005491
		private void ControllerState_SkinChanged(object sender, EventArgs e)
		{
			this.UpdateSkin();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000729C File Offset: 0x0000549C
		private void LoadChromeViewModels()
		{
			this.chromeModels.Clear();
			double num = 0.0;
			for (int i = 0; i < this.skin.Display.ChromeCount; i++)
			{
				bool hasSecondDisplay = i == 0 && this.skin.Display.DisplaysPerChrome == 2;
				EmulatorWindowViewModel.ChromeViewModel chromeViewModel = new EmulatorWindowViewModel.ChromeViewModel(this, i, hasSecondDisplay);
				chromeViewModel.PosX = num;
				num += this.DeviceImageWidth;
				this.chromeModels.Add(chromeViewModel);
			}
			this.OnActiveChromeCountChanged();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007320 File Offset: 0x00005520
		private void UpdateSkin()
		{
			this.skin = this.controllerState.Skin;
			this.skin.PropertyChanged += this.Skin_PropertyChanged;
			this.skinButtonItems.Clear();
			BitmapSource skinBitmap = this.skin.GetSkinBitmap(SkinBitmapIndex.Up);
			BitmapSource skinBitmap2 = this.skin.GetSkinBitmap(SkinBitmapIndex.Down);
			Geometry geometry = new RectangleGeometry(new Rect(0.0, 0.0, (double)skinBitmap.PixelWidth, (double)skinBitmap.PixelHeight));
			foreach (ISkinButtonInfo skinButtonInfo in this.skin.Buttons)
			{
				int width = skinButtonInfo.Bounds.Width;
				int height = skinButtonInfo.Bounds.Height;
				int num = width * 4;
				byte[] pixels = new byte[num * skinButtonInfo.Bounds.Height];
				byte[] pixels2 = new byte[num * skinButtonInfo.Bounds.Height];
				Int32Rect sourceRect = new Int32Rect(skinButtonInfo.Bounds.Left, skinButtonInfo.Bounds.Top, width, height);
				skinBitmap.CopyPixels(sourceRect, pixels, num, 0);
				skinBitmap2.CopyPixels(sourceRect, pixels2, num, 0);
				Rect rect = new Rect((double)sourceRect.X, (double)sourceRect.Y, (double)sourceRect.Width, (double)sourceRect.Height);
				rect.Inflate(-2.0, -2.0);
				RectangleGeometry geometry2 = new RectangleGeometry(rect);
				geometry = Geometry.Combine(geometry, geometry2, GeometryCombineMode.Exclude, null);
				WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Bgra32, null);
				writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels2, num, 0);
				writeableBitmap.Freeze();
				WriteableBitmap writeableBitmap2 = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Bgra32, null);
				writeableBitmap2.WritePixels(new Int32Rect(0, 0, width, height), pixels, num, 0);
				writeableBitmap2.Freeze();
				SkinButtonItemViewModel item = new SkinButtonItemViewModel(this, skinButtonInfo, writeableBitmap2, writeableBitmap);
				this.skinButtonItems.Add(item);
			}
			this.rightResizeRectOffsetFromRight = 0;
			this.rightResizeRectTop = 0;
			this.rightResizeRectLen = skinBitmap.PixelHeight;
			int num2 = 100;
			int pixelHeight = skinBitmap.PixelHeight;
			int num3 = num2 * 4;
			byte[] array = new byte[num3 * pixelHeight];
			Int32Rect sourceRect2 = new Int32Rect(skinBitmap.PixelWidth - num2 - 1, 0, num2, pixelHeight);
			skinBitmap.CopyPixels(sourceRect2, array, num3, 0);
			int i = skinBitmap.PixelHeight - 100;
			int j;
			for (j = num2 - 1; j >= 0; j--)
			{
				if (array[i * num3 + j * 4 + 3] != 0)
				{
					this.rightResizeRectOffsetFromRight = num2 - 1 - j;
					break;
				}
			}
			j++;
			if (j < num2)
			{
				while (i > 0)
				{
					if (array[i * num3 + j * 4 + 3] != 0)
					{
						this.rightResizeRectTop = i + 1;
						this.rightResizeRectLen = skinBitmap.PixelHeight - i;
						break;
					}
					i--;
				}
			}
			this.ImageMask = geometry;
			this.DeviceImage = this.skin.GetSkinBitmap(SkinBitmapIndex.Up);
			this.OnPropertyChanged("DisplayScale");
			this.LoadChromeViewModels();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007694 File Offset: 0x00005894
		private void Skin_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Orientation")
			{
				this.OnOrientationChanged();
				return;
			}
			if (e.PropertyName == "DisplayScale")
			{
				this.OnDisplayScaleChanged();
				return;
			}
			if (e.PropertyName == "MonitorMode")
			{
				this.UpdateSingleDisplayTranslate();
				return;
			}
			if (e.PropertyName == "ActiveChromeCount")
			{
				this.OnActiveChromeCountChanged();
				return;
			}
			if (e.PropertyName == "ShowExternalDisplay")
			{
				this.OnShowExternalDisplayChanged();
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000771D File Offset: 0x0000591D
		private void OnActiveChromeCountChanged()
		{
			this.OnPropertyChanged("VisibleChromeModels");
			this.OnPropertyChanged("ActualChromeWidth");
			this.OnPropertyChanged("ActualChromeHeight");
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007740 File Offset: 0x00005940
		private void UpdateSingleDisplayTranslate()
		{
			this.OnPropertyChanged("SingleDisplayTranslateX");
			this.OnPropertyChanged("SingleDisplayTranslateY");
			this.OnPropertyChanged("ActualChromeWidth");
			this.OnPropertyChanged("ActualChromeHeight");
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007770 File Offset: 0x00005970
		private void OnDisplayScaleChanged()
		{
			this.OnPropertyChanged("DisplayScale");
			this.OnPropertyChanged("ActualChromeWidth");
			this.OnPropertyChanged("ActualChromeHeight");
			this.OnPropertyChanged("ActualExternWidth");
			this.OnPropertyChanged("ActualExternHeight");
			this.OnPropertyChanged("RightSizeRectCanvasTop");
			this.OnPropertyChanged("RightSizeRectCanvasRight");
			this.OnPropertyChanged("RightSizeRectHeight");
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000077D5 File Offset: 0x000059D5
		private void Skin_DisplayScaleChanged(object sender, EventArgs e)
		{
			this.OnDisplayScaleChanged();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000077DD File Offset: 0x000059DD
		private void OnShowExternalDisplayChanged()
		{
			this.OnPropertyChanged("ExternWidth");
			this.OnPropertyChanged("ExternHeight");
			this.OnPropertyChanged("ActualExternWidth");
			this.OnPropertyChanged("ActualExternHeight");
			this.OnPropertyChanged("ExternalDisplayVisibility");
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007818 File Offset: 0x00005A18
		private void OnOrientationChanged()
		{
			this.OnPropertyChanged("RotateAngle");
			this.RefreshDisplayLandscapeRotations();
			this.UpdateSingleDisplayTranslate();
			this.OnPropertyChanged("ActualChromeWidth");
			this.OnPropertyChanged("ActualChromeHeight");
			this.OnPropertyChanged("TranslateX");
			this.OnPropertyChanged("TranslateY");
			this.OnPropertyChanged("RightSizeRectCanvasTop");
			this.OnPropertyChanged("RightSizeRectCanvasRight");
			this.OnPropertyChanged("RightSizeRectHeight");
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007889 File Offset: 0x00005A89
		private void OnMainToolbarButtonClicked(IXdeButton button)
		{
			if (this.hideFlyoutOnNextPress)
			{
				this.FlyoutVisibility = Visibility.Collapsed;
				this.hideFlyoutOnNextPress = false;
				this.hidFlyout = true;
				return;
			}
			this.hidFlyout = false;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000078B0 File Offset: 0x00005AB0
		private void GuestDisplay_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "RotateLandscapeUpperHalf")
			{
				this.OnPropertyChanged("RotateAngle");
				return;
			}
			if (e.PropertyName == "IsConnected")
			{
				if (this.guestDisplay.IsConnected)
				{
					this.mouseDataBuffer = new BufferBlock<EmulatorWindowViewModel.MouseData>();
					Task.Run(delegate()
					{
						this.ConsumeMouseDataAync().Wait();
					});
				}
				else if (this.mouseDataBuffer != null)
				{
					BufferBlock<EmulatorWindowViewModel.MouseData> bufferBlock = this.mouseDataBuffer;
					if (bufferBlock != null)
					{
						bufferBlock.Complete();
					}
					this.mouseDataBuffer = null;
				}
				this.RefreshMessageVis();
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007940 File Offset: 0x00005B40
		private void RefreshMessageVis()
		{
			foreach (EmulatorWindowViewModel.ChromeViewModel chromeViewModel in this.ChromeModels)
			{
				chromeViewModel.RefreshMessageVis();
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000798C File Offset: 0x00005B8C
		private void RefreshDisplayLandscapeRotations()
		{
			foreach (EmulatorWindowViewModel.ChromeViewModel chromeViewModel in this.ChromeModels)
			{
				chromeViewModel.RefreshDisplayLandscapeRotations();
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000079D8 File Offset: 0x00005BD8
		private void Sku_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Toolbar")
			{
				this.Toolbar.LoadFromSku(this.sku);
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000079FD File Offset: 0x00005BFD
		private void DisplayOutput_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(null);
			this.OnPropertyChanged("EffectiveTouchMode");
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007A14 File Offset: 0x00005C14
		private void InputSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "TouchMode")
			{
				if (this.EffectiveTouchMode == TouchMode.Mouse || this.EffectiveTouchMode == TouchMode.Disabled)
				{
					this.TurnOffRepeatTimer();
				}
				if (this.inputSettings.TouchMode == TouchMode.MultiTouch)
				{
					this.EnsureMultiTouchOnScreen();
				}
				this.OnPropertyChanged("MultiTouchVisibility");
				this.OnPropertyChanged("EffectiveTouchMode");
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007A74 File Offset: 0x00005C74
		private void EnsureMultiTouchOnScreen()
		{
			Rect firstDisplayOutputRect = this.window.GetFirstDisplayOutputRect();
			System.Windows.Point point = new System.Windows.Point(this.MultiX, this.MultiY);
			if (!firstDisplayOutputRect.Contains(point))
			{
				double num = firstDisplayOutputRect.Left + firstDisplayOutputRect.Width / 2.0;
				double num2 = firstDisplayOutputRect.Top + firstDisplayOutputRect.Height / 2.0;
				this.MultiX = num;
				this.MultiY = num2;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007AEC File Offset: 0x00005CEC
		private void LoadItems(IList<ToolBarItemViewModel> list, IEnumerable<IXdeToolbarItem> items, Executed execCallback)
		{
			list.Clear();
			foreach (IXdeToolbarItem xdeToolbarItem in items)
			{
				if (xdeToolbarItem.Name == "StockPlugin.Separator")
				{
					list.Add(new ToolBarItemViewModel(xdeToolbarItem));
				}
				else if (xdeToolbarItem is IXdeTrackbar)
				{
					IXdeTrackbar trackbar = (IXdeTrackbar)xdeToolbarItem;
					list.Add(new ToolBarSliderViewModel(trackbar));
				}
				else if (xdeToolbarItem is IXdeButton)
				{
					list.Add(new ToolBarButtonViewModel((IXdeButton)xdeToolbarItem, execCallback));
				}
			}
		}

		// Token: 0x04000077 RID: 119
		private const double DefaultFontSize = 20.0;

		// Token: 0x04000078 RID: 120
		private const double ResizeRectOffset = -8.0;

		// Token: 0x04000079 RID: 121
		private const string SeparatorName = "StockPlugin.Separator";

		// Token: 0x0400007A RID: 122
		private string windowText;

		// Token: 0x0400007B RID: 123
		private ObservableCollection<ToolBarItemViewModel> toolbarItems = new ObservableCollection<ToolBarItemViewModel>();

		// Token: 0x0400007C RID: 124
		private ObservableCollection<ToolBarItemViewModel> flyoutItems = new ObservableCollection<ToolBarItemViewModel>();

		// Token: 0x0400007D RID: 125
		private ObservableCollection<SkinButtonItemViewModel> skinButtonItems = new ObservableCollection<SkinButtonItemViewModel>();

		// Token: 0x0400007E RID: 126
		private IXdeControllerState controllerState;

		// Token: 0x0400007F RID: 127
		private IXdeDisplayOutput output;

		// Token: 0x04000080 RID: 128
		private IXdeSkin skin;

		// Token: 0x04000081 RID: 129
		private IXdeSku sku;

		// Token: 0x04000082 RID: 130
		private EmulatorWindow window;

		// Token: 0x04000083 RID: 131
		private Visibility flyoutVisibility = Visibility.Collapsed;

		// Token: 0x04000084 RID: 132
		private IXdeGuestDisplay guestDisplay;

		// Token: 0x04000085 RID: 133
		private BitmapSource deviceImage;

		// Token: 0x04000086 RID: 134
		private double multiX;

		// Token: 0x04000087 RID: 135
		private double multiY;

		// Token: 0x04000088 RID: 136
		private double multiPoint1X;

		// Token: 0x04000089 RID: 137
		private double multiPoint1Y;

		// Token: 0x0400008A RID: 138
		private bool hideFlyoutOnNextPress;

		// Token: 0x0400008B RID: 139
		private bool hidFlyout;

		// Token: 0x0400008C RID: 140
		private Thickness flyoutMargin;

		// Token: 0x0400008D RID: 141
		private double lastActualWidth;

		// Token: 0x0400008E RID: 142
		private InputSettings inputSettings;

		// Token: 0x0400008F RID: 143
		private List<EmulatorWindowViewModel.ChromeViewModel> chromeModels = new List<EmulatorWindowViewModel.ChromeViewModel>();

		// Token: 0x04000090 RID: 144
		private bool shuttingDown;

		// Token: 0x04000091 RID: 145
		private bool dontMoveLeft;

		// Token: 0x04000092 RID: 146
		private bool firstRender = true;

		// Token: 0x04000093 RID: 147
		private int rightResizeRectOffsetFromRight;

		// Token: 0x04000094 RID: 148
		private int rightResizeRectTop;

		// Token: 0x04000095 RID: 149
		private int rightResizeRectLen;

		// Token: 0x04000096 RID: 150
		private System.Threading.Timer inputRepeatTimer;

		// Token: 0x04000097 RID: 151
		private object repeatInputPointsSync = new object();

		// Token: 0x04000098 RID: 152
		private NativeMethods.POINTER_TYPE_INFO[] repeatInputPoints;

		// Token: 0x04000099 RID: 153
		private IXdeAutomationInput injectedInput;

		// Token: 0x0400009A RID: 154
		private bool showWindowText;

		// Token: 0x0400009B RID: 155
		private BufferBlock<EmulatorWindowViewModel.MouseData> mouseDataBuffer;

		// Token: 0x0400009C RID: 156
		private Geometry imageMask;

		// Token: 0x02000036 RID: 54
		private enum MouseOp
		{
			// Token: 0x04000185 RID: 389
			LeftDown,
			// Token: 0x04000186 RID: 390
			LeftUp,
			// Token: 0x04000187 RID: 391
			RightDown,
			// Token: 0x04000188 RID: 392
			RightUp,
			// Token: 0x04000189 RID: 393
			Move
		}

		// Token: 0x02000037 RID: 55
		private struct MouseData
		{
			// Token: 0x0400018A RID: 394
			public MouseButtons mouseButtons;

			// Token: 0x0400018B RID: 395
			public System.Windows.Point loc;

			// Token: 0x0400018C RID: 396
			public EmulatorWindowViewModel.MouseOp mouseOp;

			// Token: 0x0400018D RID: 397
			public TouchMode touchMode;
		}

		// Token: 0x02000038 RID: 56
		public class ChromeViewModel : ViewModelBase
		{
			// Token: 0x06000434 RID: 1076 RVA: 0x00010938 File Offset: 0x0000EB38
			public ChromeViewModel(EmulatorWindowViewModel parent, int displayIndex, bool hasSecondDisplay)
			{
				this.parent = parent;
				this.displayIndex = displayIndex;
				this.hasSecondDisplay = hasSecondDisplay;
				parent.PropertyChanged += this.Parent_PropertyChanged;
				this.message = ((this.displayIndex == 0) ? Resources.LoadingMessage : string.Empty);
			}

			// Token: 0x17000191 RID: 401
			// (get) Token: 0x06000435 RID: 1077 RVA: 0x00010993 File Offset: 0x0000EB93
			public EmulatorWindowViewModel Parent
			{
				get
				{
					return this.parent;
				}
			}

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001099B File Offset: 0x0000EB9B
			public bool HasSecondDisplay
			{
				get
				{
					return this.hasSecondDisplay;
				}
			}

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x06000437 RID: 1079 RVA: 0x000109A3 File Offset: 0x0000EBA3
			public int DisplayIndex
			{
				get
				{
					return this.displayIndex;
				}
			}

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x06000438 RID: 1080 RVA: 0x000109AC File Offset: 0x0000EBAC
			public System.Windows.Input.Cursor DisplayCursor
			{
				get
				{
					switch (this.parent.EffectiveTouchMode)
					{
					default:
						return System.Windows.Input.Cursors.Arrow;
					case TouchMode.MultiTouch:
					case TouchMode.SingleTouch:
						return System.Windows.Input.Cursors.Hand;
					case TouchMode.Pen:
						return System.Windows.Input.Cursors.Pen;
					}
				}
			}

			// Token: 0x17000195 RID: 405
			// (get) Token: 0x06000439 RID: 1081 RVA: 0x000109EE File Offset: 0x0000EBEE
			public Visibility DisplayVisiblity
			{
				get
				{
					if (this.StatusMessageVisiblity != Visibility.Visible)
					{
						return Visibility.Visible;
					}
					return Visibility.Collapsed;
				}
			}

			// Token: 0x17000196 RID: 406
			// (get) Token: 0x0600043A RID: 1082 RVA: 0x000109FB File Offset: 0x0000EBFB
			public Visibility Display2Visiblity
			{
				get
				{
					if (this.DisplayVisiblity != Visibility.Visible || !this.hasSecondDisplay)
					{
						return Visibility.Collapsed;
					}
					return Visibility.Visible;
				}
			}

			// Token: 0x17000197 RID: 407
			// (get) Token: 0x0600043B RID: 1083 RVA: 0x00010A10 File Offset: 0x0000EC10
			public Visibility StatusMessageVisiblity
			{
				get
				{
					if (this.parent.ShuttingDown)
					{
						return Visibility.Visible;
					}
					Visibility result = ((this.displayIndex == 0 && this.GuestDisplay == null) || !this.GuestDisplay.IsConnected) ? Visibility.Visible : Visibility.Collapsed;
					this.lastVis = result;
					return result;
				}
			}

			// Token: 0x17000198 RID: 408
			// (get) Token: 0x0600043C RID: 1084 RVA: 0x00010A56 File Offset: 0x0000EC56
			// (set) Token: 0x0600043D RID: 1085 RVA: 0x00010A5E File Offset: 0x0000EC5E
			public double PosX
			{
				get
				{
					return this.posX;
				}
				set
				{
					this.posX = value;
					this.OnPropertyChanged("PosX");
				}
			}

			// Token: 0x17000199 RID: 409
			// (get) Token: 0x0600043E RID: 1086 RVA: 0x00010A72 File Offset: 0x0000EC72
			// (set) Token: 0x0600043F RID: 1087 RVA: 0x00010A7A File Offset: 0x0000EC7A
			public double PosY
			{
				get
				{
					return this.posY;
				}
				set
				{
					this.posY = value;
					this.OnPropertyChanged("PosY");
				}
			}

			// Token: 0x1700019A RID: 410
			// (get) Token: 0x06000440 RID: 1088 RVA: 0x00010A8E File Offset: 0x0000EC8E
			public double DeviceImageWidth
			{
				get
				{
					return this.parent.DeviceImageWidth;
				}
			}

			// Token: 0x1700019B RID: 411
			// (get) Token: 0x06000441 RID: 1089 RVA: 0x00010A9B File Offset: 0x0000EC9B
			public double DeviceImageHeight
			{
				get
				{
					return this.parent.DeviceImageHeight;
				}
			}

			// Token: 0x1700019C RID: 412
			// (get) Token: 0x06000442 RID: 1090 RVA: 0x00010AA8 File Offset: 0x0000ECA8
			public BitmapSource DeviceImage
			{
				get
				{
					return this.parent.DeviceImage;
				}
			}

			// Token: 0x1700019D RID: 413
			// (get) Token: 0x06000443 RID: 1091 RVA: 0x00010AB5 File Offset: 0x0000ECB5
			public ObservableCollection<SkinButtonItemViewModel> SkinButtonItems
			{
				get
				{
					return this.parent.SkinButtonItems;
				}
			}

			// Token: 0x1700019E RID: 414
			// (get) Token: 0x06000444 RID: 1092 RVA: 0x00010AC2 File Offset: 0x0000ECC2
			public Geometry ImageMask
			{
				get
				{
					return this.parent.ImageMask;
				}
			}

			// Token: 0x1700019F RID: 415
			// (get) Token: 0x06000445 RID: 1093 RVA: 0x00010ACF File Offset: 0x0000ECCF
			public double Display1RotateAngle
			{
				get
				{
					if (this.GuestDisplay != null && this.GuestDisplay.RotateLandscapeUpperHalf && this.Skin.Orientation == DisplayOrientation.LandscapeRight)
					{
						return 180.0;
					}
					return 0.0;
				}
			}

			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x06000446 RID: 1094 RVA: 0x00010B07 File Offset: 0x0000ED07
			public double Display2RotateAngle
			{
				get
				{
					if (this.hasSecondDisplay && this.GuestDisplay != null && this.GuestDisplay.RotateLandscapeUpperHalf && this.Skin.Orientation == DisplayOrientation.LandscapeLeft)
					{
						return 180.0;
					}
					return 0.0;
				}
			}

			// Token: 0x170001A1 RID: 417
			// (get) Token: 0x06000447 RID: 1095 RVA: 0x00010B48 File Offset: 0x0000ED48
			public double DescaledFontSize
			{
				get
				{
					double displayScale = this.parent.DisplayScale;
					if (displayScale <= 0.0)
					{
						return 20.0;
					}
					return 20.0 / displayScale;
				}
			}

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x06000448 RID: 1096 RVA: 0x00010B82 File Offset: 0x0000ED82
			// (set) Token: 0x06000449 RID: 1097 RVA: 0x00010B8A File Offset: 0x0000ED8A
			public string Message
			{
				get
				{
					return this.message;
				}
				set
				{
					if (this.message != value)
					{
						this.message = value;
						this.OnPropertyChanged("Message");
					}
				}
			}

			// Token: 0x170001A3 RID: 419
			// (get) Token: 0x0600044A RID: 1098 RVA: 0x00010BAC File Offset: 0x0000EDAC
			public ImageSource Display1
			{
				get
				{
					return this.ImageSource1;
				}
			}

			// Token: 0x170001A4 RID: 420
			// (get) Token: 0x0600044B RID: 1099 RVA: 0x00010BB4 File Offset: 0x0000EDB4
			public double Display1Width
			{
				get
				{
					return (double)((this.Skin != null) ? this.Skin.Display.DisplayWidth : 0);
				}
			}

			// Token: 0x170001A5 RID: 421
			// (get) Token: 0x0600044C RID: 1100 RVA: 0x00010BD2 File Offset: 0x0000EDD2
			public double Display1CenterX
			{
				get
				{
					return this.Display1Width / 2.0;
				}
			}

			// Token: 0x170001A6 RID: 422
			// (get) Token: 0x0600044D RID: 1101 RVA: 0x00010BE4 File Offset: 0x0000EDE4
			public double Display1Height
			{
				get
				{
					return (double)((this.Skin != null) ? this.Skin.Display.DisplayHeight : 0);
				}
			}

			// Token: 0x170001A7 RID: 423
			// (get) Token: 0x0600044E RID: 1102 RVA: 0x00010C02 File Offset: 0x0000EE02
			public double Display1CenterY
			{
				get
				{
					return this.Display1Height / 2.0;
				}
			}

			// Token: 0x170001A8 RID: 424
			// (get) Token: 0x0600044F RID: 1103 RVA: 0x00010C14 File Offset: 0x0000EE14
			public double Display1X
			{
				get
				{
					return (double)((this.Skin != null) ? this.Skin.Display.DisplayPosX : 0);
				}
			}

			// Token: 0x170001A9 RID: 425
			// (get) Token: 0x06000450 RID: 1104 RVA: 0x00010C32 File Offset: 0x0000EE32
			public double Display1Y
			{
				get
				{
					return (double)((this.Skin != null) ? this.Skin.Display.DisplayPosY : 0);
				}
			}

			// Token: 0x170001AA RID: 426
			// (get) Token: 0x06000451 RID: 1105 RVA: 0x00010C50 File Offset: 0x0000EE50
			public Thickness Display1Margin
			{
				get
				{
					return new Thickness(this.Display1X, this.Display1Y, 0.0, 0.0);
				}
			}

			// Token: 0x170001AB RID: 427
			// (get) Token: 0x06000452 RID: 1106 RVA: 0x00010C75 File Offset: 0x0000EE75
			public ImageSource Display2
			{
				get
				{
					return this.ImageSource2;
				}
			}

			// Token: 0x170001AC RID: 428
			// (get) Token: 0x06000453 RID: 1107 RVA: 0x00010C7D File Offset: 0x0000EE7D
			public double Display2Width
			{
				get
				{
					return this.Display1Width;
				}
			}

			// Token: 0x170001AD RID: 429
			// (get) Token: 0x06000454 RID: 1108 RVA: 0x00010C85 File Offset: 0x0000EE85
			public double Display2CenterX
			{
				get
				{
					return this.Display2Width / 2.0;
				}
			}

			// Token: 0x170001AE RID: 430
			// (get) Token: 0x06000455 RID: 1109 RVA: 0x00010C97 File Offset: 0x0000EE97
			public double Display2Height
			{
				get
				{
					return this.Display1Height;
				}
			}

			// Token: 0x170001AF RID: 431
			// (get) Token: 0x06000456 RID: 1110 RVA: 0x00010C9F File Offset: 0x0000EE9F
			public double Display2CenterY
			{
				get
				{
					return this.Display2Height / 2.0;
				}
			}

			// Token: 0x170001B0 RID: 432
			// (get) Token: 0x06000457 RID: 1111 RVA: 0x00010CB4 File Offset: 0x0000EEB4
			public double Display2X
			{
				get
				{
					if (this.Skin == null)
					{
						return 0.0;
					}
					if (!this.Skin.Display.DisplaysStackedVertically)
					{
						return this.Display1X + this.Display1Width + (double)this.Skin.Display.DisplayGapWidth;
					}
					return this.Display1X;
				}
			}

			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06000458 RID: 1112 RVA: 0x00010D0C File Offset: 0x0000EF0C
			public double Display2Y
			{
				get
				{
					if (this.Skin == null)
					{
						return 0.0;
					}
					if (!this.Skin.Display.DisplaysStackedVertically)
					{
						return this.Display1Y;
					}
					return this.Display1Y + this.Display1Height + (double)this.Skin.Display.DisplayGapWidth;
				}
			}

			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x06000459 RID: 1113 RVA: 0x00010D63 File Offset: 0x0000EF63
			public Thickness Display2Margin
			{
				get
				{
					return new Thickness(this.Display2X, this.Display2Y, 0.0, 0.0);
				}
			}

			// Token: 0x170001B3 RID: 435
			// (get) Token: 0x0600045A RID: 1114 RVA: 0x00010D88 File Offset: 0x0000EF88
			private IXdeGuestDisplay GuestDisplay
			{
				get
				{
					return this.parent.GuestDisplay;
				}
			}

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x0600045B RID: 1115 RVA: 0x00010D95 File Offset: 0x0000EF95
			private IXdeSkin Skin
			{
				get
				{
					return this.parent.skin;
				}
			}

			// Token: 0x170001B5 RID: 437
			// (get) Token: 0x0600045C RID: 1116 RVA: 0x00010DA2 File Offset: 0x0000EFA2
			private IXdeDisplayOutput Output
			{
				get
				{
					return this.parent.output;
				}
			}

			// Token: 0x170001B6 RID: 438
			// (get) Token: 0x0600045D RID: 1117 RVA: 0x00010DAF File Offset: 0x0000EFAF
			private ImageSource ImageSource1
			{
				get
				{
					if (this.Output == null)
					{
						return null;
					}
					return this.Output.GetDisplayOutput(this.displayIndex);
				}
			}

			// Token: 0x170001B7 RID: 439
			// (get) Token: 0x0600045E RID: 1118 RVA: 0x00010DCC File Offset: 0x0000EFCC
			private ImageSource ImageSource2
			{
				get
				{
					if (this.Output == null || !this.hasSecondDisplay)
					{
						return null;
					}
					return this.Output.GetDisplayOutput(this.displayIndex + 1);
				}
			}

			// Token: 0x0600045F RID: 1119 RVA: 0x00010DF3 File Offset: 0x0000EFF3
			public void RefreshMessageVis()
			{
				this.OnPropertyChanged("StatusMessageVisiblity");
				this.OnPropertyChanged("DisplayVisiblity");
				this.OnPropertyChanged("Display2Visiblity");
			}

			// Token: 0x06000460 RID: 1120 RVA: 0x00010E16 File Offset: 0x0000F016
			public void RefreshDisplayLandscapeRotations()
			{
				this.OnPropertyChanged("Display1RotateAngle");
				this.OnPropertyChanged("Display2RotateAngle");
			}

			// Token: 0x06000461 RID: 1121 RVA: 0x00010E30 File Offset: 0x0000F030
			private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == null)
				{
					this.OnPropertyChanged(null);
					return;
				}
				if (e.PropertyName == "DisplayScale")
				{
					this.OnPropertyChanged("DescaledFontSize");
					return;
				}
				if (e.PropertyName == "RotateAngle")
				{
					this.RefreshDisplayLandscapeRotations();
					return;
				}
				if (e.PropertyName == "EffectiveTouchMode")
				{
					this.OnPropertyChanged("DisplayCursor");
				}
			}

			// Token: 0x0400018E RID: 398
			private string message;

			// Token: 0x0400018F RID: 399
			private int displayIndex;

			// Token: 0x04000190 RID: 400
			private bool hasSecondDisplay;

			// Token: 0x04000191 RID: 401
			private EmulatorWindowViewModel parent;

			// Token: 0x04000192 RID: 402
			private Visibility lastVis = Visibility.Hidden;

			// Token: 0x04000193 RID: 403
			private double posX;

			// Token: 0x04000194 RID: 404
			private double posY;
		}
	}
}
