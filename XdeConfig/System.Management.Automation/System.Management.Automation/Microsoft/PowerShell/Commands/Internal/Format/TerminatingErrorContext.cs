using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200052B RID: 1323
	internal sealed class TerminatingErrorContext
	{
		// Token: 0x06003744 RID: 14148 RVA: 0x00129BA3 File Offset: 0x00127DA3
		internal TerminatingErrorContext(PSCmdlet command)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this._command = command;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x00129BC0 File Offset: 0x00127DC0
		internal void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			this._command.ThrowTerminatingError(errorRecord);
		}

		// Token: 0x04001C49 RID: 7241
		private PSCmdlet _command;
	}
}
