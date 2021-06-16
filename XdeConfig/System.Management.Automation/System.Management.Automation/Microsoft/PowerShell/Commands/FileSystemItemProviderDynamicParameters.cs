using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000778 RID: 1912
	public class FileSystemItemProviderDynamicParameters
	{
		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x0019476B File Offset: 0x0019296B
		// (set) Token: 0x06004C72 RID: 19570 RVA: 0x00194773 File Offset: 0x00192973
		[Parameter]
		public DateTime? OlderThan { get; set; }

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x0019477C File Offset: 0x0019297C
		// (set) Token: 0x06004C74 RID: 19572 RVA: 0x00194784 File Offset: 0x00192984
		[Parameter]
		public DateTime? NewerThan { get; set; }
	}
}
