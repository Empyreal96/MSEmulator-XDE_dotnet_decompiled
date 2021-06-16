using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200041D RID: 1053
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class PSDefaultValueAttribute : ParsingBaseAttribute
	{
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x00100576 File Offset: 0x000FE776
		// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x0010057E File Offset: 0x000FE77E
		public object Value { get; set; }

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x00100587 File Offset: 0x000FE787
		// (set) Token: 0x06002ED9 RID: 11993 RVA: 0x0010058F File Offset: 0x000FE78F
		public string Help { get; set; }
	}
}
