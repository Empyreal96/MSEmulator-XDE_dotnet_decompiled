using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200006F RID: 111
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("19CD856B-C542-4C53-ACEE-F127E3BE1A59")]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings3 : IMsRdpClientAdvancedSettings2
	{
		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06002048 RID: 8264
		// (set) Token: 0x06002047 RID: 8263
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x0600204A RID: 8266
		// (set) Token: 0x06002049 RID: 8265
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x0600204C RID: 8268
		// (set) Token: 0x0600204B RID: 8267
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F07 RID: 3847
		// (set) Token: 0x0600204D RID: 8269
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F08 RID: 3848
		// (set) Token: 0x0600204E RID: 8270
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F09 RID: 3849
		// (set) Token: 0x0600204F RID: 8271
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F0A RID: 3850
		// (set) Token: 0x06002050 RID: 8272
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06002052 RID: 8274
		// (set) Token: 0x06002051 RID: 8273
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06002054 RID: 8276
		// (set) Token: 0x06002053 RID: 8275
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06002056 RID: 8278
		// (set) Token: 0x06002055 RID: 8277
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06002058 RID: 8280
		// (set) Token: 0x06002057 RID: 8279
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x0600205A RID: 8282
		// (set) Token: 0x06002059 RID: 8281
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x0600205C RID: 8284
		// (set) Token: 0x0600205B RID: 8283
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x0600205E RID: 8286
		// (set) Token: 0x0600205D RID: 8285
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06002060 RID: 8288
		// (set) Token: 0x0600205F RID: 8287
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06002062 RID: 8290
		// (set) Token: 0x06002061 RID: 8289
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06002064 RID: 8292
		// (set) Token: 0x06002063 RID: 8291
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06002066 RID: 8294
		// (set) Token: 0x06002065 RID: 8293
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06002068 RID: 8296
		// (set) Token: 0x06002067 RID: 8295
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x0600206A RID: 8298
		// (set) Token: 0x06002069 RID: 8297
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x0600206C RID: 8300
		// (set) Token: 0x0600206B RID: 8299
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x0600206E RID: 8302
		// (set) Token: 0x0600206D RID: 8301
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06002070 RID: 8304
		// (set) Token: 0x0600206F RID: 8303
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06002072 RID: 8306
		// (set) Token: 0x06002071 RID: 8305
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06002074 RID: 8308
		// (set) Token: 0x06002073 RID: 8307
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06002076 RID: 8310
		// (set) Token: 0x06002075 RID: 8309
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06002078 RID: 8312
		// (set) Token: 0x06002077 RID: 8311
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x0600207A RID: 8314
		// (set) Token: 0x06002079 RID: 8313
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x0600207C RID: 8316
		// (set) Token: 0x0600207B RID: 8315
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x0600207E RID: 8318
		// (set) Token: 0x0600207D RID: 8317
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06002080 RID: 8320
		// (set) Token: 0x0600207F RID: 8319
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06002082 RID: 8322
		// (set) Token: 0x06002081 RID: 8321
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06002084 RID: 8324
		// (set) Token: 0x06002083 RID: 8323
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06002086 RID: 8326
		// (set) Token: 0x06002085 RID: 8325
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06002088 RID: 8328
		// (set) Token: 0x06002087 RID: 8327
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F27 RID: 3879
		// (set) Token: 0x06002089 RID: 8329
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x0600208B RID: 8331
		// (set) Token: 0x0600208A RID: 8330
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x0600208D RID: 8333
		// (set) Token: 0x0600208C RID: 8332
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x0600208F RID: 8335
		// (set) Token: 0x0600208E RID: 8334
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06002091 RID: 8337
		// (set) Token: 0x06002090 RID: 8336
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06002093 RID: 8339
		// (set) Token: 0x06002092 RID: 8338
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06002095 RID: 8341
		// (set) Token: 0x06002094 RID: 8340
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06002097 RID: 8343
		// (set) Token: 0x06002096 RID: 8342
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06002099 RID: 8345
		// (set) Token: 0x06002098 RID: 8344
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x0600209B RID: 8347
		// (set) Token: 0x0600209A RID: 8346
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x0600209D RID: 8349
		// (set) Token: 0x0600209C RID: 8348
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x0600209F RID: 8351
		// (set) Token: 0x0600209E RID: 8350
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x060020A1 RID: 8353
		// (set) Token: 0x060020A0 RID: 8352
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x060020A3 RID: 8355
		// (set) Token: 0x060020A2 RID: 8354
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x060020A5 RID: 8357
		// (set) Token: 0x060020A4 RID: 8356
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x060020A7 RID: 8359
		// (set) Token: 0x060020A6 RID: 8358
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x060020A9 RID: 8361
		// (set) Token: 0x060020A8 RID: 8360
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x060020AB RID: 8363
		// (set) Token: 0x060020AA RID: 8362
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x060020AD RID: 8365
		// (set) Token: 0x060020AC RID: 8364
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x060020AF RID: 8367
		// (set) Token: 0x060020AE RID: 8366
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F3B RID: 3899
		// (set) Token: 0x060020B0 RID: 8368
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x060020B2 RID: 8370
		// (set) Token: 0x060020B1 RID: 8369
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x060020B4 RID: 8372
		// (set) Token: 0x060020B3 RID: 8371
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x060020B6 RID: 8374
		// (set) Token: 0x060020B5 RID: 8373
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x060020B8 RID: 8376
		// (set) Token: 0x060020B7 RID: 8375
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x060020BA RID: 8378
		// (set) Token: 0x060020B9 RID: 8377
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x060020BC RID: 8380
		// (set) Token: 0x060020BB RID: 8379
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x060020BE RID: 8382
		// (set) Token: 0x060020BD RID: 8381
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x060020C0 RID: 8384
		// (set) Token: 0x060020BF RID: 8383
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x060020C2 RID: 8386
		// (set) Token: 0x060020C1 RID: 8385
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060020C4 RID: 8388
		// (set) Token: 0x060020C3 RID: 8387
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060020C6 RID: 8390
		// (set) Token: 0x060020C5 RID: 8389
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F47 RID: 3911
		// (set) Token: 0x060020C7 RID: 8391
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060020C9 RID: 8393
		// (set) Token: 0x060020C8 RID: 8392
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060020CA RID: 8394
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060020CC RID: 8396
		// (set) Token: 0x060020CB RID: 8395
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x060020CE RID: 8398
		// (set) Token: 0x060020CD RID: 8397
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060020D0 RID: 8400
		// (set) Token: 0x060020CF RID: 8399
		[DispId(210)]
		bool ConnectionBarShowMinimizeButton { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060020D2 RID: 8402
		// (set) Token: 0x060020D1 RID: 8401
		[DispId(211)]
		bool ConnectionBarShowRestoreButton { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
