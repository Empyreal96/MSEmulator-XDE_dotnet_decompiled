using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000024 RID: 36
	public class IMsTscAxEvents_OnNetworkBandwidthChangedEvent
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x0000366B File Offset: 0x0000186B
		public IMsTscAxEvents_OnNetworkBandwidthChangedEvent(int qualityLevel)
		{
			this.qualityLevel = qualityLevel;
		}

		// Token: 0x0400003C RID: 60
		public int qualityLevel;
	}
}
