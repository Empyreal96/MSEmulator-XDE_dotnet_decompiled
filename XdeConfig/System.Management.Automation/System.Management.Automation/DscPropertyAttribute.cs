using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000412 RID: 1042
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DscPropertyAttribute : CmdletMetadataAttribute
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06002E96 RID: 11926 RVA: 0x001001BD File Offset: 0x000FE3BD
		// (set) Token: 0x06002E97 RID: 11927 RVA: 0x001001C5 File Offset: 0x000FE3C5
		public bool Key { get; set; }

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06002E98 RID: 11928 RVA: 0x001001CE File Offset: 0x000FE3CE
		// (set) Token: 0x06002E99 RID: 11929 RVA: 0x001001D6 File Offset: 0x000FE3D6
		public bool Mandatory { get; set; }

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06002E9A RID: 11930 RVA: 0x001001DF File Offset: 0x000FE3DF
		// (set) Token: 0x06002E9B RID: 11931 RVA: 0x001001E7 File Offset: 0x000FE3E7
		public bool NotConfigurable { get; set; }
	}
}
