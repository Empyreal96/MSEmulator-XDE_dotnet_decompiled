using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000069 RID: 105
	public enum VGPUStatus
	{
		// Token: 0x04000172 RID: 370
		Unknown,
		// Token: 0x04000173 RID: 371
		Enabled,
		// Token: 0x04000174 RID: 372
		Running,
		// Token: 0x04000175 RID: 373
		DisabledByCommandLine,
		// Token: 0x04000176 RID: 374
		DisabledByRegistry,
		// Token: 0x04000177 RID: 375
		DisabledBySku,
		// Token: 0x04000178 RID: 376
		DisabledByUser,
		// Token: 0x04000179 RID: 377
		NoCompatibleHostHardwareFound,
		// Token: 0x0400017A RID: 378
		VMFailedToStartWithVGPU,
		// Token: 0x0400017B RID: 379
		VGPUNotFoundOnGuest,
		// Token: 0x0400017C RID: 380
		VGPUDriverInFailedState,
		// Token: 0x0400017D RID: 381
		WdpQueryFailed
	}
}
