using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000093 RID: 147
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7FF17599-DA2C-4677-AD35-F60C04FE1585")]
	[ComImport]
	public interface IMsRdpDriveCollection
	{
		// Token: 0x060024B5 RID: 9397
		[MethodImpl(MethodImplOptions.InternalCall)]
		void RescanDrives(bool vboolDynRedir);

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x060024B6 RID: 9398
		[DispId(231)]
		IMsRdpDrive DriveByIndex { [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x060024B7 RID: 9399
		[DispId(233)]
		uint DriveCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
