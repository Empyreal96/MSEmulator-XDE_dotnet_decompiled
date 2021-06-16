using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000077 RID: 119
	[Guid("26036036-4010-4578-8091-0DB9A1EDF9C3")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings7 : IMsRdpClientAdvancedSettings6
	{
		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x060022DE RID: 8926
		// (set) Token: 0x060022DD RID: 8925
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x060022E0 RID: 8928
		// (set) Token: 0x060022DF RID: 8927
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x060022E2 RID: 8930
		// (set) Token: 0x060022E1 RID: 8929
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001063 RID: 4195
		// (set) Token: 0x060022E3 RID: 8931
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001064 RID: 4196
		// (set) Token: 0x060022E4 RID: 8932
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001065 RID: 4197
		// (set) Token: 0x060022E5 RID: 8933
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001066 RID: 4198
		// (set) Token: 0x060022E6 RID: 8934
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x060022E8 RID: 8936
		// (set) Token: 0x060022E7 RID: 8935
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x060022EA RID: 8938
		// (set) Token: 0x060022E9 RID: 8937
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x060022EC RID: 8940
		// (set) Token: 0x060022EB RID: 8939
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x060022EE RID: 8942
		// (set) Token: 0x060022ED RID: 8941
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x060022F0 RID: 8944
		// (set) Token: 0x060022EF RID: 8943
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x060022F2 RID: 8946
		// (set) Token: 0x060022F1 RID: 8945
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x060022F4 RID: 8948
		// (set) Token: 0x060022F3 RID: 8947
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x060022F6 RID: 8950
		// (set) Token: 0x060022F5 RID: 8949
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x060022F8 RID: 8952
		// (set) Token: 0x060022F7 RID: 8951
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x060022FA RID: 8954
		// (set) Token: 0x060022F9 RID: 8953
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x060022FC RID: 8956
		// (set) Token: 0x060022FB RID: 8955
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x060022FE RID: 8958
		// (set) Token: 0x060022FD RID: 8957
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06002300 RID: 8960
		// (set) Token: 0x060022FF RID: 8959
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06002302 RID: 8962
		// (set) Token: 0x06002301 RID: 8961
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06002304 RID: 8964
		// (set) Token: 0x06002303 RID: 8963
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06002306 RID: 8966
		// (set) Token: 0x06002305 RID: 8965
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06002308 RID: 8968
		// (set) Token: 0x06002307 RID: 8967
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x0600230A RID: 8970
		// (set) Token: 0x06002309 RID: 8969
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x0600230C RID: 8972
		// (set) Token: 0x0600230B RID: 8971
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x0600230E RID: 8974
		// (set) Token: 0x0600230D RID: 8973
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06002310 RID: 8976
		// (set) Token: 0x0600230F RID: 8975
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06002312 RID: 8978
		// (set) Token: 0x06002311 RID: 8977
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06002314 RID: 8980
		// (set) Token: 0x06002313 RID: 8979
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06002316 RID: 8982
		// (set) Token: 0x06002315 RID: 8981
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06002318 RID: 8984
		// (set) Token: 0x06002317 RID: 8983
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x0600231A RID: 8986
		// (set) Token: 0x06002319 RID: 8985
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x0600231C RID: 8988
		// (set) Token: 0x0600231B RID: 8987
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x0600231E RID: 8990
		// (set) Token: 0x0600231D RID: 8989
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001083 RID: 4227
		// (set) Token: 0x0600231F RID: 8991
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06002321 RID: 8993
		// (set) Token: 0x06002320 RID: 8992
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06002323 RID: 8995
		// (set) Token: 0x06002322 RID: 8994
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06002325 RID: 8997
		// (set) Token: 0x06002324 RID: 8996
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06002327 RID: 8999
		// (set) Token: 0x06002326 RID: 8998
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06002329 RID: 9001
		// (set) Token: 0x06002328 RID: 9000
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x0600232B RID: 9003
		// (set) Token: 0x0600232A RID: 9002
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x0600232D RID: 9005
		// (set) Token: 0x0600232C RID: 9004
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x0600232F RID: 9007
		// (set) Token: 0x0600232E RID: 9006
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06002331 RID: 9009
		// (set) Token: 0x06002330 RID: 9008
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06002333 RID: 9011
		// (set) Token: 0x06002332 RID: 9010
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06002335 RID: 9013
		// (set) Token: 0x06002334 RID: 9012
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06002337 RID: 9015
		// (set) Token: 0x06002336 RID: 9014
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06002339 RID: 9017
		// (set) Token: 0x06002338 RID: 9016
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x0600233B RID: 9019
		// (set) Token: 0x0600233A RID: 9018
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x0600233D RID: 9021
		// (set) Token: 0x0600233C RID: 9020
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x0600233F RID: 9023
		// (set) Token: 0x0600233E RID: 9022
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06002341 RID: 9025
		// (set) Token: 0x06002340 RID: 9024
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06002343 RID: 9027
		// (set) Token: 0x06002342 RID: 9026
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06002345 RID: 9029
		// (set) Token: 0x06002344 RID: 9028
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001097 RID: 4247
		// (set) Token: 0x06002346 RID: 9030
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06002348 RID: 9032
		// (set) Token: 0x06002347 RID: 9031
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x0600234A RID: 9034
		// (set) Token: 0x06002349 RID: 9033
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x0600234C RID: 9036
		// (set) Token: 0x0600234B RID: 9035
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x0600234E RID: 9038
		// (set) Token: 0x0600234D RID: 9037
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06002350 RID: 9040
		// (set) Token: 0x0600234F RID: 9039
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06002352 RID: 9042
		// (set) Token: 0x06002351 RID: 9041
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06002354 RID: 9044
		// (set) Token: 0x06002353 RID: 9043
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06002356 RID: 9046
		// (set) Token: 0x06002355 RID: 9045
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06002358 RID: 9048
		// (set) Token: 0x06002357 RID: 9047
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x0600235A RID: 9050
		// (set) Token: 0x06002359 RID: 9049
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x0600235C RID: 9052
		// (set) Token: 0x0600235B RID: 9051
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A3 RID: 4259
		// (set) Token: 0x0600235D RID: 9053
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x0600235F RID: 9055
		// (set) Token: 0x0600235E RID: 9054
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06002360 RID: 9056
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06002362 RID: 9058
		// (set) Token: 0x06002361 RID: 9057
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06002364 RID: 9060
		// (set) Token: 0x06002363 RID: 9059
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06002366 RID: 9062
		// (set) Token: 0x06002365 RID: 9061
		[DispId(210)]
		bool ConnectionBarShowMinimizeButton { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06002368 RID: 9064
		// (set) Token: 0x06002367 RID: 9063
		[DispId(211)]
		bool ConnectionBarShowRestoreButton { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x0600236A RID: 9066
		// (set) Token: 0x06002369 RID: 9065
		[DispId(212)]
		uint AuthenticationLevel { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x0600236C RID: 9068
		// (set) Token: 0x0600236B RID: 9067
		[DispId(213)]
		bool RedirectClipboard { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x0600236E RID: 9070
		// (set) Token: 0x0600236D RID: 9069
		[DispId(215)]
		uint AudioRedirectionMode { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06002370 RID: 9072
		// (set) Token: 0x0600236F RID: 9071
		[DispId(216)]
		bool ConnectionBarShowPinButton { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06002372 RID: 9074
		// (set) Token: 0x06002371 RID: 9073
		[DispId(217)]
		bool PublicMode { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06002374 RID: 9076
		// (set) Token: 0x06002373 RID: 9075
		[DispId(218)]
		bool RedirectDevices { [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06002376 RID: 9078
		// (set) Token: 0x06002375 RID: 9077
		[DispId(219)]
		bool RedirectPOSDevices { [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06002378 RID: 9080
		// (set) Token: 0x06002377 RID: 9079
		[DispId(220)]
		int BitmapVirtualCache32BppSize { [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x0600237A RID: 9082
		// (set) Token: 0x06002379 RID: 9081
		[DispId(221)]
		bool RelativeMouseMode { [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x0600237B RID: 9083
		// (set) Token: 0x0600237C RID: 9084
		[DispId(222)]
		string AuthenticationServiceClass { [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x0600237D RID: 9085
		// (set) Token: 0x0600237E RID: 9086
		[DispId(223)]
		string PCB { [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06002380 RID: 9088
		// (set) Token: 0x0600237F RID: 9087
		[DispId(224)]
		int HotKeyFocusReleaseLeft { [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06002382 RID: 9090
		// (set) Token: 0x06002381 RID: 9089
		[DispId(225)]
		int HotKeyFocusReleaseRight { [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06002384 RID: 9092
		// (set) Token: 0x06002383 RID: 9091
		[DispId(17)]
		bool EnableCredSspSupport { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06002385 RID: 9093
		[DispId(226)]
		uint AuthenticationType { [DispId(226)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06002387 RID: 9095
		// (set) Token: 0x06002386 RID: 9094
		[DispId(227)]
		bool ConnectToAdministerServer { [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06002389 RID: 9097
		// (set) Token: 0x06002388 RID: 9096
		[DispId(228)]
		bool AudioCaptureRedirectionMode { [DispId(228)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(228)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x0600238B RID: 9099
		// (set) Token: 0x0600238A RID: 9098
		[DispId(229)]
		uint VideoPlaybackMode { [DispId(229)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(229)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x0600238D RID: 9101
		// (set) Token: 0x0600238C RID: 9100
		[DispId(230)]
		bool EnableSuperPan { [DispId(230)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(230)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x0600238F RID: 9103
		// (set) Token: 0x0600238E RID: 9102
		[DispId(231)]
		uint SuperPanAccelerationFactor { [DispId(231)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(231)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06002391 RID: 9105
		// (set) Token: 0x06002390 RID: 9104
		[DispId(232)]
		bool NegotiateSecurityLayer { [DispId(232)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(232)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06002393 RID: 9107
		// (set) Token: 0x06002392 RID: 9106
		[DispId(233)]
		uint AudioQualityMode { [DispId(233)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(233)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06002395 RID: 9109
		// (set) Token: 0x06002394 RID: 9108
		[DispId(234)]
		bool RedirectDirectX { [DispId(234)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(234)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06002397 RID: 9111
		// (set) Token: 0x06002396 RID: 9110
		[DispId(235)]
		uint NetworkConnectionType { [DispId(235)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(235)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
