using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200005C RID: 92
	public class IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEvent
	{
		// Token: 0x06000EB5 RID: 3765 RVA: 0x00026FC2 File Offset: 0x000251C2
		public IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEvent(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		// Token: 0x04000306 RID: 774
		public int width;

		// Token: 0x04000307 RID: 775
		public int height;
	}
}
