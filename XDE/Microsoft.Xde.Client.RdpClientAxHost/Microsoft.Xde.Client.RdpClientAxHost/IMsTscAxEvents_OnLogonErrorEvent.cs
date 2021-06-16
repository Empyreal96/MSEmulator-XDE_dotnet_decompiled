using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200001A RID: 26
	public class IMsTscAxEvents_OnLogonErrorEvent
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00003620 File Offset: 0x00001820
		public IMsTscAxEvents_OnLogonErrorEvent(int lError)
		{
			this.lError = lError;
		}

		// Token: 0x04000037 RID: 55
		public int lError;
	}
}
