using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xde.Common.Properties;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200007C RID: 124
	public class IPSubnet
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x00007B6F File Offset: 0x00005D6F
		public IPSubnet(IPAddress ipPrefix, int maskBitLength)
		{
			this.Init(ipPrefix, maskBitLength);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00007B80 File Offset: 0x00005D80
		public IPSubnet(string ipPrefixStr, int maskBitLength)
		{
			IPAddress ipPrefix = IPAddress.Parse(ipPrefixStr);
			this.Init(ipPrefix, maskBitLength);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00007BA4 File Offset: 0x00005DA4
		public IPSubnet(string ipPrefixStr, string ipMaskStr)
		{
			IPAddress ipAddr = IPAddress.Parse(ipPrefixStr);
			IPAddress ipaddress = IPAddress.Parse(ipMaskStr);
			if (!this.IsValidIPMask(ipaddress))
			{
				throw new ArgumentException(Resources.InvalidIPMask);
			}
			this.MaskBitLength = this.GetkMaskLengthFromIPSubnet(ipaddress);
			this.IPPrefix = this.ApplySubnetMask(ipAddr, ipaddress);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public IPSubnet(IPAddress ipPrefix, IPAddress ipMask)
		{
			if (!this.IsValidIPMask(ipMask))
			{
				throw new ArgumentException(Resources.InvalidIPMask);
			}
			this.MaskBitLength = this.GetkMaskLengthFromIPSubnet(ipMask);
			this.IPPrefix = this.ApplySubnetMask(ipPrefix, ipMask);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00007C2C File Offset: 0x00005E2C
		public IPSubnet(string ipSubnetStr)
		{
			string[] array = ipSubnetStr.Split(new char[]
			{
				'/'
			});
			if (array.Length != 2)
			{
				throw new ArgumentException(Resources.InvalidIPPrefixString);
			}
			IPAddress ipPrefix = IPAddress.Parse(array[0]);
			int maskBitLength = int.Parse(array[1]);
			this.Init(ipPrefix, maskBitLength);
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00007C7A File Offset: 0x00005E7A
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00007C82 File Offset: 0x00005E82
		public IPAddress IPPrefix { get; private set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00007C8B File Offset: 0x00005E8B
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00007C93 File Offset: 0x00005E93
		public IPAddress IPMask { get; private set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00007C9C File Offset: 0x00005E9C
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public int MaskBitLength { get; private set; }

		// Token: 0x060002F2 RID: 754 RVA: 0x00007CB0 File Offset: 0x00005EB0
		public override string ToString()
		{
			if (this.subnetStr == null)
			{
				this.subnetStr = this.IPPrefix.ToString() + "/" + this.MaskBitLength.ToString();
			}
			return this.subnetStr;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00007CF4 File Offset: 0x00005EF4
		public bool Equals(IPSubnet ipSubnet)
		{
			return this.MaskBitLength == ipSubnet.MaskBitLength && this.IPPrefix.Equals(ipSubnet.IPPrefix);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00007D1C File Offset: 0x00005F1C
		public bool Overlaps(IPSubnet ipSubnet)
		{
			if (this.MaskBitLength == ipSubnet.MaskBitLength && this.IPPrefix.Equals(ipSubnet.IPPrefix))
			{
				return true;
			}
			int maskBitLength = Math.Min(this.MaskBitLength, ipSubnet.MaskBitLength);
			IPSubnet ipsubnet = new IPSubnet(this.IPPrefix, maskBitLength);
			IPSubnet ipsubnet2 = new IPSubnet(ipSubnet.IPPrefix, maskBitLength);
			return ipsubnet.IPPrefix.Equals(ipsubnet2.IPPrefix);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00007D8C File Offset: 0x00005F8C
		public bool IsSubset(IPSubnet stdIPSubnet)
		{
			return new IPSubnet(this.IPPrefix, stdIPSubnet.MaskBitLength).Equals(stdIPSubnet);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00007DAA File Offset: 0x00005FAA
		private void Init(IPAddress ipPrefix, int maskBitLength)
		{
			this.CheckValidIPSubnet(ipPrefix, maskBitLength);
			this.MaskBitLength = maskBitLength;
			this.IPMask = this.GetIPMaskFromMaskLength(this.MaskBitLength);
			this.IPPrefix = this.ApplySubnetMask(ipPrefix, this.IPMask);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00007DE0 File Offset: 0x00005FE0
		private IPAddress GetIPMaskFromMaskLength(int maskBitLength)
		{
			uint num = 0U;
			if (maskBitLength >= 0 && maskBitLength <= 32)
			{
				num = (uint)IPAddress.HostToNetworkOrder(-1 << 32 - maskBitLength);
			}
			return new IPAddress((long)((ulong)num));
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00007E0E File Offset: 0x0000600E
		private int GetkMaskLengthFromIPSubnet(IPAddress ipMask)
		{
			return this.CountSetBits(BitConverter.ToInt32(ipMask.GetAddressBytes(), 0));
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00007E24 File Offset: 0x00006024
		private bool IsValidIPMask(IPAddress ipMask)
		{
			bool result = false;
			if (ipMask == null)
			{
				result = false;
			}
			if (ipMask.GetAddressBytes().Length != 4)
			{
				result = false;
			}
			int num = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ipMask.GetAddressBytes(), 0));
			num = ~num + 1;
			if (num > 0 && (num & num - 1) == 0)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00007E6C File Offset: 0x0000606C
		private int CountSetBits(int num)
		{
			int num2 = 0;
			while (num != 0)
			{
				num &= num - 1;
				num2++;
			}
			return num2;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00007E8E File Offset: 0x0000608E
		private void CheckValidIPSubnet(IPAddress prefix, int maskBitLength)
		{
			if (prefix.AddressFamily != AddressFamily.InterNetwork)
			{
				throw new ArgumentException("Invalid IPPrefix for IPPrefix");
			}
			if (maskBitLength < 0 && maskBitLength > 32)
			{
				throw new ArgumentException("Invalid MaskBitLength for IPPrefix");
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00007EB8 File Offset: 0x000060B8
		private IPAddress ApplySubnetMask(IPAddress ipAddr, IPAddress mask)
		{
			if (ipAddr == null)
			{
				throw new ArgumentNullException("ipAddr");
			}
			if (mask == null)
			{
				throw new ArgumentNullException("mask");
			}
			byte[] addressBytes = ipAddr.GetAddressBytes();
			byte[] addressBytes2 = mask.GetAddressBytes();
			if (addressBytes.Length != 4)
			{
				throw new ArgumentOutOfRangeException("ipAddr");
			}
			if (addressBytes2.Length != 4)
			{
				throw new ArgumentOutOfRangeException("mask");
			}
			int num = 3;
			addressBytes[num] &= addressBytes2[3];
			int num2 = 2;
			addressBytes[num2] &= addressBytes2[2];
			int num3 = 1;
			addressBytes[num3] &= addressBytes2[1];
			int num4 = 0;
			addressBytes[num4] &= addressBytes2[0];
			return new IPAddress(addressBytes);
		}

		// Token: 0x040001BF RID: 447
		private const int BitsInInt = 32;

		// Token: 0x040001C0 RID: 448
		private string subnetStr;
	}
}
