using System;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200001B RID: 27
	public class SkinButtonItemViewModel : ViewModelBase
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00007D94 File Offset: 0x00005F94
		public SkinButtonItemViewModel(EmulatorWindowViewModel parent, ISkinButtonInfo buttonInfo, BitmapSource upImage, BitmapSource downImage)
		{
			this.parent = parent;
			this.buttonInfo = buttonInfo;
			this.upImage = upImage;
			this.downImage = downImage;
			this.scanCodes = new ushort[buttonInfo.KeyCode.Length];
			for (int i = 0; i < buttonInfo.KeyCode.Length; i++)
			{
				this.scanCodes[i] = SkinButtonItemViewModel.KeysToScanCode(buttonInfo.KeyCode[i]);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00007DFF File Offset: 0x00005FFF
		public BitmapSource DownImage
		{
			get
			{
				return this.downImage;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00007E07 File Offset: 0x00006007
		public BitmapSource UpImage
		{
			get
			{
				return this.upImage;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007E10 File Offset: 0x00006010
		public double X
		{
			get
			{
				return (double)this.buttonInfo.Bounds.Left;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007E34 File Offset: 0x00006034
		public double Y
		{
			get
			{
				return (double)this.buttonInfo.Bounds.Top;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007E58 File Offset: 0x00006058
		public double Width
		{
			get
			{
				return (double)this.buttonInfo.Bounds.Width;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007E7C File Offset: 0x0000607C
		public double Height
		{
			get
			{
				return (double)this.buttonInfo.Bounds.Height;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007EA0 File Offset: 0x000060A0
		public string ToolTip
		{
			get
			{
				switch (this.buttonInfo.ButtonType)
				{
				case SkinButtonType.VolumeUp:
					return Resources.ToolTip_VolumeUp;
				case SkinButtonType.VolumeDown:
					return Resources.ToolTip_VolumeDown;
				case SkinButtonType.Power:
					return Resources.ToolTip_Power;
				default:
					return null;
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007EE2 File Offset: 0x000060E2
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00007EEC File Offset: 0x000060EC
		public bool IsPressed
		{
			get
			{
				return this.isPressed;
			}
			set
			{
				if (this.isPressed != value)
				{
					this.isPressed = value;
					foreach (ushort num in this.scanCodes)
					{
						bool extended = num == 91;
						IXdeGuestDisplay guestDisplay = this.parent.GuestDisplay;
						if (guestDisplay != null)
						{
							IXdeGuestInput guestInput = guestDisplay.GuestInput;
							if (guestInput != null)
							{
								guestInput.SendKeyboardEvent(num, !this.isPressed, true, extended);
							}
						}
					}
					this.OnPropertyChanged("IsPressed");
				}
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007F60 File Offset: 0x00006160
		private static ushort KeysToScanCode(Keys keys)
		{
			if (keys == Keys.LWin)
			{
				return 91;
			}
			switch (keys)
			{
			case Keys.F1:
			case Keys.F2:
			case Keys.F3:
			case Keys.F4:
			case Keys.F5:
			case Keys.F6:
			case Keys.F7:
			case Keys.F8:
			case Keys.F9:
			case Keys.F10:
				return (ushort)(keys - Keys.F1 + 59);
			case Keys.F11:
				return 87;
			case Keys.F12:
				return 88;
			default:
				throw new ArgumentOutOfRangeException("keys");
			}
		}

		// Token: 0x040000A1 RID: 161
		private const ushort LWinScanCode = 91;

		// Token: 0x040000A2 RID: 162
		private const ushort F1ScanCode = 59;

		// Token: 0x040000A3 RID: 163
		private const ushort F11ScanCode = 87;

		// Token: 0x040000A4 RID: 164
		private const ushort F12ScanCode = 88;

		// Token: 0x040000A5 RID: 165
		private EmulatorWindowViewModel parent;

		// Token: 0x040000A6 RID: 166
		private BitmapSource upImage;

		// Token: 0x040000A7 RID: 167
		private BitmapSource downImage;

		// Token: 0x040000A8 RID: 168
		private ISkinButtonInfo buttonInfo;

		// Token: 0x040000A9 RID: 169
		private bool isPressed;

		// Token: 0x040000AA RID: 170
		private ushort[] scanCodes;
	}
}
