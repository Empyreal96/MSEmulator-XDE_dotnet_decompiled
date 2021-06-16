using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200006B RID: 107
	[Guid("605BEFCF-39C1-45CC-A811-068FB7BE346D")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsRdpClientSecuredSettings : IMsTscSecuredSettings
	{
		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06001FB6 RID: 8118
		// (set) Token: 0x06001FB5 RID: 8117
		[DispId(1)]
		string StartProgram { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06001FB8 RID: 8120
		// (set) Token: 0x06001FB7 RID: 8119
		[DispId(2)]
		string WorkDir { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06001FBA RID: 8122
		// (set) Token: 0x06001FB9 RID: 8121
		[DispId(3)]
		int FullScreen { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06001FBC RID: 8124
		// (set) Token: 0x06001FBB RID: 8123
		[DispId(4)]
		int KeyboardHookMode { [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(4)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06001FBE RID: 8126
		// (set) Token: 0x06001FBD RID: 8125
		[DispId(5)]
		int AudioRedirectionMode { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
