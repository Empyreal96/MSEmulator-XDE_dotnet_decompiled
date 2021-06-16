using System;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000276 RID: 630
	internal abstract class ExecutionCmdletHelper : IThrottleOperation
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x000AB09C File Offset: 0x000A929C
		internal Pipeline Pipeline
		{
			get
			{
				return this.pipeline;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x000AB0A4 File Offset: 0x000A92A4
		internal Exception InternalException
		{
			get
			{
				return this.internalException;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x000AB0B5 File Offset: 0x000A92B5
		// (set) Token: 0x06001DAA RID: 7594 RVA: 0x000AB0AC File Offset: 0x000A92AC
		internal Runspace PipelineRunspace { get; set; }

		// Token: 0x04000D24 RID: 3364
		protected Pipeline pipeline;

		// Token: 0x04000D25 RID: 3365
		protected Exception internalException;
	}
}
