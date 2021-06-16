using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000858 RID: 2136
	public abstract class ConsoleCmdletsBase : PSCmdlet
	{
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x001B6FB8 File Offset: 0x001B51B8
		internal RunspaceConfigForSingleShell Runspace
		{
			get
			{
				return base.Context.RunspaceConfiguration as RunspaceConfigForSingleShell;
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06005236 RID: 21046 RVA: 0x001B6FD7 File Offset: 0x001B51D7
		internal InitialSessionState InitialSessionState
		{
			get
			{
				return base.Context.InitialSessionState;
			}
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x001B6FE4 File Offset: 0x001B51E4
		internal void ThrowError(object targetObject, string errorId, Exception innerException, ErrorCategory category)
		{
			base.ThrowTerminatingError(new ErrorRecord(innerException, errorId, category, targetObject));
		}
	}
}
