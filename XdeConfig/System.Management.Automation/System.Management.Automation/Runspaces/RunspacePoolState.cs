using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000244 RID: 580
	public enum RunspacePoolState
	{
		// Token: 0x04000B43 RID: 2883
		BeforeOpen,
		// Token: 0x04000B44 RID: 2884
		Opening,
		// Token: 0x04000B45 RID: 2885
		Opened,
		// Token: 0x04000B46 RID: 2886
		Closed,
		// Token: 0x04000B47 RID: 2887
		Closing,
		// Token: 0x04000B48 RID: 2888
		Broken,
		// Token: 0x04000B49 RID: 2889
		Disconnecting,
		// Token: 0x04000B4A RID: 2890
		Disconnected,
		// Token: 0x04000B4B RID: 2891
		Connecting
	}
}
