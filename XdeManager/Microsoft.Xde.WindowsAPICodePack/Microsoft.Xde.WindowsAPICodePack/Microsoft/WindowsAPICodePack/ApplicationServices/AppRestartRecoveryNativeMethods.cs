using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x0200003E RID: 62
	internal static class AppRestartRecoveryNativeMethods
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00005D73 File Offset: 0x00003F73
		internal static AppRestartRecoveryNativeMethods.InternalRecoveryCallback InternalCallback
		{
			get
			{
				return AppRestartRecoveryNativeMethods.internalCallback;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00005D7C File Offset: 0x00003F7C
		private static uint InternalRecoveryHandler(IntPtr parameter)
		{
			bool flag = false;
			AppRestartRecoveryNativeMethods.ApplicationRecoveryInProgress(out flag);
			GCHandle gchandle = GCHandle.FromIntPtr(parameter);
			(gchandle.Target as RecoveryData).Invoke();
			gchandle.Free();
			return 0U;
		}

		// Token: 0x0600021A RID: 538
		[DllImport("kernel32.dll")]
		internal static extern void ApplicationRecoveryFinished([MarshalAs(UnmanagedType.Bool)] bool success);

		// Token: 0x0600021B RID: 539
		[DllImport("kernel32.dll")]
		internal static extern HResult ApplicationRecoveryInProgress([MarshalAs(UnmanagedType.Bool)] out bool canceled);

		// Token: 0x0600021C RID: 540
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern HResult RegisterApplicationRecoveryCallback(AppRestartRecoveryNativeMethods.InternalRecoveryCallback callback, IntPtr param, uint pingInterval, uint flags);

		// Token: 0x0600021D RID: 541
		[DllImport("kernel32.dll")]
		internal static extern HResult RegisterApplicationRestart([MarshalAs(UnmanagedType.BStr)] string commandLineArgs, RestartRestrictions flags);

		// Token: 0x0600021E RID: 542
		[DllImport("kernel32.dll")]
		internal static extern HResult UnregisterApplicationRecoveryCallback();

		// Token: 0x0600021F RID: 543
		[DllImport("kernel32.dll")]
		internal static extern HResult UnregisterApplicationRestart();

		// Token: 0x040001A9 RID: 425
		private static AppRestartRecoveryNativeMethods.InternalRecoveryCallback internalCallback = new AppRestartRecoveryNativeMethods.InternalRecoveryCallback(AppRestartRecoveryNativeMethods.InternalRecoveryHandler);

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x0600029C RID: 668
		internal delegate uint InternalRecoveryCallback(IntPtr state);
	}
}
