using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x02000040 RID: 64
	[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
	[Guid("57D25668-625A-4905-BE4E-304CAA13F89C")]
	[ComImport]
	public interface IRemoteDesktopClient
	{
		// Token: 0x06001E74 RID: 7796
		[DispId(701)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void Connect();

		// Token: 0x06001E75 RID: 7797
		[DispId(702)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void Disconnect();

		// Token: 0x06001E76 RID: 7798
		[DispId(703)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		void DeleteSavedCredentials([MarshalAs(UnmanagedType.BStr)] [In] string serverName);

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06001E77 RID: 7799
		[DispId(710)]
		IRemoteDesktopClientSettings Settings { [DispId(710)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06001E78 RID: 7800
		[DispId(711)]
		IRemoteDesktopClientActions Actions { [DispId(711)] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
