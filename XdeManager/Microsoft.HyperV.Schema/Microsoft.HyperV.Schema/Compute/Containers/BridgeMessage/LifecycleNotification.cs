using System;
using System.CodeDom.Compiler;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001AA RID: 426
	[GeneratedCode("MarsComp", "")]
	public enum LifecycleNotification
	{
		// Token: 0x04000977 RID: 2423
		None,
		// Token: 0x04000978 RID: 2424
		ColdStart,
		// Token: 0x04000979 RID: 2425
		RestoreStart,
		// Token: 0x0400097A RID: 2426
		Pause,
		// Token: 0x0400097B RID: 2427
		Resume,
		// Token: 0x0400097C RID: 2428
		Shutdown
	}
}
