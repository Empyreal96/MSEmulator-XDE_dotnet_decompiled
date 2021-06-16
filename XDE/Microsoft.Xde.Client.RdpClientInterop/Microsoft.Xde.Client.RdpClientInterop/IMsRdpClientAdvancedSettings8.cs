using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200007C RID: 124
	[Guid("89ACB528-2557-4D16-8625-226A30E97E9A")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings8 : IMsRdpClientAdvancedSettings7
	{
		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x060023D2 RID: 9170
		// (set) Token: 0x060023D1 RID: 9169
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x060023D4 RID: 9172
		// (set) Token: 0x060023D3 RID: 9171
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x060023D6 RID: 9174
		// (set) Token: 0x060023D5 RID: 9173
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010E4 RID: 4324
		// (set) Token: 0x060023D7 RID: 9175
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010E5 RID: 4325
		// (set) Token: 0x060023D8 RID: 9176
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010E6 RID: 4326
		// (set) Token: 0x060023D9 RID: 9177
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010E7 RID: 4327
		// (set) Token: 0x060023DA RID: 9178
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x060023DC RID: 9180
		// (set) Token: 0x060023DB RID: 9179
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x060023DE RID: 9182
		// (set) Token: 0x060023DD RID: 9181
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x060023E0 RID: 9184
		// (set) Token: 0x060023DF RID: 9183
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x060023E2 RID: 9186
		// (set) Token: 0x060023E1 RID: 9185
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x060023E4 RID: 9188
		// (set) Token: 0x060023E3 RID: 9187
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x060023E6 RID: 9190
		// (set) Token: 0x060023E5 RID: 9189
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x060023E8 RID: 9192
		// (set) Token: 0x060023E7 RID: 9191
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x060023EA RID: 9194
		// (set) Token: 0x060023E9 RID: 9193
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x060023EC RID: 9196
		// (set) Token: 0x060023EB RID: 9195
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x060023EE RID: 9198
		// (set) Token: 0x060023ED RID: 9197
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x060023F0 RID: 9200
		// (set) Token: 0x060023EF RID: 9199
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x060023F2 RID: 9202
		// (set) Token: 0x060023F1 RID: 9201
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x060023F4 RID: 9204
		// (set) Token: 0x060023F3 RID: 9203
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x060023F6 RID: 9206
		// (set) Token: 0x060023F5 RID: 9205
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x060023F8 RID: 9208
		// (set) Token: 0x060023F7 RID: 9207
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x060023FA RID: 9210
		// (set) Token: 0x060023F9 RID: 9209
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x060023FC RID: 9212
		// (set) Token: 0x060023FB RID: 9211
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x060023FE RID: 9214
		// (set) Token: 0x060023FD RID: 9213
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06002400 RID: 9216
		// (set) Token: 0x060023FF RID: 9215
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06002402 RID: 9218
		// (set) Token: 0x06002401 RID: 9217
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06002404 RID: 9220
		// (set) Token: 0x06002403 RID: 9219
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06002406 RID: 9222
		// (set) Token: 0x06002405 RID: 9221
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06002408 RID: 9224
		// (set) Token: 0x06002407 RID: 9223
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x0600240A RID: 9226
		// (set) Token: 0x06002409 RID: 9225
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x0600240C RID: 9228
		// (set) Token: 0x0600240B RID: 9227
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x0600240E RID: 9230
		// (set) Token: 0x0600240D RID: 9229
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06002410 RID: 9232
		// (set) Token: 0x0600240F RID: 9231
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06002412 RID: 9234
		// (set) Token: 0x06002411 RID: 9233
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001104 RID: 4356
		// (set) Token: 0x06002413 RID: 9235
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06002415 RID: 9237
		// (set) Token: 0x06002414 RID: 9236
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06002417 RID: 9239
		// (set) Token: 0x06002416 RID: 9238
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06002419 RID: 9241
		// (set) Token: 0x06002418 RID: 9240
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x0600241B RID: 9243
		// (set) Token: 0x0600241A RID: 9242
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x0600241D RID: 9245
		// (set) Token: 0x0600241C RID: 9244
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x0600241F RID: 9247
		// (set) Token: 0x0600241E RID: 9246
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06002421 RID: 9249
		// (set) Token: 0x06002420 RID: 9248
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06002423 RID: 9251
		// (set) Token: 0x06002422 RID: 9250
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06002425 RID: 9253
		// (set) Token: 0x06002424 RID: 9252
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06002427 RID: 9255
		// (set) Token: 0x06002426 RID: 9254
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06002429 RID: 9257
		// (set) Token: 0x06002428 RID: 9256
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x0600242B RID: 9259
		// (set) Token: 0x0600242A RID: 9258
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x0600242D RID: 9261
		// (set) Token: 0x0600242C RID: 9260
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x0600242F RID: 9263
		// (set) Token: 0x0600242E RID: 9262
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06002431 RID: 9265
		// (set) Token: 0x06002430 RID: 9264
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x06002433 RID: 9267
		// (set) Token: 0x06002432 RID: 9266
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x06002435 RID: 9269
		// (set) Token: 0x06002434 RID: 9268
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06002437 RID: 9271
		// (set) Token: 0x06002436 RID: 9270
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06002439 RID: 9273
		// (set) Token: 0x06002438 RID: 9272
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001118 RID: 4376
		// (set) Token: 0x0600243A RID: 9274
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x0600243C RID: 9276
		// (set) Token: 0x0600243B RID: 9275
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x0600243E RID: 9278
		// (set) Token: 0x0600243D RID: 9277
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06002440 RID: 9280
		// (set) Token: 0x0600243F RID: 9279
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06002442 RID: 9282
		// (set) Token: 0x06002441 RID: 9281
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06002444 RID: 9284
		// (set) Token: 0x06002443 RID: 9283
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06002446 RID: 9286
		// (set) Token: 0x06002445 RID: 9285
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06002448 RID: 9288
		// (set) Token: 0x06002447 RID: 9287
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x0600244A RID: 9290
		// (set) Token: 0x06002449 RID: 9289
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x0600244C RID: 9292
		// (set) Token: 0x0600244B RID: 9291
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x0600244E RID: 9294
		// (set) Token: 0x0600244D RID: 9293
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06002450 RID: 9296
		// (set) Token: 0x0600244F RID: 9295
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001124 RID: 4388
		// (set) Token: 0x06002451 RID: 9297
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x06002453 RID: 9299
		// (set) Token: 0x06002452 RID: 9298
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x06002454 RID: 9300
		[DispId(205)]
		bool CanAutoReconnect { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x06002456 RID: 9302
		// (set) Token: 0x06002455 RID: 9301
		[DispId(206)]
		bool EnableAutoReconnect { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x06002458 RID: 9304
		// (set) Token: 0x06002457 RID: 9303
		[DispId(207)]
		int MaxReconnectAttempts { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x0600245A RID: 9306
		// (set) Token: 0x06002459 RID: 9305
		[DispId(210)]
		bool ConnectionBarShowMinimizeButton { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x0600245C RID: 9308
		// (set) Token: 0x0600245B RID: 9307
		[DispId(211)]
		bool ConnectionBarShowRestoreButton { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x0600245E RID: 9310
		// (set) Token: 0x0600245D RID: 9309
		[DispId(212)]
		uint AuthenticationLevel { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06002460 RID: 9312
		// (set) Token: 0x0600245F RID: 9311
		[DispId(213)]
		bool RedirectClipboard { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06002462 RID: 9314
		// (set) Token: 0x06002461 RID: 9313
		[DispId(215)]
		uint AudioRedirectionMode { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06002464 RID: 9316
		// (set) Token: 0x06002463 RID: 9315
		[DispId(216)]
		bool ConnectionBarShowPinButton { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x06002466 RID: 9318
		// (set) Token: 0x06002465 RID: 9317
		[DispId(217)]
		bool PublicMode { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06002468 RID: 9320
		// (set) Token: 0x06002467 RID: 9319
		[DispId(218)]
		bool RedirectDevices { [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(218)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x0600246A RID: 9322
		// (set) Token: 0x06002469 RID: 9321
		[DispId(219)]
		bool RedirectPOSDevices { [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(219)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x0600246C RID: 9324
		// (set) Token: 0x0600246B RID: 9323
		[DispId(220)]
		int BitmapVirtualCache32BppSize { [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(220)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x0600246E RID: 9326
		// (set) Token: 0x0600246D RID: 9325
		[DispId(221)]
		bool RelativeMouseMode { [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(221)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x0600246F RID: 9327
		// (set) Token: 0x06002470 RID: 9328
		[DispId(222)]
		string AuthenticationServiceClass { [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(222)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x06002471 RID: 9329
		// (set) Token: 0x06002472 RID: 9330
		[DispId(223)]
		string PCB { [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(223)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x06002474 RID: 9332
		// (set) Token: 0x06002473 RID: 9331
		[DispId(224)]
		int HotKeyFocusReleaseLeft { [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(224)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x06002476 RID: 9334
		// (set) Token: 0x06002475 RID: 9333
		[DispId(225)]
		int HotKeyFocusReleaseRight { [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(225)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x06002478 RID: 9336
		// (set) Token: 0x06002477 RID: 9335
		[DispId(17)]
		bool EnableCredSspSupport { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x06002479 RID: 9337
		[DispId(226)]
		uint AuthenticationType { [DispId(226)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x0600247B RID: 9339
		// (set) Token: 0x0600247A RID: 9338
		[DispId(227)]
		bool ConnectToAdministerServer { [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(227)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x0600247D RID: 9341
		// (set) Token: 0x0600247C RID: 9340
		[DispId(228)]
		bool AudioCaptureRedirectionMode { [DispId(228)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(228)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x0600247F RID: 9343
		// (set) Token: 0x0600247E RID: 9342
		[DispId(229)]
		uint VideoPlaybackMode { [DispId(229)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(229)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x06002481 RID: 9345
		// (set) Token: 0x06002480 RID: 9344
		[DispId(230)]
		bool EnableSuperPan { [DispId(230)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(230)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06002483 RID: 9347
		// (set) Token: 0x06002482 RID: 9346
		[DispId(231)]
		uint SuperPanAccelerationFactor { [DispId(231)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(231)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06002485 RID: 9349
		// (set) Token: 0x06002484 RID: 9348
		[DispId(232)]
		bool NegotiateSecurityLayer { [DispId(232)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(232)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06002487 RID: 9351
		// (set) Token: 0x06002486 RID: 9350
		[DispId(233)]
		uint AudioQualityMode { [DispId(233)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(233)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x06002489 RID: 9353
		// (set) Token: 0x06002488 RID: 9352
		[DispId(234)]
		bool RedirectDirectX { [DispId(234)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(234)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x0600248B RID: 9355
		// (set) Token: 0x0600248A RID: 9354
		[DispId(235)]
		uint NetworkConnectionType { [DispId(235)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(235)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x0600248D RID: 9357
		// (set) Token: 0x0600248C RID: 9356
		[DispId(236)]
		bool BandwidthDetection { [DispId(236)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(236)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
