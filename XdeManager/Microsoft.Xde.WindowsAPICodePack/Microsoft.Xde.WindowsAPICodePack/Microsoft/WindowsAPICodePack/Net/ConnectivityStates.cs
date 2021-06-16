using System;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000017 RID: 23
	[Flags]
	public enum ConnectivityStates
	{
		// Token: 0x04000109 RID: 265
		None = 0,
		// Token: 0x0400010A RID: 266
		IPv4Internet = 64,
		// Token: 0x0400010B RID: 267
		IPv4LocalNetwork = 32,
		// Token: 0x0400010C RID: 268
		IPv4NoTraffic = 1,
		// Token: 0x0400010D RID: 269
		IPv4Subnet = 16,
		// Token: 0x0400010E RID: 270
		IPv6Internet = 1024,
		// Token: 0x0400010F RID: 271
		IPv6LocalNetwork = 512,
		// Token: 0x04000110 RID: 272
		IPv6NoTraffic = 2,
		// Token: 0x04000111 RID: 273
		IPv6Subnet = 256
	}
}
