using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000011 RID: 17
	[Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B")]
	[TypeLibType(4160)]
	[ComImport]
	internal interface INetworkListManager
	{
		// Token: 0x060000D6 RID: 214
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumerable GetNetworks([In] NetworkConnectivityLevels Flags);

		// Token: 0x060000D7 RID: 215
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		INetwork GetNetwork([In] Guid gdNetworkId);

		// Token: 0x060000D8 RID: 216
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumerable GetNetworkConnections();

		// Token: 0x060000D9 RID: 217
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000DA RID: 218
		bool IsConnectedToInternet { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000DB RID: 219
		bool IsConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x060000DC RID: 220
		[MethodImpl(MethodImplOptions.InternalCall)]
		ConnectivityStates GetConnectivity();
	}
}
