using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000016 RID: 22
	public class IMsTscAxEvents_OnRemoteProgramDisplayedEvent
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000035ED File Offset: 0x000017ED
		public IMsTscAxEvents_OnRemoteProgramDisplayedEvent(bool vbDisplayed, uint uDisplayInformation)
		{
			this.vbDisplayed = vbDisplayed;
			this.uDisplayInformation = uDisplayInformation;
		}

		// Token: 0x04000032 RID: 50
		public bool vbDisplayed;

		// Token: 0x04000033 RID: 51
		public uint uDisplayInformation;
	}
}
