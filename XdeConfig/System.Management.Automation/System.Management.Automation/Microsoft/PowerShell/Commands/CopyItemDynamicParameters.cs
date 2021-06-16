using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200076F RID: 1903
	internal sealed class CopyItemDynamicParameters
	{
		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x001943CE File Offset: 0x001925CE
		// (set) Token: 0x06004C46 RID: 19526 RVA: 0x001943D6 File Offset: 0x001925D6
		[Parameter]
		[ValidateNotNullOrEmpty]
		public PSSession FromSession { get; set; }

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x001943DF File Offset: 0x001925DF
		// (set) Token: 0x06004C48 RID: 19528 RVA: 0x001943E7 File Offset: 0x001925E7
		[ValidateNotNullOrEmpty]
		[Parameter]
		public PSSession ToSession { get; set; }
	}
}
