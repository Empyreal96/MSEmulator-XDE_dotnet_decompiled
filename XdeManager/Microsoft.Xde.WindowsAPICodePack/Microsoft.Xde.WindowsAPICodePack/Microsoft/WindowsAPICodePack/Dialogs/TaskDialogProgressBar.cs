using System;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200002E RID: 46
	public class TaskDialogProgressBar : TaskDialogBar
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x00005936 File Offset: 0x00003B36
		public TaskDialogProgressBar()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00005946 File Offset: 0x00003B46
		public TaskDialogProgressBar(string name) : base(name)
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00005957 File Offset: 0x00003B57
		public TaskDialogProgressBar(int minimum, int maximum, int value)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
			this.Value = value;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000597C File Offset: 0x00003B7C
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00005984 File Offset: 0x00003B84
		public int Minimum
		{
			get
			{
				return this._minimum;
			}
			set
			{
				base.CheckPropertyChangeAllowed("Minimum");
				if (value < 0)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarMinValueGreaterThanZero, "value");
				}
				if (value >= this.Maximum)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarMinValueLessThanMax, "value");
				}
				this._minimum = value;
				base.ApplyPropertyChange("Minimum");
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000059DB File Offset: 0x00003BDB
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000059E3 File Offset: 0x00003BE3
		public int Maximum
		{
			get
			{
				return this._maximum;
			}
			set
			{
				base.CheckPropertyChangeAllowed("Maximum");
				if (value < this.Minimum)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarMaxValueGreaterThanMin, "value");
				}
				this._maximum = value;
				base.ApplyPropertyChange("Maximum");
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00005A1B File Offset: 0x00003C1B
		// (set) Token: 0x060001EF RID: 495 RVA: 0x00005A24 File Offset: 0x00003C24
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				base.CheckPropertyChangeAllowed("Value");
				if (value < this.Minimum || value > this.Maximum)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange, "value");
				}
				this._value = value;
				base.ApplyPropertyChange("Value");
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00005A70 File Offset: 0x00003C70
		internal bool HasValidValues
		{
			get
			{
				return this._minimum <= this._value && this._value <= this._maximum;
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00005A93 File Offset: 0x00003C93
		protected internal override void Reset()
		{
			base.Reset();
			this._value = this._minimum;
		}

		// Token: 0x04000179 RID: 377
		private int _minimum;

		// Token: 0x0400017A RID: 378
		private int _value;

		// Token: 0x0400017B RID: 379
		private int _maximum = 100;
	}
}
