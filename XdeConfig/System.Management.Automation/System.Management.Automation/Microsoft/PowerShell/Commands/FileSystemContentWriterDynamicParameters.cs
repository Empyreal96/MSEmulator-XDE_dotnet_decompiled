using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000776 RID: 1910
	public class FileSystemContentWriterDynamicParameters : FileSystemContentDynamicParametersBase
	{
		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x001946DF File Offset: 0x001928DF
		// (set) Token: 0x06004C67 RID: 19559 RVA: 0x001946EC File Offset: 0x001928EC
		[Parameter]
		public SwitchParameter NoNewline
		{
			get
			{
				return this.suppressNewline;
			}
			set
			{
				this.suppressNewline = value;
			}
		}

		// Token: 0x040024D7 RID: 9431
		private bool suppressNewline;
	}
}
