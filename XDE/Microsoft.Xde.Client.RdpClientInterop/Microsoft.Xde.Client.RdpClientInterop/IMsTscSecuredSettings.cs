using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000045 RID: 69
	[Guid("C9D65442-A0F9-45B2-8F73-D61D2DB8CBB6")]
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[ComImport]
	public interface IMsTscSecuredSettings
	{
		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06001EC1 RID: 7873
		// (set) Token: 0x06001EC0 RID: 7872
		[DispId(1)]
		string StartProgram { [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06001EC3 RID: 7875
		// (set) Token: 0x06001EC2 RID: 7874
		[DispId(2)]
		string WorkDir { [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2)] [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.BStr)] [param: In] set; }

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06001EC5 RID: 7877
		// (set) Token: 0x06001EC4 RID: 7876
		[DispId(3)]
		int FullScreen { [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] get; [DispId(3)] [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
