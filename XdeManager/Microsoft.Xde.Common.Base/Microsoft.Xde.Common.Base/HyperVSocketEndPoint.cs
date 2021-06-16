using System;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000006 RID: 6
	public class HyperVSocketEndPoint : EndPoint
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public HyperVSocketEndPoint(Guid vmid, Guid serviceid)
		{
			this.sockaddr.Family = 34;
			this.sockaddr.Reserved = 0;
			this.sockaddr.VmId = vmid;
			this.sockaddr.ServiceId = serviceid;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002AD9 File Offset: 0x00000CD9
		public override AddressFamily AddressFamily
		{
			get
			{
				return (AddressFamily)34;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public override SocketAddress Serialize()
		{
			byte[] structBytes = StructUtils.GetStructBytes(this.sockaddr);
			SocketAddress socketAddress = new SocketAddress(this.AddressFamily, structBytes.Length);
			for (int i = 0; i < structBytes.Length; i++)
			{
				socketAddress[i] = structBytes[i];
			}
			return socketAddress;
		}

		// Token: 0x04000014 RID: 20
		public const int AF_HYPERV = 34;

		// Token: 0x04000015 RID: 21
		public const int HV_PROTOCOL_RAW = 1;

		// Token: 0x04000016 RID: 22
		private HyperVSocketEndPoint.SOCKADDR_HV sockaddr;

		// Token: 0x02000036 RID: 54
		private struct SOCKADDR_HV
		{
			// Token: 0x04000138 RID: 312
			public short Family;

			// Token: 0x04000139 RID: 313
			public short Reserved;

			// Token: 0x0400013A RID: 314
			public Guid VmId;

			// Token: 0x0400013B RID: 315
			public Guid ServiceId;
		}
	}
}
