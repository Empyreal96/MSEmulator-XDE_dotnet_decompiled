using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation
{
	// Token: 0x020008E3 RID: 2275
	internal class WindowsErrorReporting
	{
		// Token: 0x060055C2 RID: 21954 RVA: 0x001C2CF8 File Offset: 0x001C0EF8
		private static string TruncateExeName(string nameOfExe, int maxLength)
		{
			nameOfExe = nameOfExe.Trim();
			if (nameOfExe.Length > maxLength && nameOfExe.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
			{
				nameOfExe = nameOfExe.Substring(0, nameOfExe.Length - ".exe".Length);
			}
			return WindowsErrorReporting.TruncateBucketParameter(nameOfExe, maxLength);
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x001C2D45 File Offset: 0x001C0F45
		private static string TruncateTypeName(string typeName, int maxLength)
		{
			if (typeName.Length > maxLength)
			{
				typeName = typeName.Substring(typeName.Length - maxLength, maxLength);
			}
			return typeName;
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x001C2D64 File Offset: 0x001C0F64
		private static string TruncateExceptionType(string exceptionType, int maxLength)
		{
			if (exceptionType.Length > maxLength && exceptionType.EndsWith("Exception", StringComparison.OrdinalIgnoreCase))
			{
				exceptionType = exceptionType.Substring(0, exceptionType.Length - "Exception".Length);
			}
			if (exceptionType.Length > maxLength)
			{
				exceptionType = WindowsErrorReporting.TruncateTypeName(exceptionType, maxLength);
			}
			return WindowsErrorReporting.TruncateBucketParameter(exceptionType, maxLength);
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x001C2DBC File Offset: 0x001C0FBC
		private static string TruncateBucketParameter(string message, int maxLength)
		{
			if (message == null)
			{
				return string.Empty;
			}
			int num = maxLength * 30 / 100;
			if (message.Length > maxLength)
			{
				int num2 = maxLength - num - "..".Length;
				message = message.Substring(0, num) + ".." + message.Substring(message.Length - num2, num2);
			}
			return message;
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x001C2E18 File Offset: 0x001C1018
		private static string StackFrame2BucketParameter(StackFrame frame, int maxLength)
		{
			MethodBase method = frame.GetMethod();
			if (method == null)
			{
				return string.Empty;
			}
			Type declaringType = method.DeclaringType;
			if (declaringType == null)
			{
				string name = method.Name;
				return WindowsErrorReporting.TruncateBucketParameter(name, maxLength);
			}
			string text = declaringType.FullName;
			string text2 = "." + method.Name;
			if (maxLength > text2.Length)
			{
				text = WindowsErrorReporting.TruncateTypeName(text, maxLength - text2.Length);
			}
			else
			{
				text = WindowsErrorReporting.TruncateTypeName(text, 1);
			}
			return WindowsErrorReporting.TruncateBucketParameter(text + text2, maxLength);
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x001C2EA8 File Offset: 0x001C10A8
		private static string GetDeepestFrame(Exception exception, int maxLength)
		{
			StackTrace stackTrace = new StackTrace(exception);
			StackFrame frame = stackTrace.GetFrame(0);
			return WindowsErrorReporting.StackFrame2BucketParameter(frame, maxLength);
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x001C2ECC File Offset: 0x001C10CC
		private static bool IsPowerShellModule(string moduleName, bool globalMember)
		{
			foreach (string value in WindowsErrorReporting.powerShellModulesWithGlobalMembers)
			{
				if (moduleName.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			if (!globalMember)
			{
				foreach (string value2 in WindowsErrorReporting.powerShellModulesWithoutGlobalMembers)
				{
					if (moduleName.Equals(value2, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x001C2F38 File Offset: 0x001C1138
		private static string GetDeepestPowerShellFrame(Exception exception, int maxLength)
		{
			StackTrace stackTrace = new StackTrace(exception);
			foreach (StackFrame stackFrame in stackTrace.GetFrames())
			{
				MethodBase method = stackFrame.GetMethod();
				if (method != null)
				{
					Module module = method.Module;
					if (module != null)
					{
						Type declaringType = method.DeclaringType;
						string name = module.Name;
						if (WindowsErrorReporting.IsPowerShellModule(name, declaringType == null))
						{
							return WindowsErrorReporting.StackFrame2BucketParameter(stackFrame, maxLength);
						}
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x001C2FC1 File Offset: 0x001C11C1
		private static void SetBucketParameter(WindowsErrorReporting.ReportHandle reportHandle, WindowsErrorReporting.BucketParameterId bucketParameterId, string value)
		{
			WindowsErrorReporting.HandleHResult(WindowsErrorReporting.NativeMethods.WerReportSetParameter(reportHandle, bucketParameterId, bucketParameterId.ToString(), value));
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x001C2FDC File Offset: 0x001C11DC
		private static string GetThreadName()
		{
			string text = Thread.CurrentThread.Name;
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x001C3000 File Offset: 0x001C1200
		private static void SetBucketParameters(WindowsErrorReporting.ReportHandle reportHandle, Exception uncaughtException)
		{
			Exception ex = uncaughtException;
			while (ex.InnerException != null)
			{
				ex = ex.InnerException;
			}
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.NameOfExe, WindowsErrorReporting.TruncateExeName(WindowsErrorReporting.nameOfExe, 20));
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.FileVersionOfSystemManagementAutomation, WindowsErrorReporting.TruncateBucketParameter(WindowsErrorReporting.versionOfPowerShellLibraries, 16));
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.InnermostExceptionType, WindowsErrorReporting.TruncateExceptionType(ex.GetType().FullName, 40));
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.OutermostExceptionType, WindowsErrorReporting.TruncateExceptionType(uncaughtException.GetType().FullName, 40));
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.DeepestFrame, WindowsErrorReporting.GetDeepestFrame(uncaughtException, 50));
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.DeepestPowerShellFrame, WindowsErrorReporting.GetDeepestPowerShellFrame(uncaughtException, 50));
			WindowsErrorReporting.SetBucketParameter(reportHandle, WindowsErrorReporting.BucketParameterId.ThreadName, WindowsErrorReporting.TruncateBucketParameter(WindowsErrorReporting.GetThreadName(), 20));
		}

		// Token: 0x060055CD RID: 21965 RVA: 0x001C30AC File Offset: 0x001C12AC
		private static void FindStaticInformation()
		{
			string location = typeof(PSObject).Assembly.Location;
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(location);
			WindowsErrorReporting.versionOfPowerShellLibraries = versionInfo.FileVersion;
			WindowsErrorReporting.currentProcess = Process.GetCurrentProcess();
			ProcessModule mainModule = PsUtils.GetMainModule(WindowsErrorReporting.currentProcess);
			if (mainModule != null)
			{
				WindowsErrorReporting.applicationPath = mainModule.FileName;
			}
			WindowsErrorReporting.nameOfExe = Path.GetFileName(WindowsErrorReporting.applicationPath);
			WindowsErrorReporting.hCurrentProcess = WindowsErrorReporting.currentProcess.Handle;
			WindowsErrorReporting.hwndMainWindow = WindowsErrorReporting.currentProcess.MainWindowHandle;
			WindowsErrorReporting.applicationName = WindowsErrorReporting.currentProcess.ProcessName;
		}

		// Token: 0x060055CE RID: 21966 RVA: 0x001C313F File Offset: 0x001C133F
		private static void HandleHResult(int hresult)
		{
			Marshal.ThrowExceptionForHR(hresult);
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x001C3148 File Offset: 0x001C1348
		private static bool IsWindowsErrorReportingAvailable()
		{
			if (WindowsErrorReporting.isWindowsErrorReportingAvailable == null)
			{
				Version version = Environment.OSVersion.Version;
				WindowsErrorReporting.isWindowsErrorReportingAvailable = new bool?(version.Major >= 6);
			}
			return WindowsErrorReporting.isWindowsErrorReportingAvailable.Value;
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x001C318C File Offset: 0x001C138C
		private static void SubmitReport(Exception uncaughtException)
		{
			lock (WindowsErrorReporting.reportCreationLock)
			{
				if (uncaughtException == null)
				{
					throw new ArgumentNullException("uncaughtException");
				}
				WindowsErrorReporting.ReportInformation reportInformation = new WindowsErrorReporting.ReportInformation();
				reportInformation.dwSize = Marshal.SizeOf(reportInformation);
				reportInformation.hProcess = WindowsErrorReporting.hCurrentProcess;
				reportInformation.hwndParent = WindowsErrorReporting.hwndMainWindow;
				reportInformation.wzApplicationName = WindowsErrorReporting.applicationName;
				reportInformation.wzApplicationPath = WindowsErrorReporting.applicationPath;
				reportInformation.wzConsentKey = null;
				reportInformation.wzDescription = null;
				reportInformation.wzFriendlyEventName = null;
				WindowsErrorReporting.ReportHandle reportHandle;
				WindowsErrorReporting.HandleHResult(WindowsErrorReporting.NativeMethods.WerReportCreate("PowerShell", WindowsErrorReporting.ReportType.WerReportCritical, reportInformation, out reportHandle));
				using (reportHandle)
				{
					WindowsErrorReporting.SetBucketParameters(reportHandle, uncaughtException);
					WindowsErrorReporting.HandleHResult(WindowsErrorReporting.NativeMethods.WerReportAddDump(reportHandle, WindowsErrorReporting.hCurrentProcess, IntPtr.Zero, WindowsErrorReporting.DumpType.MiniDump, IntPtr.Zero, IntPtr.Zero, (WindowsErrorReporting.DumpFlags)0U));
					WindowsErrorReporting.SubmitResult exitCode = WindowsErrorReporting.SubmitResult.ReportFailed;
					WindowsErrorReporting.SubmitFlags submitFlags = WindowsErrorReporting.SubmitFlags.HonorRecovery | WindowsErrorReporting.SubmitFlags.HonorRestart | WindowsErrorReporting.SubmitFlags.AddRegisteredData | WindowsErrorReporting.SubmitFlags.OutOfProcess;
					if (WindowsErrorReporting.unattendedServerMode)
					{
						submitFlags |= WindowsErrorReporting.SubmitFlags.Queue;
					}
					WindowsErrorReporting.HandleHResult(WindowsErrorReporting.NativeMethods.WerReportSubmit(reportHandle, WindowsErrorReporting.Consent.NotAsked, submitFlags, out exitCode));
					Environment.Exit((int)exitCode);
				}
			}
		}

		// Token: 0x060055D1 RID: 21969 RVA: 0x001C32A8 File Offset: 0x001C14A8
		internal static void WaitForPendingReports()
		{
			lock (WindowsErrorReporting.reportCreationLock)
			{
			}
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x001C32E4 File Offset: 0x001C14E4
		internal static void FailFast(Exception exception)
		{
			try
			{
				if (WindowsErrorReporting.registered && exception != null)
				{
					WindowsErrorReporting.SubmitReport(exception);
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				Environment.FailFast((exception != null) ? exception.Message : string.Empty);
			}
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x001C333C File Offset: 0x001C153C
		internal static void RegisterWindowsErrorReporting(bool unattendedServer)
		{
			lock (WindowsErrorReporting.registrationLock)
			{
				if (!WindowsErrorReporting.registered && WindowsErrorReporting.IsWindowsErrorReportingAvailable())
				{
					try
					{
						WindowsErrorReporting.FindStaticInformation();
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
					try
					{
						WindowsErrorReporting.unattendedServerMode = unattendedServer;
						if (unattendedServer)
						{
							WindowsErrorReporting.HandleHResult(WindowsErrorReporting.NativeMethods.WerSetFlags(WindowsErrorReporting.ReportingFlags.Queue));
						}
						else
						{
							WindowsErrorReporting.HandleHResult(WindowsErrorReporting.NativeMethods.WerSetFlags((WindowsErrorReporting.ReportingFlags)0U));
						}
						AppDomain.CurrentDomain.UnhandledException += WindowsErrorReporting.CurrentDomain_UnhandledException;
						WindowsErrorReporting.registered = true;
					}
					catch (Exception e2)
					{
						CommandProcessorBase.CheckForSevereException(e2);
					}
				}
			}
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x001C33F4 File Offset: 0x001C15F4
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				WindowsErrorReporting.SubmitReport(ex);
			}
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x001C3416 File Offset: 0x001C1616
		internal static void WriteMiniDump(string file)
		{
			WindowsErrorReporting.WriteMiniDump(file, WindowsErrorReporting.MiniDumpType.MiniDumpNormal);
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x001C3420 File Offset: 0x001C1620
		internal static void WriteMiniDump(string file, WindowsErrorReporting.MiniDumpType dumpType)
		{
			Process process = Process.GetCurrentProcess();
			using (FileStream fileStream = new FileStream(file, FileMode.Create))
			{
				WindowsErrorReporting.NativeMethods.MiniDumpWriteDump(process.Handle, process.Id, fileStream.SafeFileHandle, dumpType, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}
		}

		// Token: 0x04002D55 RID: 11605
		private const string powerShellEventType = "PowerShell";

		// Token: 0x04002D56 RID: 11606
		private static readonly string[] powerShellModulesWithoutGlobalMembers = new string[]
		{
			"Microsoft.PowerShell.Commands.Diagnostics.dll",
			"Microsoft.PowerShell.Commands.Management.dll",
			"Microsoft.PowerShell.Commands.Utility.dll",
			"Microsoft.PowerShell.Security.dll",
			"System.Management.Automation.dll",
			"Microsoft.PowerShell.ConsoleHost.dll",
			"Microsoft.PowerShell.Editor.dll",
			"Microsoft.PowerShell.GPowerShell.dll",
			"Microsoft.PowerShell.GraphicalHost.dll"
		};

		// Token: 0x04002D57 RID: 11607
		private static readonly string[] powerShellModulesWithGlobalMembers = new string[]
		{
			"powershell.exe",
			"powershell_ise.exe",
			"pwrshplugin.dll",
			"pwrshsip.dll",
			"pshmsglh.dll",
			"PSEvents.dll"
		};

		// Token: 0x04002D58 RID: 11608
		private static string versionOfPowerShellLibraries = string.Empty;

		// Token: 0x04002D59 RID: 11609
		private static string nameOfExe = "GetMainModuleError";

		// Token: 0x04002D5A RID: 11610
		private static string applicationName = "GetMainModuleError";

		// Token: 0x04002D5B RID: 11611
		private static string applicationPath = "GetMainModuleError";

		// Token: 0x04002D5C RID: 11612
		private static IntPtr hCurrentProcess = IntPtr.Zero;

		// Token: 0x04002D5D RID: 11613
		private static IntPtr hwndMainWindow = IntPtr.Zero;

		// Token: 0x04002D5E RID: 11614
		private static Process currentProcess = null;

		// Token: 0x04002D5F RID: 11615
		private static bool? isWindowsErrorReportingAvailable;

		// Token: 0x04002D60 RID: 11616
		private static readonly object reportCreationLock = new object();

		// Token: 0x04002D61 RID: 11617
		private static readonly object registrationLock = new object();

		// Token: 0x04002D62 RID: 11618
		private static bool registered = false;

		// Token: 0x04002D63 RID: 11619
		private static bool unattendedServerMode = false;

		// Token: 0x020008E4 RID: 2276
		[Flags]
		private enum DumpFlags : uint
		{
			// Token: 0x04002D65 RID: 11621
			NoHeap_OnQueue = 1U
		}

		// Token: 0x020008E5 RID: 2277
		private enum DumpType : uint
		{
			// Token: 0x04002D67 RID: 11623
			MicroDump = 1U,
			// Token: 0x04002D68 RID: 11624
			MiniDump,
			// Token: 0x04002D69 RID: 11625
			HeapDump
		}

		// Token: 0x020008E6 RID: 2278
		private enum BucketParameterId : uint
		{
			// Token: 0x04002D6B RID: 11627
			NameOfExe,
			// Token: 0x04002D6C RID: 11628
			FileVersionOfSystemManagementAutomation,
			// Token: 0x04002D6D RID: 11629
			InnermostExceptionType,
			// Token: 0x04002D6E RID: 11630
			OutermostExceptionType,
			// Token: 0x04002D6F RID: 11631
			DeepestPowerShellFrame,
			// Token: 0x04002D70 RID: 11632
			DeepestFrame,
			// Token: 0x04002D71 RID: 11633
			ThreadName,
			// Token: 0x04002D72 RID: 11634
			Param7,
			// Token: 0x04002D73 RID: 11635
			Param8,
			// Token: 0x04002D74 RID: 11636
			Param9
		}

		// Token: 0x020008E7 RID: 2279
		private enum ReportType : uint
		{
			// Token: 0x04002D76 RID: 11638
			WerReportNonCritical,
			// Token: 0x04002D77 RID: 11639
			WerReportCritical,
			// Token: 0x04002D78 RID: 11640
			WerReportApplicationCrash,
			// Token: 0x04002D79 RID: 11641
			WerReportApplicationHang,
			// Token: 0x04002D7A RID: 11642
			WerReportKernel,
			// Token: 0x04002D7B RID: 11643
			WerReportInvalid
		}

		// Token: 0x020008E8 RID: 2280
		[Flags]
		internal enum MiniDumpType : uint
		{
			// Token: 0x04002D7D RID: 11645
			MiniDumpNormal = 0U,
			// Token: 0x04002D7E RID: 11646
			MiniDumpWithDataSegs = 1U,
			// Token: 0x04002D7F RID: 11647
			MiniDumpWithFullMemory = 2U,
			// Token: 0x04002D80 RID: 11648
			MiniDumpWithHandleData = 4U,
			// Token: 0x04002D81 RID: 11649
			MiniDumpFilterMemory = 8U,
			// Token: 0x04002D82 RID: 11650
			MiniDumpScanMemory = 16U,
			// Token: 0x04002D83 RID: 11651
			MiniDumpWithUnloadedModules = 32U,
			// Token: 0x04002D84 RID: 11652
			MiniDumpWithIndirectlyReferencedMemory = 64U,
			// Token: 0x04002D85 RID: 11653
			MiniDumpFilterModulePaths = 128U,
			// Token: 0x04002D86 RID: 11654
			MiniDumpWithProcessThreadData = 256U,
			// Token: 0x04002D87 RID: 11655
			MiniDumpWithPrivateReadWriteMemory = 512U,
			// Token: 0x04002D88 RID: 11656
			MiniDumpWithoutOptionalData = 1024U,
			// Token: 0x04002D89 RID: 11657
			MiniDumpWithFullMemoryInfo = 2048U,
			// Token: 0x04002D8A RID: 11658
			MiniDumpWithThreadInfo = 4096U,
			// Token: 0x04002D8B RID: 11659
			MiniDumpWithCodeSegs = 8192U
		}

		// Token: 0x020008E9 RID: 2281
		private enum Consent : uint
		{
			// Token: 0x04002D8D RID: 11661
			NotAsked = 1U,
			// Token: 0x04002D8E RID: 11662
			Approved,
			// Token: 0x04002D8F RID: 11663
			Denied,
			// Token: 0x04002D90 RID: 11664
			AlwaysPrompt
		}

		// Token: 0x020008EA RID: 2282
		[Flags]
		private enum SubmitFlags : uint
		{
			// Token: 0x04002D92 RID: 11666
			HonorRecovery = 1U,
			// Token: 0x04002D93 RID: 11667
			HonorRestart = 2U,
			// Token: 0x04002D94 RID: 11668
			Queue = 4U,
			// Token: 0x04002D95 RID: 11669
			ShowDebug = 8U,
			// Token: 0x04002D96 RID: 11670
			AddRegisteredData = 16U,
			// Token: 0x04002D97 RID: 11671
			OutOfProcess = 32U,
			// Token: 0x04002D98 RID: 11672
			NoCloseUI = 64U,
			// Token: 0x04002D99 RID: 11673
			NoQueue = 128U,
			// Token: 0x04002D9A RID: 11674
			NoArchive = 256U,
			// Token: 0x04002D9B RID: 11675
			StartMinimized = 512U,
			// Token: 0x04002D9C RID: 11676
			OutOfProcesAsync = 1024U,
			// Token: 0x04002D9D RID: 11677
			BypassDataThrottling = 2048U,
			// Token: 0x04002D9E RID: 11678
			ArchiveParametersOnly = 4096U
		}

		// Token: 0x020008EB RID: 2283
		private enum SubmitResult : uint
		{
			// Token: 0x04002DA0 RID: 11680
			ReportQueued = 1U,
			// Token: 0x04002DA1 RID: 11681
			ReportUploaded,
			// Token: 0x04002DA2 RID: 11682
			ReportDebug,
			// Token: 0x04002DA3 RID: 11683
			ReportFailed,
			// Token: 0x04002DA4 RID: 11684
			Disabled,
			// Token: 0x04002DA5 RID: 11685
			ReportCancelled,
			// Token: 0x04002DA6 RID: 11686
			DisabledQueue,
			// Token: 0x04002DA7 RID: 11687
			ReportAsync,
			// Token: 0x04002DA8 RID: 11688
			CustomAction
		}

		// Token: 0x020008EC RID: 2284
		private enum ReportingFlags : uint
		{
			// Token: 0x04002DAA RID: 11690
			NoHeap = 1U,
			// Token: 0x04002DAB RID: 11691
			Queue,
			// Token: 0x04002DAC RID: 11692
			DisableThreadSuspension = 4U,
			// Token: 0x04002DAD RID: 11693
			QueueUpload = 8U
		}

		// Token: 0x020008ED RID: 2285
		private class ReportHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x060055D9 RID: 21977 RVA: 0x001C358A File Offset: 0x001C178A
			private ReportHandle() : base(true)
			{
			}

			// Token: 0x060055DA RID: 21978 RVA: 0x001C3593 File Offset: 0x001C1793
			protected override bool ReleaseHandle()
			{
				return 0 == WindowsErrorReporting.NativeMethods.WerReportCloseHandle(this.handle);
			}
		}

		// Token: 0x020008EE RID: 2286
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private class ReportInformation
		{
			// Token: 0x04002DAE RID: 11694
			private const int MAX_PATH = 260;

			// Token: 0x04002DAF RID: 11695
			internal int dwSize;

			// Token: 0x04002DB0 RID: 11696
			internal IntPtr hProcess;

			// Token: 0x04002DB1 RID: 11697
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			internal string wzConsentKey;

			// Token: 0x04002DB2 RID: 11698
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string wzFriendlyEventName;

			// Token: 0x04002DB3 RID: 11699
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string wzApplicationName;

			// Token: 0x04002DB4 RID: 11700
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string wzApplicationPath;

			// Token: 0x04002DB5 RID: 11701
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
			internal string wzDescription;

			// Token: 0x04002DB6 RID: 11702
			internal IntPtr hwndParent;
		}

		// Token: 0x020008EF RID: 2287
		private static class NativeMethods
		{
			// Token: 0x060055DC RID: 21980
			[DllImport("wer.dll", CharSet = CharSet.Unicode)]
			internal static extern int WerReportCreate([MarshalAs(UnmanagedType.LPWStr)] string pwzEventType, WindowsErrorReporting.ReportType repType, [MarshalAs(UnmanagedType.LPStruct)] WindowsErrorReporting.ReportInformation reportInformation, out WindowsErrorReporting.ReportHandle reportHandle);

			// Token: 0x060055DD RID: 21981
			[DllImport("wer.dll", CharSet = CharSet.Unicode)]
			internal static extern int WerReportSetParameter(WindowsErrorReporting.ReportHandle reportHandle, WindowsErrorReporting.BucketParameterId bucketParameterId, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			// Token: 0x060055DE RID: 21982
			[DllImport("wer.dll", CharSet = CharSet.Unicode)]
			internal static extern int WerReportAddDump(WindowsErrorReporting.ReportHandle reportHandle, IntPtr hProcess, IntPtr hThread, WindowsErrorReporting.DumpType dumpType, IntPtr pExceptionParam, IntPtr dumpCustomOptions, WindowsErrorReporting.DumpFlags dumpFlags);

			// Token: 0x060055DF RID: 21983
			[DllImport("wer.dll")]
			internal static extern int WerReportSubmit(WindowsErrorReporting.ReportHandle reportHandle, WindowsErrorReporting.Consent consent, WindowsErrorReporting.SubmitFlags flags, out WindowsErrorReporting.SubmitResult result);

			// Token: 0x060055E0 RID: 21984
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[DllImport("wer.dll")]
			internal static extern int WerReportCloseHandle(IntPtr reportHandle);

			// Token: 0x060055E1 RID: 21985
			[DllImport("kernel32.dll")]
			internal static extern int WerSetFlags(WindowsErrorReporting.ReportingFlags flags);

			// Token: 0x060055E2 RID: 21986
			[DllImport("DbgHelp.dll", SetLastError = true)]
			internal static extern bool MiniDumpWriteDump(IntPtr hProcess, int processId, SafeFileHandle hFile, WindowsErrorReporting.MiniDumpType dumpType, IntPtr exceptionParam, IntPtr userStreamParam, IntPtr callackParam);

			// Token: 0x04002DB7 RID: 11703
			internal const string WerDll = "wer.dll";
		}
	}
}
