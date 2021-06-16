using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000011 RID: 17
	internal class NativeMethods
	{
		// Token: 0x06000113 RID: 275
		[DllImport("winhttp", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool WinHttpGetIEProxyConfigForCurrentUser(ref NativeMethods.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG proxyConfig);

		// Token: 0x0200002B RID: 43
		public struct WINHTTP_CURRENT_USER_IE_PROXY_CONFIG
		{
			// Token: 0x17000042 RID: 66
			// (get) Token: 0x0600011D RID: 285 RVA: 0x000065D8 File Offset: 0x000047D8
			public string AutoConfigUrl
			{
				get
				{
					return Marshal.PtrToStringUni(this.hglobalAutoConfigUrl);
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x0600011E RID: 286 RVA: 0x000065E5 File Offset: 0x000047E5
			public string Proxy
			{
				get
				{
					return Marshal.PtrToStringUni(this.hglobalProxy);
				}
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x0600011F RID: 287 RVA: 0x000065F2 File Offset: 0x000047F2
			public string ProxyBypass
			{
				get
				{
					return Marshal.PtrToStringUni(this.hglobalProxyBypass);
				}
			}

			// Token: 0x06000120 RID: 288 RVA: 0x000065FF File Offset: 0x000047FF
			public void Free()
			{
				NativeMethods.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG.FreeHglobal(ref this.hglobalAutoConfigUrl);
				NativeMethods.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG.FreeHglobal(ref this.hglobalProxy);
				NativeMethods.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG.FreeHglobal(ref this.hglobalProxyBypass);
			}

			// Token: 0x06000121 RID: 289 RVA: 0x00006622 File Offset: 0x00004822
			private static void FreeHglobal(ref IntPtr hglobal)
			{
				if (hglobal != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(hglobal);
					hglobal = IntPtr.Zero;
				}
			}

			// Token: 0x040000C4 RID: 196
			[MarshalAs(UnmanagedType.Bool)]
			public bool fAutoDetect;

			// Token: 0x040000C5 RID: 197
			public IntPtr hglobalAutoConfigUrl;

			// Token: 0x040000C6 RID: 198
			public IntPtr hglobalProxy;

			// Token: 0x040000C7 RID: 199
			public IntPtr hglobalProxyBypass;
		}
	}
}
