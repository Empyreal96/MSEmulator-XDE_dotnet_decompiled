using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000004 RID: 4
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[TypeLibType(TypeLibTypeFlags.FDispatchable)]
	[Guid("336D5562-EFA8-482E-8CB3-C5C0FC7A7DB6")]
	[ComImport]
	public interface IMsTscAxEvents
	{
		// Token: 0x0600001F RID: 31
		[DispId(1)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnConnecting();

		// Token: 0x06000020 RID: 32
		[DispId(2)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnConnected();

		// Token: 0x06000021 RID: 33
		[DispId(3)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnLoginComplete();

		// Token: 0x06000022 RID: 34
		[DispId(4)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnDisconnected([In] int discReason);

		// Token: 0x06000023 RID: 35
		[DispId(5)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnEnterFullScreenMode();

		// Token: 0x06000024 RID: 36
		[DispId(6)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnLeaveFullScreenMode();

		// Token: 0x06000025 RID: 37
		[DispId(7)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnChannelReceivedData([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string data);

		// Token: 0x06000026 RID: 38
		[DispId(8)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRequestGoFullScreen();

		// Token: 0x06000027 RID: 39
		[DispId(9)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRequestLeaveFullScreen();

		// Token: 0x06000028 RID: 40
		[DispId(10)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnFatalError([In] int errorCode);

		// Token: 0x06000029 RID: 41
		[DispId(11)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnWarning([In] int warningCode);

		// Token: 0x0600002A RID: 42
		[DispId(12)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRemoteDesktopSizeChange([In] int width, [In] int height);

		// Token: 0x0600002B RID: 43
		[DispId(13)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnIdleTimeoutNotification();

		// Token: 0x0600002C RID: 44
		[DispId(14)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRequestContainerMinimize();

		// Token: 0x0600002D RID: 45
		[DispId(15)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnConfirmClose(out bool pfAllowClose);

		// Token: 0x0600002E RID: 46
		[DispId(16)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnReceivedTSPublicKey([MarshalAs(UnmanagedType.BStr)] [In] string publicKey, out bool pfContinueLogon);

		// Token: 0x0600002F RID: 47
		[DispId(17)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAutoReconnecting([In] int disconnectReason, [In] int attemptCount, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.AutoReconnectContinueState")] out AutoReconnectContinueState pArcContinueStatus);

		// Token: 0x06000030 RID: 48
		[DispId(18)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAuthenticationWarningDisplayed();

		// Token: 0x06000031 RID: 49
		[DispId(19)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAuthenticationWarningDismissed();

		// Token: 0x06000032 RID: 50
		[DispId(20)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRemoteProgramResult([MarshalAs(UnmanagedType.BStr)] [In] string bstrRemoteProgram, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RemoteProgramResult")] [In] RemoteProgramResult lError, [In] bool vbIsExecutable);

		// Token: 0x06000033 RID: 51
		[DispId(21)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRemoteProgramDisplayed([In] bool vbDisplayed, [In] uint uDisplayInformation);

		// Token: 0x06000034 RID: 52
		[DispId(29)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnRemoteWindowDisplayed([In] bool vbDisplayed, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [In] ref _RemotableHandle hwnd, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RemoteWindowDisplayedAttribute")] [In] RemoteWindowDisplayedAttribute windowAttribute);

		// Token: 0x06000035 RID: 53
		[DispId(22)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnLogonError([In] int lError);

		// Token: 0x06000036 RID: 54
		[DispId(23)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnFocusReleased([In] int iDirection);

		// Token: 0x06000037 RID: 55
		[DispId(24)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnUserNameAcquired([MarshalAs(UnmanagedType.BStr)] [In] string bstrUserName);

		// Token: 0x06000038 RID: 56
		[DispId(26)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnMouseInputModeChanged([In] bool fMouseModeRelative);

		// Token: 0x06000039 RID: 57
		[DispId(28)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnServiceMessageReceived([MarshalAs(UnmanagedType.BStr)] [In] string serviceMessage);

		// Token: 0x0600003A RID: 58
		[DispId(30)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnConnectionBarPullDown();

		// Token: 0x0600003B RID: 59
		[DispId(32)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnNetworkBandwidthChanged([In] int qualityLevel);

		// Token: 0x0600003C RID: 60
		[DispId(33)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		void OnAutoReconnected();
	}
}
