using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Base
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IXdeInputSettings))]
	[Export(typeof(InputSettings))]
	public class InputSettings : IXdeInputSettings, INotifyPropertyChanged
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000024F3 File Offset: 0x000006F3
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000024FB File Offset: 0x000006FB
		public TouchMode TouchMode
		{
			get
			{
				return this.touchMode;
			}
			set
			{
				if (this.touchMode != value)
				{
					this.touchMode = value;
					this.OnPropertyChanged("TouchMode");
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002518 File Offset: 0x00000718
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002520 File Offset: 0x00000720
		public Cursor HostCursor { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002529 File Offset: 0x00000729
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002531 File Offset: 0x00000731
		public bool HostCursorDisabledInMouseMode { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000253A File Offset: 0x0000073A
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002542 File Offset: 0x00000742
		public int TouchPressure
		{
			get
			{
				return this.touchPressure;
			}
			set
			{
				value = MathUtils.Clamp<int>(value, this.MinPressure, this.MaxPressure);
				if (this.touchPressure != value)
				{
					this.touchPressure = value;
					this.OnPropertyChanged("TouchPressure");
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002573 File Offset: 0x00000773
		public int MaxPressure
		{
			get
			{
				return 1024;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000257A File Offset: 0x0000077A
		public int MinPressure
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000257D File Offset: 0x0000077D
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002585 File Offset: 0x00000785
		public bool DragWindowIfMouseDownOnDisabledInput
		{
			get
			{
				return this.dragWindowIfMouseDownOnDisabledInput;
			}
			set
			{
				if (this.dragWindowIfMouseDownOnDisabledInput != value)
				{
					this.dragWindowIfMouseDownOnDisabledInput = value;
					this.OnPropertyChanged("DragWindowIfMouseDownOnDisabledInput");
				}
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000020 RID: 32 RVA: 0x000025A4 File Offset: 0x000007A4
		// (remove) Token: 0x06000021 RID: 33 RVA: 0x000025DC File Offset: 0x000007DC
		public event KeyEventHandler KeyDown;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000022 RID: 34 RVA: 0x00002614 File Offset: 0x00000814
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x0000264C File Offset: 0x0000084C
		public event KeyEventHandler KeyUp;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000024 RID: 36 RVA: 0x00002684 File Offset: 0x00000884
		// (remove) Token: 0x06000025 RID: 37 RVA: 0x000026BC File Offset: 0x000008BC
		public event MouseEventHandler MouseDown;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000026 RID: 38 RVA: 0x000026F4 File Offset: 0x000008F4
		// (remove) Token: 0x06000027 RID: 39 RVA: 0x0000272C File Offset: 0x0000092C
		public event MouseEventHandler MouseUp;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000028 RID: 40 RVA: 0x00002764 File Offset: 0x00000964
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x0000279C File Offset: 0x0000099C
		public event MouseEventHandler MouseMove;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600002A RID: 42 RVA: 0x000027D4 File Offset: 0x000009D4
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x0000280C File Offset: 0x00000A0C
		public event EventHandler GotFocus;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600002C RID: 44 RVA: 0x00002844 File Offset: 0x00000A44
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x0000287C File Offset: 0x00000A7C
		public event EventHandler LostFocus;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600002E RID: 46 RVA: 0x000028B4 File Offset: 0x00000AB4
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x000028EC File Offset: 0x00000AEC
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002921 File Offset: 0x00000B21
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002929 File Offset: 0x00000B29
		public bool KeyboardEnabled
		{
			get
			{
				return this.keyboardEnabled;
			}
			set
			{
				if (this.keyboardEnabled != value)
				{
					this.keyboardEnabled = value;
					this.OnPropertyChanged("KeyboardEnabled");
				}
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002946 File Offset: 0x00000B46
		public void OnMouseDown(MouseButtons buttons, int x, int y)
		{
			MouseEventHandler mouseDown = this.MouseDown;
			if (mouseDown == null)
			{
				return;
			}
			mouseDown(this, new MouseEventArgs(buttons, 1, x, y, 0));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002963 File Offset: 0x00000B63
		public void OnMouseMove(MouseButtons buttons, int x, int y)
		{
			MouseEventHandler mouseMove = this.MouseMove;
			if (mouseMove == null)
			{
				return;
			}
			mouseMove(this, new MouseEventArgs(buttons, 0, x, y, 0));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002980 File Offset: 0x00000B80
		public void OnMouseUp(MouseButtons buttons, int x, int y)
		{
			MouseEventHandler mouseUp = this.MouseUp;
			if (mouseUp == null)
			{
				return;
			}
			mouseUp(this, new MouseEventArgs(buttons, 0, x, y, 0));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000299D File Offset: 0x00000B9D
		public void OnGotFocus()
		{
			EventHandler gotFocus = this.GotFocus;
			if (gotFocus == null)
			{
				return;
			}
			gotFocus(this, EventArgs.Empty);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000029B5 File Offset: 0x00000BB5
		public void OnLostFocus()
		{
			EventHandler lostFocus = this.LostFocus;
			if (lostFocus == null)
			{
				return;
			}
			lostFocus(this, EventArgs.Empty);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000029CD File Offset: 0x00000BCD
		public void OnKeyDown(KeyEventArgs args)
		{
			KeyEventHandler keyDown = this.KeyDown;
			if (keyDown == null)
			{
				return;
			}
			keyDown(this, args);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000029E1 File Offset: 0x00000BE1
		public void OnKeyUp(KeyEventArgs args)
		{
			KeyEventHandler keyUp = this.KeyUp;
			if (keyUp == null)
			{
				return;
			}
			keyUp(this, args);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000029F5 File Offset: 0x00000BF5
		private void OnPropertyChanged(string propName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		// Token: 0x04000010 RID: 16
		private const int MaxPressureValue = 1024;

		// Token: 0x04000011 RID: 17
		private const int MinPressureValue = 1;

		// Token: 0x04000012 RID: 18
		private int touchPressure = 512;

		// Token: 0x04000013 RID: 19
		private TouchMode touchMode = TouchMode.Mouse;

		// Token: 0x04000014 RID: 20
		private bool keyboardEnabled = true;

		// Token: 0x04000015 RID: 21
		private bool dragWindowIfMouseDownOnDisabledInput = true;
	}
}
