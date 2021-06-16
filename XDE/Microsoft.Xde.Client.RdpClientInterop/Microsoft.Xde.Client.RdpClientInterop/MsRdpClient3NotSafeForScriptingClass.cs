using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200001A RID: 26
	[TypeLibType(TypeLibTypeFlags.FCanCreate | TypeLibTypeFlags.FControl)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("Microsoft.Xde.Client.RdpClientInterop.IMsTscAxEvents\0\0")]
	[Guid("ACE575FD-1FCF-4074-9401-EBAB990FA9DE")]
	[ComImport]
	public class MsRdpClient3NotSafeForScriptingClass : IMsRdpClient3, MsRdpClient3NotSafeForScripting, IMsTscAxEvents_Event, IMsRdpClient2, IMsRdpClient, IMsTscAx, IMsTscAx_Redist, IMsTscNonScriptable, IMsRdpClientNonScriptable
	{
		// Token: 0x060005BE RID: 1470
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern MsRdpClient3NotSafeForScriptingClass();

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060005C0 RID: 1472
		// (set) Token: 0x060005BF RID: 1471
		[DispId(1)]
		public virtual extern string Server { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060005C2 RID: 1474
		// (set) Token: 0x060005C1 RID: 1473
		[DispId(2)]
		public virtual extern string Domain { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060005C4 RID: 1476
		// (set) Token: 0x060005C3 RID: 1475
		[DispId(3)]
		public virtual extern string UserName { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060005C6 RID: 1478
		// (set) Token: 0x060005C5 RID: 1477
		[DispId(4)]
		public virtual extern string DisconnectedText { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060005C8 RID: 1480
		// (set) Token: 0x060005C7 RID: 1479
		[DispId(5)]
		public virtual extern string ConnectingText { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060005C9 RID: 1481
		[DispId(6)]
		public virtual extern short Connected { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060005CB RID: 1483
		// (set) Token: 0x060005CA RID: 1482
		[DispId(12)]
		public virtual extern int DesktopWidth { [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(12)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060005CD RID: 1485
		// (set) Token: 0x060005CC RID: 1484
		[DispId(13)]
		public virtual extern int DesktopHeight { [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(13)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060005CF RID: 1487
		// (set) Token: 0x060005CE RID: 1486
		[DispId(16)]
		public virtual extern int StartConnected { [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(16)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060005D0 RID: 1488
		[DispId(17)]
		public virtual extern int HorizontalScrollBarVisible { [DispId(17)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060005D1 RID: 1489
		[DispId(18)]
		public virtual extern int VerticalScrollBarVisible { [DispId(18)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000222 RID: 546
		// (set) Token: 0x060005D2 RID: 1490
		[DispId(19)]
		public virtual extern string FullScreenTitle { [DispId(19)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060005D3 RID: 1491
		[DispId(20)]
		public virtual extern int CipherStrength { [DispId(20)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060005D4 RID: 1492
		[DispId(21)]
		public virtual extern string Version { [DispId(21)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060005D5 RID: 1493
		[DispId(22)]
		public virtual extern int SecuredSettingsEnabled { [DispId(22)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060005D6 RID: 1494
		[DispId(97)]
		public virtual extern IMsTscSecuredSettings SecuredSettings { [DispId(97)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060005D7 RID: 1495
		[DispId(98)]
		public virtual extern IMsTscAdvancedSettings AdvancedSettings { [DispId(98)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060005D8 RID: 1496
		[DispId(99)]
		public virtual extern IMsTscDebug Debugger { [DispId(99)] [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x060005D9 RID: 1497
		[DispId(30)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Connect();

		// Token: 0x060005DA RID: 1498
		[DispId(31)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void Disconnect();

		// Token: 0x060005DB RID: 1499
		[DispId(33)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x060005DC RID: 1500
		[DispId(34)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060005DE RID: 1502
		// (set) Token: 0x060005DD RID: 1501
		[DispId(100)]
		public virtual extern int ColorDepth { [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(100)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060005DF RID: 1503
		[DispId(101)]
		public virtual extern IMsRdpClientAdvancedSettings AdvancedSettings2 { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060005E0 RID: 1504
		[DispId(102)]
		public virtual extern IMsRdpClientSecuredSettings SecuredSettings2 { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060005E1 RID: 1505
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		[DispId(103)]
		public virtual extern ExtendedDisconnectReasonCode ExtendedDisconnectReason { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060005E3 RID: 1507
		// (set) Token: 0x060005E2 RID: 1506
		[DispId(104)]
		public virtual extern bool FullScreen { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x060005E4 RID: 1508
		[DispId(35)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x060005E5 RID: 1509
		[DispId(36)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x060005E6 RID: 1510
		[DispId(37)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus RequestClose();

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060005E7 RID: 1511
		[DispId(200)]
		public virtual extern IMsRdpClientAdvancedSettings2 AdvancedSettings3 { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060005E9 RID: 1513
		// (set) Token: 0x060005E8 RID: 1512
		[DispId(201)]
		public virtual extern string ConnectedStatusText { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060005EA RID: 1514
		[DispId(300)]
		public virtual extern IMsRdpClientAdvancedSettings3 AdvancedSettings4 { [DispId(300)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x060005EB RID: 1515
		// (remove) Token: 0x060005EC RID: 1516
		public virtual extern event IMsTscAxEvents_OnConnectingEventHandler OnConnecting;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x060005ED RID: 1517
		// (remove) Token: 0x060005EE RID: 1518
		public virtual extern event IMsTscAxEvents_OnConnectedEventHandler OnConnected;

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x060005EF RID: 1519
		// (remove) Token: 0x060005F0 RID: 1520
		public virtual extern event IMsTscAxEvents_OnLoginCompleteEventHandler OnLoginComplete;

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x060005F1 RID: 1521
		// (remove) Token: 0x060005F2 RID: 1522
		public virtual extern event IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected;

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x060005F3 RID: 1523
		// (remove) Token: 0x060005F4 RID: 1524
		public virtual extern event IMsTscAxEvents_OnEnterFullScreenModeEventHandler OnEnterFullScreenMode;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x060005F5 RID: 1525
		// (remove) Token: 0x060005F6 RID: 1526
		public virtual extern event IMsTscAxEvents_OnLeaveFullScreenModeEventHandler OnLeaveFullScreenMode;

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x060005F7 RID: 1527
		// (remove) Token: 0x060005F8 RID: 1528
		public virtual extern event IMsTscAxEvents_OnChannelReceivedDataEventHandler OnChannelReceivedData;

		// Token: 0x140000F8 RID: 248
		// (add) Token: 0x060005F9 RID: 1529
		// (remove) Token: 0x060005FA RID: 1530
		public virtual extern event IMsTscAxEvents_OnRequestGoFullScreenEventHandler OnRequestGoFullScreen;

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x060005FB RID: 1531
		// (remove) Token: 0x060005FC RID: 1532
		public virtual extern event IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler OnRequestLeaveFullScreen;

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x060005FD RID: 1533
		// (remove) Token: 0x060005FE RID: 1534
		public virtual extern event IMsTscAxEvents_OnFatalErrorEventHandler OnFatalError;

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x060005FF RID: 1535
		// (remove) Token: 0x06000600 RID: 1536
		public virtual extern event IMsTscAxEvents_OnWarningEventHandler OnWarning;

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06000601 RID: 1537
		// (remove) Token: 0x06000602 RID: 1538
		public virtual extern event IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler OnRemoteDesktopSizeChange;

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x06000603 RID: 1539
		// (remove) Token: 0x06000604 RID: 1540
		public virtual extern event IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler OnIdleTimeoutNotification;

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x06000605 RID: 1541
		// (remove) Token: 0x06000606 RID: 1542
		public virtual extern event IMsTscAxEvents_OnRequestContainerMinimizeEventHandler OnRequestContainerMinimize;

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x06000607 RID: 1543
		// (remove) Token: 0x06000608 RID: 1544
		public virtual extern event IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose;

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x06000609 RID: 1545
		// (remove) Token: 0x0600060A RID: 1546
		public virtual extern event IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler OnReceivedTSPublicKey;

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x0600060B RID: 1547
		// (remove) Token: 0x0600060C RID: 1548
		public virtual extern event IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting;

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x0600060D RID: 1549
		// (remove) Token: 0x0600060E RID: 1550
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler OnAuthenticationWarningDisplayed;

		// Token: 0x14000103 RID: 259
		// (add) Token: 0x0600060F RID: 1551
		// (remove) Token: 0x06000610 RID: 1552
		public virtual extern event IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler OnAuthenticationWarningDismissed;

		// Token: 0x14000104 RID: 260
		// (add) Token: 0x06000611 RID: 1553
		// (remove) Token: 0x06000612 RID: 1554
		public virtual extern event IMsTscAxEvents_OnRemoteProgramResultEventHandler OnRemoteProgramResult;

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x06000613 RID: 1555
		// (remove) Token: 0x06000614 RID: 1556
		public virtual extern event IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler OnRemoteProgramDisplayed;

		// Token: 0x14000106 RID: 262
		// (add) Token: 0x06000615 RID: 1557
		// (remove) Token: 0x06000616 RID: 1558
		public virtual extern event IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler OnRemoteWindowDisplayed;

		// Token: 0x14000107 RID: 263
		// (add) Token: 0x06000617 RID: 1559
		// (remove) Token: 0x06000618 RID: 1560
		public virtual extern event IMsTscAxEvents_OnLogonErrorEventHandler OnLogonError;

		// Token: 0x14000108 RID: 264
		// (add) Token: 0x06000619 RID: 1561
		// (remove) Token: 0x0600061A RID: 1562
		public virtual extern event IMsTscAxEvents_OnFocusReleasedEventHandler OnFocusReleased;

		// Token: 0x14000109 RID: 265
		// (add) Token: 0x0600061B RID: 1563
		// (remove) Token: 0x0600061C RID: 1564
		public virtual extern event IMsTscAxEvents_OnUserNameAcquiredEventHandler OnUserNameAcquired;

		// Token: 0x1400010A RID: 266
		// (add) Token: 0x0600061D RID: 1565
		// (remove) Token: 0x0600061E RID: 1566
		public virtual extern event IMsTscAxEvents_OnMouseInputModeChangedEventHandler OnMouseInputModeChanged;

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x0600061F RID: 1567
		// (remove) Token: 0x06000620 RID: 1568
		public virtual extern event IMsTscAxEvents_OnServiceMessageReceivedEventHandler OnServiceMessageReceived;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06000621 RID: 1569
		// (remove) Token: 0x06000622 RID: 1570
		public virtual extern event IMsTscAxEvents_OnConnectionBarPullDownEventHandler OnConnectionBarPullDown;

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06000623 RID: 1571
		// (remove) Token: 0x06000624 RID: 1572
		public virtual extern event IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler OnNetworkBandwidthChanged;

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x06000625 RID: 1573
		// (remove) Token: 0x06000626 RID: 1574
		public virtual extern event IMsTscAxEvents_OnAutoReconnectedEventHandler OnAutoReconnected;

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000628 RID: 1576
		// (set) Token: 0x06000627 RID: 1575
		public virtual extern string IMsRdpClient2_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600062A RID: 1578
		// (set) Token: 0x06000629 RID: 1577
		public virtual extern string IMsRdpClient2_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600062C RID: 1580
		// (set) Token: 0x0600062B RID: 1579
		public virtual extern string IMsRdpClient2_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600062E RID: 1582
		// (set) Token: 0x0600062D RID: 1581
		public virtual extern string IMsRdpClient2_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000630 RID: 1584
		// (set) Token: 0x0600062F RID: 1583
		public virtual extern string IMsRdpClient2_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000631 RID: 1585
		public virtual extern short IMsRdpClient2_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000633 RID: 1587
		// (set) Token: 0x06000632 RID: 1586
		public virtual extern int IMsRdpClient2_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000635 RID: 1589
		// (set) Token: 0x06000634 RID: 1588
		public virtual extern int IMsRdpClient2_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000637 RID: 1591
		// (set) Token: 0x06000636 RID: 1590
		public virtual extern int IMsRdpClient2_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000638 RID: 1592
		public virtual extern int IMsRdpClient2_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000639 RID: 1593
		public virtual extern int IMsRdpClient2_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700023C RID: 572
		// (set) Token: 0x0600063A RID: 1594
		public virtual extern string IMsRdpClient2_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600063B RID: 1595
		public virtual extern int IMsRdpClient2_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600063C RID: 1596
		public virtual extern string IMsRdpClient2_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600063D RID: 1597
		public virtual extern int IMsRdpClient2_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600063E RID: 1598
		public virtual extern IMsTscSecuredSettings IMsRdpClient2_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600063F RID: 1599
		public virtual extern IMsTscAdvancedSettings IMsRdpClient2_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000640 RID: 1600
		public virtual extern IMsTscDebug IMsRdpClient2_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000641 RID: 1601
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Connect();

		// Token: 0x06000642 RID: 1602
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_Disconnect();

		// Token: 0x06000643 RID: 1603
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000644 RID: 1604
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000646 RID: 1606
		// (set) Token: 0x06000645 RID: 1605
		public virtual extern int IMsRdpClient2_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000647 RID: 1607
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient2_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000648 RID: 1608
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient2_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000649 RID: 1609
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient2_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600064B RID: 1611
		// (set) Token: 0x0600064A RID: 1610
		public virtual extern bool IMsRdpClient2_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x0600064C RID: 1612
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient2_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x0600064D RID: 1613
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient2_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x0600064E RID: 1614
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient2_RequestClose();

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600064F RID: 1615
		public virtual extern IMsRdpClientAdvancedSettings2 IMsRdpClient2_AdvancedSettings3 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000651 RID: 1617
		// (set) Token: 0x06000650 RID: 1616
		public virtual extern string IMsRdpClient2_ConnectedStatusText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000653 RID: 1619
		// (set) Token: 0x06000652 RID: 1618
		public virtual extern string IMsRdpClient_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000655 RID: 1621
		// (set) Token: 0x06000654 RID: 1620
		public virtual extern string IMsRdpClient_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000657 RID: 1623
		// (set) Token: 0x06000656 RID: 1622
		public virtual extern string IMsRdpClient_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000659 RID: 1625
		// (set) Token: 0x06000658 RID: 1624
		public virtual extern string IMsRdpClient_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600065B RID: 1627
		// (set) Token: 0x0600065A RID: 1626
		public virtual extern string IMsRdpClient_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600065C RID: 1628
		public virtual extern short IMsRdpClient_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600065E RID: 1630
		// (set) Token: 0x0600065D RID: 1629
		public virtual extern int IMsRdpClient_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000660 RID: 1632
		// (set) Token: 0x0600065F RID: 1631
		public virtual extern int IMsRdpClient_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000662 RID: 1634
		// (set) Token: 0x06000661 RID: 1633
		public virtual extern int IMsRdpClient_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000663 RID: 1635
		public virtual extern int IMsRdpClient_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000664 RID: 1636
		public virtual extern int IMsRdpClient_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000255 RID: 597
		// (set) Token: 0x06000665 RID: 1637
		public virtual extern string IMsRdpClient_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000666 RID: 1638
		public virtual extern int IMsRdpClient_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000667 RID: 1639
		public virtual extern string IMsRdpClient_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000668 RID: 1640
		public virtual extern int IMsRdpClient_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000669 RID: 1641
		public virtual extern IMsTscSecuredSettings IMsRdpClient_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600066A RID: 1642
		public virtual extern IMsTscAdvancedSettings IMsRdpClient_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600066B RID: 1643
		public virtual extern IMsTscDebug IMsRdpClient_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x0600066C RID: 1644
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Connect();

		// Token: 0x0600066D RID: 1645
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_Disconnect();

		// Token: 0x0600066E RID: 1646
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x0600066F RID: 1647
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000671 RID: 1649
		// (set) Token: 0x06000670 RID: 1648
		public virtual extern int IMsRdpClient_ColorDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000672 RID: 1650
		public virtual extern IMsRdpClientAdvancedSettings IMsRdpClient_AdvancedSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000673 RID: 1651
		public virtual extern IMsRdpClientSecuredSettings IMsRdpClient_SecuredSettings2 { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000674 RID: 1652
		[ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")]
		public virtual extern ExtendedDisconnectReasonCode IMsRdpClient_ExtendedDisconnectReason { [MethodImpl(MethodImplOptions.InternalCall)] [return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ExtendedDisconnectReasonCode")] get; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000676 RID: 1654
		// (set) Token: 0x06000675 RID: 1653
		public virtual extern bool IMsRdpClient_FullScreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x06000677 RID: 1655
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClient_SetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [In] int chanOptions);

		// Token: 0x06000678 RID: 1656
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern int IMsRdpClient_GetVirtualChannelOptions([MarshalAs(UnmanagedType.BStr)] [In] string chanName);

		// Token: 0x06000679 RID: 1657
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: ComAliasName("Microsoft.Xde.Client.RdpClientInterop.ControlCloseStatus")]
		public virtual extern ControlCloseStatus IMsRdpClient_RequestClose();

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600067B RID: 1659
		// (set) Token: 0x0600067A RID: 1658
		public virtual extern string IMsTscAx_Server { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600067D RID: 1661
		// (set) Token: 0x0600067C RID: 1660
		public virtual extern string IMsTscAx_Domain { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600067F RID: 1663
		// (set) Token: 0x0600067E RID: 1662
		public virtual extern string IMsTscAx_UserName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000681 RID: 1665
		// (set) Token: 0x06000680 RID: 1664
		public virtual extern string IMsTscAx_DisconnectedText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000683 RID: 1667
		// (set) Token: 0x06000682 RID: 1666
		public virtual extern string IMsTscAx_ConnectingText { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000684 RID: 1668
		public virtual extern short IMsTscAx_Connected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000686 RID: 1670
		// (set) Token: 0x06000685 RID: 1669
		public virtual extern int IMsTscAx_DesktopWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000688 RID: 1672
		// (set) Token: 0x06000687 RID: 1671
		public virtual extern int IMsTscAx_DesktopHeight { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600068A RID: 1674
		// (set) Token: 0x06000689 RID: 1673
		public virtual extern int IMsTscAx_StartConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600068B RID: 1675
		public virtual extern int IMsTscAx_HorizontalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600068C RID: 1676
		public virtual extern int IMsTscAx_VerticalScrollBarVisible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700026C RID: 620
		// (set) Token: 0x0600068D RID: 1677
		public virtual extern string IMsTscAx_FullScreenTitle { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600068E RID: 1678
		public virtual extern int IMsTscAx_CipherStrength { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600068F RID: 1679
		public virtual extern string IMsTscAx_Version { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000690 RID: 1680
		public virtual extern int IMsTscAx_SecuredSettingsEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000691 RID: 1681
		public virtual extern IMsTscSecuredSettings IMsTscAx_SecuredSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000692 RID: 1682
		public virtual extern IMsTscAdvancedSettings IMsTscAx_AdvancedSettings { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000693 RID: 1683
		public virtual extern IMsTscDebug IMsTscAx_Debugger { [TypeLibFunc(TypeLibFuncFlags.FHidden)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x06000694 RID: 1684
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Connect();

		// Token: 0x06000695 RID: 1685
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_Disconnect();

		// Token: 0x06000696 RID: 1686
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_CreateVirtualChannels([MarshalAs(UnmanagedType.BStr)] [In] string newVal);

		// Token: 0x06000697 RID: 1687
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsTscAx_SendOnVirtualChannel([MarshalAs(UnmanagedType.BStr)] [In] string chanName, [MarshalAs(UnmanagedType.BStr)] [In] string ChanData);

		// Token: 0x17000273 RID: 627
		// (set) Token: 0x06000698 RID: 1688
		public virtual extern string ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600069A RID: 1690
		// (set) Token: 0x06000699 RID: 1689
		public virtual extern string PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600069C RID: 1692
		// (set) Token: 0x0600069B RID: 1691
		public virtual extern string PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600069E RID: 1694
		// (set) Token: 0x0600069D RID: 1693
		public virtual extern string BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060006A0 RID: 1696
		// (set) Token: 0x0600069F RID: 1695
		public virtual extern string BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x060006A1 RID: 1697
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void ResetPassword();

		// Token: 0x17000278 RID: 632
		// (set) Token: 0x060006A2 RID: 1698
		public virtual extern string IMsRdpClientNonScriptable_ClearTextPassword { [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060006A4 RID: 1700
		// (set) Token: 0x060006A3 RID: 1699
		public virtual extern string IMsRdpClientNonScriptable_PortablePassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060006A6 RID: 1702
		// (set) Token: 0x060006A5 RID: 1701
		public virtual extern string IMsRdpClientNonScriptable_PortableSalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060006A8 RID: 1704
		// (set) Token: 0x060006A7 RID: 1703
		public virtual extern string IMsRdpClientNonScriptable_BinaryPassword { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060006AA RID: 1706
		// (set) Token: 0x060006A9 RID: 1705
		public virtual extern string IMsRdpClientNonScriptable_BinarySalt { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x060006AB RID: 1707
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void IMsRdpClientNonScriptable_ResetPassword();

		// Token: 0x060006AC RID: 1708
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void NotifyRedirectDeviceChange([ComAliasName("Microsoft.Xde.Client.RdpClientInterop.UINT_PTR")] [In] uint wParam, [ComAliasName("Microsoft.Xde.Client.RdpClientInterop.LONG_PTR")] [In] int lParam);

		// Token: 0x060006AD RID: 1709
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern void SendKeys([In] int numKeys, [In] ref bool pbArrayKeyUp, [In] ref int plKeyData);
	}
}
