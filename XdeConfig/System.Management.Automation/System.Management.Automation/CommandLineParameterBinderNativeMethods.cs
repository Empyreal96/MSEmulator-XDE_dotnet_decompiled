using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation
{
	// Token: 0x0200008F RID: 143
	internal static class CommandLineParameterBinderNativeMethods
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x00022D34 File Offset: 0x00020F34
		public static string[] PreParseCommandLine(string commandLine)
		{
			int num = 0;
			IntPtr intPtr = CommandLineParameterBinderNativeMethods.CommandLineToArgvW(commandLine, out num);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			string[] result;
			try
			{
				string[] array = new string[num - 1];
				for (int i = 1; i < num; i++)
				{
					array[i - 1] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(intPtr, i * IntPtr.Size));
				}
				result = array;
			}
			finally
			{
				CommandLineParameterBinderNativeMethods.LocalFree(intPtr);
			}
			return result;
		}

		// Token: 0x06000736 RID: 1846
		[DllImport("shell32.dll", SetLastError = true)]
		private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

		// Token: 0x06000737 RID: 1847
		[DllImport("kernel32.dll")]
		private static extern IntPtr LocalFree(IntPtr hMem);
	}
}
