using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000052 RID: 82
	public class IRemoteDesktopClientEvents_OnStatusChangedEvent
	{
		// Token: 0x06000E9C RID: 3740 RVA: 0x00026F3C File Offset: 0x0002513C
		public IRemoteDesktopClientEvents_OnStatusChangedEvent(int statusCode, string statusMessage)
		{
			this.statusCode = statusCode;
			this.statusMessage = statusMessage;
		}

		// Token: 0x040002F9 RID: 761
		public int statusCode;

		// Token: 0x040002FA RID: 762
		public string statusMessage;
	}
}
