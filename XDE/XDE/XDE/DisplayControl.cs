using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000014 RID: 20
	public class DisplayControl : Image
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005C2C File Offset: 0x00003E2C
		public Point DesktopOffset
		{
			get
			{
				if (this.desktopOffset == null)
				{
					if (!this.Skin.Display.DisplaysStackedVertically)
					{
						this.desktopOffset = new Point?(new Point((double)(this.Skin.Display.DisplayWidth * this.DisplayIndex), 0.0));
					}
					else
					{
						this.desktopOffset = new Point?(new Point(0.0, (double)(this.Skin.Display.DisplayHeight * this.DisplayIndex)));
					}
				}
				return this.desktopOffset.Value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005CC8 File Offset: 0x00003EC8
		private EmulatorWindowViewModel EmulatorViewModel
		{
			get
			{
				if (this.emulatorViewModel != null)
				{
					return this.emulatorViewModel;
				}
				if (this.ChromeViewModel != null)
				{
					this.emulatorViewModel = this.ChromeViewModel.Parent;
					int num = this.ChromeViewModel.DisplayIndex;
					if (base.Name == "DisplayControl2")
					{
						num++;
					}
					this.DisplayIndex = num;
				}
				else
				{
					this.emulatorViewModel = (base.DataContext as EmulatorWindowViewModel);
					this.DisplayIndex = this.Skin.Display.DisplayCount;
				}
				return this.emulatorViewModel;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005D55 File Offset: 0x00003F55
		private EmulatorWindowViewModel.ChromeViewModel ChromeViewModel
		{
			get
			{
				return base.DataContext as EmulatorWindowViewModel.ChromeViewModel;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005D62 File Offset: 0x00003F62
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00005D6A File Offset: 0x00003F6A
		private int DisplayIndex { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005D73 File Offset: 0x00003F73
		private IXdeSkin Skin
		{
			get
			{
				return this.EmulatorViewModel.Window.ControllerState.Skin;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005D8C File Offset: 0x00003F8C
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			e.Handled = true;
			if (this.EmulatorViewModel.InputSettings.TouchMode == TouchMode.Disabled && this.EmulatorViewModel.InputSettings.DragWindowIfMouseDownOnDisabledInput)
			{
				this.EmulatorViewModel.Window.DragMove();
				return;
			}
			this.GiveGuestFocus();
			MouseButtons mouseButtonsFromEventArgs = DisplayControl.GetMouseButtonsFromEventArgs(e);
			Point displayControlMousePos = this.GetDisplayControlMousePos(e);
			this.EmulatorViewModel.OnDisplayMouseLeftButtonDown(mouseButtonsFromEventArgs, displayControlMousePos);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005E00 File Offset: 0x00004000
		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			e.Handled = true;
			MouseButtons mouseButtonsFromEventArgs = DisplayControl.GetMouseButtonsFromEventArgs(e);
			Point displayControlMousePos = this.GetDisplayControlMousePos(e);
			this.EmulatorViewModel.OnDisplayMouseMove(mouseButtonsFromEventArgs, displayControlMousePos);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005E38 File Offset: 0x00004038
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			e.Handled = true;
			base.OnMouseLeftButtonUp(e);
			MouseButtons mouseButtonsFromEventArgs = DisplayControl.GetMouseButtonsFromEventArgs(e);
			Point displayControlMousePos = this.GetDisplayControlMousePos(e);
			this.EmulatorViewModel.OnDisplayMouseLeftButtonUp(mouseButtonsFromEventArgs, displayControlMousePos);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005E6F File Offset: 0x0000406F
		protected override void OnTouchDown(TouchEventArgs e)
		{
			this.EmulatorViewModel.Window.GiveGuestFocus();
			base.OnTouchDown(e);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005E88 File Offset: 0x00004088
		protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			e.Handled = true;
			this.EmulatorViewModel.OnDisplayMouseLeave();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005EA4 File Offset: 0x000040A4
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseRightButtonDown(e);
			e.Handled = true;
			MouseButtons mouseButtonsFromEventArgs = DisplayControl.GetMouseButtonsFromEventArgs(e);
			Point displayControlMousePos = this.GetDisplayControlMousePos(e);
			this.EmulatorViewModel.OnDisplayMouseRightButtonDown(mouseButtonsFromEventArgs, displayControlMousePos);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005EDC File Offset: 0x000040DC
		protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseRightButtonUp(e);
			e.Handled = true;
			MouseButtons mouseButtonsFromEventArgs = DisplayControl.GetMouseButtonsFromEventArgs(e);
			Point displayControlMousePos = this.GetDisplayControlMousePos(e);
			this.EmulatorViewModel.OnDisplayMouseRightButtonUp(mouseButtonsFromEventArgs, displayControlMousePos);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005F13 File Offset: 0x00004113
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
			e.Handled = true;
			this.EmulatorViewModel.OnDisplayMouseWheel(e.Delta);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005F34 File Offset: 0x00004134
		private static MouseButtons GetMouseButtonsFromEventArgs(System.Windows.Input.MouseEventArgs args)
		{
			MouseButtons mouseButtons = MouseButtons.None;
			if (args.LeftButton == MouseButtonState.Pressed)
			{
				mouseButtons |= MouseButtons.Left;
			}
			if (args.RightButton == MouseButtonState.Pressed)
			{
				mouseButtons |= MouseButtons.Right;
			}
			if (args.MiddleButton == MouseButtonState.Pressed)
			{
				mouseButtons |= MouseButtons.Middle;
			}
			return mouseButtons;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005F78 File Offset: 0x00004178
		private Point GetDisplayControlMousePos(System.Windows.Input.MouseEventArgs e)
		{
			Point position = e.GetPosition(this);
			Point point = this.DesktopOffset;
			position.X += point.X;
			position.Y += point.Y;
			return position;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005FBF File Offset: 0x000041BF
		private void GiveGuestFocus()
		{
			this.EmulatorViewModel.Window.GiveGuestFocus();
		}

		// Token: 0x04000067 RID: 103
		private EmulatorWindowViewModel emulatorViewModel;

		// Token: 0x04000068 RID: 104
		private Point? desktopOffset;
	}
}
