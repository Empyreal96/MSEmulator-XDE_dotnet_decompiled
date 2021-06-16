using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000079 RID: 121
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("25F2CE20-8B1D-4971-A7CD-549DAE201FC0")]
	[ComImport]
	public interface IMsRdpClientSecuredSettings2 : IMsRdpClientSecuredSettings
	{
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060023C0 RID: 9152
		// (set) Token: 0x060023BF RID: 9151
		[DispId(1)]
		string StartProgram { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060023C2 RID: 9154
		// (set) Token: 0x060023C1 RID: 9153
		[DispId(2)]
		string WorkDir { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060023C4 RID: 9156
		// (set) Token: 0x060023C3 RID: 9155
		[DispId(3)]
		int FullScreen { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060023C6 RID: 9158
		// (set) Token: 0x060023C5 RID: 9157
		[DispId(4)]
		int KeyboardHookMode { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060023C8 RID: 9160
		// (set) Token: 0x060023C7 RID: 9159
		[DispId(5)]
		int AudioRedirectionMode { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060023C9 RID: 9161
		// (set) Token: 0x060023CA RID: 9162
		[DispId(6)]
		string PCB { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }
	}
}
