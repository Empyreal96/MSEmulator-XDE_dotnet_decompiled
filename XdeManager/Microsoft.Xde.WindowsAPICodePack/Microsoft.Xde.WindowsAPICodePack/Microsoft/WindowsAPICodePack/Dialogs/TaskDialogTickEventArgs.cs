using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000035 RID: 53
	public class TaskDialogTickEventArgs : EventArgs
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x00005AA7 File Offset: 0x00003CA7
		public TaskDialogTickEventArgs(int ticks)
		{
			this.Ticks = ticks;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00005AB6 File Offset: 0x00003CB6
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00005ABE File Offset: 0x00003CBE
		public int Ticks { get; private set; }
	}
}
