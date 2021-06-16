using System;
using System.Runtime.Serialization;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000078 RID: 120
	[DataContract]
	public class XdeDisplayMessageInfo
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00007ACF File Offset: 0x00005CCF
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x00007AD7 File Offset: 0x00005CD7
		[DataMember]
		public string Instruction { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00007AE0 File Offset: 0x00005CE0
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x00007AE8 File Offset: 0x00005CE8
		[DataMember]
		public TaskDialogStandardButtons Buttons { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00007AF1 File Offset: 0x00005CF1
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x00007AF9 File Offset: 0x00005CF9
		[DataMember]
		public TaskDialogStandardIcon Icon { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00007B02 File Offset: 0x00005D02
		// (set) Token: 0x060002DA RID: 730 RVA: 0x00007B0A File Offset: 0x00005D0A
		[DataMember]
		public string Text { get; set; }
	}
}
