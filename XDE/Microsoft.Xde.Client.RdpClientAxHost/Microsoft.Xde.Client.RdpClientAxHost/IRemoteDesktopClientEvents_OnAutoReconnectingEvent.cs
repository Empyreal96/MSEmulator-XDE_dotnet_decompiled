using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000054 RID: 84
	public class IRemoteDesktopClientEvents_OnAutoReconnectingEvent
	{
		// Token: 0x06000EA1 RID: 3745 RVA: 0x00026F52 File Offset: 0x00025152
		public IRemoteDesktopClientEvents_OnAutoReconnectingEvent(int disconnectReason, int extendedDisconnectReason, string disconnectErrorMessage, bool networkAvailable, int attemptCount, int maxAttemptCount)
		{
			this.disconnectReason = disconnectReason;
			this.extendedDisconnectReason = extendedDisconnectReason;
			this.disconnectErrorMessage = disconnectErrorMessage;
			this.networkAvailable = networkAvailable;
			this.attemptCount = attemptCount;
			this.maxAttemptCount = maxAttemptCount;
		}

		// Token: 0x040002FB RID: 763
		public int disconnectReason;

		// Token: 0x040002FC RID: 764
		public int extendedDisconnectReason;

		// Token: 0x040002FD RID: 765
		public string disconnectErrorMessage;

		// Token: 0x040002FE RID: 766
		public bool networkAvailable;

		// Token: 0x040002FF RID: 767
		public int attemptCount;

		// Token: 0x04000300 RID: 768
		public int maxAttemptCount;
	}
}
