using System;
using System.Security.AccessControl;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000035 RID: 53
	public sealed class NewFileOptions
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000B510 File Offset: 0x00009710
		public NewFileOptions()
		{
			this.Compressed = null;
			this.CreateShortNames = null;
			this.SecurityDescriptor = null;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000B548 File Offset: 0x00009748
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000B550 File Offset: 0x00009750
		public bool? Compressed { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000B559 File Offset: 0x00009759
		// (set) Token: 0x0600021B RID: 539 RVA: 0x0000B561 File Offset: 0x00009761
		public bool? CreateShortNames { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000B56A File Offset: 0x0000976A
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000B572 File Offset: 0x00009772
		public RawSecurityDescriptor SecurityDescriptor { get; set; }
	}
}
