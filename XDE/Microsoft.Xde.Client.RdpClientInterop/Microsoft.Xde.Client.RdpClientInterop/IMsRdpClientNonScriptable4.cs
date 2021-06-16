using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002F RID: 47
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("F50FA8AA-1C7D-4F59-B15C-A90CACAE1FCB")]
	[ComImport]
	public interface IMsRdpClientNonScriptable4 : IMsRdpClientNonScriptable3
	{
		// Token: 0x170006BE RID: 1726
		// (set) Token: 0x06000FD0 RID: 4048
		[DispId(1)]
		string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06000FD2 RID: 4050
		// (set) Token: 0x06000FD1 RID: 4049
		[DispId(2)]
		string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06000FD4 RID: 4052
		// (set) Token: 0x06000FD3 RID: 4051
		[DispId(3)]
		string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06000FD6 RID: 4054
		// (set) Token: 0x06000FD5 RID: 4053
		[DispId(4)]
		string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06000FD8 RID: 4056
		// (set) Token: 0x06000FD7 RID: 4055
		[DispId(5)]
		string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000FD9 RID: 4057
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ResetPassword();

		// Token: 0x06000FDA RID: 4058
		[MethodImpl(MethodImplOptions.InternalCall)]
		void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000FDB RID: 4059
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06000FDD RID: 4061
		// (set) Token: 0x06000FDC RID: 4060
		[DispId(13)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		IntPtr UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06000FDF RID: 4063
		// (set) Token: 0x06000FDE RID: 4062
		[DispId(14)]
		bool ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06000FE1 RID: 4065
		// (set) Token: 0x06000FE0 RID: 4064
		[DispId(15)]
		bool PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06000FE3 RID: 4067
		// (set) Token: 0x06000FE2 RID: 4066
		[DispId(16)]
		bool NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06000FE5 RID: 4069
		// (set) Token: 0x06000FE4 RID: 4068
		[DispId(17)]
		bool EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06000FE7 RID: 4071
		// (set) Token: 0x06000FE6 RID: 4070
		[DispId(21)]
		bool RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06000FE9 RID: 4073
		// (set) Token: 0x06000FE8 RID: 4072
		[DispId(20)]
		bool RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06000FEA RID: 4074
		[DispId(18)]
		IMsRdpDeviceCollection DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06000FEB RID: 4075
		[DispId(19)]
		IMsRdpDriveCollection DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06000FED RID: 4077
		// (set) Token: 0x06000FEC RID: 4076
		[DispId(23)]
		bool WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06000FEF RID: 4079
		// (set) Token: 0x06000FEE RID: 4078
		[DispId(22)]
		bool WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06000FF1 RID: 4081
		// (set) Token: 0x06000FF0 RID: 4080
		[DispId(24)]
		string ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06000FF3 RID: 4083
		// (set) Token: 0x06000FF2 RID: 4082
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")]
		[DispId(25)]
		RedirectionWarningType RedirectionWarningType { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] [param: In] set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06000FF5 RID: 4085
		// (set) Token: 0x06000FF4 RID: 4084
		[DispId(28)]
		bool MarkRdpSettingsSecure { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06000FF7 RID: 4087
		// (set) Token: 0x06000FF6 RID: 4086
		[DispId(26)]
		object PublisherCertificateChain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06000FF9 RID: 4089
		// (set) Token: 0x06000FF8 RID: 4088
		[DispId(27)]
		bool WarnAboutPrinterRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06000FFB RID: 4091
		// (set) Token: 0x06000FFA RID: 4090
		[DispId(29)]
		bool AllowCredentialSaving { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06000FFD RID: 4093
		// (set) Token: 0x06000FFC RID: 4092
		[DispId(30)]
		bool PromptForCredsOnClient { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06000FFF RID: 4095
		// (set) Token: 0x06000FFE RID: 4094
		[DispId(31)]
		bool LaunchedViaClientShellInterface { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001001 RID: 4097
		// (set) Token: 0x06001000 RID: 4096
		[DispId(32)]
		bool TrustedZoneSite { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
