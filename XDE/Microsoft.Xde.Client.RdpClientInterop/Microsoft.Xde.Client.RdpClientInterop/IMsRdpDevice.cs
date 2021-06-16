using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000094 RID: 148
	[Guid("60C3B9C8-9E92-4F5E-A3E7-604A912093EA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IMsRdpDevice
	{
		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x060024B8 RID: 9400
		[DispId(222)]
		string DeviceInstanceId { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x060024B9 RID: 9401
		[DispId(220)]
		string FriendlyName { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x060024BA RID: 9402
		[DispId(221)]
		string DeviceDescription { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x060024BC RID: 9404
		// (set) Token: 0x060024BB RID: 9403
		[DispId(223)]
		bool RedirectionState { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
