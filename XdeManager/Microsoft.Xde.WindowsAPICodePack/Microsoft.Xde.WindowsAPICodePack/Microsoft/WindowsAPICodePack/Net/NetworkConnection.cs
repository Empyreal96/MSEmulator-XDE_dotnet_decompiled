using System;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000015 RID: 21
	public class NetworkConnection
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x00003728 File Offset: 0x00001928
		internal NetworkConnection(INetworkConnection networkConnection)
		{
			this.networkConnection = networkConnection;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003737 File Offset: 0x00001937
		public Network Network
		{
			get
			{
				return new Network(this.networkConnection.GetNetwork());
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003749 File Offset: 0x00001949
		public Guid AdapterId
		{
			get
			{
				return this.networkConnection.GetAdapterId();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003756 File Offset: 0x00001956
		public Guid ConnectionId
		{
			get
			{
				return this.networkConnection.GetConnectionId();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00003763 File Offset: 0x00001963
		public ConnectivityStates Connectivity
		{
			get
			{
				return this.networkConnection.GetConnectivity();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00003770 File Offset: 0x00001970
		public DomainType DomainType
		{
			get
			{
				return this.networkConnection.GetDomainType();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000377D File Offset: 0x0000197D
		public bool IsConnectedToInternet
		{
			get
			{
				return this.networkConnection.IsConnectedToInternet;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000378A File Offset: 0x0000198A
		public bool IsConnected
		{
			get
			{
				return this.networkConnection.IsConnected;
			}
		}

		// Token: 0x04000106 RID: 262
		private INetworkConnection networkConnection;
	}
}
