using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000012 RID: 18
	[ClassInterface(0)]
	[Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B")]
	[ComSourceInterfaces("Microsoft.Windows.NetworkList.Internal.INetworkEvents\0Microsoft.Windows.NetworkList.Internal.INetworkConnectionEvents\0Microsoft.Windows.NetworkList.Internal.INetworkListManagerEvents\0")]
	[TypeLibType(2)]
	[ComImport]
	internal class NetworkListManagerClass : INetworkListManager
	{
		// Token: 0x060000DD RID: 221
		[DispId(7)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public virtual extern ConnectivityStates GetConnectivity();

		// Token: 0x060000DE RID: 222
		[DispId(2)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern INetwork GetNetwork([In] Guid gdNetworkId);

		// Token: 0x060000DF RID: 223
		[DispId(4)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);

		// Token: 0x060000E0 RID: 224
		[DispId(3)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern IEnumerable GetNetworkConnections();

		// Token: 0x060000E1 RID: 225
		[DispId(1)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern IEnumerable GetNetworks([In] NetworkConnectivityLevels Flags);

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000E2 RID: 226
		[DispId(6)]
		public virtual extern bool IsConnected { [DispId(6)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000E3 RID: 227
		[DispId(5)]
		public virtual extern bool IsConnectedToInternet { [DispId(5)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x060000E4 RID: 228
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern NetworkListManagerClass();
	}
}
