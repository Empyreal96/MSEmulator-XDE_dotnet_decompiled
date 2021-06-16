using System;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Remoting;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200034A RID: 842
	[Cmdlet("Exit", "PSSession", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135212")]
	public class ExitPSSessionCommand : PSRemotingCmdlet
	{
		// Token: 0x0600295F RID: 10591 RVA: 0x000E7570 File Offset: 0x000E5770
		protected override void ProcessRecord()
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = base.Host as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null)
			{
				base.WriteError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.HostDoesNotSupportPushRunspace)), PSRemotingErrorId.HostDoesNotSupportPushRunspace.ToString(), ErrorCategory.InvalidArgument, null));
				return;
			}
			hostSupportsInteractiveSession.PopRunspace();
		}
	}
}
