using System;
using System.Collections.Generic;

namespace System.Management.Automation.Host
{
	// Token: 0x02000226 RID: 550
	internal class TranscriptionData
	{
		// Token: 0x060019E6 RID: 6630 RVA: 0x0009AE6E File Offset: 0x0009906E
		internal TranscriptionData()
		{
			this.Transcripts = new List<TranscriptionOption>();
			this.SystemTranscript = null;
			this.CommandBeingIgnored = null;
			this.IsHelperCommand = false;
			this.PromptText = "PS>";
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x0009AEA1 File Offset: 0x000990A1
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x0009AEA9 File Offset: 0x000990A9
		internal List<TranscriptionOption> Transcripts { get; private set; }

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x0009AEB2 File Offset: 0x000990B2
		// (set) Token: 0x060019EA RID: 6634 RVA: 0x0009AEBA File Offset: 0x000990BA
		internal TranscriptionOption SystemTranscript { get; set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x0009AEC3 File Offset: 0x000990C3
		// (set) Token: 0x060019EC RID: 6636 RVA: 0x0009AECB File Offset: 0x000990CB
		internal string CommandBeingIgnored { get; set; }

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x0009AED4 File Offset: 0x000990D4
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x0009AEDC File Offset: 0x000990DC
		internal bool IsHelperCommand { get; set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0009AEE5 File Offset: 0x000990E5
		// (set) Token: 0x060019F0 RID: 6640 RVA: 0x0009AEED File Offset: 0x000990ED
		internal string PromptText { get; set; }
	}
}
