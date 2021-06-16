using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200000A RID: 10
	public class IMsTscAxEvents_OnWarningEvent
	{
		// Token: 0x06000090 RID: 144 RVA: 0x0000357E File Offset: 0x0000177E
		public IMsTscAxEvents_OnWarningEvent(int warningCode)
		{
			this.warningCode = warningCode;
		}

		// Token: 0x04000026 RID: 38
		public int warningCode;
	}
}
