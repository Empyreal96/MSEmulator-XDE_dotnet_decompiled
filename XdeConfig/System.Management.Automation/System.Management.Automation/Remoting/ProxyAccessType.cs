using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200034B RID: 843
	public enum ProxyAccessType
	{
		// Token: 0x0400146A RID: 5226
		None,
		// Token: 0x0400146B RID: 5227
		IEConfig,
		// Token: 0x0400146C RID: 5228
		WinHttpConfig,
		// Token: 0x0400146D RID: 5229
		AutoDetect = 4,
		// Token: 0x0400146E RID: 5230
		NoProxyServer = 8
	}
}
