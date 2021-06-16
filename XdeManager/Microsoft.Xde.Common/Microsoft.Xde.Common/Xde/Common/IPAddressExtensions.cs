using System;
using System.Net;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000023 RID: 35
	public static class IPAddressExtensions
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00004DD0 File Offset: 0x00002FD0
		public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
		{
			byte[] addressBytes = address.GetAddressBytes();
			byte[] addressBytes2 = subnetMask.GetAddressBytes();
			if (addressBytes.Length != addressBytes2.Length)
			{
				throw new ArgumentException(Strings.InvalidIPAddressLengths);
			}
			byte[] array = new byte[addressBytes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (addressBytes[i] & addressBytes2[i]);
			}
			return new IPAddress(array);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004E28 File Offset: 0x00003028
		public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
		{
			object networkAddress = address.GetNetworkAddress(subnetMask);
			IPAddress networkAddress2 = address2.GetNetworkAddress(subnetMask);
			return networkAddress.Equals(networkAddress2);
		}
	}
}
