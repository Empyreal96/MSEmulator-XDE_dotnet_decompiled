using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000035 RID: 53
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4F6996D5-D7B1-412C-B0FF-063718566907")]
	[ComImport]
	public interface IMsRdpClientNonScriptable5 : IMsRdpClientNonScriptable4
	{
		// Token: 0x170008DE RID: 2270
		// (set) Token: 0x06001405 RID: 5125
		[DispId(1)]
		string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06001407 RID: 5127
		// (set) Token: 0x06001406 RID: 5126
		[DispId(2)]
		string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06001409 RID: 5129
		// (set) Token: 0x06001408 RID: 5128
		[DispId(3)]
		string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x0600140B RID: 5131
		// (set) Token: 0x0600140A RID: 5130
		[DispId(4)]
		string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x0600140D RID: 5133
		// (set) Token: 0x0600140C RID: 5132
		[DispId(5)]
		string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600140E RID: 5134
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ResetPassword();

		// Token: 0x0600140F RID: 5135
		[MethodImpl(MethodImplOptions.InternalCall)]
		void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06001410 RID: 5136
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06001412 RID: 5138
		// (set) Token: 0x06001411 RID: 5137
		[DispId(13)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		IntPtr UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06001414 RID: 5140
		// (set) Token: 0x06001413 RID: 5139
		[DispId(14)]
		bool ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06001416 RID: 5142
		// (set) Token: 0x06001415 RID: 5141
		[DispId(15)]
		bool PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06001418 RID: 5144
		// (set) Token: 0x06001417 RID: 5143
		[DispId(16)]
		bool NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x0600141A RID: 5146
		// (set) Token: 0x06001419 RID: 5145
		[DispId(17)]
		bool EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x0600141C RID: 5148
		// (set) Token: 0x0600141B RID: 5147
		[DispId(21)]
		bool RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600141E RID: 5150
		// (set) Token: 0x0600141D RID: 5149
		[DispId(20)]
		bool RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600141F RID: 5151
		[DispId(18)]
		IMsRdpDeviceCollection DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06001420 RID: 5152
		[DispId(19)]
		IMsRdpDriveCollection DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06001422 RID: 5154
		// (set) Token: 0x06001421 RID: 5153
		[DispId(23)]
		bool WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06001424 RID: 5156
		// (set) Token: 0x06001423 RID: 5155
		[DispId(22)]
		bool WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06001426 RID: 5158
		// (set) Token: 0x06001425 RID: 5157
		[DispId(24)]
		string ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06001428 RID: 5160
		// (set) Token: 0x06001427 RID: 5159
		[DispId(25)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")]
		RedirectionWarningType RedirectionWarningType { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] [param: In] set; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600142A RID: 5162
		// (set) Token: 0x06001429 RID: 5161
		[DispId(28)]
		bool MarkRdpSettingsSecure { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x0600142C RID: 5164
		// (set) Token: 0x0600142B RID: 5163
		[DispId(26)]
		object PublisherCertificateChain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x0600142E RID: 5166
		// (set) Token: 0x0600142D RID: 5165
		[DispId(27)]
		bool WarnAboutPrinterRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06001430 RID: 5168
		// (set) Token: 0x0600142F RID: 5167
		[DispId(29)]
		bool AllowCredentialSaving { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06001432 RID: 5170
		// (set) Token: 0x06001431 RID: 5169
		[DispId(30)]
		bool PromptForCredsOnClient { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06001434 RID: 5172
		// (set) Token: 0x06001433 RID: 5171
		[DispId(31)]
		bool LaunchedViaClientShellInterface { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06001436 RID: 5174
		// (set) Token: 0x06001435 RID: 5173
		[DispId(32)]
		bool TrustedZoneSite { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06001438 RID: 5176
		// (set) Token: 0x06001437 RID: 5175
		[DispId(33)]
		bool UseMultimon { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06001439 RID: 5177
		[DispId(35)]
		uint RemoteMonitorCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x0600143A RID: 5178
		[MethodImpl(MethodImplOptions.InternalCall)]
		void GetRemoteMonitorsBoundingBox(out int pLeft, out int pTop, out int pRight, out int pBottom);

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x0600143B RID: 5179
		[DispId(37)]
		bool RemoteMonitorLayoutMatchesLocal { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170008FA RID: 2298
		// (set) Token: 0x0600143C RID: 5180
		[DispId(38)]
		bool DisableConnectionBar { [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x0600143E RID: 5182
		// (set) Token: 0x0600143D RID: 5181
		[DispId(39)]
		bool DisableRemoteAppCapsCheck { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06001440 RID: 5184
		// (set) Token: 0x0600143F RID: 5183
		[DispId(40)]
		bool WarnAboutDirectXRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06001442 RID: 5186
		// (set) Token: 0x06001441 RID: 5185
		[DispId(41)]
		bool AllowPromptingForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
