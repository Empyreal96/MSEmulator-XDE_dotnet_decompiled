using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000036 RID: 54
	[CLSCompliant(false)]
	public static class ApplicationRestartRecoveryManager
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00005AC8 File Offset: 0x00003CC8
		public static void RegisterForApplicationRecovery(RecoverySettings settings)
		{
			CoreHelpers.ThrowIfNotVista();
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			GCHandle value = GCHandle.Alloc(settings.RecoveryData);
			HResult hresult = AppRestartRecoveryNativeMethods.RegisterApplicationRecoveryCallback(AppRestartRecoveryNativeMethods.InternalCallback, (IntPtr)value, settings.PingInterval, 0U);
			if (CoreErrorHelper.Succeeded(hresult))
			{
				return;
			}
			if (hresult == HResult.InvalidArguments)
			{
				throw new ArgumentException(LocalizedMessages.ApplicationRecoveryBadParameters, "settings");
			}
			throw new ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToRegister);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00005B37 File Offset: 0x00003D37
		public static void UnregisterApplicationRecovery()
		{
			CoreHelpers.ThrowIfNotVista();
			if (!CoreErrorHelper.Succeeded(AppRestartRecoveryNativeMethods.UnregisterApplicationRecoveryCallback()))
			{
				throw new ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToUnregister);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00005B55 File Offset: 0x00003D55
		public static void UnregisterApplicationRestart()
		{
			CoreHelpers.ThrowIfNotVista();
			if (!CoreErrorHelper.Succeeded(AppRestartRecoveryNativeMethods.UnregisterApplicationRestart()))
			{
				throw new ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToUnregisterForRestart);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00005B74 File Offset: 0x00003D74
		public static bool ApplicationRecoveryInProgress()
		{
			CoreHelpers.ThrowIfNotVista();
			bool result = false;
			if (!CoreErrorHelper.Succeeded(AppRestartRecoveryNativeMethods.ApplicationRecoveryInProgress(out result)))
			{
				throw new InvalidOperationException(LocalizedMessages.ApplicationRecoveryMustBeCalledFromCallback);
			}
			return result;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00005BA2 File Offset: 0x00003DA2
		public static void ApplicationRecoveryFinished(bool success)
		{
			CoreHelpers.ThrowIfNotVista();
			AppRestartRecoveryNativeMethods.ApplicationRecoveryFinished(success);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void RegisterForApplicationRestart(RestartSettings settings)
		{
			CoreHelpers.ThrowIfNotVista();
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			HResult hresult = AppRestartRecoveryNativeMethods.RegisterApplicationRestart(settings.Command, settings.Restrictions);
			if (hresult == HResult.Fail)
			{
				throw new InvalidOperationException(LocalizedMessages.ApplicationRecoveryFailedToRegisterForRestart);
			}
			if (hresult == HResult.InvalidArguments)
			{
				throw new ArgumentException(LocalizedMessages.ApplicationRecoverFailedToRegisterForRestartBadParameters);
			}
		}
	}
}
