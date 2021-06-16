using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000011 RID: 17
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public enum HCS_NOTIFICATIONS : uint
	{
		// Token: 0x04000032 RID: 50
		HcsNotificationInvalid,
		// Token: 0x04000033 RID: 51
		HcsNotificationSystemExited,
		// Token: 0x04000034 RID: 52
		HcsNotificationSystemCreateCompleted,
		// Token: 0x04000035 RID: 53
		HcsNotificationSystemStartCompleted,
		// Token: 0x04000036 RID: 54
		HcsNotificationSystemPauseCompleted,
		// Token: 0x04000037 RID: 55
		HcsNotificationSystemResumeCompleted,
		// Token: 0x04000038 RID: 56
		HcsNotificationSystemCrashReport,
		// Token: 0x04000039 RID: 57
		HcsNotificationSystemSiloJobCreated,
		// Token: 0x0400003A RID: 58
		HcsNotificationSystemSaveCompleted,
		// Token: 0x0400003B RID: 59
		HcsNotificationProcessExited = 65536U,
		// Token: 0x0400003C RID: 60
		HcsNotificationServiceDisconnect = 16777216U,
		// Token: 0x0400003D RID: 61
		HcsNotificationFlagsReserved = 4026531840U
	}
}
