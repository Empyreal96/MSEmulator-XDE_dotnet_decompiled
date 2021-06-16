using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001F3 RID: 499
	public enum RunspaceAvailability
	{
		// Token: 0x040009B9 RID: 2489
		None,
		// Token: 0x040009BA RID: 2490
		Available,
		// Token: 0x040009BB RID: 2491
		AvailableForNestedCommand,
		// Token: 0x040009BC RID: 2492
		Busy,
		// Token: 0x040009BD RID: 2493
		RemoteDebug
	}
}
