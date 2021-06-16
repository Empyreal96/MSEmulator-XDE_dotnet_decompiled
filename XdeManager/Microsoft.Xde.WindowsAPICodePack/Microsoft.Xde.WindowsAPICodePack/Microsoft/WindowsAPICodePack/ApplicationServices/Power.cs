using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000045 RID: 69
	internal static class Power
	{
		// Token: 0x0600023E RID: 574 RVA: 0x000062C4 File Offset: 0x000044C4
		internal static PowerManagementNativeMethods.SystemPowerCapabilities GetSystemPowerCapabilities()
		{
			PowerManagementNativeMethods.SystemPowerCapabilities result;
			if (PowerManagementNativeMethods.CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel.SystemPowerCapabilities, IntPtr.Zero, 0U, out result, (uint)Marshal.SizeOf(typeof(PowerManagementNativeMethods.SystemPowerCapabilities))) == 3221225506U)
			{
				throw new UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessCapabilities);
			}
			return result;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00006304 File Offset: 0x00004504
		internal static PowerManagementNativeMethods.SystemBatteryState GetSystemBatteryState()
		{
			PowerManagementNativeMethods.SystemBatteryState result;
			if (PowerManagementNativeMethods.CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel.SystemBatteryState, IntPtr.Zero, 0U, out result, (uint)Marshal.SizeOf(typeof(PowerManagementNativeMethods.SystemBatteryState))) == 3221225506U)
			{
				throw new UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessBatteryState);
			}
			return result;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00006341 File Offset: 0x00004541
		internal static int RegisterPowerSettingNotification(IntPtr handle, Guid powerSetting)
		{
			return PowerManagementNativeMethods.RegisterPowerSettingNotification(handle, ref powerSetting, 0);
		}
	}
}
