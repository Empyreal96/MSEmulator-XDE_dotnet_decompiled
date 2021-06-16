using System;
using System.Management.Automation;
using System.Management.Automation.Host;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000533 RID: 1331
	[Cmdlet("Out", "Host", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113365", RemotingCapability = RemotingCapability.None)]
	public class OutHostCommand : FrontEndCommandBase
	{
		// Token: 0x0600376A RID: 14186 RVA: 0x0012A53C File Offset: 0x0012873C
		public OutHostCommand()
		{
			this.implementation = new OutputManagerInner();
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x0012A54F File Offset: 0x0012874F
		// (set) Token: 0x0600376C RID: 14188 RVA: 0x0012A55C File Offset: 0x0012875C
		[Parameter]
		public SwitchParameter Paging
		{
			get
			{
				return this.paging;
			}
			set
			{
				this.paging = value;
			}
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x0012A56C File Offset: 0x0012876C
		protected override void BeginProcessing()
		{
			PSHostUserInterface ui = base.Host.UI;
			ConsoleLineOutput lineOutput = new ConsoleLineOutput(ui, this.paging, new TerminatingErrorContext(this));
			((OutputManagerInner)this.implementation).LineOutput = lineOutput;
			base.BeginProcessing();
		}

		// Token: 0x04001C66 RID: 7270
		private bool paging;
	}
}
