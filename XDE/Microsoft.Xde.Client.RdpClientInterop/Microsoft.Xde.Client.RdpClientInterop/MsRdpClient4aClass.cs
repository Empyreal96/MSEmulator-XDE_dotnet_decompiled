using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000026 RID: 38
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("Microsoft.Xde.Client.RdpClientInterop.IMsTscAxEvents\0\0")]
	[ComConversionLoss]
	[Guid("54CE37E0-9834-41AE-9896-4DAB69DC022B")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate | TypeLibTypeFlags.FControl)]
	[ComImport]
	public class MsRdpClient4aClass : IMsRdpClient4, MsRdpClient4a, IMsTscAxEvents_Event, IMsRdpClient3, IMsRdpClient2, IMsRdpClient, IMsTscAx, IMsTscAx_Redist, IMsTscNonScriptable, IMsRdpClientNonScriptable, IMsRdpClientNonScriptable2
	{
		// Token: 0x06000B1F RID: 2847
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern MsRdpClient4aClass();

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000B21 RID: 2849
		// (set) Token: 0x06000B20 RID: 2848
		[DispId(1)]
		public virtual extern string Server { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000B23 RID: 2851
		// (set) Token: 0x06000B22 RID: 2850
		[DispId(2)]
		public virtual extern string Domain { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000B25 RID: 2853
		// (set) Token: 0x06000B24 RID: 2852
		[DispId(3)]
		public virtual extern string UserName { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000B27 RID: 2855
		// (set) Token: 0x06000B26 RID: 2854
		[DispId(4)]
		public virtual extern string DisconnectedText { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000B29 RID: 2857
		// (set) Token: 0x06000B28 RID: 2856
		[DispId(5)]
		public virtual extern string ConnectingText { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000B2A RID: 2858
		[DispId(6)]
		public virtual extern short Connected { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000B2C RID: 2860
		// (set) Token: 0x06000B2B RID: 2859
		[DispId(12)]
		public virtual extern int DesktopWidth { [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000B2E RID: 2862
		// (set) Token: 0x06000B2D RID: 2861
		[DispId(13)]
		public virtual extern int DesktopHeight { [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000B30 RID: 2864
		// (set) Token: 0x06000B2F RID: 2863
		[DispId(16)]
		public virtual extern int StartConnected { [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000B31 RID: 2865
		[DispId(17)]
		public virtual extern int HorizontalScrollBarVisible { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000B32 RID: 2866
		[DispId(18)]
		public virtual extern int VerticalScrollBarVisible { [DispId(18)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000483 RID: 1155
		// (set) Token: 0x06000B33 RID: 2867
		[DispId(19)]
		public virtual extern string FullScreenTitle { [DispId(19)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000B34 RID: 2868
		[DispId(20)]
		public virtual extern int CipherStrength { [DispId(20)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000B35 RID: 2869
		[DispId(21)]
		public virtual extern string Version { [DispId(21)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000B36 RID: 2870
		[DispId(22)]
		public virtual extern int SecuredSettingsEnabled { [DispId(22)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000B37 RID: 2871
		[DispId(97)]
		public virtual extern IMsTscSecuredSettings SecuredSettings { [DispId(97)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000B38 RID: 2872
		[DispId(98)]
		public virtual extern IMsTscAdvancedSettings AdvancedSettings { [DispId(98)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000B39 RID: 2873
		[DispId(99)]
		public virtual extern IMsTscDebug Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [DispId(99)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000B3A RID: 2874
		[DispId(30)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Connect();

		// Token: 0x06000B3B RID: 2875
		[DispId(31)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Disconnect();

		// Token: 0x06000B3C RID: 2876
		[DispId(33)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000B3D RID: 2877
		[DispId(34)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000B3F RID: 2879
		// (set) Token: 0x06000B3E RID: 2878
		[DispId(100)]
		public virtual extern int ColorDepth { [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000B40 RID: 2880
		[DispId(101)]
		public virtual extern IMsRdpClientAdvancedSettings AdvancedSettings2 { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000B41 RID: 2881
		[DispId(102)]
		public virtual extern IMsRdpClientSecuredSettings SecuredSettings2 { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000B42 RID: 2882
		[DispId(103)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode ExtendedDisconnectReason { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000B44 RID: 2884
		// (set) Token: 0x06000B43 RID: 2883
		[DispId(104)]
		public virtual extern bool FullScreen { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000B45 RID: 2885
		[DispId(35)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000B46 RID: 2886
		[DispId(36)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000B47 RID: 2887
		[DispId(37)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus RequestClose();

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000B48 RID: 2888
		[DispId(200)]
		public virtual extern IMsRdpClientAdvancedSettings2 AdvancedSettings3 { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000B4A RID: 2890
		// (set) Token: 0x06000B49 RID: 2889
		[DispId(201)]
		public virtual extern string ConnectedStatusText { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000B4B RID: 2891
		[DispId(300)]
		public virtual extern IMsRdpClientAdvancedSettings3 AdvancedSettings4 { [DispId(300)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000B4C RID: 2892
		[DispId(400)]
		public virtual extern IMsRdpClientAdvancedSettings4 AdvancedSettings5 { [DispId(400)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x14000187 RID: 391
		// (add) Token: 0x06000B4D RID: 2893
		// (remove) Token: 0x06000B4E RID: 2894
		public virtual extern event IMsTscAxEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x14000188 RID: 392
		// (add) Token: 0x06000B4F RID: 2895
		// (remove) Token: 0x06000B50 RID: 2896
		public virtual extern event IMsTscAxEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x14000189 RID: 393
		// (add) Token: 0x06000B51 RID: 2897
		// (remove) Token: 0x06000B52 RID: 2898
		public virtual extern event IMsTscAxEvents_OnLoginCompleteEventHandler OnLoginComplete;

		// Token: 0x1400018A RID: 394
		// (add) Token: 0x06000B53 RID: 2899
		// (remove) Token: 0x06000B54 RID: 2900
		public virtual extern event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x1400018B RID: 395
		// (add) Token: 0x06000B55 RID: 2901
		// (remove) Token: 0x06000B56 RID: 2902
		public virtual extern event IMsTscAxEvents_OnEnterFullScreenModeEventHandler OnEnterFullScreenMode;

		// Token: 0x1400018C RID: 396
		// (add) Token: 0x06000B57 RID: 2903
		// (remove) Token: 0x06000B58 RID: 2904
		public virtual extern event IMsTscAxEvents_OnLeaveFullScreenModeEventHandler OnLeaveFullScreenMode;

		// Token: 0x1400018D RID: 397
		// (add) Token: 0x06000B59 RID: 2905
		// (remove) Token: 0x06000B5A RID: 2906
		public virtual extern event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x1400018E RID: 398
		// (add) Token: 0x06000B5B RID: 2907
		// (remove) Token: 0x06000B5C RID: 2908
		public virtual extern event IMsTscAxEvents_OnRequestGoFullScreenEventHandler OnRequestGoFullScreen;

		// Token: 0x1400018F RID: 399
		// (add) Token: 0x06000B5D RID: 2909
		// (remove) Token: 0x06000B5E RID: 2910
		public virtual extern event IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000190 RID: 400
		// (add) Token: 0x06000B5F RID: 2911
		// (remove) Token: 0x06000B60 RID: 2912
		public virtual extern event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000191 RID: 401
		// (add) Token: 0x06000B61 RID: 2913
		// (remove) Token: 0x06000B62 RID: 2914
		public virtual extern event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000192 RID: 402
		// (add) Token: 0x06000B63 RID: 2915
		// (remove) Token: 0x06000B64 RID: 2916
		public virtual extern event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000193 RID: 403
		// (add) Token: 0x06000B65 RID: 2917
		// (remove) Token: 0x06000B66 RID: 2918
		public virtual extern event IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler OnIdleTimeoutNotification;

		// Token: 0x14000194 RID: 404
		// (add) Token: 0x06000B67 RID: 2919
		// (remove) Token: 0x06000B68 RID: 2920
		public virtual extern event IMsTscAxEvents_OnRequestContainerMinimizeEventHandler OnRequestContainerMinimize;

		// Token: 0x14000195 RID: 405
		// (add) Token: 0x06000B69 RID: 2921
		// (remove) Token: 0x06000B6A RID: 2922
		public virtual extern event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000196 RID: 406
		// (add) Token: 0x06000B6B RID: 2923
		// (remove) Token: 0x06000B6C RID: 2924
		public virtual extern event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000197 RID: 407
		// (add) Token: 0x06000B6D RID: 2925
		// (remove) Token: 0x06000B6E RID: 2926
		public virtual extern event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000198 RID: 408
		// (add) Token: 0x06000B6F RID: 2927
		// (remove) Token: 0x06000B70 RID: 2928
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000199 RID: 409
		// (add) Token: 0x06000B71 RID: 2929
		// (remove) Token: 0x06000B72 RID: 2930
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler OnAuthenticationWarningDismissed;

		// Token: 0x1400019A RID: 410
		// (add) Token: 0x06000B73 RID: 2931
		// (remove) Token: 0x06000B74 RID: 2932
		public virtual extern event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x1400019B RID: 411
		// (add) Token: 0x06000B75 RID: 2933
		// (remove) Token: 0x06000B76 RID: 2934
		public virtual extern event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x1400019C RID: 412
		// (add) Token: 0x06000B77 RID: 2935
		// (remove) Token: 0x06000B78 RID: 2936
		public virtual extern event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x1400019D RID: 413
		// (add) Token: 0x06000B79 RID: 2937
		// (remove) Token: 0x06000B7A RID: 2938
		public virtual extern event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x1400019E RID: 414
		// (add) Token: 0x06000B7B RID: 2939
		// (remove) Token: 0x06000B7C RID: 2940
		public virtual extern event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x1400019F RID: 415
		// (add) Token: 0x06000B7D RID: 2941
		// (remove) Token: 0x06000B7E RID: 2942
		public virtual extern event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140001A0 RID: 416
		// (add) Token: 0x06000B7F RID: 2943
		// (remove) Token: 0x06000B80 RID: 2944
		public virtual extern event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140001A1 RID: 417
		// (add) Token: 0x06000B81 RID: 2945
		// (remove) Token: 0x06000B82 RID: 2946
		public virtual extern event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140001A2 RID: 418
		// (add) Token: 0x06000B83 RID: 2947
		// (remove) Token: 0x06000B84 RID: 2948
		public virtual extern event IMsTscAxEvents_OnConnectionBarPullDownEventHandler OnConnectionBarPullDown;

		// Token: 0x140001A3 RID: 419
		// (add) Token: 0x06000B85 RID: 2949
		// (remove) Token: 0x06000B86 RID: 2950
		public virtual extern event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140001A4 RID: 420
		// (add) Token: 0x06000B87 RID: 2951
		// (remove) Token: 0x06000B88 RID: 2952
		public virtual extern event IMsTscAxEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000B8A RID: 2954
		// (set) Token: 0x06000B89 RID: 2953
		public virtual extern string IMsRdpClient3_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000B8C RID: 2956
		// (set) Token: 0x06000B8B RID: 2955
		public virtual extern string IMsRdpClient3_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000B8E RID: 2958
		// (set) Token: 0x06000B8D RID: 2957
		public virtual extern string IMsRdpClient3_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000B90 RID: 2960
		// (set) Token: 0x06000B8F RID: 2959
		public virtual extern string IMsRdpClient3_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000B92 RID: 2962
		// (set) Token: 0x06000B91 RID: 2961
		public virtual extern string IMsRdpClient3_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000B93 RID: 2963
		public virtual extern short IMsRdpClient3_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000B95 RID: 2965
		// (set) Token: 0x06000B94 RID: 2964
		public virtual extern int IMsRdpClient3_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000B97 RID: 2967
		// (set) Token: 0x06000B96 RID: 2966
		public virtual extern int IMsRdpClient3_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000B99 RID: 2969
		// (set) Token: 0x06000B98 RID: 2968
		public virtual extern int IMsRdpClient3_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000B9A RID: 2970
		public virtual extern int IMsRdpClient3_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000B9B RID: 2971
		public virtual extern int IMsRdpClient3_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700049E RID: 1182
		// (set) Token: 0x06000B9C RID: 2972
		public virtual extern string IMsRdpClient3_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000B9D RID: 2973
		public virtual extern int IMsRdpClient3_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000B9E RID: 2974
		public virtual extern string IMsRdpClient3_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000B9F RID: 2975
		public virtual extern int IMsRdpClient3_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000BA0 RID: 2976
		public virtual extern IMsTscSecuredSettings IMsRdpClient3_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000BA1 RID: 2977
		public virtual extern IMsTscAdvancedSettings IMsRdpClient3_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000BA2 RID: 2978
		public virtual extern IMsTscDebug IMsRdpClient3_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000BA3 RID: 2979
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_Connect();

		// Token: 0x06000BA4 RID: 2980
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_Disconnect();

		// Token: 0x06000BA5 RID: 2981
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000BA6 RID: 2982
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000BA8 RID: 2984
		// (set) Token: 0x06000BA7 RID: 2983
		public virtual extern int IMsRdpClient3_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000BA9 RID: 2985
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient3_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000BAA RID: 2986
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient3_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000BAB RID: 2987
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient3_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000BAD RID: 2989
		// (set) Token: 0x06000BAC RID: 2988
		public virtual extern bool IMsRdpClient3_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000BAE RID: 2990
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000BAF RID: 2991
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient3_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000BB0 RID: 2992
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient3_RequestClose();

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000BB1 RID: 2993
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient3_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000BB3 RID: 2995
		// (set) Token: 0x06000BB2 RID: 2994
		public virtual extern string IMsRdpClient3_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000BB4 RID: 2996
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient3_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000BB6 RID: 2998
		// (set) Token: 0x06000BB5 RID: 2997
		public virtual extern string IMsRdpClient2_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000BB8 RID: 3000
		// (set) Token: 0x06000BB7 RID: 2999
		public virtual extern string IMsRdpClient2_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000BBA RID: 3002
		// (set) Token: 0x06000BB9 RID: 3001
		public virtual extern string IMsRdpClient2_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000BBC RID: 3004
		// (set) Token: 0x06000BBB RID: 3003
		public virtual extern string IMsRdpClient2_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000BBE RID: 3006
		// (set) Token: 0x06000BBD RID: 3005
		public virtual extern string IMsRdpClient2_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000BBF RID: 3007
		public virtual extern short IMsRdpClient2_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000BC1 RID: 3009
		// (set) Token: 0x06000BC0 RID: 3008
		public virtual extern int IMsRdpClient2_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000BC3 RID: 3011
		// (set) Token: 0x06000BC2 RID: 3010
		public virtual extern int IMsRdpClient2_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000BC5 RID: 3013
		// (set) Token: 0x06000BC4 RID: 3012
		public virtual extern int IMsRdpClient2_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000BC6 RID: 3014
		public virtual extern int IMsRdpClient2_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000BC7 RID: 3015
		public virtual extern int IMsRdpClient2_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004B8 RID: 1208
		// (set) Token: 0x06000BC8 RID: 3016
		public virtual extern string IMsRdpClient2_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000BC9 RID: 3017
		public virtual extern int IMsRdpClient2_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000BCA RID: 3018
		public virtual extern string IMsRdpClient2_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000BCB RID: 3019
		public virtual extern int IMsRdpClient2_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000BCC RID: 3020
		public virtual extern IMsTscSecuredSettings IMsRdpClient2_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000BCD RID: 3021
		public virtual extern IMsTscAdvancedSettings IMsRdpClient2_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06000BCE RID: 3022
		public virtual extern IMsTscDebug IMsRdpClient2_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000BCF RID: 3023
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Connect();

		// Token: 0x06000BD0 RID: 3024
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Disconnect();

		// Token: 0x06000BD1 RID: 3025
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000BD2 RID: 3026
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06000BD4 RID: 3028
		// (set) Token: 0x06000BD3 RID: 3027
		public virtual extern int IMsRdpClient2_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000BD5 RID: 3029
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient2_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000BD6 RID: 3030
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient2_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000BD7 RID: 3031
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient2_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000BD9 RID: 3033
		// (set) Token: 0x06000BD8 RID: 3032
		public virtual extern bool IMsRdpClient2_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000BDA RID: 3034
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000BDB RID: 3035
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient2_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000BDC RID: 3036
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient2_RequestClose();

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000BDD RID: 3037
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient2_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000BDF RID: 3039
		// (set) Token: 0x06000BDE RID: 3038
		public virtual extern string IMsRdpClient2_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000BE1 RID: 3041
		// (set) Token: 0x06000BE0 RID: 3040
		public virtual extern string IMsRdpClient_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000BE3 RID: 3043
		// (set) Token: 0x06000BE2 RID: 3042
		public virtual extern string IMsRdpClient_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000BE5 RID: 3045
		// (set) Token: 0x06000BE4 RID: 3044
		public virtual extern string IMsRdpClient_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000BE7 RID: 3047
		// (set) Token: 0x06000BE6 RID: 3046
		public virtual extern string IMsRdpClient_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000BE9 RID: 3049
		// (set) Token: 0x06000BE8 RID: 3048
		public virtual extern string IMsRdpClient_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000BEA RID: 3050
		public virtual extern short IMsRdpClient_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000BEC RID: 3052
		// (set) Token: 0x06000BEB RID: 3051
		public virtual extern int IMsRdpClient_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000BEE RID: 3054
		// (set) Token: 0x06000BED RID: 3053
		public virtual extern int IMsRdpClient_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000BF0 RID: 3056
		// (set) Token: 0x06000BEF RID: 3055
		public virtual extern int IMsRdpClient_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000BF1 RID: 3057
		public virtual extern int IMsRdpClient_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000BF2 RID: 3058
		public virtual extern int IMsRdpClient_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004D1 RID: 1233
		// (set) Token: 0x06000BF3 RID: 3059
		public virtual extern string IMsRdpClient_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000BF4 RID: 3060
		public virtual extern int IMsRdpClient_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000BF5 RID: 3061
		public virtual extern string IMsRdpClient_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000BF6 RID: 3062
		public virtual extern int IMsRdpClient_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000BF7 RID: 3063
		public virtual extern IMsTscSecuredSettings IMsRdpClient_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000BF8 RID: 3064
		public virtual extern IMsTscAdvancedSettings IMsRdpClient_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000BF9 RID: 3065
		public virtual extern IMsTscDebug IMsRdpClient_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000BFA RID: 3066
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Connect();

		// Token: 0x06000BFB RID: 3067
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Disconnect();

		// Token: 0x06000BFC RID: 3068
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000BFD RID: 3069
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000BFF RID: 3071
		// (set) Token: 0x06000BFE RID: 3070
		public virtual extern int IMsRdpClient_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000C00 RID: 3072
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000C01 RID: 3073
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000C02 RID: 3074
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000C04 RID: 3076
		// (set) Token: 0x06000C03 RID: 3075
		public virtual extern bool IMsRdpClient_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000C05 RID: 3077
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000C06 RID: 3078
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000C07 RID: 3079
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient_RequestClose();

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000C09 RID: 3081
		// (set) Token: 0x06000C08 RID: 3080
		public virtual extern string IMsTscAx_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000C0B RID: 3083
		// (set) Token: 0x06000C0A RID: 3082
		public virtual extern string IMsTscAx_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000C0D RID: 3085
		// (set) Token: 0x06000C0C RID: 3084
		public virtual extern string IMsTscAx_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000C0F RID: 3087
		// (set) Token: 0x06000C0E RID: 3086
		public virtual extern string IMsTscAx_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000C11 RID: 3089
		// (set) Token: 0x06000C10 RID: 3088
		public virtual extern string IMsTscAx_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000C12 RID: 3090
		public virtual extern short IMsTscAx_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000C14 RID: 3092
		// (set) Token: 0x06000C13 RID: 3091
		public virtual extern int IMsTscAx_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000C16 RID: 3094
		// (set) Token: 0x06000C15 RID: 3093
		public virtual extern int IMsTscAx_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000C18 RID: 3096
		// (set) Token: 0x06000C17 RID: 3095
		public virtual extern int IMsTscAx_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000C19 RID: 3097
		public virtual extern int IMsTscAx_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000C1A RID: 3098
		public virtual extern int IMsTscAx_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004E8 RID: 1256
		// (set) Token: 0x06000C1B RID: 3099
		public virtual extern string IMsTscAx_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000C1C RID: 3100
		public virtual extern int IMsTscAx_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000C1D RID: 3101
		public virtual extern string IMsTscAx_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000C1E RID: 3102
		public virtual extern int IMsTscAx_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000C1F RID: 3103
		public virtual extern IMsTscSecuredSettings IMsTscAx_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000C20 RID: 3104
		public virtual extern IMsTscAdvancedSettings IMsTscAx_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000C21 RID: 3105
		public virtual extern IMsTscDebug IMsTscAx_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000C22 RID: 3106
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Connect();

		// Token: 0x06000C23 RID: 3107
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Disconnect();

		// Token: 0x06000C24 RID: 3108
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000C25 RID: 3109
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170004EF RID: 1263
		// (set) Token: 0x06000C26 RID: 3110
		public virtual extern string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000C28 RID: 3112
		// (set) Token: 0x06000C27 RID: 3111
		public virtual extern string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000C2A RID: 3114
		// (set) Token: 0x06000C29 RID: 3113
		public virtual extern string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000C2C RID: 3116
		// (set) Token: 0x06000C2B RID: 3115
		public virtual extern string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000C2E RID: 3118
		// (set) Token: 0x06000C2D RID: 3117
		public virtual extern string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000C2F RID: 3119
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void ResetPassword();

		// Token: 0x170004F4 RID: 1268
		// (set) Token: 0x06000C30 RID: 3120
		public virtual extern string IMsRdpClientNonScriptable_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000C32 RID: 3122
		// (set) Token: 0x06000C31 RID: 3121
		public virtual extern string IMsRdpClientNonScriptable_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000C34 RID: 3124
		// (set) Token: 0x06000C33 RID: 3123
		public virtual extern string IMsRdpClientNonScriptable_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000C36 RID: 3126
		// (set) Token: 0x06000C35 RID: 3125
		public virtual extern string IMsRdpClientNonScriptable_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000C38 RID: 3128
		// (set) Token: 0x06000C37 RID: 3127
		public virtual extern string IMsRdpClientNonScriptable_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000C39 RID: 3129
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable_ResetPassword();

		// Token: 0x06000C3A RID: 3130
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000C3B RID: 3131
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170004F9 RID: 1273
		// (set) Token: 0x06000C3C RID: 3132
		public virtual extern string IMsRdpClientNonScriptable2_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000C3E RID: 3134
		// (set) Token: 0x06000C3D RID: 3133
		public virtual extern string IMsRdpClientNonScriptable2_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000C40 RID: 3136
		// (set) Token: 0x06000C3F RID: 3135
		public virtual extern string IMsRdpClientNonScriptable2_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000C42 RID: 3138
		// (set) Token: 0x06000C41 RID: 3137
		public virtual extern string IMsRdpClientNonScriptable2_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000C44 RID: 3140
		// (set) Token: 0x06000C43 RID: 3139
		public virtual extern string IMsRdpClientNonScriptable2_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000C45 RID: 3141
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_ResetPassword();

		// Token: 0x06000C46 RID: 3142
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000C47 RID: 3143
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000C49 RID: 3145
		// (set) Token: 0x06000C48 RID: 3144
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }
	}
}
