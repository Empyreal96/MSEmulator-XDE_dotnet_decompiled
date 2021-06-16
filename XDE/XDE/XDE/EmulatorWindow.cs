using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Xde.Base;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000013 RID: 19
	[Export(typeof(IXdeView))]
	[Export(typeof(IXdeDisplayInformation))]
	public class EmulatorWindow : Window, IXdeView, System.Windows.Forms.IWin32Window, IXdeDisplayInformation, IComponentConnector
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000478C File Offset: 0x0000298C
		public EmulatorWindow()
		{
			this.InitializeComponent();
			base.Loaded += this.EmulatorWindow_Loaded;
			this.model = new EmulatorWindowViewModel(this);
			base.DataContext = this.model;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600008E RID: 142 RVA: 0x000047DC File Offset: 0x000029DC
		// (remove) Token: 0x0600008F RID: 143 RVA: 0x00004814 File Offset: 0x00002A14
		public event EventHandler Shown;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000090 RID: 144 RVA: 0x0000484C File Offset: 0x00002A4C
		// (remove) Token: 0x06000091 RID: 145 RVA: 0x00004884 File Offset: 0x00002A84
		public event EventHandler Load;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000092 RID: 146 RVA: 0x000048BC File Offset: 0x00002ABC
		// (remove) Token: 0x06000093 RID: 147 RVA: 0x000048F4 File Offset: 0x00002AF4
		public event EventHandler<ResolutionChangedEventArgs> PhysicalResolutionChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000094 RID: 148 RVA: 0x0000492C File Offset: 0x00002B2C
		// (remove) Token: 0x06000095 RID: 149 RVA: 0x00004964 File Offset: 0x00002B64
		public event EventHandler RdpDisconnected;

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004999 File Offset: 0x00002B99
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000049A1 File Offset: 0x00002BA1
		[Import]
		public IXdeControllerState ControllerState
		{
			get
			{
				return this.controllerState;
			}
			set
			{
				this.controllerState = value;
				this.model.ControllerState = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000049B6 File Offset: 0x00002BB6
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000049C3 File Offset: 0x00002BC3
		[Import]
		public IXdeAutomationInput InjectedInput
		{
			get
			{
				return this.model.InjectedInput;
			}
			set
			{
				this.model.InjectedInput = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000049D1 File Offset: 0x00002BD1
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000049D9 File Offset: 0x00002BD9
		[Import]
		public InputSettings InputSettings
		{
			get
			{
				return this.inputSettings;
			}
			set
			{
				this.inputSettings = value;
				this.model.InputSettings = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000049EE File Offset: 0x00002BEE
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000049F6 File Offset: 0x00002BF6
		[Import]
		public IXdeSku Sku
		{
			get
			{
				return this.sku;
			}
			set
			{
				this.sku = value;
				this.model.Sku = value;
				this.sku.PropertyChanged += this.Sku_PropertyChanged;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004A22 File Offset: 0x00002C22
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004A2A File Offset: 0x00002C2A
		[Import]
		public IXdeDisplayOutput DisplayOutput { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004A33 File Offset: 0x00002C33
		public IXdeToolbar Toolbar
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004A3C File Offset: 0x00002C3C
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004A7F File Offset: 0x00002C7F
		public System.Drawing.Point Location
		{
			get
			{
				System.Drawing.Point location = default(System.Drawing.Point);
				base.Dispatcher.Invoke(delegate()
				{
					location.X = (int)this.Left;
					location.Y = (int)this.Top;
				});
				return location;
			}
			set
			{
				base.Left = (double)value.X;
				base.Top = (double)value.Y;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004AA0 File Offset: 0x00002CA0
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004ACC File Offset: 0x00002CCC
		public System.Drawing.Point ScreenLocation
		{
			get
			{
				NativeMethods.RECT rect;
				NativeMethods.GetWindowRect(this.Handle, out rect);
				return new System.Drawing.Point(rect.left, rect.top);
			}
			set
			{
				NativeMethods.SetWindowPos(this.Handle, IntPtr.Zero, value.X, value.Y, 0, 0, 21U);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004AF4 File Offset: 0x00002CF4
		public System.Drawing.Rectangle DesktopBounds
		{
			get
			{
				return new System.Drawing.Rectangle(this.Location, new System.Drawing.Size
				{
					Width = (int)base.ActualWidth,
					Height = (int)base.ActualHeight
				});
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004B34 File Offset: 0x00002D34
		public System.Drawing.Rectangle ToolbarDesktopBounds
		{
			get
			{
				System.Windows.Point point = this.MainToolbar.TransformToAncestor(this).Transform(new System.Windows.Point(0.0, 0.0));
				point.X += (double)this.Location.X;
				point.Y += (double)this.Location.Y;
				return new System.Drawing.Rectangle((int)point.X, (int)point.Y, (int)this.MainToolbar.ActualWidth, (int)this.MainToolbar.ActualHeight);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004BD1 File Offset: 0x00002DD1
		public System.Drawing.Rectangle DisplayDesktopBounds
		{
			get
			{
				return new System.Drawing.Rectangle(this.Location, new System.Drawing.Size((int)base.Width, (int)base.Height));
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004BF1 File Offset: 0x00002DF1
		public System.Drawing.Size PhysicalResolution
		{
			get
			{
				if (this.GuestDisplay == null)
				{
					return System.Drawing.Size.Empty;
				}
				return this.GuestDisplay.PhysicalResolution;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004C0C File Offset: 0x00002E0C
		public System.Drawing.Rectangle CurrentVirtualMachineDisplayBounds
		{
			get
			{
				return new System.Drawing.Rectangle(System.Drawing.Point.Empty, this.PhysicalResolution);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004C1E File Offset: 0x00002E1E
		public System.Windows.Forms.IWin32Window TopWindow
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004C21 File Offset: 0x00002E21
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00004C2E File Offset: 0x00002E2E
		public string Text
		{
			get
			{
				return this.model.WindowText;
			}
			set
			{
				this.model.WindowText = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004C3C File Offset: 0x00002E3C
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004C49 File Offset: 0x00002E49
		public bool ShowDisplayName
		{
			get
			{
				return this.model.ShowWindowText;
			}
			set
			{
				this.model.ShowWindowText = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004C57 File Offset: 0x00002E57
		public string ScreenText
		{
			get
			{
				return this.model.Message;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004C64 File Offset: 0x00002E64
		public IntPtr Handle
		{
			get
			{
				return base.Dispatcher.Invoke<IntPtr>(() => new WindowInteropHelper(this).Handle);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004C7D File Offset: 0x00002E7D
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00004C85 File Offset: 0x00002E85
		public IXdeGuestDisplay GuestDisplay { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004C8E File Offset: 0x00002E8E
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00004C96 File Offset: 0x00002E96
		Icon IXdeView.Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				if (this.icon != null)
				{
					base.Icon = XamlUtils.ConvertBitmapToBitmapImage(this.icon.ToBitmap());
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004CBD File Offset: 0x00002EBD
		float IXdeDisplayInformation.DisplayScale
		{
			get
			{
				if (this.controllerState == null || this.controllerState.Skin == null)
				{
					return 0f;
				}
				return (float)this.controllerState.Skin.DisplayScale;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004CEB File Offset: 0x00002EEB
		System.Drawing.Size IXdeDisplayInformation.DisplaySize
		{
			get
			{
				return this.PhysicalResolution;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004CF3 File Offset: 0x00002EF3
		System.Drawing.Size IXdeDisplayInformation.GuestLogicalResolution
		{
			get
			{
				return this.PhysicalResolution;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004CFB File Offset: 0x00002EFB
		System.Drawing.Size IXdeDisplayInformation.PhysicalResolution
		{
			get
			{
				if (this.GuestDisplay == null)
				{
					return System.Drawing.Size.Empty;
				}
				return this.GuestDisplay.PhysicalResolution;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004D16 File Offset: 0x00002F16
		DisplayOrientation IXdeDisplayInformation.Orientation
		{
			get
			{
				if (this.controllerState == null || this.controllerState.Skin == null)
				{
					return DisplayOrientation.Portrait;
				}
				return this.controllerState.Skin.Orientation;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004D3F File Offset: 0x00002F3F
		int IXdeDisplayInformation.GapWidth
		{
			get
			{
				if (this.controllerState == null || this.controllerState.Skin == null)
				{
					return 0;
				}
				return this.controllerState.Skin.Display.DisplayGapWidth;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004D70 File Offset: 0x00002F70
		double IXdeView.Dpi
		{
			get
			{
				return VisualTreeHelper.GetDpi(this).DpiScaleX;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004D8B File Offset: 0x00002F8B
		private bool IsGuestFocused
		{
			get
			{
				return FocusManager.GetFocusedElement(this) == this.allChromesCanvas;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004D9C File Offset: 0x00002F9C
		private System.Windows.Point LocationInDeviceUnits
		{
			get
			{
				return PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.Transform(new System.Windows.Point(base.Left, base.Top));
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public Rect GetFirstDisplayOutputRect()
		{
			DisplayControl displayControl = this.GetDisplayControl(0, 0);
			Rect rect = displayControl.TransformToVisual(this.allChromesCanvas).TransformBounds(new Rect(displayControl.RenderSize));
			return this.allChromesCanvas.RenderTransform.TransformBounds(rect);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004E18 File Offset: 0x00003018
		public void AsynchronousClose()
		{
			base.Dispatcher.BeginInvoke(new Action(delegate()
			{
				base.Close();
			}), Array.Empty<object>());
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004E38 File Offset: 0x00003038
		public void BringAppToFront()
		{
			if (base.WindowState == WindowState.Minimized)
			{
				base.WindowState = WindowState.Normal;
			}
			bool topmost = base.Topmost;
			base.Topmost = true;
			base.Focus();
			base.Topmost = topmost;
			base.Activate();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004E78 File Offset: 0x00003078
		public void FitToScreen()
		{
			System.Windows.Point point = base.PointToScreen(new System.Windows.Point(base.Left, base.Top));
			Screen screen = Screen.FromPoint(new System.Drawing.Point((int)point.X, (int)point.Y));
			this.FitToScreen(screen);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004EC0 File Offset: 0x000030C0
		private Screen GetCurrentScreen()
		{
			System.Windows.Point point = base.PointToScreen(new System.Windows.Point(base.Left, base.Top));
			return Screen.FromPoint(new System.Drawing.Point((int)point.X, (int)point.Y));
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004F00 File Offset: 0x00003100
		public void CenterToScreen()
		{
			Screen currentScreen = this.GetCurrentScreen();
			this.FitToScreen(currentScreen);
			VisualTreeHelper.GetDpi(this);
			double num = (double)currentScreen.WorkingArea.X + ((double)currentScreen.WorkingArea.Width / 2.0 - (this.model.ActualChromeWidth + (double)this.ToolbarDesktopBounds.Width) * this.model.CurrentDpi / 2.0);
			double num2 = (double)currentScreen.WorkingArea.Y + ((double)currentScreen.WorkingArea.Height / 2.0 - this.model.TotalActualHeight * this.model.CurrentDpi / 2.0);
			num = Math.Max((double)currentScreen.WorkingArea.X, num);
			num2 = Math.Max((double)currentScreen.WorkingArea.Y, num2);
			this.ScreenLocation = new System.Drawing.Point((int)num, (int)num2);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005005 File Offset: 0x00003205
		public void ConnectToVirtualMachineGuid(string virtualMachineName, string guid)
		{
			this.GuestDisplay.RedirectToBuffer = true;
			this.model.Output = this.DisplayOutput;
			this.GuestDisplay.ConnectToGuest();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000502F File Offset: 0x0000322F
		public void IndicateGuestVideoDisconnected()
		{
			this.model.Message = Microsoft.Xde.Client.Resources.GuestVideoLost;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005041 File Offset: 0x00003241
		public void IndicateFullBoot()
		{
			this.model.Message = Microsoft.Xde.Client.Resources.FullBootMessage;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005053 File Offset: 0x00003253
		public void IndicateRestoring()
		{
			this.model.Message = Microsoft.Xde.Client.Resources.RestoringMessage;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005065 File Offset: 0x00003265
		public void IndicateShutdown()
		{
			this.model.Message = Microsoft.Xde.Client.Resources.EmulatorShuttingDown;
			this.model.ShuttingDown = true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005083 File Offset: 0x00003283
		public object Invoke(Delegate method)
		{
			base.Dispatcher.Invoke(method, Array.Empty<object>());
			return null;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005098 File Offset: 0x00003298
		public void BeginInvoke(Delegate method)
		{
			base.Dispatcher.BeginInvoke(method, Array.Empty<object>());
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000050AC File Offset: 0x000032AC
		public void Minimize()
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000050B5 File Offset: 0x000032B5
		public void Run()
		{
			base.ShowDialog();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000050C0 File Offset: 0x000032C0
		public System.Windows.Point GetFlyoutLocationForButton(string buttonName)
		{
			double y = 0.0;
			double actualWidth = this.MainToolbar.ActualWidth;
			foreach (object obj in ((WrapPanel)this.MainToolbar.Template.FindName("ItemsPanel", this.MainToolbar)).Children)
			{
				FrameworkElement frameworkElement = (FrameworkElement)obj;
				ToolBarButtonViewModel toolBarButtonViewModel = frameworkElement.DataContext as ToolBarButtonViewModel;
				if (toolBarButtonViewModel != null && buttonName == toolBarButtonViewModel.Name)
				{
					y = frameworkElement.TransformToVisual(this.MainToolbar).TransformBounds(new Rect(frameworkElement.RenderSize)).Top;
					break;
				}
			}
			return new System.Windows.Point(actualWidth, y);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000519C File Offset: 0x0000339C
		public void GiveGuestFocus()
		{
			FocusManager.SetFocusedElement(this, this.allChromesCanvas);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000051AA File Offset: 0x000033AA
		public void GiveToolbarFocus()
		{
			FocusManager.SetFocusedElement(this, this.ToolbarGrid);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000051B8 File Offset: 0x000033B8
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			this.model.OnRender();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000051CC File Offset: 0x000033CC
		protected override void OnStateChanged(EventArgs e)
		{
			if (base.WindowState == WindowState.Maximized)
			{
				base.WindowState = WindowState.Normal;
				return;
			}
			base.OnStateChanged(e);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000051E6 File Offset: 0x000033E6
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);
			this.model.OnDpiScaleChanged(oldDpi.DpiScaleX, newDpi.DpiScaleX);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005209 File Offset: 0x00003409
		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.keyboardHook != null)
			{
				this.keyboardHook.Dispose();
				this.keyboardHook = null;
			}
			base.OnClosing(e);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000522C File Offset: 0x0000342C
		protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
		{
			if (this.IsGuestFocused)
			{
				e.Handled = true;
				return;
			}
			base.OnPreviewKeyDown(e);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005245 File Offset: 0x00003445
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			base.DragMove();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005254 File Offset: 0x00003454
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			this.InputSettings.OnGotFocus();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005268 File Offset: 0x00003468
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			this.InputSettings.OnLostFocus();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000527C File Offset: 0x0000347C
		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005285 File Offset: 0x00003485
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this.keyboardHook != null)
			{
				this.keyboardHook.Enabled = true;
			}
			EventHandler shown = this.Shown;
			if (shown == null)
			{
				return;
			}
			shown(this, EventArgs.Empty);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000052B8 File Offset: 0x000034B8
		protected override void OnDeactivated(EventArgs e)
		{
			base.OnDeactivated(e);
			if (this.keyboardHook != null)
			{
				this.keyboardHook.Enabled = false;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000052D8 File Offset: 0x000034D8
		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
		{
			if (this.currentCorner == ResizeCorner.None)
			{
				base.OnMouseMove(e);
				return;
			}
			System.Windows.Point position = Mouse.GetPosition(this);
			Vector diff = position - this.cornerDownAt;
			this.model.AttemptResize(this.currentCorner, diff);
			this.cornerDownAt = position;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005322 File Offset: 0x00003522
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this.currentCorner == ResizeCorner.None)
			{
				base.OnMouseLeftButtonUp(e);
				return;
			}
			base.ReleaseMouseCapture();
			this.currentCorner = ResizeCorner.None;
			base.Cursor = System.Windows.Input.Cursors.Arrow;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000534C File Offset: 0x0000354C
		private void FitToScreen(Screen screen)
		{
			int num = screen.WorkingArea.Height * 85 / 100;
			int num2 = screen.WorkingArea.Width * 85 / 100;
			float newScale = 1f;
			IXdeSkin skin = (this.controllerState != null) ? this.controllerState.Skin : null;
			if (skin == null)
			{
				return;
			}
			System.Drawing.Size unscaledBitmapSize = skin.UnscaledBitmapSize;
			unscaledBitmapSize.Width *= skin.ActiveChromeCount;
			if (skin.Orientation != DisplayOrientation.Portrait)
			{
				unscaledBitmapSize = new System.Drawing.Size(unscaledBitmapSize.Height, unscaledBitmapSize.Width);
			}
			if (unscaledBitmapSize.Height > num)
			{
				newScale = (float)num / (float)unscaledBitmapSize.Height;
			}
			if (unscaledBitmapSize.Width > num2)
			{
				float num3 = (float)num2 / (float)unscaledBitmapSize.Width;
				if (num3 < newScale)
				{
					newScale = num3;
				}
			}
			newScale = MathUtils.Clamp<float>(newScale, 0.1f, 1f);
			this.Invoke(new MethodInvoker(delegate
			{
				skin.DisplayScale = (double)newScale;
			}));
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005474 File Offset: 0x00003674
		private void EmulatorWindow_Loaded(object sender, RoutedEventArgs e)
		{
			EventHandler load = this.Load;
			if (load != null)
			{
				load(this, EventArgs.Empty);
			}
			this.keyboardHook = new KeyboardHook();
			this.keyboardHook.Flags = (KeyboardHookFlags.EatAltTab | KeyboardHookFlags.EatWindowsKey);
			this.keyboardHook.KeyIntercepted += this.KeyboardHook_KeyIntercepted;
			this.InjectedInput.PropertyChanged += this.InjectedInput_PropertyChanged;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000054E0 File Offset: 0x000036E0
		private void InjectedInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsConnected" && this.InjectedInput.IsConnected && this.inputSettings.TouchMode != TouchMode.Mouse)
			{
				NativeMethods.POINTER_TYPE_INFO[] touchInputs = new NativeMethods.POINTER_TYPE_INFO[]
				{
					EmulatorWindowViewModel.GetTouchPoint(0.0, 0.0, 1U, TouchEventType.Hover)
				};
				this.InjectedInput.SendTouchInfo(touchInputs);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005550 File Offset: 0x00003750
		private void KeyboardHook_KeyIntercepted(object sender, KeyboardHookEventArgs e)
		{
			ushort num = (ushort)e.ScanCode;
			bool flag = e.Extended;
			if (!this.IsGuestFocused)
			{
				return;
			}
			if (flag && num == 54)
			{
				num = 42;
				flag = false;
			}
			System.Windows.Forms.KeyEventArgs args = new System.Windows.Forms.KeyEventArgs((Keys)e.KeyCode);
			if (e.Down)
			{
				this.InputSettings.OnKeyDown(args);
			}
			else
			{
				this.InputSettings.OnKeyUp(args);
			}
			if (this.InputSettings.KeyboardEnabled)
			{
				IXdeGuestDisplay guestDisplay = this.GuestDisplay;
				if (guestDisplay == null)
				{
					return;
				}
				guestDisplay.GuestInput.SendKeyboardEvent(num, !e.Down, true, flag);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000055E0 File Offset: 0x000037E0
		private void Sku_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Toolbar")
			{
				this.model.Toolbar.LoadFromSku(this.sku);
				return;
			}
			if (e.PropertyName == "Options")
			{
				this.GuestDisplay = this.sku.Options.GuestDisplay;
				this.host.Child = this.GuestDisplay.Control;
				this.host.Child.Visible = false;
				this.host.Visibility = Visibility.Collapsed;
				this.model.GuestDisplay = this.GuestDisplay;
				this.GuestDisplay.PropertyChanged += this.GuestDisplay_PropertyChanged;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005699 File Offset: 0x00003899
		private DisplayControl GetDisplayControl(int chromeIndex, int displayIndex)
		{
			return this.GetDisplayControl(this.model.ChromeModels[chromeIndex], displayIndex);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000056B4 File Offset: 0x000038B4
		private DisplayControl GetDisplayControl(EmulatorWindowViewModel.ChromeViewModel item, int displayIndex)
		{
			ContentPresenter contentPresenter = (ContentPresenter)this.ChromesList.ItemContainerGenerator.ContainerFromItem(item);
			return contentPresenter.ContentTemplate.FindName("DisplayControl" + (displayIndex + 1).ToString(), contentPresenter) as DisplayControl;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005700 File Offset: 0x00003900
		private void GuestDisplay_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "PhysicalResolution")
			{
				this.GuestDisplay_OnPhysicalResolutionChanged();
				return;
			}
			if (e.PropertyName == "IsConnected" && !this.GuestDisplay.IsConnected)
			{
				EventHandler rdpDisconnected = this.RdpDisconnected;
				if (rdpDisconnected == null)
				{
					return;
				}
				rdpDisconnected(this, EventArgs.Empty);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000575B File Offset: 0x0000395B
		private void GuestDisplay_OnPhysicalResolutionChanged()
		{
			EventHandler<ResolutionChangedEventArgs> physicalResolutionChanged = this.PhysicalResolutionChanged;
			if (physicalResolutionChanged == null)
			{
				return;
			}
			physicalResolutionChanged(this, new ResolutionChangedEventArgs(this.GuestDisplay.PhysicalResolution));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005780 File Offset: 0x00003980
		private void MultiTouch_PointsTouched(object sender, MultiTouchEventArgs e)
		{
			List<System.Windows.Point> list = new List<System.Windows.Point>();
			List<uint> list2 = new List<uint>();
			DisplayControl displayControl = null;
			System.Windows.Point[] array = new System.Windows.Point[]
			{
				e.Point1,
				e.Point2
			};
			uint num = 0U;
			while ((ulong)num < (ulong)((long)array.Length))
			{
				System.Windows.Point point = array[(int)num];
				HitTestResult hitTestResult = VisualTreeHelper.HitTest(this.ChromesList, point);
				if (hitTestResult != null)
				{
					DisplayControl displayControl2 = hitTestResult.VisualHit as DisplayControl;
					if (displayControl2 != null)
					{
						System.Windows.Point point2 = this.allChromesCanvas.TransformToVisual(displayControl2).Transform(point);
						System.Windows.Point item = new System.Windows.Point(point2.X + displayControl2.DesktopOffset.X, point2.Y + displayControl2.DesktopOffset.Y);
						uint item2 = num + 1U;
						list.Add(item);
						list2.Add(item2);
						displayControl = displayControl2;
					}
				}
				num += 1U;
			}
			if (displayControl != null)
			{
				this.model.ProcessTouchPoints(list.ToArray(), e.Type, list2.ToArray());
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000588B File Offset: 0x00003A8B
		private void ToolbarGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.GiveToolbarFocus();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005893 File Offset: 0x00003A93
		private void AllChromesCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.GiveGuestFocus();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000589C File Offset: 0x00003A9C
		private void ResizeCorner_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender == this.TopLeftSizeRect)
			{
				this.currentCorner = ResizeCorner.TopLeft;
			}
			else if (sender == this.TopRightSizeRect)
			{
				this.currentCorner = ResizeCorner.TopRight;
			}
			else if (sender == this.BottomLeftSizeRect)
			{
				this.currentCorner = ResizeCorner.BottomLeft;
			}
			else if (sender == this.BottomRightSizeRect)
			{
				this.currentCorner = ResizeCorner.BottomRight;
			}
			else if (sender == this.TopSizeRect)
			{
				this.currentCorner = ResizeCorner.Top;
			}
			else if (sender == this.BottomSizeRect)
			{
				this.currentCorner = ResizeCorner.Bottom;
			}
			else if (sender == this.LeftSizeRect)
			{
				this.currentCorner = ResizeCorner.Left;
			}
			else
			{
				if (sender != this.RightSizeRect)
				{
					throw new InvalidOperationException();
				}
				this.currentCorner = ResizeCorner.Right;
			}
			FrameworkElement frameworkElement = (FrameworkElement)sender;
			this.cornerDownAt = Mouse.GetPosition(this);
			base.CaptureMouse();
			base.Cursor = frameworkElement.Cursor;
			e.Handled = true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000596F File Offset: 0x00003B6F
		private void CaptionPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				this.CenterToScreen();
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005980 File Offset: 0x00003B80
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/XDE;V10.1.0.0;component/controls/emulatorwindow.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000059B0 File Offset: 0x00003BB0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000059BC File Offset: 0x00003BBC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.MainGrid = (Grid)target;
				return;
			case 2:
				this.CaptionPanel = (StackPanel)target;
				this.CaptionPanel.MouseLeftButtonDown += this.CaptionPanel_MouseLeftButtonDown;
				return;
			case 3:
				this.allChromesCanvas = (Canvas)target;
				this.allChromesCanvas.MouseLeftButtonDown += this.AllChromesCanvas_MouseLeftButtonDown;
				return;
			case 4:
				this.ChromesList = (ItemsControl)target;
				return;
			case 5:
				this.MultiTouch = (MultiTouch)target;
				return;
			case 6:
				this.TopSizeRect = (System.Windows.Shapes.Rectangle)target;
				this.TopSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 7:
				this.BottomSizeRect = (System.Windows.Shapes.Rectangle)target;
				this.BottomSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 8:
				this.LeftSizeRect = (System.Windows.Shapes.Rectangle)target;
				this.LeftSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 9:
				this.RightSizeRect = (System.Windows.Shapes.Rectangle)target;
				this.RightSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 10:
				this.BottomLeftSizeRect = (Polygon)target;
				this.BottomLeftSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 11:
				this.TopLeftSizeRect = (Polygon)target;
				this.TopLeftSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 12:
				this.TopRightSizeRect = (Polygon)target;
				this.TopRightSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 13:
				this.BottomRightSizeRect = (Polygon)target;
				this.BottomRightSizeRect.MouseLeftButtonDown += this.ResizeCorner_MouseLeftButtonDown;
				return;
			case 14:
				this.ExternDisplayControl = (DisplayControl)target;
				return;
			case 15:
				this.ToolbarGrid = (Grid)target;
				this.ToolbarGrid.MouseLeftButtonDown += this.ToolbarGrid_MouseLeftButtonDown;
				return;
			case 16:
				this.MainToolbar = (XdeToolbar)target;
				return;
			case 17:
				this.FlyoutToolbar = (XdeToolbar)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005BFB File Offset: 0x00003DFB
		void IXdeView.add_Closing(CancelEventHandler value)
		{
			base.Closing += value;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005C04 File Offset: 0x00003E04
		void IXdeView.remove_Closing(CancelEventHandler value)
		{
			base.Closing -= value;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005C0D File Offset: 0x00003E0D
		void IXdeView.Close()
		{
			base.Close();
		}

		// Token: 0x04000046 RID: 70
		private EmulatorWindowViewModel model;

		// Token: 0x04000047 RID: 71
		private InputSettings inputSettings;

		// Token: 0x04000048 RID: 72
		private WindowsFormsHost host = new WindowsFormsHost();

		// Token: 0x04000049 RID: 73
		private Icon icon;

		// Token: 0x0400004A RID: 74
		private KeyboardHook keyboardHook;

		// Token: 0x0400004B RID: 75
		private System.Windows.Point cornerDownAt;

		// Token: 0x0400004C RID: 76
		private ResizeCorner currentCorner;

		// Token: 0x0400004D RID: 77
		private IXdeControllerState controllerState;

		// Token: 0x0400004E RID: 78
		private IXdeSku sku;

		// Token: 0x04000055 RID: 85
		internal Grid MainGrid;

		// Token: 0x04000056 RID: 86
		internal StackPanel CaptionPanel;

		// Token: 0x04000057 RID: 87
		internal Canvas allChromesCanvas;

		// Token: 0x04000058 RID: 88
		internal ItemsControl ChromesList;

		// Token: 0x04000059 RID: 89
		internal MultiTouch MultiTouch;

		// Token: 0x0400005A RID: 90
		internal System.Windows.Shapes.Rectangle TopSizeRect;

		// Token: 0x0400005B RID: 91
		internal System.Windows.Shapes.Rectangle BottomSizeRect;

		// Token: 0x0400005C RID: 92
		internal System.Windows.Shapes.Rectangle LeftSizeRect;

		// Token: 0x0400005D RID: 93
		internal System.Windows.Shapes.Rectangle RightSizeRect;

		// Token: 0x0400005E RID: 94
		internal Polygon BottomLeftSizeRect;

		// Token: 0x0400005F RID: 95
		internal Polygon TopLeftSizeRect;

		// Token: 0x04000060 RID: 96
		internal Polygon TopRightSizeRect;

		// Token: 0x04000061 RID: 97
		internal Polygon BottomRightSizeRect;

		// Token: 0x04000062 RID: 98
		internal DisplayControl ExternDisplayControl;

		// Token: 0x04000063 RID: 99
		internal Grid ToolbarGrid;

		// Token: 0x04000064 RID: 100
		internal XdeToolbar MainToolbar;

		// Token: 0x04000065 RID: 101
		internal XdeToolbar FlyoutToolbar;

		// Token: 0x04000066 RID: 102
		private bool _contentLoaded;
	}
}
