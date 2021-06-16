using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000010 RID: 16
	[TypeLibType(4160)]
	[Guid("DCB00005-570F-4A9B-8D69-199FDBA5723B")]
	[ComImport]
	internal interface INetworkConnection
	{
		// Token: 0x060000CF RID: 207
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		INetwork GetNetwork();

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D0 RID: 208
		bool IsConnectedToInternet { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D1 RID: 209
		bool IsConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x060000D2 RID: 210
		[MethodImpl(MethodImplOptions.InternalCall)]
		ConnectivityStates GetConnectivity();

		// Token: 0x060000D3 RID: 211
		[MethodImpl(MethodImplOptions.InternalCall)]
		Guid GetConnectionId();

		// Token: 0x060000D4 RID: 212
		[MethodImpl(MethodImplOptions.InternalCall)]
		Guid GetAdapterId();

		// Token: 0x060000D5 RID: 213
		[MethodImpl(MethodImplOptions.InternalCall)]
		DomainType GetDomainType();
	}
}
