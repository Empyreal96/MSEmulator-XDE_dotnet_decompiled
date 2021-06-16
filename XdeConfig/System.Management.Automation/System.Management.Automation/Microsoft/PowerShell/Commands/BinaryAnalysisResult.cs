using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000AB RID: 171
	internal class BinaryAnalysisResult
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00035200 File Offset: 0x00033400
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x00035208 File Offset: 0x00033408
		internal List<string> DetectedCmdlets { get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00035211 File Offset: 0x00033411
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x00035219 File Offset: 0x00033419
		internal List<Tuple<string, string>> DetectedAliases { get; set; }
	}
}
