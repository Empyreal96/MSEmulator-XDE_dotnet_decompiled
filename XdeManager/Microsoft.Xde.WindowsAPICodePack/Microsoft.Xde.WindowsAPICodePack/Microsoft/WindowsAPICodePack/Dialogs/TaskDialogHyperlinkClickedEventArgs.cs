using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200002D RID: 45
	public class TaskDialogHyperlinkClickedEventArgs : EventArgs
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x00005916 File Offset: 0x00003B16
		public TaskDialogHyperlinkClickedEventArgs(string linkText)
		{
			this.LinkText = linkText;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00005925 File Offset: 0x00003B25
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000592D File Offset: 0x00003B2D
		public string LinkText { get; set; }
	}
}
