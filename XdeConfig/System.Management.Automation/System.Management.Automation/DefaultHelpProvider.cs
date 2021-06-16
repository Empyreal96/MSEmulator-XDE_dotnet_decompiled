using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020001BE RID: 446
	internal class DefaultHelpProvider : HelpFileHelpProvider
	{
		// Token: 0x060014B4 RID: 5300 RVA: 0x00080DA3 File Offset: 0x0007EFA3
		internal DefaultHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00080DAC File Offset: 0x0007EFAC
		internal override string Name
		{
			get
			{
				return "Default Help Provider";
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x00080DB3 File Offset: 0x0007EFB3
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.DefaultHelp;
			}
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00080DBC File Offset: 0x0007EFBC
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			HelpRequest helpRequest2 = helpRequest.Clone();
			helpRequest2.Target = "default";
			return base.ExactMatchHelp(helpRequest2);
		}
	}
}
