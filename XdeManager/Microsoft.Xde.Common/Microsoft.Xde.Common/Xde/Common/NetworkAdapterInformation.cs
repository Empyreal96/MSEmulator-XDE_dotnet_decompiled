using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000040 RID: 64
	public struct NetworkAdapterInformation
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004E9D File Offset: 0x0000309D
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00004EA5 File Offset: 0x000030A5
		public string MacAddress { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00004EAE File Offset: 0x000030AE
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00004EB6 File Offset: 0x000030B6
		public NetworkIPAddress[] IPAddresses { get; set; }
	}
}
