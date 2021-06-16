using System;
using System.Management.Automation;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000475 RID: 1141
	public class RegistryProviderSetItemDynamicParameter
	{
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x00117709 File Offset: 0x00115909
		// (set) Token: 0x060032F0 RID: 13040 RVA: 0x00117711 File Offset: 0x00115911
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public RegistryValueKind Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x04001A8D RID: 6797
		private RegistryValueKind type;
	}
}
