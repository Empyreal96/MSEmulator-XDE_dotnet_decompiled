using System;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x0200001B RID: 27
	public static class NetworkListManager
	{
		// Token: 0x06000102 RID: 258 RVA: 0x000037C4 File Offset: 0x000019C4
		public static NetworkCollection GetNetworks(NetworkConnectivityLevels level)
		{
			CoreHelpers.ThrowIfNotVista();
			return new NetworkCollection(NetworkListManager.manager.GetNetworks(level));
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000037DB File Offset: 0x000019DB
		public static Network GetNetwork(Guid networkId)
		{
			CoreHelpers.ThrowIfNotVista();
			return new Network(NetworkListManager.manager.GetNetwork(networkId));
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000037F2 File Offset: 0x000019F2
		public static NetworkConnectionCollection GetNetworkConnections()
		{
			CoreHelpers.ThrowIfNotVista();
			return new NetworkConnectionCollection(NetworkListManager.manager.GetNetworkConnections());
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003808 File Offset: 0x00001A08
		public static NetworkConnection GetNetworkConnection(Guid networkConnectionId)
		{
			CoreHelpers.ThrowIfNotVista();
			return new NetworkConnection(NetworkListManager.manager.GetNetworkConnection(networkConnectionId));
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000381F File Offset: 0x00001A1F
		public static bool IsConnectedToInternet
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				return NetworkListManager.manager.IsConnectedToInternet;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00003830 File Offset: 0x00001A30
		public static bool IsConnected
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				return NetworkListManager.manager.IsConnected;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003841 File Offset: 0x00001A41
		public static ConnectivityStates Connectivity
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				return NetworkListManager.manager.GetConnectivity();
			}
		}

		// Token: 0x0400011E RID: 286
		private static NetworkListManagerClass manager = new NetworkListManagerClass();
	}
}
