using System;
using System.Net;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200003F RID: 63
	public struct NetworkIPAddress
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00004E6A File Offset: 0x0000306A
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00004E72 File Offset: 0x00003072
		public IPAddress IPAddress { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00004E7B File Offset: 0x0000307B
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00004E83 File Offset: 0x00003083
		public IPAddress IPSubnet { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004E8C File Offset: 0x0000308C
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00004E94 File Offset: 0x00003094
		public IpDadState DadState { get; set; }
	}
}
