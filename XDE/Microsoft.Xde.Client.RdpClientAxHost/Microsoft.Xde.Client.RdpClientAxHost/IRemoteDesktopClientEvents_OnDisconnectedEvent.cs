using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000050 RID: 80
	public class IRemoteDesktopClientEvents_OnDisconnectedEvent
	{
		// Token: 0x06000E97 RID: 3735 RVA: 0x00026F1F File Offset: 0x0002511F
		public IRemoteDesktopClientEvents_OnDisconnectedEvent(int disconnectReason, int extendedDisconnectReason, string disconnectErrorMessage)
		{
			this.disconnectReason = disconnectReason;
			this.extendedDisconnectReason = extendedDisconnectReason;
			this.disconnectErrorMessage = disconnectErrorMessage;
		}

		// Token: 0x040002F6 RID: 758
		public int disconnectReason;

		// Token: 0x040002F7 RID: 759
		public int extendedDisconnectReason;

		// Token: 0x040002F8 RID: 760
		public string disconnectErrorMessage;
	}
}
