using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000058 RID: 88
	public class IRemoteDesktopClientEvents_OnAdminMessageReceivedEvent
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x00026FA4 File Offset: 0x000251A4
		public IRemoteDesktopClientEvents_OnAdminMessageReceivedEvent(string adminMessage)
		{
			this.adminMessage = adminMessage;
		}

		// Token: 0x04000304 RID: 772
		public string adminMessage;
	}
}
