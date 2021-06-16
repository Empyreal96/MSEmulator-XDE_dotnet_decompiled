using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200006A RID: 106
	[ComConversionLoss]
	[Guid("3C65B4AB-12B3-465B-ACD4-B8DAD3BFF9E2")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientAdvancedSettings : IMsTscAdvancedSettings
	{
		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06001F33 RID: 7987
		// (set) Token: 0x06001F32 RID: 7986
		[DispId(121)]
		int Compress { [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(121)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06001F35 RID: 7989
		// (set) Token: 0x06001F34 RID: 7988
		[DispId(122)]
		int BitmapPeristence { [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(122)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06001F37 RID: 7991
		// (set) Token: 0x06001F36 RID: 7990
		[DispId(161)]
		int allowBackgroundInput { [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(161)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E75 RID: 3701
		// (set) Token: 0x06001F38 RID: 7992
		[DispId(162)]
		string KeyBoardLayoutStr { [DispId(162)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E76 RID: 3702
		// (set) Token: 0x06001F39 RID: 7993
		[DispId(170)]
		string PluginDlls { [DispId(170)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E77 RID: 3703
		// (set) Token: 0x06001F3A RID: 7994
		[DispId(171)]
		string IconFile { [DispId(171)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E78 RID: 3704
		// (set) Token: 0x06001F3B RID: 7995
		[DispId(172)]
		int IconIndex { [DispId(172)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06001F3D RID: 7997
		// (set) Token: 0x06001F3C RID: 7996
		[DispId(173)]
		int ContainerHandledFullScreen { [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(173)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06001F3F RID: 7999
		// (set) Token: 0x06001F3E RID: 7998
		[DispId(174)]
		int DisableRdpdr { [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(174)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06001F41 RID: 8001
		// (set) Token: 0x06001F40 RID: 8000
		[DispId(101)]
		int SmoothScroll { [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(101)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06001F43 RID: 8003
		// (set) Token: 0x06001F42 RID: 8002
		[DispId(102)]
		int AcceleratorPassthrough { [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(102)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06001F45 RID: 8005
		// (set) Token: 0x06001F44 RID: 8004
		[DispId(103)]
		int ShadowBitmap { [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(103)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06001F47 RID: 8007
		// (set) Token: 0x06001F46 RID: 8006
		[DispId(104)]
		int TransportType { [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(104)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06001F49 RID: 8009
		// (set) Token: 0x06001F48 RID: 8008
		[DispId(105)]
		int SasSequence { [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(105)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06001F4B RID: 8011
		// (set) Token: 0x06001F4A RID: 8010
		[DispId(106)]
		int EncryptionEnabled { [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(106)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06001F4D RID: 8013
		// (set) Token: 0x06001F4C RID: 8012
		[DispId(107)]
		int DedicatedTerminal { [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(107)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06001F4F RID: 8015
		// (set) Token: 0x06001F4E RID: 8014
		[DispId(108)]
		int RDPPort { [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(108)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06001F51 RID: 8017
		// (set) Token: 0x06001F50 RID: 8016
		[DispId(109)]
		int EnableMouse { [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(109)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x06001F53 RID: 8019
		// (set) Token: 0x06001F52 RID: 8018
		[DispId(110)]
		int DisableCtrlAltDel { [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(110)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06001F55 RID: 8021
		// (set) Token: 0x06001F54 RID: 8020
		[DispId(111)]
		int EnableWindowsKey { [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(111)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06001F57 RID: 8023
		// (set) Token: 0x06001F56 RID: 8022
		[DispId(112)]
		int DoubleClickDetect { [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(112)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06001F59 RID: 8025
		// (set) Token: 0x06001F58 RID: 8024
		[DispId(113)]
		int MaximizeShell { [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(113)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06001F5B RID: 8027
		// (set) Token: 0x06001F5A RID: 8026
		[DispId(114)]
		int HotKeyFullScreen { [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(114)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06001F5D RID: 8029
		// (set) Token: 0x06001F5C RID: 8028
		[DispId(115)]
		int HotKeyCtrlEsc { [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(115)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06001F5F RID: 8031
		// (set) Token: 0x06001F5E RID: 8030
		[DispId(116)]
		int HotKeyAltEsc { [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(116)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06001F61 RID: 8033
		// (set) Token: 0x06001F60 RID: 8032
		[DispId(117)]
		int HotKeyAltTab { [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(117)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06001F63 RID: 8035
		// (set) Token: 0x06001F62 RID: 8034
		[DispId(118)]
		int HotKeyAltShiftTab { [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(118)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06001F65 RID: 8037
		// (set) Token: 0x06001F64 RID: 8036
		[DispId(119)]
		int HotKeyAltSpace { [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(119)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06001F67 RID: 8039
		// (set) Token: 0x06001F66 RID: 8038
		[DispId(120)]
		int HotKeyCtrlAltDel { [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(120)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06001F69 RID: 8041
		// (set) Token: 0x06001F68 RID: 8040
		[DispId(123)]
		int orderDrawThreshold { [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(123)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06001F6B RID: 8043
		// (set) Token: 0x06001F6A RID: 8042
		[DispId(124)]
		int BitmapCacheSize { [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(124)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06001F6D RID: 8045
		// (set) Token: 0x06001F6C RID: 8044
		[DispId(125)]
		int BitmapVirtualCacheSize { [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(125)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x06001F6F RID: 8047
		// (set) Token: 0x06001F6E RID: 8046
		[DispId(175)]
		int ScaleBitmapCachesByBPP { [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(175)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x06001F71 RID: 8049
		// (set) Token: 0x06001F70 RID: 8048
		[DispId(126)]
		int NumBitmapCaches { [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(126)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06001F73 RID: 8051
		// (set) Token: 0x06001F72 RID: 8050
		[DispId(127)]
		int CachePersistenceActive { [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(127)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E95 RID: 3733
		// (set) Token: 0x06001F74 RID: 8052
		[DispId(138)]
		string PersistCacheDirectory { [DispId(138)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06001F76 RID: 8054
		// (set) Token: 0x06001F75 RID: 8053
		[DispId(156)]
		int brushSupportLevel { [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(156)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06001F78 RID: 8056
		// (set) Token: 0x06001F77 RID: 8055
		[DispId(157)]
		int minInputSendInterval { [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(157)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06001F7A RID: 8058
		// (set) Token: 0x06001F79 RID: 8057
		[DispId(158)]
		int InputEventsAtOnce { [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(158)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06001F7C RID: 8060
		// (set) Token: 0x06001F7B RID: 8059
		[DispId(159)]
		int maxEventCount { [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(159)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06001F7E RID: 8062
		// (set) Token: 0x06001F7D RID: 8061
		[DispId(160)]
		int keepAliveInterval { [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(160)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06001F80 RID: 8064
		// (set) Token: 0x06001F7F RID: 8063
		[DispId(163)]
		int shutdownTimeout { [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(163)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06001F82 RID: 8066
		// (set) Token: 0x06001F81 RID: 8065
		[DispId(164)]
		int overallConnectionTimeout { [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(164)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06001F84 RID: 8068
		// (set) Token: 0x06001F83 RID: 8067
		[DispId(165)]
		int singleConnectionTimeout { [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(165)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06001F86 RID: 8070
		// (set) Token: 0x06001F85 RID: 8069
		[DispId(166)]
		int KeyboardType { [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(166)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06001F88 RID: 8072
		// (set) Token: 0x06001F87 RID: 8071
		[DispId(167)]
		int KeyboardSubType { [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(167)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06001F8A RID: 8074
		// (set) Token: 0x06001F89 RID: 8073
		[DispId(168)]
		int KeyboardFunctionKey { [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(168)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06001F8C RID: 8076
		// (set) Token: 0x06001F8B RID: 8075
		[DispId(169)]
		int WinceFixedPalette { [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(169)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06001F8E RID: 8078
		// (set) Token: 0x06001F8D RID: 8077
		[DispId(178)]
		bool ConnectToServerConsole { [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(178)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06001F90 RID: 8080
		// (set) Token: 0x06001F8F RID: 8079
		[DispId(182)]
		int BitmapPersistence { [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(182)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06001F92 RID: 8082
		// (set) Token: 0x06001F91 RID: 8081
		[DispId(183)]
		int MinutesToIdleTimeout { [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(183)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06001F94 RID: 8084
		// (set) Token: 0x06001F93 RID: 8083
		[DispId(184)]
		bool SmartSizing { [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(184)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06001F96 RID: 8086
		// (set) Token: 0x06001F95 RID: 8085
		[DispId(185)]
		string RdpdrLocalPrintingDocName { [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(185)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06001F98 RID: 8088
		// (set) Token: 0x06001F97 RID: 8087
		[DispId(201)]
		string RdpdrClipCleanTempDirString { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06001F9A RID: 8090
		// (set) Token: 0x06001F99 RID: 8089
		[DispId(202)]
		string RdpdrClipPasteInfoString { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EA9 RID: 3753
		// (set) Token: 0x06001F9B RID: 8091
		[DispId(186)]
		string ClearTextPassword { [DispId(186)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06001F9D RID: 8093
		// (set) Token: 0x06001F9C RID: 8092
		[DispId(187)]
		bool DisplayConnectionBar { [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(187)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06001F9F RID: 8095
		// (set) Token: 0x06001F9E RID: 8094
		[DispId(188)]
		bool PinConnectionBar { [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(188)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06001FA1 RID: 8097
		// (set) Token: 0x06001FA0 RID: 8096
		[DispId(189)]
		bool GrabFocusOnConnect { [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(189)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06001FA3 RID: 8099
		// (set) Token: 0x06001FA2 RID: 8098
		[DispId(190)]
		string LoadBalanceInfo { [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(190)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06001FA5 RID: 8101
		// (set) Token: 0x06001FA4 RID: 8100
		[DispId(191)]
		bool RedirectDrives { [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(191)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x06001FA7 RID: 8103
		// (set) Token: 0x06001FA6 RID: 8102
		[DispId(192)]
		bool RedirectPrinters { [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(192)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06001FA9 RID: 8105
		// (set) Token: 0x06001FA8 RID: 8104
		[DispId(193)]
		bool RedirectPorts { [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(193)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06001FAB RID: 8107
		// (set) Token: 0x06001FAA RID: 8106
		[DispId(194)]
		bool RedirectSmartCards { [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(194)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06001FAD RID: 8109
		// (set) Token: 0x06001FAC RID: 8108
		[DispId(195)]
		int BitmapVirtualCache16BppSize { [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(195)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06001FAF RID: 8111
		// (set) Token: 0x06001FAE RID: 8110
		[DispId(196)]
		int BitmapVirtualCache24BppSize { [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(196)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06001FB1 RID: 8113
		// (set) Token: 0x06001FB0 RID: 8112
		[DispId(200)]
		int PerformanceFlags { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EB5 RID: 3765
		// (set) Token: 0x06001FB2 RID: 8114
		[DispId(203)]
		IntPtr ConnectWithEndpoint { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06001FB4 RID: 8116
		// (set) Token: 0x06001FB3 RID: 8115
		[DispId(204)]
		bool NotifyTSPublicKey { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
