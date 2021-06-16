using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200006E RID: 110
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("9AC42117-2B76-4320-AA44-0E616AB8437B")]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings2 : IMsRdpClientAdvancedSettings
	{
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06001FC0 RID: 8128
		// (set) Token: 0x06001FBF RID: 8127
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06001FC2 RID: 8130
		// (set) Token: 0x06001FC1 RID: 8129
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06001FC4 RID: 8132
		// (set) Token: 0x06001FC3 RID: 8131
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EBF RID: 3775
		// (set) Token: 0x06001FC5 RID: 8133
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EC0 RID: 3776
		// (set) Token: 0x06001FC6 RID: 8134
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EC1 RID: 3777
		// (set) Token: 0x06001FC7 RID: 8135
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EC2 RID: 3778
		// (set) Token: 0x06001FC8 RID: 8136
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06001FCA RID: 8138
		// (set) Token: 0x06001FC9 RID: 8137
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06001FCC RID: 8140
		// (set) Token: 0x06001FCB RID: 8139
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06001FCE RID: 8142
		// (set) Token: 0x06001FCD RID: 8141
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06001FD0 RID: 8144
		// (set) Token: 0x06001FCF RID: 8143
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06001FD2 RID: 8146
		// (set) Token: 0x06001FD1 RID: 8145
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06001FD4 RID: 8148
		// (set) Token: 0x06001FD3 RID: 8147
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06001FD6 RID: 8150
		// (set) Token: 0x06001FD5 RID: 8149
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06001FD8 RID: 8152
		// (set) Token: 0x06001FD7 RID: 8151
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06001FDA RID: 8154
		// (set) Token: 0x06001FD9 RID: 8153
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06001FDC RID: 8156
		// (set) Token: 0x06001FDB RID: 8155
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06001FDE RID: 8158
		// (set) Token: 0x06001FDD RID: 8157
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06001FE0 RID: 8160
		// (set) Token: 0x06001FDF RID: 8159
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06001FE2 RID: 8162
		// (set) Token: 0x06001FE1 RID: 8161
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06001FE4 RID: 8164
		// (set) Token: 0x06001FE3 RID: 8163
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06001FE6 RID: 8166
		// (set) Token: 0x06001FE5 RID: 8165
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06001FE8 RID: 8168
		// (set) Token: 0x06001FE7 RID: 8167
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06001FEA RID: 8170
		// (set) Token: 0x06001FE9 RID: 8169
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06001FEC RID: 8172
		// (set) Token: 0x06001FEB RID: 8171
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06001FEE RID: 8174
		// (set) Token: 0x06001FED RID: 8173
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06001FF0 RID: 8176
		// (set) Token: 0x06001FEF RID: 8175
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06001FF2 RID: 8178
		// (set) Token: 0x06001FF1 RID: 8177
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06001FF4 RID: 8180
		// (set) Token: 0x06001FF3 RID: 8179
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06001FF6 RID: 8182
		// (set) Token: 0x06001FF5 RID: 8181
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06001FF8 RID: 8184
		// (set) Token: 0x06001FF7 RID: 8183
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06001FFA RID: 8186
		// (set) Token: 0x06001FF9 RID: 8185
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06001FFC RID: 8188
		// (set) Token: 0x06001FFB RID: 8187
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06001FFE RID: 8190
		// (set) Token: 0x06001FFD RID: 8189
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06002000 RID: 8192
		// (set) Token: 0x06001FFF RID: 8191
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EDF RID: 3807
		// (set) Token: 0x06002001 RID: 8193
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06002003 RID: 8195
		// (set) Token: 0x06002002 RID: 8194
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06002005 RID: 8197
		// (set) Token: 0x06002004 RID: 8196
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06002007 RID: 8199
		// (set) Token: 0x06002006 RID: 8198
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06002009 RID: 8201
		// (set) Token: 0x06002008 RID: 8200
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x0600200B RID: 8203
		// (set) Token: 0x0600200A RID: 8202
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x0600200D RID: 8205
		// (set) Token: 0x0600200C RID: 8204
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x0600200F RID: 8207
		// (set) Token: 0x0600200E RID: 8206
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06002011 RID: 8209
		// (set) Token: 0x06002010 RID: 8208
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06002013 RID: 8211
		// (set) Token: 0x06002012 RID: 8210
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06002015 RID: 8213
		// (set) Token: 0x06002014 RID: 8212
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06002017 RID: 8215
		// (set) Token: 0x06002016 RID: 8214
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06002019 RID: 8217
		// (set) Token: 0x06002018 RID: 8216
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x0600201B RID: 8219
		// (set) Token: 0x0600201A RID: 8218
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x0600201D RID: 8221
		// (set) Token: 0x0600201C RID: 8220
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x0600201F RID: 8223
		// (set) Token: 0x0600201E RID: 8222
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06002021 RID: 8225
		// (set) Token: 0x06002020 RID: 8224
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06002023 RID: 8227
		// (set) Token: 0x06002022 RID: 8226
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06002025 RID: 8229
		// (set) Token: 0x06002024 RID: 8228
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06002027 RID: 8231
		// (set) Token: 0x06002026 RID: 8230
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EF3 RID: 3827
		// (set) Token: 0x06002028 RID: 8232
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x0600202A RID: 8234
		// (set) Token: 0x06002029 RID: 8233
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x0600202C RID: 8236
		// (set) Token: 0x0600202B RID: 8235
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x0600202E RID: 8238
		// (set) Token: 0x0600202D RID: 8237
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06002030 RID: 8240
		// (set) Token: 0x0600202F RID: 8239
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06002032 RID: 8242
		// (set) Token: 0x06002031 RID: 8241
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06002034 RID: 8244
		// (set) Token: 0x06002033 RID: 8243
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06002036 RID: 8246
		// (set) Token: 0x06002035 RID: 8245
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06002038 RID: 8248
		// (set) Token: 0x06002037 RID: 8247
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x0600203A RID: 8250
		// (set) Token: 0x06002039 RID: 8249
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x0600203C RID: 8252
		// (set) Token: 0x0600203B RID: 8251
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x0600203E RID: 8254
		// (set) Token: 0x0600203D RID: 8253
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EFF RID: 3839
		// (set) Token: 0x0600203F RID: 8255
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06002041 RID: 8257
		// (set) Token: 0x06002040 RID: 8256
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06002042 RID: 8258
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06002044 RID: 8260
		// (set) Token: 0x06002043 RID: 8259
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06002046 RID: 8262
		// (set) Token: 0x06002045 RID: 8261
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
