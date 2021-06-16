using System;
using System.CodeDom.Compiler;

namespace HCS.Compute.System
{
	// Token: 0x0200019B RID: 411
	[GeneratedCode("MarsComp", "")]
	public enum NotificationType
	{
		// Token: 0x040008FB RID: 2299
		None,
		// Token: 0x040008FC RID: 2300
		GracefulExit,
		// Token: 0x040008FD RID: 2301
		ForcedExit,
		// Token: 0x040008FE RID: 2302
		UnexpectedExit,
		// Token: 0x040008FF RID: 2303
		Reboot,
		// Token: 0x04000900 RID: 2304
		Constructed,
		// Token: 0x04000901 RID: 2305
		Started,
		// Token: 0x04000902 RID: 2306
		Paused,
		// Token: 0x04000903 RID: 2307
		CrashInitiated,
		// Token: 0x04000904 RID: 2308
		CrashReport,
		// Token: 0x04000905 RID: 2309
		SiloJobCreated,
		// Token: 0x04000906 RID: 2310
		Saved,
		// Token: 0x04000907 RID: 2311
		RdpEnhancedModeStateChanged,
		// Token: 0x04000908 RID: 2312
		Shutdown,
		// Token: 0x04000909 RID: 2313
		GetPropertiesCompleted,
		// Token: 0x0400090A RID: 2314
		ModifyCompleted,
		// Token: 0x0400090B RID: 2315
		GuestConnectionClosed,
		// Token: 0x0400090C RID: 2316
		Unknown
	}
}
