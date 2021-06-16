using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002F1 RID: 753
	[Serializable]
	internal class HyperVSocketEndPoint : EndPoint
	{
		// Token: 0x060023C2 RID: 9154 RVA: 0x000C8834 File Offset: 0x000C6A34
		public HyperVSocketEndPoint(AddressFamily AddrFamily, HyperVSocketEndPoint.HyperVSocketFlag Flag, Guid VmId, Guid ServiceId)
		{
			this.m_AddressFamily = AddrFamily;
			this.m_Flag = Flag;
			this.m_VmId = VmId;
			this.m_ServiceId = ServiceId;
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x060023C3 RID: 9155 RVA: 0x000C8859 File Offset: 0x000C6A59
		public override AddressFamily AddressFamily
		{
			get
			{
				return this.m_AddressFamily;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x000C8861 File Offset: 0x000C6A61
		// (set) Token: 0x060023C5 RID: 9157 RVA: 0x000C8869 File Offset: 0x000C6A69
		public HyperVSocketEndPoint.HyperVSocketFlag Flag
		{
			get
			{
				return this.m_Flag;
			}
			set
			{
				this.m_Flag = value;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000C8872 File Offset: 0x000C6A72
		// (set) Token: 0x060023C7 RID: 9159 RVA: 0x000C887A File Offset: 0x000C6A7A
		public Guid VmId
		{
			get
			{
				return this.m_VmId;
			}
			set
			{
				this.m_VmId = value;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000C8883 File Offset: 0x000C6A83
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x000C888B File Offset: 0x000C6A8B
		public Guid ServiceId
		{
			get
			{
				return this.m_ServiceId;
			}
			set
			{
				this.m_VmId = value;
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x000C8894 File Offset: 0x000C6A94
		public override EndPoint Create(SocketAddress SockAddr)
		{
			if (SockAddr == null || SockAddr.Family != (AddressFamily)34 || SockAddr.Size != 34)
			{
				return null;
			}
			HyperVSocketEndPoint hyperVSocketEndPoint = new HyperVSocketEndPoint(SockAddr.Family, HyperVSocketEndPoint.HyperVSocketFlag.VM, Guid.Empty, Guid.Empty);
			string text = SockAddr.ToString();
			hyperVSocketEndPoint.Flag = (HyperVSocketEndPoint.HyperVSocketFlag)short.Parse(text.Substring(2, 2), CultureInfo.InvariantCulture);
			hyperVSocketEndPoint.VmId = new Guid(text.Substring(4, 16));
			hyperVSocketEndPoint.ServiceId = new Guid(text.Substring(20, 16));
			return hyperVSocketEndPoint;
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x000C891C File Offset: 0x000C6B1C
		public override bool Equals(object obj)
		{
			HyperVSocketEndPoint hyperVSocketEndPoint = (HyperVSocketEndPoint)obj;
			return hyperVSocketEndPoint != null && (this.m_AddressFamily == hyperVSocketEndPoint.AddressFamily && this.m_Flag == hyperVSocketEndPoint.Flag && this.m_VmId == hyperVSocketEndPoint.VmId && this.m_ServiceId == hyperVSocketEndPoint.ServiceId);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000C897A File Offset: 0x000C6B7A
		public override int GetHashCode()
		{
			return this.Serialize().GetHashCode();
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000C8988 File Offset: 0x000C6B88
		public override SocketAddress Serialize()
		{
			SocketAddress socketAddress = new SocketAddress(this.m_AddressFamily, 36);
			byte[] array = this.m_VmId.ToByteArray();
			byte[] array2 = this.m_ServiceId.ToByteArray();
			socketAddress[2] = (byte)this.m_Flag;
			for (int i = 0; i < array.Length; i++)
			{
				socketAddress[i + 4] = array[i];
			}
			for (int j = 0; j < array2.Length; j++)
			{
				socketAddress[j + 4 + array.Length] = array2[j];
			}
			return socketAddress;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000C8A08 File Offset: 0x000C6C08
		public override string ToString()
		{
			return ((ushort)this.m_Flag).ToString(CultureInfo.InvariantCulture) + this.m_VmId.ToString() + this.m_ServiceId.ToString();
		}

		// Token: 0x0400118A RID: 4490
		public const AddressFamily AF_HYPERV = (AddressFamily)34;

		// Token: 0x0400118B RID: 4491
		public const int HYPERV_SOCK_ADDR_SIZE = 36;

		// Token: 0x0400118C RID: 4492
		private AddressFamily m_AddressFamily;

		// Token: 0x0400118D RID: 4493
		private HyperVSocketEndPoint.HyperVSocketFlag m_Flag;

		// Token: 0x0400118E RID: 4494
		private Guid m_VmId;

		// Token: 0x0400118F RID: 4495
		private Guid m_ServiceId;

		// Token: 0x020002F2 RID: 754
		public enum HyperVSocketFlag
		{
			// Token: 0x04001191 RID: 4497
			VM,
			// Token: 0x04001192 RID: 4498
			HyperVContainer
		}
	}
}
