using System;

namespace Microsoft.Xde.Interface
{
	// Token: 0x02000006 RID: 6
	public enum VirtualMachineEnabledState
	{
		// Token: 0x0400000A RID: 10
		Unknown,
		// Token: 0x0400000B RID: 11
		Enabled = 2,
		// Token: 0x0400000C RID: 12
		Disabled,
		// Token: 0x0400000D RID: 13
		Shutdown,
		// Token: 0x0400000E RID: 14
		Paused = 32768,
		// Token: 0x0400000F RID: 15
		Suspended,
		// Token: 0x04000010 RID: 16
		Starting,
		// Token: 0x04000011 RID: 17
		Snapshotting,
		// Token: 0x04000012 RID: 18
		Saving = 32773,
		// Token: 0x04000013 RID: 19
		Stopping,
		// Token: 0x04000014 RID: 20
		Pausing = 32776,
		// Token: 0x04000015 RID: 21
		Resuming
	}
}
