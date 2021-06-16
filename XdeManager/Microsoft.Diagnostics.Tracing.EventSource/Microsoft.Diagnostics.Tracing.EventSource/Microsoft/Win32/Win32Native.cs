using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Win32
{
	// Token: 0x02000088 RID: 136
	internal static class Win32Native
	{
		// Token: 0x06000340 RID: 832
		[SecurityCritical]
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode)]
		internal static extern int FormatMessageW(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, [Out] StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

		// Token: 0x06000341 RID: 833 RVA: 0x00010288 File Offset: 0x0000E488
		[SecuritySafeCritical]
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = Win32Native.FormatMessageW(12800, IntPtr.Zero, errorCode, 0, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			return Microsoft.Diagnostics.Tracing.Internal.Environment.GetRuntimeResourceString("UnknownError_Num", new object[]
			{
				errorCode
			});
		}

		// Token: 0x06000342 RID: 834
		[SecurityCritical]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint GetCurrentProcessId();

		// Token: 0x040001BF RID: 447
		private const string CoreProcessThreadsApiSet = "kernel32.dll";

		// Token: 0x040001C0 RID: 448
		private const string CoreLocalizationApiSet = "kernel32.dll";

		// Token: 0x040001C1 RID: 449
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040001C2 RID: 450
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040001C3 RID: 451
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
	}
}
