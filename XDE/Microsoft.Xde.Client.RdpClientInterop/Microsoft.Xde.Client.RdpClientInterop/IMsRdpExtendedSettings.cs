using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000037 RID: 55
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("302D8188-0052-4807-806A-362B628F9AC5")]
	[ComImport]
	public interface IMsRdpExtendedSettings
	{
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06001446 RID: 5190
		// (set) Token: 0x06001445 RID: 5189
		[DispId(1)]
		object Property { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Struct)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: MarshalAs(UnmanagedType.Struct)] [param: In] set; }
	}
}
