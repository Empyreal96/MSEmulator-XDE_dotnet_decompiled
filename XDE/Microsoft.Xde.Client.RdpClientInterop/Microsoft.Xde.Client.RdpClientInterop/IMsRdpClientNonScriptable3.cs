using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000029 RID: 41
	[Guid("B3378D90-0728-45C7-8ED7-B6159FB92219")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IMsRdpClientNonScriptable3 : IMsRdpClientNonScriptable2
	{
		// Token: 0x1700051E RID: 1310
		// (set) Token: 0x06000C7C RID: 3196
		[DispId(1)]
		string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000C7E RID: 3198
		// (set) Token: 0x06000C7D RID: 3197
		[DispId(2)]
		string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000C80 RID: 3200
		// (set) Token: 0x06000C7F RID: 3199
		[DispId(3)]
		string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000C82 RID: 3202
		// (set) Token: 0x06000C81 RID: 3201
		[DispId(4)]
		string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000C84 RID: 3204
		// (set) Token: 0x06000C83 RID: 3203
		[DispId(5)]
		string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000C85 RID: 3205
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ResetPassword();

		// Token: 0x06000C86 RID: 3206
		[MethodImpl(MethodImplOptions.InternalCall)]
		void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000C87 RID: 3207
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000C89 RID: 3209
		// (set) Token: 0x06000C88 RID: 3208
		[DispId(13)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		IntPtr UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000C8B RID: 3211
		// (set) Token: 0x06000C8A RID: 3210
		[DispId(14)]
		bool ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000C8D RID: 3213
		// (set) Token: 0x06000C8C RID: 3212
		[DispId(15)]
		bool PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000C8F RID: 3215
		// (set) Token: 0x06000C8E RID: 3214
		[DispId(16)]
		bool NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000C91 RID: 3217
		// (set) Token: 0x06000C90 RID: 3216
		[DispId(17)]
		bool EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000C93 RID: 3219
		// (set) Token: 0x06000C92 RID: 3218
		[DispId(21)]
		bool RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06000C95 RID: 3221
		// (set) Token: 0x06000C94 RID: 3220
		[DispId(20)]
		bool RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06000C96 RID: 3222
		[DispId(18)]
		IMsRdpDeviceCollection DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06000C97 RID: 3223
		[DispId(19)]
		IMsRdpDriveCollection DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06000C99 RID: 3225
		// (set) Token: 0x06000C98 RID: 3224
		[DispId(23)]
		bool WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06000C9B RID: 3227
		// (set) Token: 0x06000C9A RID: 3226
		[DispId(22)]
		bool WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06000C9D RID: 3229
		// (set) Token: 0x06000C9C RID: 3228
		[DispId(24)]
		string ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }
	}
}
