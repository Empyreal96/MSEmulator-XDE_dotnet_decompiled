using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200077A RID: 1914
	public class FileSystemProviderRemoveItemDynamicParameters
	{
		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06004C79 RID: 19577 RVA: 0x001947AE File Offset: 0x001929AE
		// (set) Token: 0x06004C7A RID: 19578 RVA: 0x001947B6 File Offset: 0x001929B6
		[ValidateNotNullOrEmpty]
		[Parameter]
		public string[] Stream { get; set; }
	}
}
