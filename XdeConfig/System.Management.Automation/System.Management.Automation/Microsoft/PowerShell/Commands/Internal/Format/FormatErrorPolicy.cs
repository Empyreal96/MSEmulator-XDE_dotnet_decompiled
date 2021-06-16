using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000922 RID: 2338
	internal sealed class FormatErrorPolicy
	{
		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x0600578A RID: 22410 RVA: 0x001C90E1 File Offset: 0x001C72E1
		// (set) Token: 0x06005789 RID: 22409 RVA: 0x001C90C6 File Offset: 0x001C72C6
		internal bool ShowErrorsAsMessages
		{
			get
			{
				return this._showErrorsAsMessages != null && this._showErrorsAsMessages.Value;
			}
			set
			{
				if (this._showErrorsAsMessages == null)
				{
					this._showErrorsAsMessages = new bool?(value);
				}
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x0600578C RID: 22412 RVA: 0x001C9118 File Offset: 0x001C7318
		// (set) Token: 0x0600578B RID: 22411 RVA: 0x001C90FD File Offset: 0x001C72FD
		internal bool ShowErrorsInFormattedOutput
		{
			get
			{
				return this._showErrorsInFormattedOutput != null && this._showErrorsInFormattedOutput.Value;
			}
			set
			{
				if (this._showErrorsInFormattedOutput == null)
				{
					this._showErrorsInFormattedOutput = new bool?(value);
				}
			}
		}

		// Token: 0x04002EAA RID: 11946
		private bool? _showErrorsAsMessages;

		// Token: 0x04002EAB RID: 11947
		private bool? _showErrorsInFormattedOutput = null;

		// Token: 0x04002EAC RID: 11948
		internal string errorStringInFormattedOutput = "#ERR";

		// Token: 0x04002EAD RID: 11949
		internal string formatErrorStringInFormattedOutput = "#FMTERR";
	}
}
