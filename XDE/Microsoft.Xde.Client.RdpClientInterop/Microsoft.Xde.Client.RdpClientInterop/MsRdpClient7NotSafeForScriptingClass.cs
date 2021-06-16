using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000038 RID: 56
	[TypeLibType(TypeLibTypeFlags.FCanCreate | TypeLibTypeFlags.FControl)]
	[ComConversionLoss]
	[ComSourceInterfaces("Microsoft.Xde.Client.RdpClientInterop.IMsTscAxEvents\0\0")]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("54D38BF7-B1EF-4479-9674-1BD6EA465258")]
	[ComImport]
	public class MsRdpClient7NotSafeForScriptingClass : IMsRdpClient7, MsRdpClient7NotSafeForScripting, IMsTscAxEvents_Event, IMsRdpClient6, IMsRdpClient5, IMsRdpClient4, IMsRdpClient3, IMsRdpClient2, IMsRdpClient, IMsTscAx, IMsTscAx_Redist, IMsTscNonScriptable, IMsRdpClientNonScriptable, IMsRdpClientNonScriptable2, IMsRdpClientNonScriptable3, IMsRdpClientNonScriptable4, IMsRdpClientNonScriptable5, IMsRdpPreferredRedirectionInfo, IMsRdpExtendedSettings
	{
		// Token: 0x06001447 RID: 5191
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern MsRdpClient7NotSafeForScriptingClass();

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06001449 RID: 5193
		// (set) Token: 0x06001448 RID: 5192
		[DispId(1)]
		public virtual extern string Server { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x0600144B RID: 5195
		// (set) Token: 0x0600144A RID: 5194
		[DispId(2)]
		public virtual extern string Domain { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x0600144D RID: 5197
		// (set) Token: 0x0600144C RID: 5196
		[DispId(3)]
		public virtual extern string UserName { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x0600144F RID: 5199
		// (set) Token: 0x0600144E RID: 5198
		[DispId(4)]
		public virtual extern string DisconnectedText { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06001451 RID: 5201
		// (set) Token: 0x06001450 RID: 5200
		[DispId(5)]
		public virtual extern string ConnectingText { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06001452 RID: 5202
		[DispId(6)]
		public virtual extern short Connected { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06001454 RID: 5204
		// (set) Token: 0x06001453 RID: 5203
		[DispId(12)]
		public virtual extern int DesktopWidth { [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06001456 RID: 5206
		// (set) Token: 0x06001455 RID: 5205
		[DispId(13)]
		public virtual extern int DesktopHeight { [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06001458 RID: 5208
		// (set) Token: 0x06001457 RID: 5207
		[DispId(16)]
		public virtual extern int StartConnected { [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06001459 RID: 5209
		[DispId(17)]
		public virtual extern int HorizontalScrollBarVisible { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600145A RID: 5210
		[DispId(18)]
		public virtual extern int VerticalScrollBarVisible { [DispId(18)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700090B RID: 2315
		// (set) Token: 0x0600145B RID: 5211
		[DispId(19)]
		public virtual extern string FullScreenTitle { [DispId(19)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600145C RID: 5212
		[DispId(20)]
		public virtual extern int CipherStrength { [DispId(20)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600145D RID: 5213
		[DispId(21)]
		public virtual extern string Version { [DispId(21)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x0600145E RID: 5214
		[DispId(22)]
		public virtual extern int SecuredSettingsEnabled { [DispId(22)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x0600145F RID: 5215
		[DispId(97)]
		public virtual extern IMsTscSecuredSettings SecuredSettings { [DispId(97)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06001460 RID: 5216
		[DispId(98)]
		public virtual extern IMsTscAdvancedSettings AdvancedSettings { [DispId(98)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06001461 RID: 5217
		[DispId(99)]
		public virtual extern IMsTscDebug Debugger { [DispId(99)] [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06001462 RID: 5218
		[DispId(30)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Connect();

		// Token: 0x06001463 RID: 5219
		[DispId(31)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Disconnect();

		// Token: 0x06001464 RID: 5220
		[DispId(33)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06001465 RID: 5221
		[DispId(34)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06001467 RID: 5223
		// (set) Token: 0x06001466 RID: 5222
		[DispId(100)]
		public virtual extern int ColorDepth { [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06001468 RID: 5224
		[DispId(101)]
		public virtual extern IMsRdpClientAdvancedSettings AdvancedSettings2 { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06001469 RID: 5225
		[DispId(102)]
		public virtual extern IMsRdpClientSecuredSettings SecuredSettings2 { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x0600146A RID: 5226
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		[DispId(103)]
		public virtual extern ExtendedDisconnectReasonCode ExtendedDisconnectReason { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x0600146C RID: 5228
		// (set) Token: 0x0600146B RID: 5227
		[DispId(104)]
		public virtual extern bool FullScreen { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x0600146D RID: 5229
		[DispId(35)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x0600146E RID: 5230
		[DispId(36)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x0600146F RID: 5231
		[DispId(37)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus RequestClose();

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06001470 RID: 5232
		[DispId(200)]
		public virtual extern IMsRdpClientAdvancedSettings2 AdvancedSettings3 { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06001472 RID: 5234
		// (set) Token: 0x06001471 RID: 5233
		[DispId(201)]
		public virtual extern string ConnectedStatusText { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06001473 RID: 5235
		[DispId(300)]
		public virtual extern IMsRdpClientAdvancedSettings3 AdvancedSettings4 { [DispId(300)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06001474 RID: 5236
		[DispId(400)]
		public virtual extern IMsRdpClientAdvancedSettings4 AdvancedSettings5 { [DispId(400)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06001475 RID: 5237
		[DispId(500)]
		public virtual extern IMsRdpClientTransportSettings TransportSettings { [DispId(500)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06001476 RID: 5238
		[DispId(502)]
		public virtual extern IMsRdpClientAdvancedSettings5 AdvancedSettings6 { [DispId(502)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06001477 RID: 5239
		[DispId(503)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public virtual extern string GetErrorDescription([In] uint disconnectReason, [In] uint ExtendedDisconnectReason);

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06001478 RID: 5240
		[DispId(504)]
		public virtual extern ITSRemoteProgram RemoteProgram { [DispId(504)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06001479 RID: 5241
		[DispId(505)]
		public virtual extern IMsRdpClientShell MsRdpClientShell { [DispId(505)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x0600147A RID: 5242
		[DispId(507)]
		public virtual extern IMsRdpClientAdvancedSettings6 AdvancedSettings7 { [DispId(507)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x0600147B RID: 5243
		[DispId(506)]
		public virtual extern IMsRdpClientTransportSettings2 TransportSettings2 { [DispId(506)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600147C RID: 5244
		[DispId(600)]
		public virtual extern IMsRdpClientAdvancedSettings7 AdvancedSettings8 { [DispId(600)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600147D RID: 5245
		[DispId(601)]
		public virtual extern IMsRdpClientTransportSettings3 TransportSettings3 { [DispId(601)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x0600147E RID: 5246
		[DispId(602)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public virtual extern string GetStatusText([In] uint statusCode);

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600147F RID: 5247
		[DispId(603)]
		public virtual extern IMsRdpClientSecuredSettings2 SecuredSettings3 { [DispId(603)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06001480 RID: 5248
		[DispId(604)]
		public virtual extern ITSRemoteProgram2 RemoteProgram2 { [DispId(604)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1400021D RID: 541
		// (add) Token: 0x06001481 RID: 5249
		// (remove) Token: 0x06001482 RID: 5250
		public virtual extern event IMsTscAxEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x1400021E RID: 542
		// (add) Token: 0x06001483 RID: 5251
		// (remove) Token: 0x06001484 RID: 5252
		public virtual extern event IMsTscAxEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x1400021F RID: 543
		// (add) Token: 0x06001485 RID: 5253
		// (remove) Token: 0x06001486 RID: 5254
		public virtual extern event IMsTscAxEvents_OnLoginCompleteEventHandler OnLoginComplete;

		// Token: 0x14000220 RID: 544
		// (add) Token: 0x06001487 RID: 5255
		// (remove) Token: 0x06001488 RID: 5256
		public virtual extern event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x14000221 RID: 545
		// (add) Token: 0x06001489 RID: 5257
		// (remove) Token: 0x0600148A RID: 5258
		public virtual extern event IMsTscAxEvents_OnEnterFullScreenModeEventHandler OnEnterFullScreenMode;

		// Token: 0x14000222 RID: 546
		// (add) Token: 0x0600148B RID: 5259
		// (remove) Token: 0x0600148C RID: 5260
		public virtual extern event IMsTscAxEvents_OnLeaveFullScreenModeEventHandler OnLeaveFullScreenMode;

		// Token: 0x14000223 RID: 547
		// (add) Token: 0x0600148D RID: 5261
		// (remove) Token: 0x0600148E RID: 5262
		public virtual extern event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x14000224 RID: 548
		// (add) Token: 0x0600148F RID: 5263
		// (remove) Token: 0x06001490 RID: 5264
		public virtual extern event IMsTscAxEvents_OnRequestGoFullScreenEventHandler OnRequestGoFullScreen;

		// Token: 0x14000225 RID: 549
		// (add) Token: 0x06001491 RID: 5265
		// (remove) Token: 0x06001492 RID: 5266
		public virtual extern event IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler OnRequestLeaveFullScreen;

		// Token: 0x14000226 RID: 550
		// (add) Token: 0x06001493 RID: 5267
		// (remove) Token: 0x06001494 RID: 5268
		public virtual extern event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x14000227 RID: 551
		// (add) Token: 0x06001495 RID: 5269
		// (remove) Token: 0x06001496 RID: 5270
		public virtual extern event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x14000228 RID: 552
		// (add) Token: 0x06001497 RID: 5271
		// (remove) Token: 0x06001498 RID: 5272
		public virtual extern event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x14000229 RID: 553
		// (add) Token: 0x06001499 RID: 5273
		// (remove) Token: 0x0600149A RID: 5274
		public virtual extern event IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler OnIdleTimeoutNotification;

		// Token: 0x1400022A RID: 554
		// (add) Token: 0x0600149B RID: 5275
		// (remove) Token: 0x0600149C RID: 5276
		public virtual extern event IMsTscAxEvents_OnRequestContainerMinimizeEventHandler OnRequestContainerMinimize;

		// Token: 0x1400022B RID: 555
		// (add) Token: 0x0600149D RID: 5277
		// (remove) Token: 0x0600149E RID: 5278
		public virtual extern event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x1400022C RID: 556
		// (add) Token: 0x0600149F RID: 5279
		// (remove) Token: 0x060014A0 RID: 5280
		public virtual extern event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x1400022D RID: 557
		// (add) Token: 0x060014A1 RID: 5281
		// (remove) Token: 0x060014A2 RID: 5282
		public virtual extern event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x1400022E RID: 558
		// (add) Token: 0x060014A3 RID: 5283
		// (remove) Token: 0x060014A4 RID: 5284
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x1400022F RID: 559
		// (add) Token: 0x060014A5 RID: 5285
		// (remove) Token: 0x060014A6 RID: 5286
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000230 RID: 560
		// (add) Token: 0x060014A7 RID: 5287
		// (remove) Token: 0x060014A8 RID: 5288
		public virtual extern event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000231 RID: 561
		// (add) Token: 0x060014A9 RID: 5289
		// (remove) Token: 0x060014AA RID: 5290
		public virtual extern event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000232 RID: 562
		// (add) Token: 0x060014AB RID: 5291
		// (remove) Token: 0x060014AC RID: 5292
		public virtual extern event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000233 RID: 563
		// (add) Token: 0x060014AD RID: 5293
		// (remove) Token: 0x060014AE RID: 5294
		public virtual extern event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000234 RID: 564
		// (add) Token: 0x060014AF RID: 5295
		// (remove) Token: 0x060014B0 RID: 5296
		public virtual extern event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000235 RID: 565
		// (add) Token: 0x060014B1 RID: 5297
		// (remove) Token: 0x060014B2 RID: 5298
		public virtual extern event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x14000236 RID: 566
		// (add) Token: 0x060014B3 RID: 5299
		// (remove) Token: 0x060014B4 RID: 5300
		public virtual extern event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x14000237 RID: 567
		// (add) Token: 0x060014B5 RID: 5301
		// (remove) Token: 0x060014B6 RID: 5302
		public virtual extern event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x14000238 RID: 568
		// (add) Token: 0x060014B7 RID: 5303
		// (remove) Token: 0x060014B8 RID: 5304
		public virtual extern event IMsTscAxEvents_OnConnectionBarPullDownEventHandler OnConnectionBarPullDown;

		// Token: 0x14000239 RID: 569
		// (add) Token: 0x060014B9 RID: 5305
		// (remove) Token: 0x060014BA RID: 5306
		public virtual extern event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400023A RID: 570
		// (add) Token: 0x060014BB RID: 5307
		// (remove) Token: 0x060014BC RID: 5308
		public virtual extern event IMsTscAxEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060014BE RID: 5310
		// (set) Token: 0x060014BD RID: 5309
		public virtual extern string IMsRdpClient6_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060014C0 RID: 5312
		// (set) Token: 0x060014BF RID: 5311
		public virtual extern string IMsRdpClient6_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060014C2 RID: 5314
		// (set) Token: 0x060014C1 RID: 5313
		public virtual extern string IMsRdpClient6_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060014C4 RID: 5316
		// (set) Token: 0x060014C3 RID: 5315
		public virtual extern string IMsRdpClient6_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060014C6 RID: 5318
		// (set) Token: 0x060014C5 RID: 5317
		public virtual extern string IMsRdpClient6_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060014C7 RID: 5319
		public virtual extern short IMsRdpClient6_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060014C9 RID: 5321
		// (set) Token: 0x060014C8 RID: 5320
		public virtual extern int IMsRdpClient6_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060014CB RID: 5323
		// (set) Token: 0x060014CA RID: 5322
		public virtual extern int IMsRdpClient6_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060014CD RID: 5325
		// (set) Token: 0x060014CC RID: 5324
		public virtual extern int IMsRdpClient6_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060014CE RID: 5326
		public virtual extern int IMsRdpClient6_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060014CF RID: 5327
		public virtual extern int IMsRdpClient6_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000930 RID: 2352
		// (set) Token: 0x060014D0 RID: 5328
		public virtual extern string IMsRdpClient6_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060014D1 RID: 5329
		public virtual extern int IMsRdpClient6_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060014D2 RID: 5330
		public virtual extern string IMsRdpClient6_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060014D3 RID: 5331
		public virtual extern int IMsRdpClient6_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060014D4 RID: 5332
		public virtual extern IMsTscSecuredSettings IMsRdpClient6_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060014D5 RID: 5333
		public virtual extern IMsTscAdvancedSettings IMsRdpClient6_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060014D6 RID: 5334
		public virtual extern IMsTscDebug IMsRdpClient6_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x060014D7 RID: 5335
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient6_Connect();

		// Token: 0x060014D8 RID: 5336
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient6_Disconnect();

		// Token: 0x060014D9 RID: 5337
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient6_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x060014DA RID: 5338
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient6_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060014DC RID: 5340
		// (set) Token: 0x060014DB RID: 5339
		public virtual extern int IMsRdpClient6_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060014DD RID: 5341
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient6_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060014DE RID: 5342
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient6_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060014DF RID: 5343
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient6_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060014E1 RID: 5345
		// (set) Token: 0x060014E0 RID: 5344
		public virtual extern bool IMsRdpClient6_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x060014E2 RID: 5346
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient6_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x060014E3 RID: 5347
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient6_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x060014E4 RID: 5348
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient6_RequestClose();

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060014E5 RID: 5349
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient6_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060014E7 RID: 5351
		// (set) Token: 0x060014E6 RID: 5350
		public virtual extern string IMsRdpClient6_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060014E8 RID: 5352
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient6_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060014E9 RID: 5353
		public virtual extern IMsRdpClientAdvancedSettings4 IMsRdpClient6_AdvancedSettings5 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060014EA RID: 5354
		public virtual extern IMsRdpClientTransportSettings IMsRdpClient6_TransportSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060014EB RID: 5355
		public virtual extern IMsRdpClientAdvancedSettings5 IMsRdpClient6_AdvancedSettings6 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x060014EC RID: 5356
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public virtual extern string IMsRdpClient6_GetErrorDescription([In] uint disconnectReason, [In] uint ExtendedDisconnectReason);

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060014ED RID: 5357
		public virtual extern ITSRemoteProgram IMsRdpClient6_RemoteProgram { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060014EE RID: 5358
		public virtual extern IMsRdpClientShell IMsRdpClient6_MsRdpClientShell { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060014EF RID: 5359
		public virtual extern IMsRdpClientAdvancedSettings6 IMsRdpClient6_AdvancedSettings7 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060014F0 RID: 5360
		public virtual extern IMsRdpClientTransportSettings2 IMsRdpClient6_TransportSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060014F2 RID: 5362
		// (set) Token: 0x060014F1 RID: 5361
		public virtual extern string IMsRdpClient5_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060014F4 RID: 5364
		// (set) Token: 0x060014F3 RID: 5363
		public virtual extern string IMsRdpClient5_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060014F6 RID: 5366
		// (set) Token: 0x060014F5 RID: 5365
		public virtual extern string IMsRdpClient5_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060014F8 RID: 5368
		// (set) Token: 0x060014F7 RID: 5367
		public virtual extern string IMsRdpClient5_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060014FA RID: 5370
		// (set) Token: 0x060014F9 RID: 5369
		public virtual extern string IMsRdpClient5_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060014FB RID: 5371
		public virtual extern short IMsRdpClient5_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060014FD RID: 5373
		// (set) Token: 0x060014FC RID: 5372
		public virtual extern int IMsRdpClient5_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060014FF RID: 5375
		// (set) Token: 0x060014FE RID: 5374
		public virtual extern int IMsRdpClient5_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06001501 RID: 5377
		// (set) Token: 0x06001500 RID: 5376
		public virtual extern int IMsRdpClient5_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06001502 RID: 5378
		public virtual extern int IMsRdpClient5_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06001503 RID: 5379
		public virtual extern int IMsRdpClient5_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000951 RID: 2385
		// (set) Token: 0x06001504 RID: 5380
		public virtual extern string IMsRdpClient5_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06001505 RID: 5381
		public virtual extern int IMsRdpClient5_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06001506 RID: 5382
		public virtual extern string IMsRdpClient5_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06001507 RID: 5383
		public virtual extern int IMsRdpClient5_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06001508 RID: 5384
		public virtual extern IMsTscSecuredSettings IMsRdpClient5_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06001509 RID: 5385
		public virtual extern IMsTscAdvancedSettings IMsRdpClient5_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x0600150A RID: 5386
		public virtual extern IMsTscDebug IMsRdpClient5_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x0600150B RID: 5387
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient5_Connect();

		// Token: 0x0600150C RID: 5388
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient5_Disconnect();

		// Token: 0x0600150D RID: 5389
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient5_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x0600150E RID: 5390
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient5_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06001510 RID: 5392
		// (set) Token: 0x0600150F RID: 5391
		public virtual extern int IMsRdpClient5_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06001511 RID: 5393
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient5_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06001512 RID: 5394
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient5_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06001513 RID: 5395
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient5_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06001515 RID: 5397
		// (set) Token: 0x06001514 RID: 5396
		public virtual extern bool IMsRdpClient5_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06001516 RID: 5398
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient5_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06001517 RID: 5399
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient5_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06001518 RID: 5400
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient5_RequestClose();

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06001519 RID: 5401
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient5_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x0600151B RID: 5403
		// (set) Token: 0x0600151A RID: 5402
		public virtual extern string IMsRdpClient5_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x0600151C RID: 5404
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient5_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600151D RID: 5405
		public virtual extern IMsRdpClientAdvancedSettings4 IMsRdpClient5_AdvancedSettings5 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600151E RID: 5406
		public virtual extern IMsRdpClientTransportSettings IMsRdpClient5_TransportSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600151F RID: 5407
		public virtual extern IMsRdpClientAdvancedSettings5 IMsRdpClient5_AdvancedSettings6 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06001520 RID: 5408
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public virtual extern string IMsRdpClient5_GetErrorDescription([In] uint disconnectReason, [In] uint ExtendedDisconnectReason);

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06001521 RID: 5409
		public virtual extern ITSRemoteProgram IMsRdpClient5_RemoteProgram { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06001522 RID: 5410
		public virtual extern IMsRdpClientShell IMsRdpClient5_MsRdpClientShell { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06001524 RID: 5412
		// (set) Token: 0x06001523 RID: 5411
		public virtual extern string IMsRdpClient4_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06001526 RID: 5414
		// (set) Token: 0x06001525 RID: 5413
		public virtual extern string IMsRdpClient4_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06001528 RID: 5416
		// (set) Token: 0x06001527 RID: 5415
		public virtual extern string IMsRdpClient4_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x0600152A RID: 5418
		// (set) Token: 0x06001529 RID: 5417
		public virtual extern string IMsRdpClient4_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600152C RID: 5420
		// (set) Token: 0x0600152B RID: 5419
		public virtual extern string IMsRdpClient4_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600152D RID: 5421
		public virtual extern short IMsRdpClient4_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600152F RID: 5423
		// (set) Token: 0x0600152E RID: 5422
		public virtual extern int IMsRdpClient4_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06001531 RID: 5425
		// (set) Token: 0x06001530 RID: 5424
		public virtual extern int IMsRdpClient4_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06001533 RID: 5427
		// (set) Token: 0x06001532 RID: 5426
		public virtual extern int IMsRdpClient4_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06001534 RID: 5428
		public virtual extern int IMsRdpClient4_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06001535 RID: 5429
		public virtual extern int IMsRdpClient4_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000970 RID: 2416
		// (set) Token: 0x06001536 RID: 5430
		public virtual extern string IMsRdpClient4_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06001537 RID: 5431
		public virtual extern int IMsRdpClient4_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06001538 RID: 5432
		public virtual extern string IMsRdpClient4_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06001539 RID: 5433
		public virtual extern int IMsRdpClient4_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600153A RID: 5434
		public virtual extern IMsTscSecuredSettings IMsRdpClient4_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x0600153B RID: 5435
		public virtual extern IMsTscAdvancedSettings IMsRdpClient4_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x0600153C RID: 5436
		public virtual extern IMsTscDebug IMsRdpClient4_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x0600153D RID: 5437
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_Connect();

		// Token: 0x0600153E RID: 5438
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_Disconnect();

		// Token: 0x0600153F RID: 5439
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06001540 RID: 5440
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06001542 RID: 5442
		// (set) Token: 0x06001541 RID: 5441
		public virtual extern int IMsRdpClient4_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06001543 RID: 5443
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient4_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06001544 RID: 5444
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient4_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06001545 RID: 5445
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient4_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06001547 RID: 5447
		// (set) Token: 0x06001546 RID: 5446
		public virtual extern bool IMsRdpClient4_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06001548 RID: 5448
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient4_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06001549 RID: 5449
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient4_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x0600154A RID: 5450
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient4_RequestClose();

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x0600154B RID: 5451
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient4_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x0600154D RID: 5453
		// (set) Token: 0x0600154C RID: 5452
		public virtual extern string IMsRdpClient4_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600154E RID: 5454
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient4_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600154F RID: 5455
		public virtual extern IMsRdpClientAdvancedSettings4 IMsRdpClient4_AdvancedSettings5 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06001551 RID: 5457
		// (set) Token: 0x06001550 RID: 5456
		public virtual extern string IMsRdpClient3_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06001553 RID: 5459
		// (set) Token: 0x06001552 RID: 5458
		public virtual extern string IMsRdpClient3_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06001555 RID: 5461
		// (set) Token: 0x06001554 RID: 5460
		public virtual extern string IMsRdpClient3_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06001557 RID: 5463
		// (set) Token: 0x06001556 RID: 5462
		public virtual extern string IMsRdpClient3_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06001559 RID: 5465
		// (set) Token: 0x06001558 RID: 5464
		public virtual extern string IMsRdpClient3_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600155A RID: 5466
		public virtual extern short IMsRdpClient3_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600155C RID: 5468
		// (set) Token: 0x0600155B RID: 5467
		public virtual extern int IMsRdpClient3_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600155E RID: 5470
		// (set) Token: 0x0600155D RID: 5469
		public virtual extern int IMsRdpClient3_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06001560 RID: 5472
		// (set) Token: 0x0600155F RID: 5471
		public virtual extern int IMsRdpClient3_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06001561 RID: 5473
		public virtual extern int IMsRdpClient3_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06001562 RID: 5474
		public virtual extern int IMsRdpClient3_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700098B RID: 2443
		// (set) Token: 0x06001563 RID: 5475
		public virtual extern string IMsRdpClient3_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06001564 RID: 5476
		public virtual extern int IMsRdpClient3_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06001565 RID: 5477
		public virtual extern string IMsRdpClient3_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06001566 RID: 5478
		public virtual extern int IMsRdpClient3_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06001567 RID: 5479
		public virtual extern IMsTscSecuredSettings IMsRdpClient3_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06001568 RID: 5480
		public virtual extern IMsTscAdvancedSettings IMsRdpClient3_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06001569 RID: 5481
		public virtual extern IMsTscDebug IMsRdpClient3_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x0600156A RID: 5482
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_Connect();

		// Token: 0x0600156B RID: 5483
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_Disconnect();

		// Token: 0x0600156C RID: 5484
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x0600156D RID: 5485
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600156F RID: 5487
		// (set) Token: 0x0600156E RID: 5486
		public virtual extern int IMsRdpClient3_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06001570 RID: 5488
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient3_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06001571 RID: 5489
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient3_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06001572 RID: 5490
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient3_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06001574 RID: 5492
		// (set) Token: 0x06001573 RID: 5491
		public virtual extern bool IMsRdpClient3_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06001575 RID: 5493
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient3_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06001576 RID: 5494
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient3_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06001577 RID: 5495
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient3_RequestClose();

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06001578 RID: 5496
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient3_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600157A RID: 5498
		// (set) Token: 0x06001579 RID: 5497
		public virtual extern string IMsRdpClient3_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x0600157B RID: 5499
		public virtual extern IMsRdpClientAdvancedSettings3 IMsRdpClient3_AdvancedSettings4 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x0600157D RID: 5501
		// (set) Token: 0x0600157C RID: 5500
		public virtual extern string IMsRdpClient2_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x0600157F RID: 5503
		// (set) Token: 0x0600157E RID: 5502
		public virtual extern string IMsRdpClient2_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06001581 RID: 5505
		// (set) Token: 0x06001580 RID: 5504
		public virtual extern string IMsRdpClient2_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06001583 RID: 5507
		// (set) Token: 0x06001582 RID: 5506
		public virtual extern string IMsRdpClient2_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06001585 RID: 5509
		// (set) Token: 0x06001584 RID: 5508
		public virtual extern string IMsRdpClient2_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06001586 RID: 5510
		public virtual extern short IMsRdpClient2_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06001588 RID: 5512
		// (set) Token: 0x06001587 RID: 5511
		public virtual extern int IMsRdpClient2_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x0600158A RID: 5514
		// (set) Token: 0x06001589 RID: 5513
		public virtual extern int IMsRdpClient2_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x0600158C RID: 5516
		// (set) Token: 0x0600158B RID: 5515
		public virtual extern int IMsRdpClient2_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x0600158D RID: 5517
		public virtual extern int IMsRdpClient2_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600158E RID: 5518
		public virtual extern int IMsRdpClient2_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009A5 RID: 2469
		// (set) Token: 0x0600158F RID: 5519
		public virtual extern string IMsRdpClient2_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06001590 RID: 5520
		public virtual extern int IMsRdpClient2_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06001591 RID: 5521
		public virtual extern string IMsRdpClient2_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06001592 RID: 5522
		public virtual extern int IMsRdpClient2_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06001593 RID: 5523
		public virtual extern IMsTscSecuredSettings IMsRdpClient2_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06001594 RID: 5524
		public virtual extern IMsTscAdvancedSettings IMsRdpClient2_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06001595 RID: 5525
		public virtual extern IMsTscDebug IMsRdpClient2_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06001596 RID: 5526
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Connect();

		// Token: 0x06001597 RID: 5527
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Disconnect();

		// Token: 0x06001598 RID: 5528
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06001599 RID: 5529
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x0600159B RID: 5531
		// (set) Token: 0x0600159A RID: 5530
		public virtual extern int IMsRdpClient2_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x0600159C RID: 5532
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient2_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x0600159D RID: 5533
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient2_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x0600159E RID: 5534
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient2_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060015A0 RID: 5536
		// (set) Token: 0x0600159F RID: 5535
		public virtual extern bool IMsRdpClient2_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x060015A1 RID: 5537
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x060015A2 RID: 5538
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient2_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x060015A3 RID: 5539
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient2_RequestClose();

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060015A4 RID: 5540
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient2_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060015A6 RID: 5542
		// (set) Token: 0x060015A5 RID: 5541
		public virtual extern string IMsRdpClient2_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060015A8 RID: 5544
		// (set) Token: 0x060015A7 RID: 5543
		public virtual extern string IMsRdpClient_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060015AA RID: 5546
		// (set) Token: 0x060015A9 RID: 5545
		public virtual extern string IMsRdpClient_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060015AC RID: 5548
		// (set) Token: 0x060015AB RID: 5547
		public virtual extern string IMsRdpClient_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060015AE RID: 5550
		// (set) Token: 0x060015AD RID: 5549
		public virtual extern string IMsRdpClient_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060015B0 RID: 5552
		// (set) Token: 0x060015AF RID: 5551
		public virtual extern string IMsRdpClient_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060015B1 RID: 5553
		public virtual extern short IMsRdpClient_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060015B3 RID: 5555
		// (set) Token: 0x060015B2 RID: 5554
		public virtual extern int IMsRdpClient_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060015B5 RID: 5557
		// (set) Token: 0x060015B4 RID: 5556
		public virtual extern int IMsRdpClient_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060015B7 RID: 5559
		// (set) Token: 0x060015B6 RID: 5558
		public virtual extern int IMsRdpClient_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060015B8 RID: 5560
		public virtual extern int IMsRdpClient_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060015B9 RID: 5561
		public virtual extern int IMsRdpClient_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009BE RID: 2494
		// (set) Token: 0x060015BA RID: 5562
		public virtual extern string IMsRdpClient_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060015BB RID: 5563
		public virtual extern int IMsRdpClient_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060015BC RID: 5564
		public virtual extern string IMsRdpClient_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060015BD RID: 5565
		public virtual extern int IMsRdpClient_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060015BE RID: 5566
		public virtual extern IMsTscSecuredSettings IMsRdpClient_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060015BF RID: 5567
		public virtual extern IMsTscAdvancedSettings IMsRdpClient_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060015C0 RID: 5568
		public virtual extern IMsTscDebug IMsRdpClient_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x060015C1 RID: 5569
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Connect();

		// Token: 0x060015C2 RID: 5570
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Disconnect();

		// Token: 0x060015C3 RID: 5571
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x060015C4 RID: 5572
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060015C6 RID: 5574
		// (set) Token: 0x060015C5 RID: 5573
		public virtual extern int IMsRdpClient_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060015C7 RID: 5575
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060015C8 RID: 5576
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060015C9 RID: 5577
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060015CB RID: 5579
		// (set) Token: 0x060015CA RID: 5578
		public virtual extern bool IMsRdpClient_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x060015CC RID: 5580
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x060015CD RID: 5581
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x060015CE RID: 5582
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient_RequestClose();

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060015D0 RID: 5584
		// (set) Token: 0x060015CF RID: 5583
		public virtual extern string IMsTscAx_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060015D2 RID: 5586
		// (set) Token: 0x060015D1 RID: 5585
		public virtual extern string IMsTscAx_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060015D4 RID: 5588
		// (set) Token: 0x060015D3 RID: 5587
		public virtual extern string IMsTscAx_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060015D6 RID: 5590
		// (set) Token: 0x060015D5 RID: 5589
		public virtual extern string IMsTscAx_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060015D8 RID: 5592
		// (set) Token: 0x060015D7 RID: 5591
		public virtual extern string IMsTscAx_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060015D9 RID: 5593
		public virtual extern short IMsTscAx_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060015DB RID: 5595
		// (set) Token: 0x060015DA RID: 5594
		public virtual extern int IMsTscAx_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060015DD RID: 5597
		// (set) Token: 0x060015DC RID: 5596
		public virtual extern int IMsTscAx_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060015DF RID: 5599
		// (set) Token: 0x060015DE RID: 5598
		public virtual extern int IMsTscAx_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060015E0 RID: 5600
		public virtual extern int IMsTscAx_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060015E1 RID: 5601
		public virtual extern int IMsTscAx_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009D5 RID: 2517
		// (set) Token: 0x060015E2 RID: 5602
		public virtual extern string IMsTscAx_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060015E3 RID: 5603
		public virtual extern int IMsTscAx_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060015E4 RID: 5604
		public virtual extern string IMsTscAx_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060015E5 RID: 5605
		public virtual extern int IMsTscAx_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060015E6 RID: 5606
		public virtual extern IMsTscSecuredSettings IMsTscAx_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060015E7 RID: 5607
		public virtual extern IMsTscAdvancedSettings IMsTscAx_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060015E8 RID: 5608
		public virtual extern IMsTscDebug IMsTscAx_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x060015E9 RID: 5609
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Connect();

		// Token: 0x060015EA RID: 5610
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Disconnect();

		// Token: 0x060015EB RID: 5611
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x060015EC RID: 5612
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x170009DC RID: 2524
		// (set) Token: 0x060015ED RID: 5613
		public virtual extern string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060015EF RID: 5615
		// (set) Token: 0x060015EE RID: 5614
		public virtual extern string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060015F1 RID: 5617
		// (set) Token: 0x060015F0 RID: 5616
		public virtual extern string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060015F3 RID: 5619
		// (set) Token: 0x060015F2 RID: 5618
		public virtual extern string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060015F5 RID: 5621
		// (set) Token: 0x060015F4 RID: 5620
		public virtual extern string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x060015F6 RID: 5622
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void ResetPassword();

		// Token: 0x170009E1 RID: 2529
		// (set) Token: 0x060015F7 RID: 5623
		public virtual extern string IMsRdpClientNonScriptable_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x060015F9 RID: 5625
		// (set) Token: 0x060015F8 RID: 5624
		public virtual extern string IMsRdpClientNonScriptable_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x060015FB RID: 5627
		// (set) Token: 0x060015FA RID: 5626
		public virtual extern string IMsRdpClientNonScriptable_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060015FD RID: 5629
		// (set) Token: 0x060015FC RID: 5628
		public virtual extern string IMsRdpClientNonScriptable_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060015FF RID: 5631
		// (set) Token: 0x060015FE RID: 5630
		public virtual extern string IMsRdpClientNonScriptable_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x06001600 RID: 5632
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable_ResetPassword();

		// Token: 0x06001601 RID: 5633
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06001602 RID: 5634
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170009E6 RID: 2534
		// (set) Token: 0x06001603 RID: 5635
		public virtual extern string IMsRdpClientNonScriptable2_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06001605 RID: 5637
		// (set) Token: 0x06001604 RID: 5636
		public virtual extern string IMsRdpClientNonScriptable2_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06001607 RID: 5639
		// (set) Token: 0x06001606 RID: 5638
		public virtual extern string IMsRdpClientNonScriptable2_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06001609 RID: 5641
		// (set) Token: 0x06001608 RID: 5640
		public virtual extern string IMsRdpClientNonScriptable2_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x0600160B RID: 5643
		// (set) Token: 0x0600160A RID: 5642
		public virtual extern string IMsRdpClientNonScriptable2_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600160C RID: 5644
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_ResetPassword();

		// Token: 0x0600160D RID: 5645
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x0600160E RID: 5646
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable2_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06001610 RID: 5648
		// (set) Token: 0x0600160F RID: 5647
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x170009EC RID: 2540
		// (set) Token: 0x06001611 RID: 5649
		public virtual extern string IMsRdpClientNonScriptable3_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06001613 RID: 5651
		// (set) Token: 0x06001612 RID: 5650
		public virtual extern string IMsRdpClientNonScriptable3_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06001615 RID: 5653
		// (set) Token: 0x06001614 RID: 5652
		public virtual extern string IMsRdpClientNonScriptable3_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06001617 RID: 5655
		// (set) Token: 0x06001616 RID: 5654
		public virtual extern string IMsRdpClientNonScriptable3_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06001619 RID: 5657
		// (set) Token: 0x06001618 RID: 5656
		public virtual extern string IMsRdpClientNonScriptable3_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600161A RID: 5658
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable3_ResetPassword();

		// Token: 0x0600161B RID: 5659
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable3_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x0600161C RID: 5660
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable3_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600161E RID: 5662
		// (set) Token: 0x0600161D RID: 5661
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr IMsRdpClientNonScriptable3_UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06001620 RID: 5664
		// (set) Token: 0x0600161F RID: 5663
		public virtual extern bool ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06001622 RID: 5666
		// (set) Token: 0x06001621 RID: 5665
		public virtual extern bool PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06001624 RID: 5668
		// (set) Token: 0x06001623 RID: 5667
		public virtual extern bool NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06001626 RID: 5670
		// (set) Token: 0x06001625 RID: 5669
		public virtual extern bool EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06001628 RID: 5672
		// (set) Token: 0x06001627 RID: 5671
		public virtual extern bool RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x0600162A RID: 5674
		// (set) Token: 0x06001629 RID: 5673
		public virtual extern bool RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x0600162B RID: 5675
		public virtual extern IMsRdpDeviceCollection DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600162C RID: 5676
		public virtual extern IMsRdpDriveCollection DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600162E RID: 5678
		// (set) Token: 0x0600162D RID: 5677
		public virtual extern bool WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06001630 RID: 5680
		// (set) Token: 0x0600162F RID: 5679
		public virtual extern bool WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06001632 RID: 5682
		// (set) Token: 0x06001631 RID: 5681
		public virtual extern string ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009FD RID: 2557
		// (set) Token: 0x06001633 RID: 5683
		public virtual extern string IMsRdpClientNonScriptable4_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06001635 RID: 5685
		// (set) Token: 0x06001634 RID: 5684
		public virtual extern string IMsRdpClientNonScriptable4_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06001637 RID: 5687
		// (set) Token: 0x06001636 RID: 5686
		public virtual extern string IMsRdpClientNonScriptable4_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06001639 RID: 5689
		// (set) Token: 0x06001638 RID: 5688
		public virtual extern string IMsRdpClientNonScriptable4_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x0600163B RID: 5691
		// (set) Token: 0x0600163A RID: 5690
		public virtual extern string IMsRdpClientNonScriptable4_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600163C RID: 5692
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable4_ResetPassword();

		// Token: 0x0600163D RID: 5693
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable4_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x0600163E RID: 5694
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable4_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06001640 RID: 5696
		// (set) Token: 0x0600163F RID: 5695
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr IMsRdpClientNonScriptable4_UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06001642 RID: 5698
		// (set) Token: 0x06001641 RID: 5697
		public virtual extern bool IMsRdpClientNonScriptable4_ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06001644 RID: 5700
		// (set) Token: 0x06001643 RID: 5699
		public virtual extern bool IMsRdpClientNonScriptable4_PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06001646 RID: 5702
		// (set) Token: 0x06001645 RID: 5701
		public virtual extern bool IMsRdpClientNonScriptable4_NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06001648 RID: 5704
		// (set) Token: 0x06001647 RID: 5703
		public virtual extern bool IMsRdpClientNonScriptable4_EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x0600164A RID: 5706
		// (set) Token: 0x06001649 RID: 5705
		public virtual extern bool IMsRdpClientNonScriptable4_RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600164C RID: 5708
		// (set) Token: 0x0600164B RID: 5707
		public virtual extern bool IMsRdpClientNonScriptable4_RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x0600164D RID: 5709
		public virtual extern IMsRdpDeviceCollection IMsRdpClientNonScriptable4_DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x0600164E RID: 5710
		public virtual extern IMsRdpDriveCollection IMsRdpClientNonScriptable4_DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06001650 RID: 5712
		// (set) Token: 0x0600164F RID: 5711
		public virtual extern bool IMsRdpClientNonScriptable4_WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06001652 RID: 5714
		// (set) Token: 0x06001651 RID: 5713
		public virtual extern bool IMsRdpClientNonScriptable4_WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06001654 RID: 5716
		// (set) Token: 0x06001653 RID: 5715
		public virtual extern string IMsRdpClientNonScriptable4_ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06001656 RID: 5718
		// (set) Token: 0x06001655 RID: 5717
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")]
		public virtual extern RedirectionWarningType RedirectionWarningType { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] [param: In] set; }

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06001658 RID: 5720
		// (set) Token: 0x06001657 RID: 5719
		public virtual extern bool MarkRdpSettingsSecure { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x0600165A RID: 5722
		// (set) Token: 0x06001659 RID: 5721
		public virtual extern object PublisherCertificateChain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x0600165C RID: 5724
		// (set) Token: 0x0600165B RID: 5723
		public virtual extern bool WarnAboutPrinterRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x0600165E RID: 5726
		// (set) Token: 0x0600165D RID: 5725
		public virtual extern bool AllowCredentialSaving { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06001660 RID: 5728
		// (set) Token: 0x0600165F RID: 5727
		public virtual extern bool PromptForCredsOnClient { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06001662 RID: 5730
		// (set) Token: 0x06001661 RID: 5729
		public virtual extern bool LaunchedViaClientShellInterface { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06001664 RID: 5732
		// (set) Token: 0x06001663 RID: 5731
		public virtual extern bool TrustedZoneSite { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A16 RID: 2582
		// (set) Token: 0x06001665 RID: 5733
		public virtual extern string IMsRdpClientNonScriptable5_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06001667 RID: 5735
		// (set) Token: 0x06001666 RID: 5734
		public virtual extern string IMsRdpClientNonScriptable5_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06001669 RID: 5737
		// (set) Token: 0x06001668 RID: 5736
		public virtual extern string IMsRdpClientNonScriptable5_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x0600166B RID: 5739
		// (set) Token: 0x0600166A RID: 5738
		public virtual extern string IMsRdpClientNonScriptable5_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x0600166D RID: 5741
		// (set) Token: 0x0600166C RID: 5740
		public virtual extern string IMsRdpClientNonScriptable5_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x0600166E RID: 5742
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable5_ResetPassword();

		// Token: 0x0600166F RID: 5743
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable5_NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x06001670 RID: 5744
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable5_SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06001672 RID: 5746
		// (set) Token: 0x06001671 RID: 5745
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")]
		public virtual extern IntPtr IMsRdpClientNonScriptable5_UIParentWindowHandle { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.wireHWND")] [param: In] set; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06001674 RID: 5748
		// (set) Token: 0x06001673 RID: 5747
		public virtual extern bool IMsRdpClientNonScriptable5_ShowRedirectionWarningDialog { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06001676 RID: 5750
		// (set) Token: 0x06001675 RID: 5749
		public virtual extern bool IMsRdpClientNonScriptable5_PromptForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06001678 RID: 5752
		// (set) Token: 0x06001677 RID: 5751
		public virtual extern bool IMsRdpClientNonScriptable5_NegotiateSecurityLayer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x0600167A RID: 5754
		// (set) Token: 0x06001679 RID: 5753
		public virtual extern bool IMsRdpClientNonScriptable5_EnableCredSspSupport { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x0600167C RID: 5756
		// (set) Token: 0x0600167B RID: 5755
		public virtual extern bool IMsRdpClientNonScriptable5_RedirectDynamicDrives { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x0600167E RID: 5758
		// (set) Token: 0x0600167D RID: 5757
		public virtual extern bool IMsRdpClientNonScriptable5_RedirectDynamicDevices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x0600167F RID: 5759
		public virtual extern IMsRdpDeviceCollection IMsRdpClientNonScriptable5_DeviceCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06001680 RID: 5760
		public virtual extern IMsRdpDriveCollection IMsRdpClientNonScriptable5_DriveCollection { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06001682 RID: 5762
		// (set) Token: 0x06001681 RID: 5761
		public virtual extern bool IMsRdpClientNonScriptable5_WarnAboutSendingCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06001684 RID: 5764
		// (set) Token: 0x06001683 RID: 5763
		public virtual extern bool IMsRdpClientNonScriptable5_WarnAboutClipboardRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06001686 RID: 5766
		// (set) Token: 0x06001685 RID: 5765
		public virtual extern string IMsRdpClientNonScriptable5_ConnectionBarText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06001688 RID: 5768
		// (set) Token: 0x06001687 RID: 5767
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")]
		public virtual extern RedirectionWarningType IMsRdpClientNonScriptable5_RedirectionWarningType { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.RedirectionWarningType")] [param: In] set; }

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x0600168A RID: 5770
		// (set) Token: 0x06001689 RID: 5769
		public virtual extern bool IMsRdpClientNonScriptable5_MarkRdpSettingsSecure { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x0600168C RID: 5772
		// (set) Token: 0x0600168B RID: 5771
		public virtual extern object IMsRdpClientNonScriptable5_PublisherCertificateChain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x0600168E RID: 5774
		// (set) Token: 0x0600168D RID: 5773
		public virtual extern bool IMsRdpClientNonScriptable5_WarnAboutPrinterRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06001690 RID: 5776
		// (set) Token: 0x0600168F RID: 5775
		public virtual extern bool IMsRdpClientNonScriptable5_AllowCredentialSaving { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06001692 RID: 5778
		// (set) Token: 0x06001691 RID: 5777
		public virtual extern bool IMsRdpClientNonScriptable5_PromptForCredsOnClient { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06001694 RID: 5780
		// (set) Token: 0x06001693 RID: 5779
		public virtual extern bool IMsRdpClientNonScriptable5_LaunchedViaClientShellInterface { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06001696 RID: 5782
		// (set) Token: 0x06001695 RID: 5781
		public virtual extern bool IMsRdpClientNonScriptable5_TrustedZoneSite { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06001698 RID: 5784
		// (set) Token: 0x06001697 RID: 5783
		public virtual extern bool UseMultimon { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06001699 RID: 5785
		public virtual extern uint RemoteMonitorCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x0600169A RID: 5786
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void GetRemoteMonitorsBoundingBox(out int pLeft, out int pTop, out int pRight, out int pBottom);

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600169B RID: 5787
		public virtual extern bool RemoteMonitorLayoutMatchesLocal { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000A32 RID: 2610
		// (set) Token: 0x0600169C RID: 5788
		public virtual extern bool DisableConnectionBar { [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x0600169E RID: 5790
		// (set) Token: 0x0600169D RID: 5789
		public virtual extern bool DisableRemoteAppCapsCheck { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060016A0 RID: 5792
		// (set) Token: 0x0600169F RID: 5791
		public virtual extern bool WarnAboutDirectXRedirection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060016A2 RID: 5794
		// (set) Token: 0x060016A1 RID: 5793
		public virtual extern bool AllowPromptingForCredentials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x060016A4 RID: 5796
		// (set) Token: 0x060016A3 RID: 5795
		public virtual extern bool UseRedirectionServerName { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060016A6 RID: 5798
		// (set) Token: 0x060016A5 RID: 5797
		public virtual extern object Property { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }
	}
}
