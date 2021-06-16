using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000004 RID: 4
	public class IMsTscAxEvents_OnDisconnectedEvent
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000354A File Offset: 0x0000174A
		public IMsTscAxEvents_OnDisconnectedEvent(int discReason)
		{
			this.discReason = discReason;
		}

		// Token: 0x04000022 RID: 34
		public int discReason;
	}
}
