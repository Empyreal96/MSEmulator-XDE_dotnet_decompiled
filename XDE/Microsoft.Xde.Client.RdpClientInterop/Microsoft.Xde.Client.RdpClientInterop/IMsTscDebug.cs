using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000047 RID: 71
	[Guid("209D0EB9-6254-47B1-9033-A98DAE55BB27")]
	[TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsTscDebug
	{
		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06001ED5 RID: 7893
		// (set) Token: 0x06001ED4 RID: 7892
		[DispId(200)]
		int HatchBitmapPDU { [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(200)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06001ED7 RID: 7895
		// (set) Token: 0x06001ED6 RID: 7894
		[DispId(201)]
		int HatchSSBOrder { [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(201)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06001ED9 RID: 7897
		// (set) Token: 0x06001ED8 RID: 7896
		[DispId(202)]
		int HatchMembltOrder { [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(202)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06001EDB RID: 7899
		// (set) Token: 0x06001EDA RID: 7898
		[DispId(203)]
		int HatchIndexPDU { [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(203)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06001EDD RID: 7901
		// (set) Token: 0x06001EDC RID: 7900
		[DispId(204)]
		int LabelMemblt { [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(204)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06001EDF RID: 7903
		// (set) Token: 0x06001EDE RID: 7902
		[DispId(205)]
		int BitmapCacheMonitor { [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(205)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06001EE1 RID: 7905
		// (set) Token: 0x06001EE0 RID: 7904
		[DispId(206)]
		int MallocFailuresPercent { [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(206)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06001EE3 RID: 7907
		// (set) Token: 0x06001EE2 RID: 7906
		[DispId(207)]
		int MallocHugeFailuresPercent { [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(207)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06001EE5 RID: 7909
		// (set) Token: 0x06001EE4 RID: 7908
		[DispId(208)]
		int NetThroughput { [DispId(208)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(208)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06001EE7 RID: 7911
		// (set) Token: 0x06001EE6 RID: 7910
		[DispId(209)]
		string CLXCmdLine { [DispId(209)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(209)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06001EE9 RID: 7913
		// (set) Token: 0x06001EE8 RID: 7912
		[DispId(210)]
		string CLXDll { [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(210)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06001EEB RID: 7915
		// (set) Token: 0x06001EEA RID: 7914
		[DispId(211)]
		int RemoteProgramsHatchVisibleRegion { [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(211)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06001EED RID: 7917
		// (set) Token: 0x06001EEC RID: 7916
		[DispId(212)]
		int RemoteProgramsHatchVisibleNoDataRegion { [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(212)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06001EEF RID: 7919
		// (set) Token: 0x06001EEE RID: 7918
		[DispId(213)]
		int RemoteProgramsHatchNonVisibleRegion { [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(213)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06001EF1 RID: 7921
		// (set) Token: 0x06001EF0 RID: 7920
		[DispId(214)]
		int RemoteProgramsHatchWindow { [DispId(214)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(214)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06001EF3 RID: 7923
		// (set) Token: 0x06001EF2 RID: 7922
		[DispId(215)]
		int RemoteProgramsStayConnectOnBadCaps { [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(215)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06001EF4 RID: 7924
		[DispId(216)]
		uint ControlType { [DispId(216)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000E71 RID: 3697
		// (set) Token: 0x06001EF5 RID: 7925
		[DispId(217)]
		bool DecodeGfx { [DispId(217)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
