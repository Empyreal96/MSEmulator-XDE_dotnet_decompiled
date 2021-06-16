using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000026 RID: 38
	public class TaskDialogButton : TaskDialogButtonBase
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x000056B7 File Offset: 0x000038B7
		public TaskDialogButton()
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000056BF File Offset: 0x000038BF
		public TaskDialogButton(string name, string text) : base(name, text)
		{
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x000056C9 File Offset: 0x000038C9
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x000056D1 File Offset: 0x000038D1
		public bool UseElevationIcon
		{
			get
			{
				return this.useElevationIcon;
			}
			set
			{
				base.CheckPropertyChangeAllowed("ShowElevationIcon");
				this.useElevationIcon = value;
				base.ApplyPropertyChange("ShowElevationIcon");
			}
		}

		// Token: 0x04000168 RID: 360
		private bool useElevationIcon;
	}
}
