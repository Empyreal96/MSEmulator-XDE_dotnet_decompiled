using System;
using System.ComponentModel;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000028 RID: 40
	public class TaskDialogClosingEventArgs : CancelEventArgs
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000582F File Offset: 0x00003A2F
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00005837 File Offset: 0x00003A37
		public TaskDialogResult TaskDialogResult
		{
			get
			{
				return this.taskDialogResult;
			}
			set
			{
				this.taskDialogResult = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00005840 File Offset: 0x00003A40
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00005848 File Offset: 0x00003A48
		public string CustomButton
		{
			get
			{
				return this.customButton;
			}
			set
			{
				this.customButton = value;
			}
		}

		// Token: 0x0400016D RID: 365
		private TaskDialogResult taskDialogResult;

		// Token: 0x0400016E RID: 366
		private string customButton;
	}
}
