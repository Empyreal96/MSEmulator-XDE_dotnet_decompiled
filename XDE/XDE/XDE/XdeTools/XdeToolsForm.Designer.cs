namespace Microsoft.Xde.Client.XdeTools
{
	// Token: 0x02000030 RID: 48
	public partial class XdeToolsForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000423 RID: 1059 RVA: 0x0001069A File Offset: 0x0000E89A
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000106BC File Offset: 0x0000E8BC
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new global::System.Drawing.Size(750, 769);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Margin = new global::System.Windows.Forms.Padding(4, 5, 4, 5);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "XdeToolsForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
			this.Text = global::Microsoft.Xde.Client.Resources.ToolsWindowTitle;
			base.ResumeLayout(false);
		}

		// Token: 0x04000179 RID: 377
		private global::System.ComponentModel.IContainer components;
	}
}
