using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000070 RID: 112
	[Guid("FBA7F64E-7345-4405-AE50-FA4A763DC0DE")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings4 : IMsRdpClientAdvancedSettings3
	{
		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x060020D4 RID: 8404
		// (set) Token: 0x060020D3 RID: 8403
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060020D6 RID: 8406
		// (set) Token: 0x060020D5 RID: 8405
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060020D8 RID: 8408
		// (set) Token: 0x060020D7 RID: 8407
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F51 RID: 3921
		// (set) Token: 0x060020D9 RID: 8409
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F52 RID: 3922
		// (set) Token: 0x060020DA RID: 8410
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F53 RID: 3923
		// (set) Token: 0x060020DB RID: 8411
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F54 RID: 3924
		// (set) Token: 0x060020DC RID: 8412
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060020DE RID: 8414
		// (set) Token: 0x060020DD RID: 8413
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060020E0 RID: 8416
		// (set) Token: 0x060020DF RID: 8415
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060020E2 RID: 8418
		// (set) Token: 0x060020E1 RID: 8417
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060020E4 RID: 8420
		// (set) Token: 0x060020E3 RID: 8419
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x060020E6 RID: 8422
		// (set) Token: 0x060020E5 RID: 8421
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x060020E8 RID: 8424
		// (set) Token: 0x060020E7 RID: 8423
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x060020EA RID: 8426
		// (set) Token: 0x060020E9 RID: 8425
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x060020EC RID: 8428
		// (set) Token: 0x060020EB RID: 8427
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x060020EE RID: 8430
		// (set) Token: 0x060020ED RID: 8429
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x060020F0 RID: 8432
		// (set) Token: 0x060020EF RID: 8431
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x060020F2 RID: 8434
		// (set) Token: 0x060020F1 RID: 8433
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x060020F4 RID: 8436
		// (set) Token: 0x060020F3 RID: 8435
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x060020F6 RID: 8438
		// (set) Token: 0x060020F5 RID: 8437
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x060020F8 RID: 8440
		// (set) Token: 0x060020F7 RID: 8439
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x060020FA RID: 8442
		// (set) Token: 0x060020F9 RID: 8441
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x060020FC RID: 8444
		// (set) Token: 0x060020FB RID: 8443
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x060020FE RID: 8446
		// (set) Token: 0x060020FD RID: 8445
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06002100 RID: 8448
		// (set) Token: 0x060020FF RID: 8447
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06002102 RID: 8450
		// (set) Token: 0x06002101 RID: 8449
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06002104 RID: 8452
		// (set) Token: 0x06002103 RID: 8451
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06002106 RID: 8454
		// (set) Token: 0x06002105 RID: 8453
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06002108 RID: 8456
		// (set) Token: 0x06002107 RID: 8455
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x0600210A RID: 8458
		// (set) Token: 0x06002109 RID: 8457
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x0600210C RID: 8460
		// (set) Token: 0x0600210B RID: 8459
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x0600210E RID: 8462
		// (set) Token: 0x0600210D RID: 8461
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06002110 RID: 8464
		// (set) Token: 0x0600210F RID: 8463
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06002112 RID: 8466
		// (set) Token: 0x06002111 RID: 8465
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06002114 RID: 8468
		// (set) Token: 0x06002113 RID: 8467
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F71 RID: 3953
		// (set) Token: 0x06002115 RID: 8469
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06002117 RID: 8471
		// (set) Token: 0x06002116 RID: 8470
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06002119 RID: 8473
		// (set) Token: 0x06002118 RID: 8472
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x0600211B RID: 8475
		// (set) Token: 0x0600211A RID: 8474
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x0600211D RID: 8477
		// (set) Token: 0x0600211C RID: 8476
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x0600211F RID: 8479
		// (set) Token: 0x0600211E RID: 8478
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06002121 RID: 8481
		// (set) Token: 0x06002120 RID: 8480
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06002123 RID: 8483
		// (set) Token: 0x06002122 RID: 8482
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06002125 RID: 8485
		// (set) Token: 0x06002124 RID: 8484
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06002127 RID: 8487
		// (set) Token: 0x06002126 RID: 8486
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06002129 RID: 8489
		// (set) Token: 0x06002128 RID: 8488
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x0600212B RID: 8491
		// (set) Token: 0x0600212A RID: 8490
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x0600212D RID: 8493
		// (set) Token: 0x0600212C RID: 8492
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x0600212F RID: 8495
		// (set) Token: 0x0600212E RID: 8494
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06002131 RID: 8497
		// (set) Token: 0x06002130 RID: 8496
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06002133 RID: 8499
		// (set) Token: 0x06002132 RID: 8498
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06002135 RID: 8501
		// (set) Token: 0x06002134 RID: 8500
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06002137 RID: 8503
		// (set) Token: 0x06002136 RID: 8502
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06002139 RID: 8505
		// (set) Token: 0x06002138 RID: 8504
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x0600213B RID: 8507
		// (set) Token: 0x0600213A RID: 8506
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F85 RID: 3973
		// (set) Token: 0x0600213C RID: 8508
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x0600213E RID: 8510
		// (set) Token: 0x0600213D RID: 8509
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06002140 RID: 8512
		// (set) Token: 0x0600213F RID: 8511
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06002142 RID: 8514
		// (set) Token: 0x06002141 RID: 8513
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06002144 RID: 8516
		// (set) Token: 0x06002143 RID: 8515
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06002146 RID: 8518
		// (set) Token: 0x06002145 RID: 8517
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06002148 RID: 8520
		// (set) Token: 0x06002147 RID: 8519
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x0600214A RID: 8522
		// (set) Token: 0x06002149 RID: 8521
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x0600214C RID: 8524
		// (set) Token: 0x0600214B RID: 8523
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x0600214E RID: 8526
		// (set) Token: 0x0600214D RID: 8525
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x06002150 RID: 8528
		// (set) Token: 0x0600214F RID: 8527
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06002152 RID: 8530
		// (set) Token: 0x06002151 RID: 8529
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F91 RID: 3985
		// (set) Token: 0x06002153 RID: 8531
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06002155 RID: 8533
		// (set) Token: 0x06002154 RID: 8532
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06002156 RID: 8534
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06002158 RID: 8536
		// (set) Token: 0x06002157 RID: 8535
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x0600215A RID: 8538
		// (set) Token: 0x06002159 RID: 8537
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x0600215C RID: 8540
		// (set) Token: 0x0600215B RID: 8539
		[DispId(210)]
		bool ConnectionBarShowMinimizeButton { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x0600215E RID: 8542
		// (set) Token: 0x0600215D RID: 8541
		[DispId(211)]
		bool ConnectionBarShowRestoreButton { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06002160 RID: 8544
		// (set) Token: 0x0600215F RID: 8543
		[DispId(212)]
		uint AuthenticationLevel { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
