using System;

namespace Microsoft.WindowsAPICodePack.Net
{
	// Token: 0x02000013 RID: 19
	public class Network
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000035EE File Offset: 0x000017EE
		internal Network(INetwork network)
		{
			this.network = network;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000035FD File Offset: 0x000017FD
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x0000360A File Offset: 0x0000180A
		public NetworkCategory Category
		{
			get
			{
				return this.network.GetCategory();
			}
			set
			{
				this.network.SetCategory(value);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003618 File Offset: 0x00001818
		public DateTime ConnectedTime
		{
			get
			{
				uint num;
				uint num2;
				uint num3;
				uint num4;
				this.network.GetTimeCreatedAndConnected(out num, out num2, out num3, out num4);
				return DateTime.FromFileTimeUtc((long)((ulong)num4 << 32 | (ulong)num3));
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00003645 File Offset: 0x00001845
		public NetworkConnectionCollection Connections
		{
			get
			{
				return new NetworkConnectionCollection(this.network.GetNetworkConnections());
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00003657 File Offset: 0x00001857
		public ConnectivityStates Connectivity
		{
			get
			{
				return this.network.GetConnectivity();
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003664 File Offset: 0x00001864
		public DateTime CreatedTime
		{
			get
			{
				uint num;
				uint num2;
				uint num3;
				uint num4;
				this.network.GetTimeCreatedAndConnected(out num, out num2, out num3, out num4);
				return DateTime.FromFileTimeUtc((long)((ulong)num2 << 32 | (ulong)num));
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00003691 File Offset: 0x00001891
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000369E File Offset: 0x0000189E
		public string Description
		{
			get
			{
				return this.network.GetDescription();
			}
			set
			{
				this.network.SetDescription(value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000036AC File Offset: 0x000018AC
		public DomainType DomainType
		{
			get
			{
				return this.network.GetDomainType();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000036B9 File Offset: 0x000018B9
		public bool IsConnected
		{
			get
			{
				return this.network.IsConnected;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000036C6 File Offset: 0x000018C6
		public bool IsConnectedToInternet
		{
			get
			{
				return this.network.IsConnectedToInternet;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x000036D3 File Offset: 0x000018D3
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x000036E0 File Offset: 0x000018E0
		public string Name
		{
			get
			{
				return this.network.GetName();
			}
			set
			{
				this.network.SetName(value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x000036EE File Offset: 0x000018EE
		public Guid NetworkId
		{
			get
			{
				return this.network.GetNetworkId();
			}
		}

		// Token: 0x04000104 RID: 260
		private INetwork network;
	}
}
