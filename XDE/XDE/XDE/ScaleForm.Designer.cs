namespace Microsoft.Xde.Client
{
	// Token: 0x02000029 RID: 41
	public partial class ScaleForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000A764 File Offset: 0x00008964
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A784 File Offset: 0x00008984
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Microsoft.Xde.Client.ScaleForm));
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton3 = new global::System.Windows.Forms.RadioButton();
			this.radioButton4 = new global::System.Windows.Forms.RadioButton();
			this.customUpDown = new global::System.Windows.Forms.NumericUpDown();
			this.buttonOK = new global::System.Windows.Forms.Button();
			this.buttonCancel = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new global::System.Windows.Forms.TableLayoutPanel();
			((global::System.ComponentModel.ISupportInitialize)this.customUpDown).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.radioButton1, "radioButton1");
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.TabStop = true;
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new global::System.EventHandler(this.RadioButtonCheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton2, "radioButton2");
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.TabStop = true;
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new global::System.EventHandler(this.RadioButtonCheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton3, "radioButton3");
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.TabStop = true;
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new global::System.EventHandler(this.RadioButtonCheckedChanged);
			componentResourceManager.ApplyResources(this.radioButton4, "radioButton4");
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.TabStop = true;
			this.radioButton4.UseVisualStyleBackColor = true;
			this.radioButton4.CheckedChanged += new global::System.EventHandler(this.RadioButtonCheckedChanged);
			componentResourceManager.ApplyResources(this.customUpDown, "customUpDown");
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.customUpDown;
			int[] array = new int[4];
			array[0] = 10;
			numericUpDown.Minimum = new decimal(array);
			this.customUpDown.Name = "customUpDown";
			global::System.Windows.Forms.NumericUpDown numericUpDown2 = this.customUpDown;
			int[] array2 = new int[4];
			array2[0] = 10;
			numericUpDown2.Value = new decimal(array2);
			this.customUpDown.ValueChanged += new global::System.EventHandler(this.CustomUpDown_ValueChanged);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new global::System.EventHandler(this.ButtonOK_Click);
			this.buttonCancel.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.customUpDown, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.radioButton4);
			base.Controls.Add(this.radioButton3);
			base.Controls.Add(this.radioButton2);
			base.Controls.Add(this.radioButton1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ScaleForm";
			base.ShowInTaskbar = false;
			((global::System.ComponentModel.ISupportInitialize)this.customUpDown).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000F1 RID: 241
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000F2 RID: 242
		private global::System.Windows.Forms.RadioButton radioButton1;

		// Token: 0x040000F3 RID: 243
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x040000F4 RID: 244
		private global::System.Windows.Forms.RadioButton radioButton3;

		// Token: 0x040000F5 RID: 245
		private global::System.Windows.Forms.RadioButton radioButton4;

		// Token: 0x040000F6 RID: 246
		private global::System.Windows.Forms.NumericUpDown customUpDown;

		// Token: 0x040000F7 RID: 247
		private global::System.Windows.Forms.Button buttonOK;

		// Token: 0x040000F8 RID: 248
		private global::System.Windows.Forms.Button buttonCancel;

		// Token: 0x040000F9 RID: 249
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000FA RID: 250
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
