using System;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000027 RID: 39
	public class ModifySystemSettingsDialog : IDisposable
	{
		// Token: 0x06000242 RID: 578 RVA: 0x00009AC9 File Offset: 0x00007CC9
		public ModifySystemSettingsDialog(IXdeUiFactory factory)
		{
			this.TaskDlg = factory.CreatedLinkEnabledTaskDialog();
			this.RequiresReboot = false;
			this.RequiresElevation = false;
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000243 RID: 579 RVA: 0x00009AEC File Offset: 0x00007CEC
		// (remove) Token: 0x06000244 RID: 580 RVA: 0x00009B24 File Offset: 0x00007D24
		public event EventHandler OnApplyResult;

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009B59 File Offset: 0x00007D59
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00009B61 File Offset: 0x00007D61
		public TaskDialog TaskDlg { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00009B6A File Offset: 0x00007D6A
		// (set) Token: 0x06000248 RID: 584 RVA: 0x00009B72 File Offset: 0x00007D72
		public bool RequiresElevation { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00009B7B File Offset: 0x00007D7B
		// (set) Token: 0x0600024A RID: 586 RVA: 0x00009B83 File Offset: 0x00007D83
		public bool RequiresReboot { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00009B8C File Offset: 0x00007D8C
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00009B94 File Offset: 0x00007D94
		private bool ControlsInitialized { get; set; }

		// Token: 0x0600024D RID: 589 RVA: 0x00009B9D File Offset: 0x00007D9D
		public void ShowDialog()
		{
			if (!this.ControlsInitialized)
			{
				this.InitControls();
			}
			this.TaskDlg.Show();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009BB9 File Offset: 0x00007DB9
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this.TaskDlg != null)
			{
				this.TaskDlg.Dispose();
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009BD4 File Offset: 0x00007DD4
		private void InitControls()
		{
			if (string.IsNullOrEmpty(this.TaskDlg.Caption))
			{
				this.TaskDlg.Caption = Resources.ApplicationName;
			}
			TaskDialogButton taskDialogButton = new TaskDialogButton("buttonClose", Resources.ButtonText_Apply);
			taskDialogButton.Click += this.ButtonApply_Click;
			taskDialogButton.UseElevationIcon = (this.RequiresElevation && !UacSecurity.IsAdmin());
			this.TaskDlg.Controls.Add(taskDialogButton);
			if (this.RequiresReboot)
			{
				TaskDialogButton taskDialogButton2 = new TaskDialogButton("buttonClose", Resources.ButtonText_ApplyReboot);
				taskDialogButton2.Click += this.ButtonApplyReboot_Click;
				taskDialogButton2.UseElevationIcon = (this.RequiresElevation && !UacSecurity.IsAdmin());
				this.TaskDlg.Controls.Add(taskDialogButton2);
				this.TaskDlg.FooterText = Resources.RestartWarningText;
			}
			TaskDialogButton taskDialogButton3 = new TaskDialogButton("buttonCancel", Resources.ButtonText_Cancel);
			taskDialogButton3.Click += this.ButtonCancel_Click;
			this.TaskDlg.Controls.Add(taskDialogButton3);
			this.ControlsInitialized = true;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009CED File Offset: 0x00007EED
		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			this.TaskDlg.Close();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009CFA File Offset: 0x00007EFA
		private void ButtonApplyReboot_Click(object sender, EventArgs e)
		{
			this.ApplySettingChange();
			this.RebootHost(30);
			this.TaskDlg.Close();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009D15 File Offset: 0x00007F15
		private void ButtonApply_Click(object sender, EventArgs e)
		{
			this.ApplySettingChange();
			this.TaskDlg.Close();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009D28 File Offset: 0x00007F28
		private void ApplySettingChange()
		{
			if (this.OnApplyResult != null)
			{
				this.OnApplyResult(this, null);
			}
			if (this.RequiresReboot)
			{
				RegistryHelper.RestartPending = true;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009D50 File Offset: 0x00007F50
		private void RebootHost(int delay = 30)
		{
			string arguments = string.Format("/r /t {0} /d p:4:1", delay);
			Process.Start("shutdown", arguments);
		}
	}
}
