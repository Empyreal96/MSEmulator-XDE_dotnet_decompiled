using System;
using Microsoft.Xde.Client.RdpClientInterop;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000012 RID: 18
	public class IMsTscAxEvents_OnAutoReconnectingEvent
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x000035BA File Offset: 0x000017BA
		public IMsTscAxEvents_OnAutoReconnectingEvent(int disconnectReason, int attemptCount)
		{
			this.disconnectReason = disconnectReason;
			this.attemptCount = attemptCount;
		}

		// Token: 0x0400002C RID: 44
		public int disconnectReason;

		// Token: 0x0400002D RID: 45
		public int attemptCount;

		// Token: 0x0400002E RID: 46
		public AutoReconnectContinueState pArcContinueStatus;
	}
}
