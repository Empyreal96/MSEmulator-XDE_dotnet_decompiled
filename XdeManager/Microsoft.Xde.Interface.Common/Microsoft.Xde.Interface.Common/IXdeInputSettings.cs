using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000F RID: 15
	public interface IXdeInputSettings : INotifyPropertyChanged
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600003E RID: 62
		// (remove) Token: 0x0600003F RID: 63
		event KeyEventHandler KeyDown;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000040 RID: 64
		// (remove) Token: 0x06000041 RID: 65
		event KeyEventHandler KeyUp;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000042 RID: 66
		// (remove) Token: 0x06000043 RID: 67
		event MouseEventHandler MouseDown;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000044 RID: 68
		// (remove) Token: 0x06000045 RID: 69
		event MouseEventHandler MouseUp;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000046 RID: 70
		// (remove) Token: 0x06000047 RID: 71
		event MouseEventHandler MouseMove;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000048 RID: 72
		// (remove) Token: 0x06000049 RID: 73
		event EventHandler GotFocus;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600004A RID: 74
		// (remove) Token: 0x0600004B RID: 75
		event EventHandler LostFocus;

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600004C RID: 76
		// (set) Token: 0x0600004D RID: 77
		TouchMode TouchMode { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600004E RID: 78
		// (set) Token: 0x0600004F RID: 79
		Cursor HostCursor { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000050 RID: 80
		// (set) Token: 0x06000051 RID: 81
		bool HostCursorDisabledInMouseMode { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000052 RID: 82
		// (set) Token: 0x06000053 RID: 83
		int TouchPressure { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000054 RID: 84
		int MaxPressure { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000055 RID: 85
		int MinPressure { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000056 RID: 86
		// (set) Token: 0x06000057 RID: 87
		bool KeyboardEnabled { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000058 RID: 88
		// (set) Token: 0x06000059 RID: 89
		bool DragWindowIfMouseDownOnDisabledInput { get; set; }
	}
}
