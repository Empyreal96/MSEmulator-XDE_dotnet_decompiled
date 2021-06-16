using System;

namespace System.Management.Automation
{
	// Token: 0x020002D9 RID: 729
	[Flags]
	internal enum RemotingDestination : uint
	{
		// Token: 0x040010EF RID: 4335
		InvalidDestination = 0U,
		// Token: 0x040010F0 RID: 4336
		Client = 1U,
		// Token: 0x040010F1 RID: 4337
		Server = 2U,
		// Token: 0x040010F2 RID: 4338
		Listener = 4U
	}
}
