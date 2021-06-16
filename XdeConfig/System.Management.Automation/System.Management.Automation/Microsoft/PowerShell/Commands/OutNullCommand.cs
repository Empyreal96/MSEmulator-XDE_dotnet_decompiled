using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000531 RID: 1329
	[Cmdlet("Out", "Null", SupportsShouldProcess = false, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113366", RemotingCapability = RemotingCapability.None)]
	public class OutNullCommand : PSCmdlet
	{
		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x0012A30A File Offset: 0x0012850A
		// (set) Token: 0x06003760 RID: 14176 RVA: 0x0012A301 File Offset: 0x00128501
		[Parameter(ValueFromPipeline = true)]
		public PSObject InputObject
		{
			get
			{
				return this.inputObject;
			}
			set
			{
				this.inputObject = value;
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x0012A312 File Offset: 0x00128512
		protected override void ProcessRecord()
		{
		}

		// Token: 0x04001C62 RID: 7266
		private PSObject inputObject = AutomationNull.Value;
	}
}
