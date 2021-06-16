using System;
using System.Security.Principal;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200003F RID: 63
	public sealed class NtfsFormatOptions
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00011D23 File Offset: 0x0000FF23
		// (set) Token: 0x0600031B RID: 795 RVA: 0x00011D2B File Offset: 0x0000FF2B
		public byte[] BootCode { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00011D34 File Offset: 0x0000FF34
		// (set) Token: 0x0600031D RID: 797 RVA: 0x00011D3C File Offset: 0x0000FF3C
		public SecurityIdentifier ComputerAccount { get; set; }
	}
}
