using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000006 RID: 6
	internal static class CoreNativeMethods
	{
		// Token: 0x0600000E RID: 14
		[DllImport("user32.dll", CharSet = CharSet.Auto, PreserveSig = false, SetLastError = true)]
		public static extern void PostMessage(IntPtr windowHandle, WindowMessage message, IntPtr wparam, IntPtr lparam);

		// Token: 0x0600000F RID: 15
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, WindowMessage message, IntPtr wparam, IntPtr lparam);

		// Token: 0x06000010 RID: 16
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, IntPtr wparam, IntPtr lparam);

		// Token: 0x06000011 RID: 17
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, IntPtr wparam, [MarshalAs(UnmanagedType.LPWStr)] string lparam);

		// Token: 0x06000012 RID: 18 RVA: 0x000021DB File Offset: 0x000003DB
		public static IntPtr SendMessage(IntPtr windowHandle, uint message, int wparam, string lparam)
		{
			return CoreNativeMethods.SendMessage(windowHandle, message, (IntPtr)wparam, lparam);
		}

		// Token: 0x06000013 RID: 19
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, ref int wparam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lparam);

		// Token: 0x06000014 RID: 20
		[DllImport("kernel32.dll", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);

		// Token: 0x06000015 RID: 21
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteObject(IntPtr graphicsObjectHandle);

		// Token: 0x06000016 RID: 22
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int LoadString(IntPtr instanceHandle, int id, StringBuilder buffer, int bufferSize);

		// Token: 0x06000017 RID: 23
		[DllImport("Kernel32.dll")]
		internal static extern IntPtr LocalFree(ref Guid guid);

		// Token: 0x06000018 RID: 24
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DestroyIcon(IntPtr hIcon);

		// Token: 0x06000019 RID: 25
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern int DestroyWindow(IntPtr handle);

		// Token: 0x0600001A RID: 26 RVA: 0x000021EB File Offset: 0x000003EB
		public static int GetHiWord(long value, int size)
		{
			return (int)((short)(value >> size));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000021F4 File Offset: 0x000003F4
		public static int GetLoWord(long value)
		{
			return (int)((short)(value & 65535L));
		}

		// Token: 0x040000E1 RID: 225
		internal const int UserMessage = 1024;

		// Token: 0x040000E2 RID: 226
		internal const int EnterIdleMessage = 289;

		// Token: 0x040000E3 RID: 227
		internal const int FormatMessageFromSystem = 4096;

		// Token: 0x040000E4 RID: 228
		internal const uint ResultFailed = 2147500037U;

		// Token: 0x040000E5 RID: 229
		internal const uint ResultInvalidArgument = 2147942487U;

		// Token: 0x040000E6 RID: 230
		internal const uint ResultFalse = 1U;

		// Token: 0x040000E7 RID: 231
		internal const uint ResultNotFound = 2147943568U;

		// Token: 0x040000E8 RID: 232
		internal const int DWMNCRP_USEWINDOWSTYLE = 0;

		// Token: 0x040000E9 RID: 233
		internal const int DWMNCRP_DISABLED = 1;

		// Token: 0x040000EA RID: 234
		internal const int DWMNCRP_ENABLED = 2;

		// Token: 0x040000EB RID: 235
		internal const int DWMWA_NCRENDERING_ENABLED = 1;

		// Token: 0x040000EC RID: 236
		internal const int DWMWA_NCRENDERING_POLICY = 2;

		// Token: 0x040000ED RID: 237
		internal const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;

		// Token: 0x040000EE RID: 238
		internal const uint StatusAccessDenied = 3221225506U;

		// Token: 0x02000049 RID: 73
		public struct Size
		{
			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x0600025A RID: 602 RVA: 0x00006620 File Offset: 0x00004820
			// (set) Token: 0x0600025B RID: 603 RVA: 0x00006628 File Offset: 0x00004828
			public int Width
			{
				get
				{
					return this.width;
				}
				set
				{
					this.width = value;
				}
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x0600025C RID: 604 RVA: 0x00006631 File Offset: 0x00004831
			// (set) Token: 0x0600025D RID: 605 RVA: 0x00006639 File Offset: 0x00004839
			public int Height
			{
				get
				{
					return this.height;
				}
				set
				{
					this.height = value;
				}
			}

			// Token: 0x040001D9 RID: 473
			private int width;

			// Token: 0x040001DA RID: 474
			private int height;
		}

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x0600025F RID: 607
		public delegate int WNDPROC(IntPtr hWnd, uint uMessage, IntPtr wParam, IntPtr lParam);
	}
}
