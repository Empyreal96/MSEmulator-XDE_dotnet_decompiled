using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x0200007D RID: 125
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("48A0F2A7-2713-431F-BBAC-6F4558E7D64D")]
	[ComImport]
	public interface IRemoteDesktopClientSettings
	{
		// Token: 0x0600248E RID: 9358
		[DispId(722)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void ApplySettings([MarshalAs(UnmanagedType.BStr)] [In] string RdpFileContents);

		// Token: 0x0600248F RID: 9359
		[DispId(723)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string RetrieveSettings();

		// Token: 0x06002490 RID: 9360
		[DispId(721)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Struct)]
		object GetRdpProperty([MarshalAs(UnmanagedType.BStr)] [In] string propertyName);

		// Token: 0x06002491 RID: 9361
		[DispId(720)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void SetRdpProperty([MarshalAs(UnmanagedType.BStr)] [In] string propertyName, [MarshalAs(UnmanagedType.Struct)] [In] object Value);
	}
}
