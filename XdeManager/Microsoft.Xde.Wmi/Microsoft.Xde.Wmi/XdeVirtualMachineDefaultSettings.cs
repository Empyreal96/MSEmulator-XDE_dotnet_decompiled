using System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x0200000F RID: 15
	public class XdeVirtualMachineDefaultSettings
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x000062F1 File Offset: 0x000044F1
		public static string GetExternalNicName(string vmName)
		{
			return StringUtilities.InvariantCultureFormat("Emulator External Network Adapter for {0}", new object[]
			{
				vmName
			});
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006307 File Offset: 0x00004507
		public static string GetNatNicName(string vmName)
		{
			return StringUtilities.InvariantCultureFormat("Emulator NAT Network Adapter for {0}", new object[]
			{
				vmName
			});
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000631D File Offset: 0x0000451D
		public static string GetExternalSwitchPortName(string switchFriendlyName)
		{
			return StringUtilities.InvariantCultureFormat("External Switch Port {0}", new object[]
			{
				switchFriendlyName
			});
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006333 File Offset: 0x00004533
		public static string GetInternalSwitchPortName(string switchFriendlyName)
		{
			return StringUtilities.InvariantCultureFormat("Internal Switch Port {0}", new object[]
			{
				switchFriendlyName
			});
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006349 File Offset: 0x00004549
		public static string GetInternalEthernetPortName(string switchFriendlyName)
		{
			return StringUtilities.InvariantCultureFormat("Internal Ethernet Port {0}", new object[]
			{
				switchFriendlyName
			});
		}

		// Token: 0x04000031 RID: 49
		public const string InternalMacAddress = "02DEDEDEDEDE";

		// Token: 0x04000032 RID: 50
		public const int MaxSwitchAddresses = 1024;
	}
}
