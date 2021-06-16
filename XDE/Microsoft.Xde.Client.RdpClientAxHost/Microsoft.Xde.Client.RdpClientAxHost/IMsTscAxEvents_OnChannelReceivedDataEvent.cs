using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000006 RID: 6
	public class IMsTscAxEvents_OnChannelReceivedDataEvent
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00003559 File Offset: 0x00001759
		public IMsTscAxEvents_OnChannelReceivedDataEvent(string chanName, string data)
		{
			this.chanName = chanName;
			this.data = data;
		}

		// Token: 0x04000023 RID: 35
		public string chanName;

		// Token: 0x04000024 RID: 36
		public string data;
	}
}
