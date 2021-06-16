using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x0200080E RID: 2062
	internal class AmsiUtils
	{
		// Token: 0x06004F80 RID: 20352 RVA: 0x001A5D64 File Offset: 0x001A3F64
		internal static AmsiUtils.AmsiNativeMethods.AMSI_RESULT ScanContent(string content, string sourceMetadata)
		{
			if (string.IsNullOrEmpty(sourceMetadata))
			{
				sourceMetadata = string.Empty;
			}
			if (InternalTestHooks.UseDebugAmsiImplementation && content.IndexOf("X5O!P%@AP[4\\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*", StringComparison.Ordinal) >= 0)
			{
				return AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_DETECTED;
			}
			if (AmsiUtils.amsiInitFailed)
			{
				return AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
			}
			AmsiUtils.AmsiNativeMethods.AMSI_RESULT result;
			lock (AmsiUtils.amsiLockObject)
			{
				if (AmsiUtils.amsiInitFailed)
				{
					result = AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
				}
				else
				{
					try
					{
						int hresult;
						if (AmsiUtils.amsiContext == IntPtr.Zero)
						{
							Process currentProcess = Process.GetCurrentProcess();
							string appName;
							try
							{
								appName = StringUtil.Format("PowerShell_{0}_{1}", PsUtils.GetMainModule(currentProcess).FileName, ClrFacade.GetProcessModuleFileVersionInfo(PsUtils.GetMainModule(currentProcess)).ProductVersion);
							}
							catch (Win32Exception)
							{
								string[] commandLineArgs = Environment.GetCommandLineArgs();
								string o = (commandLineArgs.Length > 0) ? commandLineArgs[0] : currentProcess.ProcessName;
								appName = StringUtil.Format("PowerShell_{0}.exe_0.0.0.0", o);
							}
							AppDomain.CurrentDomain.ProcessExit += AmsiUtils.CurrentDomain_ProcessExit;
							hresult = AmsiUtils.AmsiNativeMethods.AmsiInitialize(appName, ref AmsiUtils.amsiContext);
							if (!Utils.Succeeded(hresult))
							{
								AmsiUtils.amsiInitFailed = true;
								return AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
							}
						}
						if (AmsiUtils.amsiSession == IntPtr.Zero)
						{
							hresult = AmsiUtils.AmsiNativeMethods.AmsiOpenSession(AmsiUtils.amsiContext, ref AmsiUtils.amsiSession);
							AmsiUtils.AmsiInitialized = true;
							if (!Utils.Succeeded(hresult))
							{
								AmsiUtils.amsiInitFailed = true;
								return AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
							}
						}
						AmsiUtils.AmsiNativeMethods.AMSI_RESULT amsi_RESULT = AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_CLEAN;
						hresult = AmsiUtils.AmsiNativeMethods.AmsiScanString(AmsiUtils.amsiContext, content, sourceMetadata, AmsiUtils.amsiSession, ref amsi_RESULT);
						if (!Utils.Succeeded(hresult))
						{
							result = AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
						}
						else
						{
							result = amsi_RESULT;
						}
					}
					catch (DllNotFoundException)
					{
						AmsiUtils.amsiInitFailed = true;
						result = AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
					}
				}
			}
			return result;
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001A5F34 File Offset: 0x001A4134
		private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			AmsiUtils.VerifyAmsiUninitializeCalled();
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x001A5F3C File Offset: 0x001A413C
		internal static void CloseSession()
		{
			if (!AmsiUtils.amsiInitFailed && AmsiUtils.amsiContext != IntPtr.Zero && AmsiUtils.amsiSession != IntPtr.Zero)
			{
				lock (AmsiUtils.amsiLockObject)
				{
					if (AmsiUtils.amsiContext != IntPtr.Zero && AmsiUtils.amsiSession != IntPtr.Zero)
					{
						AmsiUtils.AmsiNativeMethods.AmsiCloseSession(AmsiUtils.amsiContext, AmsiUtils.amsiSession);
						AmsiUtils.amsiSession = IntPtr.Zero;
					}
				}
			}
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x001A5FDC File Offset: 0x001A41DC
		internal static void Uninitialize()
		{
			AmsiUtils.AmsiUninitializeCalled = true;
			if (!AmsiUtils.amsiInitFailed)
			{
				lock (AmsiUtils.amsiLockObject)
				{
					if (AmsiUtils.amsiContext != IntPtr.Zero)
					{
						AmsiUtils.CloseSession();
						AmsiUtils.AmsiCleanedUp = true;
						AmsiUtils.AmsiNativeMethods.AmsiUninitialize(AmsiUtils.amsiContext);
						AmsiUtils.amsiContext = IntPtr.Zero;
					}
				}
			}
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x001A6054 File Offset: 0x001A4254
		private static void VerifyAmsiUninitializeCalled()
		{
		}

		// Token: 0x040028A3 RID: 10403
		private static IntPtr amsiContext = IntPtr.Zero;

		// Token: 0x040028A4 RID: 10404
		private static IntPtr amsiSession = IntPtr.Zero;

		// Token: 0x040028A5 RID: 10405
		private static bool amsiInitFailed = false;

		// Token: 0x040028A6 RID: 10406
		private static object amsiLockObject = new object();

		// Token: 0x040028A7 RID: 10407
		public static bool AmsiUninitializeCalled = false;

		// Token: 0x040028A8 RID: 10408
		public static bool AmsiInitialized = false;

		// Token: 0x040028A9 RID: 10409
		public static bool AmsiCleanedUp = false;

		// Token: 0x0200080F RID: 2063
		internal class AmsiNativeMethods
		{
			// Token: 0x06004F87 RID: 20359
			[DllImport("amsi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern int AmsiInitialize([MarshalAs(UnmanagedType.LPWStr)] [In] string appName, ref IntPtr amsiContext);

			// Token: 0x06004F88 RID: 20360
			[DllImport("amsi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern void AmsiUninitialize(IntPtr amsiContext);

			// Token: 0x06004F89 RID: 20361
			[DllImport("amsi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern int AmsiOpenSession(IntPtr amsiContext, ref IntPtr amsiSession);

			// Token: 0x06004F8A RID: 20362
			[DllImport("amsi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern void AmsiCloseSession(IntPtr amsiContext, IntPtr amsiSession);

			// Token: 0x06004F8B RID: 20363
			[DllImport("amsi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern int AmsiScanBuffer(IntPtr amsiContext, IntPtr buffer, uint length, [MarshalAs(UnmanagedType.LPWStr)] [In] string contentName, IntPtr amsiSession, ref AmsiUtils.AmsiNativeMethods.AMSI_RESULT result);

			// Token: 0x06004F8C RID: 20364
			[DllImport("amsi.dll", CallingConvention = CallingConvention.StdCall)]
			internal static extern int AmsiScanString(IntPtr amsiContext, [MarshalAs(UnmanagedType.LPWStr)] [In] string @string, [MarshalAs(UnmanagedType.LPWStr)] [In] string contentName, IntPtr amsiSession, ref AmsiUtils.AmsiNativeMethods.AMSI_RESULT result);

			// Token: 0x02000810 RID: 2064
			internal enum AMSI_RESULT
			{
				// Token: 0x040028AB RID: 10411
				AMSI_RESULT_CLEAN,
				// Token: 0x040028AC RID: 10412
				AMSI_RESULT_NOT_DETECTED,
				// Token: 0x040028AD RID: 10413
				AMSI_RESULT_DETECTED = 32768
			}
		}
	}
}
