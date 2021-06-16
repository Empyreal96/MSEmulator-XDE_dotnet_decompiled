using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000779 RID: 1913
	public class FileSystemProviderGetItemDynamicParameters
	{
		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06004C76 RID: 19574 RVA: 0x00194795 File Offset: 0x00192995
		// (set) Token: 0x06004C77 RID: 19575 RVA: 0x0019479D File Offset: 0x0019299D
		[ValidateNotNullOrEmpty]
		[Parameter]
		public string[] Stream { get; set; }
	}
}
