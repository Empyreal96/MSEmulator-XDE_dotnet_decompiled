using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200001E RID: 30
	public class IMsTscAxEvents_OnUserNameAcquiredEvent
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x0000363E File Offset: 0x0000183E
		public IMsTscAxEvents_OnUserNameAcquiredEvent(string bstrUserName)
		{
			this.bstrUserName = bstrUserName;
		}

		// Token: 0x04000039 RID: 57
		public string bstrUserName;
	}
}
