using System;
using System.Globalization;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000029 RID: 41
	public class TaskDialogCommandLink : TaskDialogButton
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x00005859 File Offset: 0x00003A59
		public TaskDialogCommandLink()
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00005861 File Offset: 0x00003A61
		public TaskDialogCommandLink(string name, string text) : base(name, text)
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000586B File Offset: 0x00003A6B
		public TaskDialogCommandLink(string name, string text, string instruction) : base(name, text)
		{
			this.instruction = instruction;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000587C File Offset: 0x00003A7C
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00005884 File Offset: 0x00003A84
		public string Instruction
		{
			get
			{
				return this.instruction;
			}
			set
			{
				this.instruction = value;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00005890 File Offset: 0x00003A90
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", base.Text ?? string.Empty, (!string.IsNullOrEmpty(base.Text) && !string.IsNullOrEmpty(this.instruction)) ? Environment.NewLine : string.Empty, this.instruction ?? string.Empty);
		}

		// Token: 0x0400016F RID: 367
		private string instruction;
	}
}
