using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000095 RID: 149
	[Guid("D28B5458-F694-47A8-8E61-40356A767E46")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IMsRdpDrive
	{
		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x060024BD RID: 9405
		[DispId(229)]
		string Name { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.BStr)] get; }

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x060024BF RID: 9407
		// (set) Token: 0x060024BE RID: 9406
		[DispId(230)]
		bool RedirectionState { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] [param: In] set; }
	}
}
