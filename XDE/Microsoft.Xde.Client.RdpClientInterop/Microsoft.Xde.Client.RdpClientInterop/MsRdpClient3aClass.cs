using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200001E RID: 30
	[TypeLibType(TypeLibTypeFlags.FCanCreate | TypeLibTypeFlags.FControl)]
	[Guid("6A6F4B83-45C5-4CA9-BDD9-0D81C12295E4")]
	[ComSourceInterfaces("Microsoft.Xde.Client.RdpClientInterop.IMsTscAxEvents\0\0")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComImport]
	public class MsRdpClient3aClass : IMsRdpClient3, MsRdpClient3a, IMsTscAxEvents_Event, IMsRdpClient2, IMsRdpClient, IMsTscAx, IMsTscAx_Redist, IMsTscNonScriptable, IMsRdpClientNonScriptable
	{
		// Token: 0x0600079E RID: 1950
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern MsRdpClient3aClass();

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060007A0 RID: 1952
		// (set) Token: 0x0600079F RID: 1951
		[DispId(1)]
		public virtual extern string Server { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060007A2 RID: 1954
		// (set) Token: 0x060007A1 RID: 1953
		[DispId(2)]
		public virtual extern string Domain { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060007A4 RID: 1956
		// (set) Token: 0x060007A3 RID: 1955
		[DispId(3)]
		public virtual extern string UserName { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060007A6 RID: 1958
		// (set) Token: 0x060007A5 RID: 1957
		[DispId(4)]
		public virtual extern string DisconnectedText { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060007A8 RID: 1960
		// (set) Token: 0x060007A7 RID: 1959
		[DispId(5)]
		public virtual extern string ConnectingText { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060007A9 RID: 1961
		[DispId(6)]
		public virtual extern short Connected { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060007AB RID: 1963
		// (set) Token: 0x060007AA RID: 1962
		[DispId(12)]
		public virtual extern int DesktopWidth { [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060007AD RID: 1965
		// (set) Token: 0x060007AC RID: 1964
		[DispId(13)]
		public virtual extern int DesktopHeight { [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060007AF RID: 1967
		// (set) Token: 0x060007AE RID: 1966
		[DispId(16)]
		public virtual extern int StartConnected { [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060007B0 RID: 1968
		[DispId(17)]
		public virtual extern int HorizontalScrollBarVisible { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060007B1 RID: 1969
		[DispId(18)]
		public virtual extern int VerticalScrollBarVisible { [DispId(18)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170002EE RID: 750
		// (set) Token: 0x060007B2 RID: 1970
		[DispId(19)]
		public virtual extern string FullScreenTitle { [DispId(19)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060007B3 RID: 1971
		[DispId(20)]
		public virtual extern int CipherStrength { [DispId(20)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060007B4 RID: 1972
		[DispId(21)]
		public virtual extern string Version { [DispId(21)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060007B5 RID: 1973
		[DispId(22)]
		public virtual extern int SecuredSettingsEnabled { [DispId(22)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060007B6 RID: 1974
		[DispId(97)]
		public virtual extern IMsTscSecuredSettings SecuredSettings { [DispId(97)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060007B7 RID: 1975
		[DispId(98)]
		public virtual extern IMsTscAdvancedSettings AdvancedSettings { [DispId(98)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060007B8 RID: 1976
		[DispId(99)]
		public virtual extern IMsTscDebug Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [DispId(99)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x060007B9 RID: 1977
		[DispId(30)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Connect();

		// Token: 0x060007BA RID: 1978
		[DispId(31)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Disconnect();

		// Token: 0x060007BB RID: 1979
		[DispId(33)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x060007BC RID: 1980
		[DispId(34)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060007BE RID: 1982
		// (set) Token: 0x060007BD RID: 1981
		[DispId(100)]
		public virtual extern int ColorDepth { [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060007BF RID: 1983
		[DispId(101)]
		public virtual extern IMsRdpClientAdvancedSettings AdvancedSettings2 { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060007C0 RID: 1984
		[DispId(102)]
		public virtual extern IMsRdpClientSecuredSettings SecuredSettings2 { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060007C1 RID: 1985
		[DispId(103)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode ExtendedDisconnectReason { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060007C3 RID: 1987
		// (set) Token: 0x060007C2 RID: 1986
		[DispId(104)]
		public virtual extern bool FullScreen { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x060007C4 RID: 1988
		[DispId(35)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x060007C5 RID: 1989
		[DispId(36)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x060007C6 RID: 1990
		[DispId(37)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus RequestClose();

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060007C7 RID: 1991
		[DispId(200)]
		public virtual extern IMsRdpClientAdvancedSettings2 AdvancedSettings3 { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060007C9 RID: 1993
		// (set) Token: 0x060007C8 RID: 1992
		[DispId(201)]
		public virtual extern string ConnectedStatusText { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060007CA RID: 1994
		[DispId(300)]
		public virtual extern IMsRdpClientAdvancedSettings3 AdvancedSettings4 { [DispId(300)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x060007CB RID: 1995
		// (remove) Token: 0x060007CC RID: 1996
		public virtual extern event IMsTscAxEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x1400012E RID: 302
		// (add) Token: 0x060007CD RID: 1997
		// (remove) Token: 0x060007CE RID: 1998
		public virtual extern event IMsTscAxEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x1400012F RID: 303
		// (add) Token: 0x060007CF RID: 1999
		// (remove) Token: 0x060007D0 RID: 2000
		public virtual extern event IMsTscAxEvents_OnLoginCompleteEventHandler OnLoginComplete;

		// Token: 0x14000130 RID: 304
		// (add) Token: 0x060007D1 RID: 2001
		// (remove) Token: 0x060007D2 RID: 2002
		public virtual extern event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000131 RID: 305
		// (add) Token: 0x060007D3 RID: 2003
		// (remove) Token: 0x060007D4 RID: 2004
		public virtual extern event IMsTscAxEvents_OnEnterFullScreenModeEventHandler OnEnterFullScreenMode;

		// Token: 0x14000132 RID: 306
		// (add) Token: 0x060007D5 RID: 2005
		// (remove) Token: 0x060007D6 RID: 2006
		public virtual extern event IMsTscAxEvents_OnLeaveFullScreenModeEventHandler OnLeaveFullScreenMode;

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x060007D7 RID: 2007
		// (remove) Token: 0x060007D8 RID: 2008
		public virtual extern event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x060007D9 RID: 2009
		// (remove) Token: 0x060007DA RID: 2010
		public virtual extern event IMsTscAxEvents_OnRequestGoFullScreenEventHandler OnRequestGoFullScreen;

		// Token: 0x14000135 RID: 309
		// (add) Token: 0x060007DB RID: 2011
		// (remove) Token: 0x060007DC RID: 2012
		public virtual extern event IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x060007DD RID: 2013
		// (remove) Token: 0x060007DE RID: 2014
		public virtual extern event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x060007DF RID: 2015
		// (remove) Token: 0x060007E0 RID: 2016
		public virtual extern event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x060007E1 RID: 2017
		// (remove) Token: 0x060007E2 RID: 2018
		public virtual extern event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x060007E3 RID: 2019
		// (remove) Token: 0x060007E4 RID: 2020
		public virtual extern event IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler OnIdleTimeoutNotification;

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x060007E5 RID: 2021
		// (remove) Token: 0x060007E6 RID: 2022
		public virtual extern event IMsTscAxEvents_OnRequestContainerMinimizeEventHandler OnRequestContainerMinimize;

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x060007E7 RID: 2023
		// (remove) Token: 0x060007E8 RID: 2024
		public virtual extern event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x060007E9 RID: 2025
		// (remove) Token: 0x060007EA RID: 2026
		public virtual extern event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x060007EB RID: 2027
		// (remove) Token: 0x060007EC RID: 2028
		public virtual extern event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x060007ED RID: 2029
		// (remove) Token: 0x060007EE RID: 2030
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x060007EF RID: 2031
		// (remove) Token: 0x060007F0 RID: 2032
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x060007F1 RID: 2033
		// (remove) Token: 0x060007F2 RID: 2034
		public virtual extern event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x060007F3 RID: 2035
		// (remove) Token: 0x060007F4 RID: 2036
		public virtual extern event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x060007F5 RID: 2037
		// (remove) Token: 0x060007F6 RID: 2038
		public virtual extern event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x060007F7 RID: 2039
		// (remove) Token: 0x060007F8 RID: 2040
		public virtual extern event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x060007F9 RID: 2041
		// (remove) Token: 0x060007FA RID: 2042
		public virtual extern event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x060007FB RID: 2043
		// (remove) Token: 0x060007FC RID: 2044
		public virtual extern event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x060007FD RID: 2045
		// (remove) Token: 0x060007FE RID: 2046
		public virtual extern event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000147 RID: 327
		// (add) Token: 0x060007FF RID: 2047
		// (remove) Token: 0x06000800 RID: 2048
		public virtual extern event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000148 RID: 328
		// (add) Token: 0x06000801 RID: 2049
		// (remove) Token: 0x06000802 RID: 2050
		public virtual extern event IMsTscAxEvents_OnConnectionBarPullDownEventHandler OnConnectionBarPullDown;

		// Token: 0x14000149 RID: 329
		// (add) Token: 0x06000803 RID: 2051
		// (remove) Token: 0x06000804 RID: 2052
		public virtual extern event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400014A RID: 330
		// (add) Token: 0x06000805 RID: 2053
		// (remove) Token: 0x06000806 RID: 2054
		public virtual extern event IMsTscAxEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000808 RID: 2056
		// (set) Token: 0x06000807 RID: 2055
		public virtual extern string IMsRdpClient2_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600080A RID: 2058
		// (set) Token: 0x06000809 RID: 2057
		public virtual extern string IMsRdpClient2_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600080C RID: 2060
		// (set) Token: 0x0600080B RID: 2059
		public virtual extern string IMsRdpClient2_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600080E RID: 2062
		// (set) Token: 0x0600080D RID: 2061
		public virtual extern string IMsRdpClient2_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000810 RID: 2064
		// (set) Token: 0x0600080F RID: 2063
		public virtual extern string IMsRdpClient2_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000811 RID: 2065
		public virtual extern short IMsRdpClient2_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000813 RID: 2067
		// (set) Token: 0x06000812 RID: 2066
		public virtual extern int IMsRdpClient2_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000815 RID: 2069
		// (set) Token: 0x06000814 RID: 2068
		public virtual extern int IMsRdpClient2_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000817 RID: 2071
		// (set) Token: 0x06000816 RID: 2070
		public virtual extern int IMsRdpClient2_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000818 RID: 2072
		public virtual extern int IMsRdpClient2_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000819 RID: 2073
		public virtual extern int IMsRdpClient2_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000308 RID: 776
		// (set) Token: 0x0600081A RID: 2074
		public virtual extern string IMsRdpClient2_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600081B RID: 2075
		public virtual extern int IMsRdpClient2_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600081C RID: 2076
		public virtual extern string IMsRdpClient2_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600081D RID: 2077
		public virtual extern int IMsRdpClient2_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600081E RID: 2078
		public virtual extern IMsTscSecuredSettings IMsRdpClient2_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600081F RID: 2079
		public virtual extern IMsTscAdvancedSettings IMsRdpClient2_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000820 RID: 2080
		public virtual extern IMsTscDebug IMsRdpClient2_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000821 RID: 2081
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Connect();

		// Token: 0x06000822 RID: 2082
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Disconnect();

		// Token: 0x06000823 RID: 2083
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000824 RID: 2084
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000826 RID: 2086
		// (set) Token: 0x06000825 RID: 2085
		public virtual extern int IMsRdpClient2_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000827 RID: 2087
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient2_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000828 RID: 2088
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient2_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000829 RID: 2089
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient2_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600082B RID: 2091
		// (set) Token: 0x0600082A RID: 2090
		public virtual extern bool IMsRdpClient2_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x0600082C RID: 2092
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x0600082D RID: 2093
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient2_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x0600082E RID: 2094
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient2_RequestClose();

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600082F RID: 2095
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient2_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000831 RID: 2097
		// (set) Token: 0x06000830 RID: 2096
		public virtual extern string IMsRdpClient2_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000833 RID: 2099
		// (set) Token: 0x06000832 RID: 2098
		public virtual extern string IMsRdpClient_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000835 RID: 2101
		// (set) Token: 0x06000834 RID: 2100
		public virtual extern string IMsRdpClient_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000837 RID: 2103
		// (set) Token: 0x06000836 RID: 2102
		public virtual extern string IMsRdpClient_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000839 RID: 2105
		// (set) Token: 0x06000838 RID: 2104
		public virtual extern string IMsRdpClient_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600083B RID: 2107
		// (set) Token: 0x0600083A RID: 2106
		public virtual extern string IMsRdpClient_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600083C RID: 2108
		public virtual extern short IMsRdpClient_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x0600083E RID: 2110
		// (set) Token: 0x0600083D RID: 2109
		public virtual extern int IMsRdpClient_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000840 RID: 2112
		// (set) Token: 0x0600083F RID: 2111
		public virtual extern int IMsRdpClient_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000842 RID: 2114
		// (set) Token: 0x06000841 RID: 2113
		public virtual extern int IMsRdpClient_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000843 RID: 2115
		public virtual extern int IMsRdpClient_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000844 RID: 2116
		public virtual extern int IMsRdpClient_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000321 RID: 801
		// (set) Token: 0x06000845 RID: 2117
		public virtual extern string IMsRdpClient_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000846 RID: 2118
		public virtual extern int IMsRdpClient_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000847 RID: 2119
		public virtual extern string IMsRdpClient_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000848 RID: 2120
		public virtual extern int IMsRdpClient_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000849 RID: 2121
		public virtual extern IMsTscSecuredSettings IMsRdpClient_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600084A RID: 2122
		public virtual extern IMsTscAdvancedSettings IMsRdpClient_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600084B RID: 2123
		public virtual extern IMsTscDebug IMsRdpClient_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x0600084C RID: 2124
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Connect();

		// Token: 0x0600084D RID: 2125
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Disconnect();

		// Token: 0x0600084E RID: 2126
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x0600084F RID: 2127
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000851 RID: 2129
		// (set) Token: 0x06000850 RID: 2128
		public virtual extern int IMsRdpClient_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000852 RID: 2130
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000853 RID: 2131
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000854 RID: 2132
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000856 RID: 2134
		// (set) Token: 0x06000855 RID: 2133
		public virtual extern bool IMsRdpClient_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000857 RID: 2135
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000858 RID: 2136
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000859 RID: 2137
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient_RequestClose();

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600085B RID: 2139
		// (set) Token: 0x0600085A RID: 2138
		public virtual extern string IMsTscAx_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600085D RID: 2141
		// (set) Token: 0x0600085C RID: 2140
		public virtual extern string IMsTscAx_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600085F RID: 2143
		// (set) Token: 0x0600085E RID: 2142
		public virtual extern string IMsTscAx_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000861 RID: 2145
		// (set) Token: 0x06000860 RID: 2144
		public virtual extern string IMsTscAx_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000863 RID: 2147
		// (set) Token: 0x06000862 RID: 2146
		public virtual extern string IMsTscAx_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000864 RID: 2148
		public virtual extern short IMsTscAx_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000866 RID: 2150
		// (set) Token: 0x06000865 RID: 2149
		public virtual extern int IMsTscAx_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000868 RID: 2152
		// (set) Token: 0x06000867 RID: 2151
		public virtual extern int IMsTscAx_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600086A RID: 2154
		// (set) Token: 0x06000869 RID: 2153
		public virtual extern int IMsTscAx_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600086B RID: 2155
		public virtual extern int IMsTscAx_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600086C RID: 2156
		public virtual extern int IMsTscAx_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000338 RID: 824
		// (set) Token: 0x0600086D RID: 2157
		public virtual extern string IMsTscAx_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600086E RID: 2158
		public virtual extern int IMsTscAx_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600086F RID: 2159
		public virtual extern string IMsTscAx_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000870 RID: 2160
		public virtual extern int IMsTscAx_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000871 RID: 2161
		public virtual extern IMsTscSecuredSettings IMsTscAx_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000872 RID: 2162
		public virtual extern IMsTscAdvancedSettings IMsTscAx_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000873 RID: 2163
		public virtual extern IMsTscDebug IMsTscAx_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000874 RID: 2164
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Connect();

		// Token: 0x06000875 RID: 2165
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Disconnect();

		// Token: 0x06000876 RID: 2166
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000877 RID: 2167
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x1700033F RID: 831
		// (set) Token: 0x06000878 RID: 2168
		public virtual extern string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600087A RID: 2170
		// (set) Token: 0x06000879 RID: 2169
		public virtual extern string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600087C RID: 2172
		// (set) Token: 0x0600087B RID: 2171
		public virtual extern string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600087E RID: 2174
		// (set) Token: 0x0600087D RID: 2173
		public virtual extern string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000880 RID: 2176
		// (set) Token: 0x0600087F RID: 2175
		public virtual extern string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000881 RID: 2177
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void ResetPassword();

		// Token: 0x17000344 RID: 836
		// (set) Token: 0x06000882 RID: 2178
		public virtual extern string IMsRdpClientNonScriptable_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000884 RID: 2180
		// (set) Token: 0x06000883 RID: 2179
		public virtual extern string IMsRdpClientNonScriptable_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000886 RID: 2182
		// (set) Token: 0x06000885 RID: 2181
		public virtual extern string IMsRdpClientNonScriptable_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000888 RID: 2184
		// (set) Token: 0x06000887 RID: 2183
		public virtual extern string IMsRdpClientNonScriptable_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600088A RID: 2186
		// (set) Token: 0x06000889 RID: 2185
		public virtual extern string IMsRdpClientNonScriptable_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600088B RID: 2187
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable_ResetPassword();

		// Token: 0x0600088C RID: 2188
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x0600088D RID: 2189
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);
	}
}
