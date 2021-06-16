using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000411 RID: 1041
	[AttributeUsage(AttributeTargets.Class)]
	public class DscResourceAttribute : CmdletMetadataAttribute
	{
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06002E93 RID: 11923 RVA: 0x001001A4 File Offset: 0x000FE3A4
		// (set) Token: 0x06002E94 RID: 11924 RVA: 0x001001AC File Offset: 0x000FE3AC
		public DSCResourceRunAsCredential RunAsCredential { get; set; }
	}
}
