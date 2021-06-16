using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000072 RID: 114
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("FBA7F64E-6783-4405-DA45-FA4A763DABD0")]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings5 : IMsRdpClientAdvancedSettings4
	{
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x0600216E RID: 8558
		// (set) Token: 0x0600216D RID: 8557
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06002170 RID: 8560
		// (set) Token: 0x0600216F RID: 8559
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06002172 RID: 8562
		// (set) Token: 0x06002171 RID: 8561
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FA3 RID: 4003
		// (set) Token: 0x06002173 RID: 8563
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FA4 RID: 4004
		// (set) Token: 0x06002174 RID: 8564
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FA5 RID: 4005
		// (set) Token: 0x06002175 RID: 8565
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FA6 RID: 4006
		// (set) Token: 0x06002176 RID: 8566
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06002178 RID: 8568
		// (set) Token: 0x06002177 RID: 8567
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x0600217A RID: 8570
		// (set) Token: 0x06002179 RID: 8569
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x0600217C RID: 8572
		// (set) Token: 0x0600217B RID: 8571
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x0600217E RID: 8574
		// (set) Token: 0x0600217D RID: 8573
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06002180 RID: 8576
		// (set) Token: 0x0600217F RID: 8575
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06002182 RID: 8578
		// (set) Token: 0x06002181 RID: 8577
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06002184 RID: 8580
		// (set) Token: 0x06002183 RID: 8579
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06002186 RID: 8582
		// (set) Token: 0x06002185 RID: 8581
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06002188 RID: 8584
		// (set) Token: 0x06002187 RID: 8583
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x0600218A RID: 8586
		// (set) Token: 0x06002189 RID: 8585
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x0600218C RID: 8588
		// (set) Token: 0x0600218B RID: 8587
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x0600218E RID: 8590
		// (set) Token: 0x0600218D RID: 8589
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06002190 RID: 8592
		// (set) Token: 0x0600218F RID: 8591
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06002192 RID: 8594
		// (set) Token: 0x06002191 RID: 8593
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06002194 RID: 8596
		// (set) Token: 0x06002193 RID: 8595
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06002196 RID: 8598
		// (set) Token: 0x06002195 RID: 8597
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06002198 RID: 8600
		// (set) Token: 0x06002197 RID: 8599
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x0600219A RID: 8602
		// (set) Token: 0x06002199 RID: 8601
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x0600219C RID: 8604
		// (set) Token: 0x0600219B RID: 8603
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x0600219E RID: 8606
		// (set) Token: 0x0600219D RID: 8605
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x060021A0 RID: 8608
		// (set) Token: 0x0600219F RID: 8607
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x060021A2 RID: 8610
		// (set) Token: 0x060021A1 RID: 8609
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x060021A4 RID: 8612
		// (set) Token: 0x060021A3 RID: 8611
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x060021A6 RID: 8614
		// (set) Token: 0x060021A5 RID: 8613
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x060021A8 RID: 8616
		// (set) Token: 0x060021A7 RID: 8615
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x060021AA RID: 8618
		// (set) Token: 0x060021A9 RID: 8617
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060021AC RID: 8620
		// (set) Token: 0x060021AB RID: 8619
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060021AE RID: 8622
		// (set) Token: 0x060021AD RID: 8621
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC3 RID: 4035
		// (set) Token: 0x060021AF RID: 8623
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x060021B1 RID: 8625
		// (set) Token: 0x060021B0 RID: 8624
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x060021B3 RID: 8627
		// (set) Token: 0x060021B2 RID: 8626
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x060021B5 RID: 8629
		// (set) Token: 0x060021B4 RID: 8628
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x060021B7 RID: 8631
		// (set) Token: 0x060021B6 RID: 8630
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x060021B9 RID: 8633
		// (set) Token: 0x060021B8 RID: 8632
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x060021BB RID: 8635
		// (set) Token: 0x060021BA RID: 8634
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x060021BD RID: 8637
		// (set) Token: 0x060021BC RID: 8636
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x060021BF RID: 8639
		// (set) Token: 0x060021BE RID: 8638
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x060021C1 RID: 8641
		// (set) Token: 0x060021C0 RID: 8640
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060021C3 RID: 8643
		// (set) Token: 0x060021C2 RID: 8642
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x060021C5 RID: 8645
		// (set) Token: 0x060021C4 RID: 8644
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x060021C7 RID: 8647
		// (set) Token: 0x060021C6 RID: 8646
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x060021C9 RID: 8649
		// (set) Token: 0x060021C8 RID: 8648
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060021CB RID: 8651
		// (set) Token: 0x060021CA RID: 8650
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060021CD RID: 8653
		// (set) Token: 0x060021CC RID: 8652
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060021CF RID: 8655
		// (set) Token: 0x060021CE RID: 8654
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x060021D1 RID: 8657
		// (set) Token: 0x060021D0 RID: 8656
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060021D3 RID: 8659
		// (set) Token: 0x060021D2 RID: 8658
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060021D5 RID: 8661
		// (set) Token: 0x060021D4 RID: 8660
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FD7 RID: 4055
		// (set) Token: 0x060021D6 RID: 8662
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060021D8 RID: 8664
		// (set) Token: 0x060021D7 RID: 8663
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060021DA RID: 8666
		// (set) Token: 0x060021D9 RID: 8665
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x060021DC RID: 8668
		// (set) Token: 0x060021DB RID: 8667
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x060021DE RID: 8670
		// (set) Token: 0x060021DD RID: 8669
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x060021E0 RID: 8672
		// (set) Token: 0x060021DF RID: 8671
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060021E2 RID: 8674
		// (set) Token: 0x060021E1 RID: 8673
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060021E4 RID: 8676
		// (set) Token: 0x060021E3 RID: 8675
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060021E6 RID: 8678
		// (set) Token: 0x060021E5 RID: 8677
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060021E8 RID: 8680
		// (set) Token: 0x060021E7 RID: 8679
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060021EA RID: 8682
		// (set) Token: 0x060021E9 RID: 8681
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x060021EC RID: 8684
		// (set) Token: 0x060021EB RID: 8683
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE3 RID: 4067
		// (set) Token: 0x060021ED RID: 8685
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060021EF RID: 8687
		// (set) Token: 0x060021EE RID: 8686
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060021F0 RID: 8688
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060021F2 RID: 8690
		// (set) Token: 0x060021F1 RID: 8689
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x060021F4 RID: 8692
		// (set) Token: 0x060021F3 RID: 8691
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x060021F6 RID: 8694
		// (set) Token: 0x060021F5 RID: 8693
		[DispId(210)]
		bool ConnectionBarShowMinimizeButton { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x060021F8 RID: 8696
		// (set) Token: 0x060021F7 RID: 8695
		[DispId(211)]
		bool ConnectionBarShowRestoreButton { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x060021FA RID: 8698
		// (set) Token: 0x060021F9 RID: 8697
		[DispId(212)]
		uint AuthenticationLevel { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x060021FC RID: 8700
		// (set) Token: 0x060021FB RID: 8699
		[DispId(213)]
		bool RedirectClipboard { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x060021FE RID: 8702
		// (set) Token: 0x060021FD RID: 8701
		[DispId(215)]
		uint AudioRedirectionMode { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06002200 RID: 8704
		// (set) Token: 0x060021FF RID: 8703
		[DispId(216)]
		bool ConnectionBarShowPinButton { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06002202 RID: 8706
		// (set) Token: 0x06002201 RID: 8705
		[DispId(217)]
		bool PublicMode { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06002204 RID: 8708
		// (set) Token: 0x06002203 RID: 8707
		[DispId(218)]
		bool RedirectDevices { [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06002206 RID: 8710
		// (set) Token: 0x06002205 RID: 8709
		[DispId(219)]
		bool RedirectPOSDevices { [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06002208 RID: 8712
		// (set) Token: 0x06002207 RID: 8711
		[DispId(220)]
		int BitmapVirtualCache32BppSize { [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
