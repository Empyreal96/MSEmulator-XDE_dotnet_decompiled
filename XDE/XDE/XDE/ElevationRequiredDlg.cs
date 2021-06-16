using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000026 RID: 38
	public sealed class ElevationRequiredDlg : IDisposable
	{
		// Token: 0x06000232 RID: 562 RVA: 0x000098E4 File Offset: 0x00007AE4
		public ElevationRequiredDlg(IXdeUiFactory factory)
		{
			this.TaskDlg = factory.CreatedLinkEnabledTaskDialog();
			if (factory.Sku != null)
			{
				this.TaskDlg.Caption = factory.Sku.Branding.DisplayName;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000991B File Offset: 0x00007B1B
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00009923 File Offset: 0x00007B23
		public TaskDialog TaskDlg { get; private set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000992C File Offset: 0x00007B2C
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00009934 File Offset: 0x00007B34
		public string RestartElevatedArgs { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000993D File Offset: 0x00007B3D
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00009945 File Offset: 0x00007B45
		public ElevationRequiredDlg.RetryXdeAsElevatedCallbackType RetryXdeAsElevatedCallback { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000994E File Offset: 0x00007B4E
		public bool ShouldRetry
		{
			get
			{
				return this.shouldRetry;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009956 File Offset: 0x00007B56
		public void ShowDialog(bool enableRestart)
		{
			this.InitControls();
			this.TaskDlg.Show();
			if (this.shouldRetry)
			{
				this.RetryXdeAsElevated(enableRestart);
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009979 File Offset: 0x00007B79
		public void Dispose()
		{
			this.TaskDlg.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000998C File Offset: 0x00007B8C
		private void InitControls()
		{
			if (string.IsNullOrEmpty(this.TaskDlg.Text))
			{
				this.TaskDlg.Text = Resources.RetryRunningAsAdmin;
			}
			this.AddRetryButton();
			if (string.IsNullOrEmpty(this.TaskDlg.InstructionText))
			{
				this.TaskDlg.InstructionText = Resources.CouldntAccessHyperVApi;
			}
			this.TaskDlg.Icon = TaskDialogStandardIcon.Information;
			this.AddCloseButton();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000099FC File Offset: 0x00007BFC
		private void AddCloseButton()
		{
			TaskDialogButton taskDialogButton = new TaskDialogButton("buttonClose", Resources.Close);
			taskDialogButton.Click += this.ButtonClose_Click;
			this.TaskDlg.Controls.Add(taskDialogButton);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00009A3C File Offset: 0x00007C3C
		private void AddRetryButton()
		{
			TaskDialogButton taskDialogButton = new TaskDialogButton("buttonRetry", Resources.Retry);
			taskDialogButton.UseElevationIcon = true;
			taskDialogButton.Click += this.ButtonRetry_Click;
			this.TaskDlg.Controls.Add(taskDialogButton);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009A83 File Offset: 0x00007C83
		private void ButtonClose_Click(object sender, EventArgs e)
		{
			this.TaskDlg.Close();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009A90 File Offset: 0x00007C90
		private void ButtonRetry_Click(object sender, EventArgs e)
		{
			this.shouldRetry = true;
			this.TaskDlg.Close();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009AA4 File Offset: 0x00007CA4
		private void RetryXdeAsElevated(bool enableRestart)
		{
			if (this.RetryXdeAsElevatedCallback != null)
			{
				this.RetryXdeAsElevatedCallback(this);
			}
			if (enableRestart)
			{
				UacSecurity.RestartElevated(this.RestartElevatedArgs, false);
			}
		}

		// Token: 0x040000E3 RID: 227
		private bool shouldRetry;

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x0600049A RID: 1178
		public delegate void RetryXdeAsElevatedCallbackType(ElevationRequiredDlg dlg);
	}
}
