using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200000C RID: 12
	public class IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent
	{
		// Token: 0x06000095 RID: 149 RVA: 0x0000358D File Offset: 0x0000178D
		public IMsTscAxEvents_OnRemoteDesktopSizeChangeEvent(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		// Token: 0x04000027 RID: 39
		public int width;

		// Token: 0x04000028 RID: 40
		public int height;
	}
}
