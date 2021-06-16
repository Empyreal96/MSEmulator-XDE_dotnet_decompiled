using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000092 RID: 146
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56540617-D281-488C-8738-6A8FDF64A118")]
	[ComImport]
	public interface IMsRdpDeviceCollection
	{
		// Token: 0x060024B1 RID: 9393
		[MethodImpl(MethodImplOptions.InternalCall)]
		void RescanDevices([In] bool vboolDynRedir);

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x060024B2 RID: 9394
		[DispId(228)]
		IMsRdpDevice DeviceByIndex { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x060024B3 RID: 9395
		[DispId(227)]
		IMsRdpDevice DeviceById { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x060024B4 RID: 9396
		[DispId(225)]
		uint DeviceCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
