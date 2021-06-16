using System;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000355 RID: 853
	[Cmdlet("Exit", "PSHostProcess", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=403737")]
	public sealed class ExitPSHostProcessCommand : PSCmdlet
	{
		// Token: 0x06002A7C RID: 10876 RVA: 0x000EAE30 File Offset: 0x000E9030
		protected override void ProcessRecord()
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = base.Host as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null)
			{
				base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.HostDoesNotSupportIASession), "ExitPSHostProcessHostDoesNotSupportIASession", ErrorCategory.InvalidArgument, null));
				return;
			}
			hostSupportsInteractiveSession.PopRunspace();
		}
	}
}
