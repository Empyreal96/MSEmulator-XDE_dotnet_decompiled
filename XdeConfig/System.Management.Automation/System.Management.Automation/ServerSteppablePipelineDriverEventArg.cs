using System;

namespace System.Management.Automation
{
	// Token: 0x02000303 RID: 771
	internal class ServerSteppablePipelineDriverEventArg : EventArgs
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x000CCA58 File Offset: 0x000CAC58
		internal ServerSteppablePipelineDriverEventArg(ServerSteppablePipelineDriver driver)
		{
			this.SteppableDriver = driver;
		}

		// Token: 0x040011EE RID: 4590
		internal ServerSteppablePipelineDriver SteppableDriver;
	}
}
