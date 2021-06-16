using System;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000066 RID: 102
	public class TaskDialogArgs : EventArgs
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000530B File Offset: 0x0000350B
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00005313 File Offset: 0x00003513
		public string Text { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000531C File Offset: 0x0000351C
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00005324 File Offset: 0x00003524
		public string Instruction { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000532D File Offset: 0x0000352D
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00005335 File Offset: 0x00003535
		public TaskDialogStandardButtons Buttons { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000533E File Offset: 0x0000353E
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00005346 File Offset: 0x00003546
		public TaskDialogStandardIcon Icon { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000534F File Offset: 0x0000354F
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00005357 File Offset: 0x00003557
		public bool CancelDialog { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00005360 File Offset: 0x00003560
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00005368 File Offset: 0x00003568
		public TaskDialogResult Result { get; set; }
	}
}
