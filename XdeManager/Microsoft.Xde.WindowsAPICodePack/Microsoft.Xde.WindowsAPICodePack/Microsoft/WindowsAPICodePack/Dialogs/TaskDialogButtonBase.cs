using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000027 RID: 39
	public abstract class TaskDialogButtonBase : TaskDialogControl
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x000056F0 File Offset: 0x000038F0
		protected TaskDialogButtonBase()
		{
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000056FF File Offset: 0x000038FF
		protected TaskDialogButtonBase(string name, string text) : base(name)
		{
			this.text = text;
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001CA RID: 458 RVA: 0x00005718 File Offset: 0x00003918
		// (remove) Token: 0x060001CB RID: 459 RVA: 0x00005750 File Offset: 0x00003950
		public event EventHandler Click;

		// Token: 0x060001CC RID: 460 RVA: 0x00005785 File Offset: 0x00003985
		internal void RaiseClickEvent()
		{
			if (!this.enabled)
			{
				return;
			}
			if (this.Click != null)
			{
				this.Click(this, EventArgs.Empty);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000057A9 File Offset: 0x000039A9
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000057B1 File Offset: 0x000039B1
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				base.CheckPropertyChangeAllowed("Text");
				this.text = value;
				base.ApplyPropertyChange("Text");
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000057D0 File Offset: 0x000039D0
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x000057D8 File Offset: 0x000039D8
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				base.CheckPropertyChangeAllowed("Enabled");
				this.enabled = value;
				base.ApplyPropertyChange("Enabled");
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x000057F7 File Offset: 0x000039F7
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x000057FF File Offset: 0x000039FF
		public bool Default
		{
			get
			{
				return this.defaultControl;
			}
			set
			{
				base.CheckPropertyChangeAllowed("Default");
				this.defaultControl = value;
				base.ApplyPropertyChange("Default");
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000581E File Offset: 0x00003A1E
		public override string ToString()
		{
			return this.text ?? string.Empty;
		}

		// Token: 0x0400016A RID: 362
		private string text;

		// Token: 0x0400016B RID: 363
		private bool enabled = true;

		// Token: 0x0400016C RID: 364
		private bool defaultControl;
	}
}
