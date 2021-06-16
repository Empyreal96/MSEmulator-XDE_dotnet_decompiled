using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000010 RID: 16
	public class IMsTscAxEvents_OnReceivedTSPublicKeyEvent
	{
		// Token: 0x0600009F RID: 159 RVA: 0x000035AB File Offset: 0x000017AB
		public IMsTscAxEvents_OnReceivedTSPublicKeyEvent(string publicKey)
		{
			this.publicKey = publicKey;
		}

		// Token: 0x0400002A RID: 42
		public string publicKey;

		// Token: 0x0400002B RID: 43
		public bool pfContinueLogon;
	}
}
