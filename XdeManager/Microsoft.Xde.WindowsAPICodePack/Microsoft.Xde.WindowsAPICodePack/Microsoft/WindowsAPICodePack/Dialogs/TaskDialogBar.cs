using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000025 RID: 37
	public class TaskDialogBar : TaskDialogControl
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00005676 File Offset: 0x00003876
		public TaskDialogBar()
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000567E File Offset: 0x0000387E
		protected TaskDialogBar(string name) : base(name)
		{
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00005687 File Offset: 0x00003887
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000568F File Offset: 0x0000388F
		public TaskDialogProgressBarState State
		{
			get
			{
				return this.state;
			}
			set
			{
				base.CheckPropertyChangeAllowed("State");
				this.state = value;
				base.ApplyPropertyChange("State");
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000056AE File Offset: 0x000038AE
		protected internal virtual void Reset()
		{
			this.state = TaskDialogProgressBarState.Normal;
		}

		// Token: 0x04000167 RID: 359
		private TaskDialogProgressBarState state;
	}
}
