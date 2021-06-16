using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000D RID: 13
	public interface IXdeGuestDisplay : IXdePluginComponent, INotifyPropertyChanged, IDisposable
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600002B RID: 43
		Control Control { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600002C RID: 44
		Size PhysicalResolution { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600002D RID: 45
		bool IsConnected { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600002E RID: 46
		IntPtr InputWindowHandle { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600002F RID: 47
		bool IsDisposing { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000030 RID: 48
		// (set) Token: 0x06000031 RID: 49
		bool RotateLandscapeUpperHalf { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000032 RID: 50
		// (set) Token: 0x06000033 RID: 51
		bool RedirectToBuffer { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000034 RID: 52
		IXdeGuestInput GuestInput { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000035 RID: 53
		bool SupportsRotateLandscapeUpperHalf { get; }

		// Token: 0x06000036 RID: 54
		void UpdateFromSku(IXdeSku sku);

		// Token: 0x06000037 RID: 55
		void ConnectToGuest();

		// Token: 0x06000038 RID: 56
		void SetOrientation(DisplayOrientation orientation);

		// Token: 0x06000039 RID: 57
		void SetScale(float scale);

		// Token: 0x0600003A RID: 58
		void Refresh();
	}
}
