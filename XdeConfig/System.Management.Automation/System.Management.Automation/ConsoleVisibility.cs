using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x0200009A RID: 154
	internal static class ConsoleVisibility
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00024934 File Offset: 0x00022B34
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x0002493B File Offset: 0x00022B3B
		public static bool AlwaysCaptureApplicationIO
		{
			get
			{
				return ConsoleVisibility._alwaysCaptureApplicationIO;
			}
			set
			{
				ConsoleVisibility._alwaysCaptureApplicationIO = value;
			}
		}

		// Token: 0x06000775 RID: 1909
		[DllImport("Kernel32.dll")]
		internal static extern IntPtr GetConsoleWindow();

		// Token: 0x06000776 RID: 1910
		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x06000777 RID: 1911
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool AllocConsole();

		// Token: 0x06000778 RID: 1912
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		// Token: 0x06000779 RID: 1913
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x0600077A RID: 1914 RVA: 0x00024944 File Offset: 0x00022B44
		internal static bool AllocateHiddenConsole()
		{
			IntPtr consoleWindow = ConsoleVisibility.GetConsoleWindow();
			if (consoleWindow != IntPtr.Zero)
			{
				return false;
			}
			IntPtr foregroundWindow = ConsoleVisibility.GetForegroundWindow();
			ConsoleVisibility.AllocConsole();
			consoleWindow = ConsoleVisibility.GetConsoleWindow();
			bool result;
			if (consoleWindow == IntPtr.Zero)
			{
				result = false;
			}
			else
			{
				result = true;
				ConsoleVisibility.ShowWindow(consoleWindow, 0);
				ConsoleVisibility.AlwaysCaptureApplicationIO = true;
			}
			if (foregroundWindow != IntPtr.Zero && ConsoleVisibility.GetForegroundWindow() != foregroundWindow)
			{
				ConsoleVisibility.SetForegroundWindow(foregroundWindow);
			}
			return result;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000249BC File Offset: 0x00022BBC
		public static void Show()
		{
			IntPtr consoleWindow = ConsoleVisibility.GetConsoleWindow();
			if (consoleWindow != IntPtr.Zero)
			{
				ConsoleVisibility.ShowWindow(consoleWindow, 5);
				ConsoleVisibility.AlwaysCaptureApplicationIO = false;
				return;
			}
			throw PSTraceSource.NewInvalidOperationException();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000249F0 File Offset: 0x00022BF0
		public static void Hide()
		{
			IntPtr consoleWindow = ConsoleVisibility.GetConsoleWindow();
			if (consoleWindow != IntPtr.Zero)
			{
				ConsoleVisibility.ShowWindow(consoleWindow, 0);
				ConsoleVisibility.AlwaysCaptureApplicationIO = true;
				return;
			}
			throw PSTraceSource.NewInvalidOperationException();
		}

		// Token: 0x0400035B RID: 859
		internal const int SW_HIDE = 0;

		// Token: 0x0400035C RID: 860
		internal const int SW_SHOWNORMAL = 1;

		// Token: 0x0400035D RID: 861
		internal const int SW_NORMAL = 1;

		// Token: 0x0400035E RID: 862
		internal const int SW_SHOWMINIMIZED = 2;

		// Token: 0x0400035F RID: 863
		internal const int SW_SHOWMAXIMIZED = 3;

		// Token: 0x04000360 RID: 864
		internal const int SW_MAXIMIZE = 3;

		// Token: 0x04000361 RID: 865
		internal const int SW_SHOWNOACTIVATE = 4;

		// Token: 0x04000362 RID: 866
		internal const int SW_SHOW = 5;

		// Token: 0x04000363 RID: 867
		internal const int SW_MINIMIZE = 6;

		// Token: 0x04000364 RID: 868
		internal const int SW_SHOWMINNOACTIVE = 7;

		// Token: 0x04000365 RID: 869
		internal const int SW_SHOWNA = 8;

		// Token: 0x04000366 RID: 870
		internal const int SW_RESTORE = 9;

		// Token: 0x04000367 RID: 871
		internal const int SW_SHOWDEFAULT = 10;

		// Token: 0x04000368 RID: 872
		internal const int SW_FORCEMINIMIZE = 11;

		// Token: 0x04000369 RID: 873
		internal const int SW_MAX = 11;

		// Token: 0x0400036A RID: 874
		private static bool _alwaysCaptureApplicationIO;
	}
}
