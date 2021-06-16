using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000022 RID: 34
	public class IMsTscAxEvents_OnServiceMessageReceivedEvent
	{
		// Token: 0x060000CC RID: 204 RVA: 0x0000365C File Offset: 0x0000185C
		public IMsTscAxEvents_OnServiceMessageReceivedEvent(string serviceMessage)
		{
			this.serviceMessage = serviceMessage;
		}

		// Token: 0x0400003B RID: 59
		public string serviceMessage;
	}
}
