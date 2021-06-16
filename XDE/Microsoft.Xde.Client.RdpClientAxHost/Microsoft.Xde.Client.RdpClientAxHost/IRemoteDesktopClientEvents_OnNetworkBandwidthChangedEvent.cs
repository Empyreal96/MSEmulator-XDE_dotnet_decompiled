using System;

namespace AxMicrosoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000056 RID: 86
	public class IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEvent
	{
		// Token: 0x06000EA6 RID: 3750 RVA: 0x00026F87 File Offset: 0x00025187
		public IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEvent(int qualityLevel, string bandwidth, string latency)
		{
			this.qualityLevel = qualityLevel;
			this.bandwidth = bandwidth;
			this.latency = latency;
		}

		// Token: 0x04000301 RID: 769
		public int qualityLevel;

		// Token: 0x04000302 RID: 770
		public string bandwidth;

		// Token: 0x04000303 RID: 771
		public string latency;
	}
}
