using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000075 RID: 117
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("222C4B5D-45D9-4DF0-A7C6-60CF9089D285")]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings6 : IMsRdpClientAdvancedSettings5
	{
		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06002216 RID: 8726
		// (set) Token: 0x06002215 RID: 8725
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06002218 RID: 8728
		// (set) Token: 0x06002217 RID: 8727
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x0600221A RID: 8730
		// (set) Token: 0x06002219 RID: 8729
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FF9 RID: 4089
		// (set) Token: 0x0600221B RID: 8731
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FFA RID: 4090
		// (set) Token: 0x0600221C RID: 8732
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FFB RID: 4091
		// (set) Token: 0x0600221D RID: 8733
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000FFC RID: 4092
		// (set) Token: 0x0600221E RID: 8734
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06002220 RID: 8736
		// (set) Token: 0x0600221F RID: 8735
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06002222 RID: 8738
		// (set) Token: 0x06002221 RID: 8737
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06002224 RID: 8740
		// (set) Token: 0x06002223 RID: 8739
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06002226 RID: 8742
		// (set) Token: 0x06002225 RID: 8741
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06002228 RID: 8744
		// (set) Token: 0x06002227 RID: 8743
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x0600222A RID: 8746
		// (set) Token: 0x06002229 RID: 8745
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x0600222C RID: 8748
		// (set) Token: 0x0600222B RID: 8747
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x0600222E RID: 8750
		// (set) Token: 0x0600222D RID: 8749
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06002230 RID: 8752
		// (set) Token: 0x0600222F RID: 8751
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06002232 RID: 8754
		// (set) Token: 0x06002231 RID: 8753
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06002234 RID: 8756
		// (set) Token: 0x06002233 RID: 8755
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06002236 RID: 8758
		// (set) Token: 0x06002235 RID: 8757
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06002238 RID: 8760
		// (set) Token: 0x06002237 RID: 8759
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x0600223A RID: 8762
		// (set) Token: 0x06002239 RID: 8761
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x0600223C RID: 8764
		// (set) Token: 0x0600223B RID: 8763
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x0600223E RID: 8766
		// (set) Token: 0x0600223D RID: 8765
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06002240 RID: 8768
		// (set) Token: 0x0600223F RID: 8767
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06002242 RID: 8770
		// (set) Token: 0x06002241 RID: 8769
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06002244 RID: 8772
		// (set) Token: 0x06002243 RID: 8771
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06002246 RID: 8774
		// (set) Token: 0x06002245 RID: 8773
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06002248 RID: 8776
		// (set) Token: 0x06002247 RID: 8775
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x0600224A RID: 8778
		// (set) Token: 0x06002249 RID: 8777
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x0600224C RID: 8780
		// (set) Token: 0x0600224B RID: 8779
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x0600224E RID: 8782
		// (set) Token: 0x0600224D RID: 8781
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06002250 RID: 8784
		// (set) Token: 0x0600224F RID: 8783
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06002252 RID: 8786
		// (set) Token: 0x06002251 RID: 8785
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06002254 RID: 8788
		// (set) Token: 0x06002253 RID: 8787
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06002256 RID: 8790
		// (set) Token: 0x06002255 RID: 8789
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001019 RID: 4121
		// (set) Token: 0x06002257 RID: 8791
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06002259 RID: 8793
		// (set) Token: 0x06002258 RID: 8792
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x0600225B RID: 8795
		// (set) Token: 0x0600225A RID: 8794
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x0600225D RID: 8797
		// (set) Token: 0x0600225C RID: 8796
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x0600225F RID: 8799
		// (set) Token: 0x0600225E RID: 8798
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06002261 RID: 8801
		// (set) Token: 0x06002260 RID: 8800
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06002263 RID: 8803
		// (set) Token: 0x06002262 RID: 8802
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06002265 RID: 8805
		// (set) Token: 0x06002264 RID: 8804
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06002267 RID: 8807
		// (set) Token: 0x06002266 RID: 8806
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06002269 RID: 8809
		// (set) Token: 0x06002268 RID: 8808
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x0600226B RID: 8811
		// (set) Token: 0x0600226A RID: 8810
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x0600226D RID: 8813
		// (set) Token: 0x0600226C RID: 8812
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x0600226F RID: 8815
		// (set) Token: 0x0600226E RID: 8814
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06002271 RID: 8817
		// (set) Token: 0x06002270 RID: 8816
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06002273 RID: 8819
		// (set) Token: 0x06002272 RID: 8818
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06002275 RID: 8821
		// (set) Token: 0x06002274 RID: 8820
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06002277 RID: 8823
		// (set) Token: 0x06002276 RID: 8822
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06002279 RID: 8825
		// (set) Token: 0x06002278 RID: 8824
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x0600227B RID: 8827
		// (set) Token: 0x0600227A RID: 8826
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x0600227D RID: 8829
		// (set) Token: 0x0600227C RID: 8828
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700102D RID: 4141
		// (set) Token: 0x0600227E RID: 8830
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06002280 RID: 8832
		// (set) Token: 0x0600227F RID: 8831
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06002282 RID: 8834
		// (set) Token: 0x06002281 RID: 8833
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06002284 RID: 8836
		// (set) Token: 0x06002283 RID: 8835
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06002286 RID: 8838
		// (set) Token: 0x06002285 RID: 8837
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06002288 RID: 8840
		// (set) Token: 0x06002287 RID: 8839
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x0600228A RID: 8842
		// (set) Token: 0x06002289 RID: 8841
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x0600228C RID: 8844
		// (set) Token: 0x0600228B RID: 8843
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x0600228E RID: 8846
		// (set) Token: 0x0600228D RID: 8845
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06002290 RID: 8848
		// (set) Token: 0x0600228F RID: 8847
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06002292 RID: 8850
		// (set) Token: 0x06002291 RID: 8849
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06002294 RID: 8852
		// (set) Token: 0x06002293 RID: 8851
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001039 RID: 4153
		// (set) Token: 0x06002295 RID: 8853
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06002297 RID: 8855
		// (set) Token: 0x06002296 RID: 8854
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06002298 RID: 8856
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x0600229A RID: 8858
		// (set) Token: 0x06002299 RID: 8857
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x0600229C RID: 8860
		// (set) Token: 0x0600229B RID: 8859
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x0600229E RID: 8862
		// (set) Token: 0x0600229D RID: 8861
		[DispId(210)]
		bool ConnectionBarShowMinimizeButton { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x060022A0 RID: 8864
		// (set) Token: 0x0600229F RID: 8863
		[DispId(211)]
		bool ConnectionBarShowRestoreButton { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x060022A2 RID: 8866
		// (set) Token: 0x060022A1 RID: 8865
		[DispId(212)]
		uint AuthenticationLevel { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x060022A4 RID: 8868
		// (set) Token: 0x060022A3 RID: 8867
		[DispId(213)]
		bool RedirectClipboard { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x060022A6 RID: 8870
		// (set) Token: 0x060022A5 RID: 8869
		[DispId(215)]
		uint AudioRedirectionMode { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x060022A8 RID: 8872
		// (set) Token: 0x060022A7 RID: 8871
		[DispId(216)]
		bool ConnectionBarShowPinButton { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x060022AA RID: 8874
		// (set) Token: 0x060022A9 RID: 8873
		[DispId(217)]
		bool PublicMode { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x060022AC RID: 8876
		// (set) Token: 0x060022AB RID: 8875
		[DispId(218)]
		bool RedirectDevices { [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x060022AE RID: 8878
		// (set) Token: 0x060022AD RID: 8877
		[DispId(219)]
		bool RedirectPOSDevices { [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x060022B0 RID: 8880
		// (set) Token: 0x060022AF RID: 8879
		[DispId(220)]
		int BitmapVirtualCache32BppSize { [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x060022B2 RID: 8882
		// (set) Token: 0x060022B1 RID: 8881
		[DispId(221)]
		bool RelativeMouseMode { [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x060022B3 RID: 8883
		// (set) Token: 0x060022B4 RID: 8884
		[DispId(222)]
		string AuthenticationServiceClass { [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x060022B5 RID: 8885
		// (set) Token: 0x060022B6 RID: 8886
		[DispId(223)]
		string PCB { [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x060022B8 RID: 8888
		// (set) Token: 0x060022B7 RID: 8887
		[DispId(224)]
		int HotKeyFocusReleaseLeft { [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x060022BA RID: 8890
		// (set) Token: 0x060022B9 RID: 8889
		[DispId(225)]
		int HotKeyFocusReleaseRight { [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x060022BC RID: 8892
		// (set) Token: 0x060022BB RID: 8891
		[DispId(17)]
		bool EnableCredSspSupport { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x060022BD RID: 8893
		[DispId(226)]
		uint AuthenticationType { [DispId(226)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x060022BF RID: 8895
		// (set) Token: 0x060022BE RID: 8894
		[DispId(227)]
		bool ConnectToAdministerServer { [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
