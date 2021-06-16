using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000029 RID: 41
	public partial class ScaleForm : Form
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000A628 File Offset: 0x00008828
		public ScaleForm()
		{
			this.InitializeComponent();
			for (Control nextControl = this.radioButton1; nextControl != null; nextControl = base.GetNextControl(nextControl, true))
			{
				RadioButton radioButton = nextControl as RadioButton;
				if (radioButton != null)
				{
					this.radioButtons.Add(radioButton);
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000A677 File Offset: 0x00008877
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000A67F File Offset: 0x0000887F
		public int DisplayScale { get; set; }

		// Token: 0x060002BD RID: 701 RVA: 0x0000A688 File Offset: 0x00008888
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.InitControls();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000A697 File Offset: 0x00008897
		private void InitControls()
		{
			this.customUpDown.Value = this.DisplayScale;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A6B0 File Offset: 0x000088B0
		private void CheckCorrectRadioButton()
		{
			int num = (int)this.customUpDown.Value;
			for (int i = 0; i < this.radioButtons.Count; i++)
			{
				bool @checked = num == ScaleForm.DefaultScales[i];
				this.radioButtons[i].Checked = @checked;
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A701 File Offset: 0x00008901
		private void RadioButtonCheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked)
			{
				this.customUpDown.Value = ScaleForm.DefaultScales[this.radioButtons.IndexOf((RadioButton)sender)];
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A737 File Offset: 0x00008937
		private void ButtonOK_Click(object sender, EventArgs e)
		{
			this.DisplayScale = (int)this.customUpDown.Value;
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A75C File Offset: 0x0000895C
		private void CustomUpDown_ValueChanged(object sender, EventArgs e)
		{
			this.CheckCorrectRadioButton();
		}

		// Token: 0x040000EE RID: 238
		private static readonly int[] DefaultScales = new int[]
		{
			33,
			50,
			66,
			100
		};

		// Token: 0x040000EF RID: 239
		private List<RadioButton> radioButtons = new List<RadioButton>();
	}
}
