using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000C3 RID: 195
	[Serializable]
	internal class RequiredModuleInfo
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0003FE7C File Offset: 0x0003E07C
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0003FE84 File Offset: 0x0003E084
		internal string Name { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0003FE8D File Offset: 0x0003E08D
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0003FE95 File Offset: 0x0003E095
		internal List<string> CommandsToPostFilter { get; set; }
	}
}
