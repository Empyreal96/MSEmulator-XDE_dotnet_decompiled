using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200002A RID: 42
	[ComSourceInterfaces("Microsoft.Xde.Client.RdpClientInterop.IMsTscAxEvents\0\0")]
	[Guid("4EB2F086-C818-447E-B32C-C51CE2B30D31")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate | TypeLibTypeFlags.FControl)]
	[ComConversionLoss]
	[ClassInterface(ClassInterfaceType.None)]
	[ComImport]
	public class MsRdpClient5NotSafeForScriptingClass : IMsRdpClient5, MsRdpClient5NotSafeForScripting, IMsTscAxEvents_Event, IMsRdpClient4, IMsRdpClient3, IMsRdpClient2, IMsRdpClient, IMsTscAx, IMsTscAx_Redist, IMsTscNonScriptable, IMsRdpClientNonScriptable, IMsRdpClientNonScriptable2, IMsRdpClientNonScriptable3
	{
		// Token: 0x06000C9E RID: 3230
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern MsRdpClient5NotSafeForScriptingClass();

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06000CA0 RID: 3232
		// (set) Token: 0x06000C9F RID: 3231
		[DispId(1)]
		public virtual extern string Server { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06000CA2 RID: 3234
		// (set) Token: 0x06000CA1 RID: 3233
		[DispId(2)]
		public virtual extern string Domain { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06000CA4 RID: 3236
		// (set) Token: 0x06000CA3 RID: 3235
		[DispId(3)]
		public virtual extern string UserName { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06000CA6 RID: 3238
		// (set) Token: 0x06000CA5 RID: 3237
		[DispId(4)]
		public virtual extern string DisconnectedText { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06000CA8 RID: 3240
		// (set) Token: 0x06000CA7 RID: 3239
		[DispId(5)]
		public virtual extern string ConnectingText { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06000CA9 RID: 3241
		[DispId(6)]
		public virtual extern short Connected { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06000CAB RID: 3243
		// (set) Token: 0x06000CAA RID: 3242
		[DispId(12)]
		public virtual extern int DesktopWidth { [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06000CAD RID: 3245
		// (set) Token: 0x06000CAC RID: 3244
		[DispId(13)]
		public virtual extern int DesktopHeight { [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06000CAF RID: 3247
		// (set) Token: 0x06000CAE RID: 3246
		[DispId(16)]
		public virtual extern int StartConnected { [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06000CB0 RID: 3248
		[DispId(17)]
		public virtual extern int HorizontalScrollBarVisible { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06000CB1 RID: 3249
		[DispId(18)]
		public virtual extern int VerticalScrollBarVisible { [DispId(18)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700053A RID: 1338
		// (set) Token: 0x06000CB2 RID: 3250
		[DispId(19)]
		public virtual extern string FullScreenTitle { [DispId(19)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06000CB3 RID: 3251
		[DispId(20)]
		public virtual extern int CipherStrength { [DispId(20)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06000CB4 RID: 3252
		[DispId(21)]
		public virtual extern string Version { [DispId(21)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06000CB5 RID: 3253
		[DispId(22)]
		public virtual extern int SecuredSettingsEnabled { [DispId(22)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06000CB6 RID: 3254
		[DispId(97)]
		public virtual extern IMsTscSecuredSettings SecuredSettings { [DispId(97)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06000CB7 RID: 3255
		[DispId(98)]
		public virtual extern IMsTscAdvancedSettings AdvancedSettings { [DispId(98)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06000CB8 RID: 3256
		[DispId(99)]
		public virtual extern IMsTscDebug Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [DispId(99)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000CB9 RID: 3257
		[DispId(30)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Connect();

		// Token: 0x06000CBA RID: 3258
		[DispId(31)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Disconnect();

		// Token: 0x06000CBB RID: 3259
		[DispId(33)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000CBC RID: 3260
		[DispId(34)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06000CBE RID: 3262
		// (set) Token: 0x06000CBD RID: 3261
		[DispId(100)]
		public virtual extern int ColorDepth { [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06000CBF RID: 3263
		[DispId(101)]
		public virtual extern IMsRdpClientAdvancedSettings AdvancedSettings2 { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06000CC0 RID: 3264
		[DispId(102)]
		public virtual extern IMsRdpClientSecuredSettings SecuredSettings2 { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06000CC1 RID: 3265
		[DispId(103)]
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode ExtendedDisconnectReason { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06000CC3 RID: 3267
		// (set) Token: 0x06000CC2 RID: 3266
		[DispId(104)]
		public virtual extern bool FullScreen { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000CC4 RID: 3268
		[DispId(35)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000CC5 RID: 3269
		[DispId(36)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000CC6 RID: 3270
		[DispId(37)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus RequestClose();

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06000CC7 RID: 3271
		[DispId(200)]
		public virtual extern IMsRdpClientAdvancedSettings2 AdvancedSettings3 { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06000CC9 RID: 3273
		// (set) Token: 0x06000CC8 RID: 3272
		[DispId(201)]
		public virtual extern string ConnectedStatusText { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06000CCA RID: 3274
		[DispId(300)]
		public virtual extern IMsRdpClientAdvancedSettings3 AdvancedSettings4 { [DispId(300)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06000CCB RID: 3275
		[DispId(400)]
		public virtual extern IMsRdpClientAdvancedSettings4 AdvancedSettings5 { [DispId(400)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06000CCC RID: 3276
		[DispId(500)]
		public virtual extern IMsRdpClientTransportSettings TransportSettings { [DispId(500)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06000CCD RID: 3277
		[DispId(502)]
		public virtual extern IMsRdpClientAdvancedSettings5 AdvancedSettings6 { [DispId(502)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000CCE RID: 3278
		[DispId(503)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public virtual extern string GetErrorDescription([In] uint disconnectReason, [In] uint ExtendedDisconnectReason);

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06000CCF RID: 3279
		[DispId(504)]
		public virtual extern ITSRemoteProgram RemoteProgram { [DispId(504)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06000CD0 RID: 3280
		[DispId(505)]
		public virtual extern IMsRdpClientShell MsRdpClientShell { [DispId(505)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x140001A5 RID: 421
		// (add) Token: 0x06000CD1 RID: 3281
		// (remove) Token: 0x06000CD2 RID: 3282
		public virtual extern event IMsTscAxEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x140001A6 RID: 422
		// (add) Token: 0x06000CD3 RID: 3283
		// (remove) Token: 0x06000CD4 RID: 3284
		public virtual extern event IMsTscAxEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x140001A7 RID: 423
		// (add) Token: 0x06000CD5 RID: 3285
		// (remove) Token: 0x06000CD6 RID: 3286
		public virtual extern event IMsTscAxEvents_OnLoginCompleteEventHandler OnLoginComplete;

		// Token: 0x140001A8 RID: 424
		// (add) Token: 0x06000CD7 RID: 3287
		// (remove) Token: 0x06000CD8 RID: 3288
		public virtual extern event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140001A9 RID: 425
		// (add) Token: 0x06000CD9 RID: 3289
		// (remove) Token: 0x06000CDA RID: 3290
		public virtual extern event IMsTscAxEvents_OnEnterFullScreenModeEventHandler OnEnterFullScreenMode;

		// Token: 0x140001AA RID: 426
		// (add) Token: 0x06000CDB RID: 3291
		// (remove) Token: 0x06000CDC RID: 3292
		public virtual extern event IMsTscAxEvents_OnLeaveFullScreenModeEventHandler OnLeaveFullScreenMode;

		// Token: 0x140001AB RID: 427
		// (add) Token: 0x06000CDD RID: 3293
		// (remove) Token: 0x06000CDE RID: 3294
		public virtual extern event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140001AC RID: 428
		// (add) Token: 0x06000CDF RID: 3295
		// (remove) Token: 0x06000CE0 RID: 3296
		public virtual extern event IMsTscAxEvents_OnRequestGoFullScreenEventHandler OnRequestGoFullScreen;

		// Token: 0x140001AD RID: 429
		// (add) Token: 0x06000CE1 RID: 3297
		// (remove) Token: 0x06000CE2 RID: 3298
		public virtual extern event IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler OnRequestLeaveFullScreen;

		// Token: 0x140001AE RID: 430
		// (add) Token: 0x06000CE3 RID: 3299
		// (remove) Token: 0x06000CE4 RID: 3300
		public virtual extern event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140001AF RID: 431
		// (add) Token: 0x06000CE5 RID: 3301
		// (remove) Token: 0x06000CE6 RID: 3302
		public virtual extern event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140001B0 RID: 432
		// (add) Token: 0x06000CE7 RID: 3303
		// (remove) Token: 0x06000CE8 RID: 3304
		public virtual extern event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140001B1 RID: 433
		// (add) Token: 0x06000CE9 RID: 3305
		// (remove) Token: 0x06000CEA RID: 3306
		public virtual extern event IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler OnIdleTimeoutNotification;

		// Token: 0x140001B2 RID: 434
		// (add) Token: 0x06000CEB RID: 3307
		// (remove) Token: 0x06000CEC RID: 3308
		public virtual extern event IMsTscAxEvents_OnRequestContainerMinimizeEventHandler OnRequestContainerMinimize;

		// Token: 0x140001B3 RID: 435
		// (add) Token: 0x06000CED RID: 3309
		// (remove) Token: 0x06000CEE RID: 3310
		public virtual extern event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x140001B4 RID: 436
		// (add) Token: 0x06000CEF RID: 3311
		// (remove) Token: 0x06000CF0 RID: 3312
		public virtual extern event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x140001B5 RID: 437
		// (add) Token: 0x06000CF1 RID: 3313
		// (remove) Token: 0x06000CF2 RID: 3314
		public virtual extern event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x140001B6 RID: 438
		// (add) Token: 0x06000CF3 RID: 3315
		// (remove) Token: 0x06000CF4 RID: 3316
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x140001B7 RID: 439
		// (add) Token: 0x06000CF5 RID: 3317
		// (remove) Token: 0x06000CF6 RID: 3318
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler OnAuthenticationWarningDismissed;

		// Token: 0x140001B8 RID: 440
		// (add) Token: 0x06000CF7 RID: 3319
		// (remove) Token: 0x06000CF8 RID: 3320
		public virtual extern event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x140001B9 RID: 441
		// (add) Token: 0x06000CF9 RID: 3321
		// (remove) Token: 0x06000CFA RID: 3322
		public virtual extern event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x140001BA RID: 442
		// (add) Token: 0x06000CFB RID: 3323
		// (remove) Token: 0x06000CFC RID: 3324
		public virtual extern event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x140001BB RID: 443
		// (add) Token: 0x06000CFD RID: 3325
		// (remove) Token: 0x06000CFE RID: 3326
		public virtual extern event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x140001BC RID: 444
		// (add) Token: 0x06000CFF RID: 3327
		// (remove) Token: 0x06000D00 RID: 3328
		public virtual extern event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x140001BD RID: 445
		// (add) Token: 0x06000D01 RID: 3329
		// (remove) Token: 0x06000D02 RID: 3330
		public virtual extern event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x140001BE RID: 446
		// (add) Token: 0x06000D03 RID: 3331
		// (remove) Token: 0x06000D04 RID: 3332
		public virtual extern event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x140001BF RID: 447
		// (add) Token: 0x06000D05 RID: 3333
		// (remove) Token: 0x06000D06 RID: 3334
		public virtual extern event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x140001C0 RID: 448
		// (add) Token: 0x06000D07 RID: 3335
		// (remove) Token: 0x06000D08 RID: 3336
		public virtual extern event IMsTscAxEvents_OnConnectionBarPullDownEventHandler OnConnectionBarPullDown;

		// Token: 0x140001C1 RID: 449
		// (add) Token: 0x06000D09 RID: 3337
		// (remove) Token: 0x06000D0A RID: 3338
		public virtual extern event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x140001C2 RID: 450
		// (add) Token: 0x06000D0B RID: 3339
		// (remove) Token: 0x06000D0C RID: 3340
		public virtual extern event IMsTscAxEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06000D0E RID: 3342
		// (set) Token: 0x06000D0D RID: 3341
		public virtual extern string IMsRdpClient4_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06000D10 RID: 3344
		// (set) Token: 0x06000D0F RID: 3343
		public virtual extern string IMsRdpClient4_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06000D12 RID: 3346
		// (set) Token: 0x06000D11 RID: 3345
		public virtual extern string IMsRdpClient4_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06000D14 RID: 3348
		// (set) Token: 0x06000D13 RID: 3347
		public virtual extern string IMsRdpClient4_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06000D16 RID: 3350
		// (set) Token: 0x06000D15 RID: 3349
		public virtual extern string IMsRdpClient4_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06000D17 RID: 3351
		public virtual extern short IMsRdpClient4_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06000D19 RID: 3353
		// (set) Token: 0x06000D18 RID: 3352
		public virtual extern int IMsRdpClient4_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06000D1B RID: 3355
		// (set) Token: 0x06000D1A RID: 3354
		public virtual extern int IMsRdpClient4_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06000D1D RID: 3357
		// (set) Token: 0x06000D1C RID: 3356
		public virtual extern int IMsRdpClient4_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06000D1E RID: 3358
		public virtual extern int IMsRdpClient4_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06000D1F RID: 3359
		public virtual extern int IMsRdpClient4_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000559 RID: 1369
		// (set) Token: 0x06000D20 RID: 3360
		public virtual extern string IMsRdpClient4_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06000D21 RID: 3361
		public virtual extern int IMsRdpClient4_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06000D22 RID: 3362
		public virtual extern string IMsRdpClient4_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06000D23 RID: 3363
		public virtual extern int IMsRdpClient4_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06000D24 RID: 3364
		public virtual extern IMsTscSecuredSettings IMsRdpClient4_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06000D25 RID: 3365
		public virtual extern IMsTscAdvancedSettings IMsRdpClient4_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06000D26 RID: 3366
		public virtual extern IMsTscDebug IMsRdpClient4_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000D27 RID: 3367
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_Connect();

		// Token: 0x06000D28 RID: 3368
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_Disconnect();

		// Token: 0x06000D29 RID: 3369
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000D2A RID: 3370
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06000D2C RID: 3372
		// (set) Token: 0x06000D2B RID: 3371
		public virtual extern int IMsRdpClient4_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06000D2D RID: 3373
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient4_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06000D2E RID: 3374
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient4_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06000D2F RID: 3375
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient4_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06000D31 RID: 3377
		// (set) Token: 0x06000D30 RID: 3376
		public virtual extern bool IMsRdpClient4_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000D32 RID: 3378
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000D33 RID: 3379
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient4_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000D34 RID: 3380
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient4_RequestClose();

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06000D35 RID: 3381
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient4_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06000D37 RID: 3383
		// (set) Token: 0x06000D36 RID: 3382
		public virtual extern string IMsRdpClient4_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06000D38 RID: 3384
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient4_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06000D39 RID: 3385
		public virtual extern IMsRdpClientAdvancedSettings4 IMsRdpClient4_AdvancedSettings5 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06000D3B RID: 3387
		// (set) Token: 0x06000D3A RID: 3386
		public virtual extern string IMsRdpClient3_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06000D3D RID: 3389
		// (set) Token: 0x06000D3C RID: 3388
		public virtual extern string IMsRdpClient3_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06000D3F RID: 3391
		// (set) Token: 0x06000D3E RID: 3390
		public virtual extern string IMsRdpClient3_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06000D41 RID: 3393
		// (set) Token: 0x06000D40 RID: 3392
		public virtual extern string IMsRdpClient3_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06000D43 RID: 3395
		// (set) Token: 0x06000D42 RID: 3394
		public virtual extern string IMsRdpClient3_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06000D44 RID: 3396
		public virtual extern short IMsRdpClient3_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06000D46 RID: 3398
		// (set) Token: 0x06000D45 RID: 3397
		public virtual extern int IMsRdpClient3_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06000D48 RID: 3400
		// (set) Token: 0x06000D47 RID: 3399
		public virtual extern int IMsRdpClient3_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06000D4A RID: 3402
		// (set) Token: 0x06000D49 RID: 3401
		public virtual extern int IMsRdpClient3_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06000D4B RID: 3403
		public virtual extern int IMsRdpClient3_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06000D4C RID: 3404
		public virtual extern int IMsRdpClient3_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000574 RID: 1396
		// (set) Token: 0x06000D4D RID: 3405
		public virtual extern string IMsRdpClient3_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06000D4E RID: 3406
		public virtual extern int IMsRdpClient3_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06000D4F RID: 3407
		public virtual extern string IMsRdpClient3_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06000D50 RID: 3408
		public virtual extern int IMsRdpClient3_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06000D51 RID: 3409
		public virtual extern IMsTscSecuredSettings IMsRdpClient3_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06000D52 RID: 3410
		public virtual extern IMsTscAdvancedSettings IMsRdpClient3_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06000D53 RID: 3411
		public virtual extern IMsTscDebug IMsRdpClient3_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000D54 RID: 3412
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_Connect();

		// Token: 0x06000D55 RID: 3413
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_Disconnect();

		// Token: 0x06000D56 RID: 3414
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000D57 RID: 3415
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06000D59 RID: 3417
		// (set) Token: 0x06000D58 RID: 3416
		public virtual extern int IMsRdpClient3_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06000D5A RID: 3418
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient3_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06000D5B RID: 3419
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient3_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06000D5C RID: 3420
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient3_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06000D5E RID: 3422
		// (set) Token: 0x06000D5D RID: 3421
		public virtual extern bool IMsRdpClient3_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000D5F RID: 3423
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000D60 RID: 3424
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient3_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000D61 RID: 3425
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient3_RequestClose();

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06000D62 RID: 3426
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient3_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06000D64 RID: 3428
		// (set) Token: 0x06000D63 RID: 3427
		public virtual extern string IMsRdpClient3_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06000D65 RID: 3429
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient3_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06000D67 RID: 3431
		// (set) Token: 0x06000D66 RID: 3430
		public virtual extern string IMsRdpClient2_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06000D69 RID: 3433
		// (set) Token: 0x06000D68 RID: 3432
		public virtual extern string IMsRdpClient2_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06000D6B RID: 3435
		// (set) Token: 0x06000D6A RID: 3434
		public virtual extern string IMsRdpClient2_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06000D6D RID: 3437
		// (set) Token: 0x06000D6C RID: 3436
		public virtual extern string IMsRdpClient2_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06000D6F RID: 3439
		// (set) Token: 0x06000D6E RID: 3438
		public virtual extern string IMsRdpClient2_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06000D70 RID: 3440
		public virtual extern short IMsRdpClient2_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06000D72 RID: 3442
		// (set) Token: 0x06000D71 RID: 3441
		public virtual extern int IMsRdpClient2_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06000D74 RID: 3444
		// (set) Token: 0x06000D73 RID: 3443
		public virtual extern int IMsRdpClient2_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06000D76 RID: 3446
		// (set) Token: 0x06000D75 RID: 3445
		public virtual extern int IMsRdpClient2_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06000D77 RID: 3447
		public virtual extern int IMsRdpClient2_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06000D78 RID: 3448
		public virtual extern int IMsRdpClient2_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700058E RID: 1422
		// (set) Token: 0x06000D79 RID: 3449
		public virtual extern string IMsRdpClient2_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06000D7A RID: 3450
		public virtual extern int IMsRdpClient2_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06000D7B RID: 3451
		public virtual extern string IMsRdpClient2_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06000D7C RID: 3452
		public virtual extern int IMsRdpClient2_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06000D7D RID: 3453
		public virtual extern IMsTscSecuredSettings IMsRdpClient2_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06000D7E RID: 3454
		public virtual extern IMsTscAdvancedSettings IMsRdpClient2_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06000D7F RID: 3455
		public virtual extern IMsTscDebug IMsRdpClient2_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000D80 RID: 3456
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Connect();

		// Token: 0x06000D81 RID: 3457
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Disconnect();

		// Token: 0x06000D82 RID: 3458
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000D83 RID: 3459
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06000D85 RID: 3461
		// (set) Token: 0x06000D84 RID: 3460
		public virtual extern int IMsRdpClient2_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06000D86 RID: 3462
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient2_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06000D87 RID: 3463
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient2_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06000D88 RID: 3464
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient2_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06000D8A RID: 3466
		// (set) Token: 0x06000D89 RID: 3465
		public virtual extern bool IMsRdpClient2_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000D8B RID: 3467
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000D8C RID: 3468
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient2_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000D8D RID: 3469
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient2_RequestClose();

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06000D8E RID: 3470
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient2_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06000D90 RID: 3472
		// (set) Token: 0x06000D8F RID: 3471
		public virtual extern string IMsRdpClient2_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06000D92 RID: 3474
		// (set) Token: 0x06000D91 RID: 3473
		public virtual extern string IMsRdpClient_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06000D94 RID: 3476
		// (set) Token: 0x06000D93 RID: 3475
		public virtual extern string IMsRdpClient_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06000D96 RID: 3478
		// (set) Token: 0x06000D95 RID: 3477
		public virtual extern string IMsRdpClient_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06000D98 RID: 3480
		// (set) Token: 0x06000D97 RID: 3479
		public virtual extern string IMsRdpClient_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06000D9A RID: 3482
		// (set) Token: 0x06000D99 RID: 3481
		public virtual extern string IMsRdpClient_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06000D9B RID: 3483
		public virtual extern short IMsRdpClient_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06000D9D RID: 3485
		// (set) Token: 0x06000D9C RID: 3484
		public virtual extern int IMsRdpClient_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06000D9F RID: 3487
		// (set) Token: 0x06000D9E RID: 3486
		public virtual extern int IMsRdpClient_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06000DA1 RID: 3489
		// (set) Token: 0x06000DA0 RID: 3488
		public virtual extern int IMsRdpClient_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06000DA2 RID: 3490
		public virtual extern int IMsRdpClient_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06000DA3 RID: 3491
		public virtual extern int IMsRdpClient_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005A7 RID: 1447
		// (set) Token: 0x06000DA4 RID: 3492
		public virtual extern string IMsRdpClient_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06000DA5 RID: 3493
		public virtual extern int IMsRdpClient_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06000DA6 RID: 3494
		public virtual extern string IMsRdpClient_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06000DA7 RID: 3495
		public virtual extern int IMsRdpClient_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06000DA8 RID: 3496
		public virtual extern IMsTscSecuredSettings IMsRdpClient_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06000DA9 RID: 3497
		public virtual extern IMsTscAdvancedSettings IMsRdpClient_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06000DAA RID: 3498
		public virtual extern IMsTscDebug IMsRdpClient_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000DAB RID: 3499
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Connect();

		// Token: 0x06000DAC RID: 3500
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Disconnect();

		// Token: 0x06000DAD RID: 3501
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000DAE RID: 3502
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06000DB0 RID: 3504
		// (set) Token: 0x06000DAF RID: 3503
		public virtual extern int IMsRdpClient_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06000DB1 RID: 3505
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06000DB2 RID: 3506
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06000DB3 RID: 3507
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06000DB5 RID: 3509
		// (set) Token: 0x06000DB4 RID: 3508
		public virtual extern bool IMsRdpClient_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000DB6 RID: 3510
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000DB7 RID: 3511
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000DB8 RID: 3512
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient_RequestClose();

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06000DBA RID: 3514
		// (set) Token: 0x06000DB9 RID: 3513
		public virtual extern string IMsTscAx_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06000DBC RID: 3516
		// (set) Token: 0x06000DBB RID: 3515
		public virtual extern string IMsTscAx_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06000DBE RID: 3518
		// (set) Token: 0x06000DBD RID: 3517
		public virtual extern string IMsTscAx_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06000DC0 RID: 3520
		// (set) Token: 0x06000DBF RID: 3519
		public virtual extern string IMsTscAx_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06000DC2 RID: 3522
		// (set) Token: 0x06000DC1 RID: 3521
		public virtual extern string IMsTscAx_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06000DC3 RID: 3523
		public virtual extern short IMsTscAx_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06000DC5 RID: 3525
		// (set) Token: 0x06000DC4 RID: 3524
		public virtual extern int IMsTscAx_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06000DC7 RID: 3527
		// (set) Token: 0x06000DC6 RID: 3526
		public virtual extern int IMsTscAx_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06000DC9 RID: 3529
		// (set) Token: 0x06000DC8 RID: 3528
		public virtual extern int IMsTscAx_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06000DCA RID: 3530
		public virtual extern int IMsTscAx_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06000DCB RID: 3531
		public virtual extern int IMsTscAx_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005BE RID: 1470
		// (set) Token: 0x06000DCC RID: 3532
		public virtual extern string IMsTscAx_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06000DCD RID: 3533
		public virtual extern int IMsTscAx_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06000DCE RID: 3534
		public virtual extern string IMsTscAx_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06000DCF RID: 3535
		public virtual extern int IMsTscAx_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06000DD0 RID: 3536
		public virtual extern IMsTscSecuredSettings IMsTscAx_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06000DD1 RID: 3537
		public virtual extern IMsTscAdvancedSettings IMsTscAx_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06000DD2 RID: 3538
		public virtual extern IMsTscDebug IMsTscAx_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000DD3 RID: 3539
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Connect();

		// Token: 0x06000DD4 RID: 3540
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Disconnect();

		// Token: 0x06000DD5 RID: 3541
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000DD6 RID: 3542
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170005C5 RID: 1477
		// (set) Token: 0x06000DD7 RID: 3543
		public virtual extern string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06000DD9 RID: 3545
		// (set) Token: 0x06000DD8 RID: 3544
		public virtual extern string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06000DDB RID: 3547
		// (set) Token: 0x06000DDA RID: 3546
		public virtual extern string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06000DDD RID: 3549
		// (set) Token: 0x06000DDC RID: 3548
		public virtual extern string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06000DDF RID: 3551
		// (set) Token: 0x06000DDE RID: 3550
		public virtual extern string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000DE0 RID: 3552
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void ResetPassword();

		// Token: 0x170005CA RID: 1482
		// (set) Token: 0x06000DE1 RID: 3553
		public virtual extern string IMsRdpClientNonScriptable_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06000DE3 RID: 3555
		// (set) Token: 0x06000DE2 RID: 3554
		public virtual extern string IMsRdpClientNonScriptable_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06000DE5 RID: 3557
		// (set) Token: 0x06000DE4 RID: 3556
		public virtual extern string IMsRdpClientNonScriptable_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06000DE7 RID: 3559
		// (set) Token: 0x06000DE6 RID: 3558
		public virtual extern string IMsRdpClientNonScriptable_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06000DE9 RID: 3561
		// (set) Token: 0x06000DE8 RID: 3560
		public virtual extern string IMsRdpClientNonScriptable_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000DEA RID: 3562
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable_ResetPassword();

		// Token: 0x06000DEB RID: 3563
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000DEC RID: 3564
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170005CF RID: 1487
		// (set) Token: 0x06000DED RID: 3565
		public virtual extern string IMsRdpClientNonScriptable2_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06000DEF RID: 3567
		// (set) Token: 0x06000DEE RID: 3566
		public virtual extern string IMsRdpClientNonScriptable2_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06000DF1 RID: 3569
		// (set) Token: 0x06000DF0 RID: 3568
		public virtual extern string IMsRdpClientNonScriptable2_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06000DF3 RID: 3571
		// (set) Token: 0x06000DF2 RID: 3570
		public virtual extern string IMsRdpClientNonScriptable2_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06000DF5 RID: 3573
		// (set) Token: 0x06000DF4 RID: 3572
		public virtual extern string IMsRdpClientNonScriptable2_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000DF6 RID: 3574
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_ResetPassword();

		// Token: 0x06000DF7 RID: 3575
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000DF8 RID: 3576
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06000DFA RID: 3578
		// (set) Token: 0x06000DF9 RID: 3577
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x170005D5 RID: 1493
		// (set) Token: 0x06000DFB RID: 3579
		public virtual extern string IMsRdpClientNonScriptable3_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06000DFD RID: 3581
		// (set) Token: 0x06000DFC RID: 3580
		public virtual extern string IMsRdpClientNonScriptable3_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06000DFF RID: 3583
		// (set) Token: 0x06000DFE RID: 3582
		public virtual extern string IMsRdpClientNonScriptable3_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06000E01 RID: 3585
		// (set) Token: 0x06000E00 RID: 3584
		public virtual extern string IMsRdpClientNonScriptable3_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06000E03 RID: 3587
		// (set) Token: 0x06000E02 RID: 3586
		public virtual extern string IMsRdpClientNonScriptable3_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06000E04 RID: 3588
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable3_ResetPassword();

		// Token: 0x06000E05 RID: 3589
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable3_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06000E06 RID: 3590
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable3_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06000E08 RID: 3592
		// (set) Token: 0x06000E07 RID: 3591
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr IMsRdpClientNonScriptable3_UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06000E0A RID: 3594
		// (set) Token: 0x06000E09 RID: 3593
		public virtual extern bool ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06000E0C RID: 3596
		// (set) Token: 0x06000E0B RID: 3595
		public virtual extern bool PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06000E0E RID: 3598
		// (set) Token: 0x06000E0D RID: 3597
		public virtual extern bool NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06000E10 RID: 3600
		// (set) Token: 0x06000E0F RID: 3599
		public virtual extern bool EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06000E12 RID: 3602
		// (set) Token: 0x06000E11 RID: 3601
		public virtual extern bool RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06000E14 RID: 3604
		// (set) Token: 0x06000E13 RID: 3603
		public virtual extern bool RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06000E15 RID: 3605
		public virtual extern IMsRdpDeviceCollection DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06000E16 RID: 3606
		public virtual extern IMsRdpDriveCollection DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06000E18 RID: 3608
		// (set) Token: 0x06000E17 RID: 3607
		public virtual extern bool WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06000E1A RID: 3610
		// (set) Token: 0x06000E19 RID: 3609
		public virtual extern bool WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06000E1C RID: 3612
		// (set) Token: 0x06000E1B RID: 3611
		public virtual extern string ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }
	}
}
