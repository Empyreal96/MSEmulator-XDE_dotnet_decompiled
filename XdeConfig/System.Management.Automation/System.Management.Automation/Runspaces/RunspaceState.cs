using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001EF RID: 495
	public enum RunspaceState
	{
		// Token: 0x040009A7 RID: 2471
		BeforeOpen,
		// Token: 0x040009A8 RID: 2472
		Opening,
		// Token: 0x040009A9 RID: 2473
		Opened,
		// Token: 0x040009AA RID: 2474
		Closed,
		// Token: 0x040009AB RID: 2475
		Closing,
		// Token: 0x040009AC RID: 2476
		Broken,
		// Token: 0x040009AD RID: 2477
		Disconnecting,
		// Token: 0x040009AE RID: 2478
		Disconnected,
		// Token: 0x040009AF RID: 2479
		Connecting
	}
}
