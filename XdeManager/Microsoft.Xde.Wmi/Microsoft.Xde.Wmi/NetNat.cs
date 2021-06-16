using System;
using System.Linq;
using System.Management;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000016 RID: 22
	public class NetNat
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00008735 File Offset: 0x00006935
		private NetNat(ManagementObject obj)
		{
			this.natObj = obj;
			this.Subnet = new IPSubnet(obj["InternalIpInterfaceAddressPrefix"].ToString());
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000875F File Offset: 0x0000695F
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00008767 File Offset: 0x00006967
		public IPSubnet Subnet { get; private set; }

		// Token: 0x06000189 RID: 393 RVA: 0x00008770 File Offset: 0x00006970
		public static bool IsSupported()
		{
			bool result;
			try
			{
				WmiBaseUtils.CreateManagementObject(WmiBaseUtils.GetStandardCimv2Scope(), "MSFT_NetNat");
				result = true;
			}
			catch (ManagementException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000087A8 File Offset: 0x000069A8
		public static NetNat GetNatInstance(string natName)
		{
			ManagementObject managementObject = NetNat.FindNatObj(natName);
			if (managementObject != null)
			{
				return new NetNat(managementObject);
			}
			return null;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000087C8 File Offset: 0x000069C8
		public static NetNat CreateNatInstance(string natName, IPSubnet subnet)
		{
			NetNat natInstance = NetNat.GetNatInstance(natName);
			if (natInstance != null)
			{
				natInstance.Delete();
			}
			return new NetNat(NetNat.CreateNatObj(natName, subnet));
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000087F4 File Offset: 0x000069F4
		public bool AreNatPropertiesValid(IPSubnet subnet)
		{
			if (this.natObj["ExternalIpInterfaceAddressPrefix"] != null)
			{
				return false;
			}
			IPSubnet ipSubnet = new IPSubnet(this.natObj["InternalIpInterfaceAddressPrefix"].ToString());
			return subnet.Equals(ipSubnet);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000883C File Offset: 0x00006A3C
		public void Delete()
		{
			this.natObj.Delete();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000884C File Offset: 0x00006A4C
		private static ManagementObject FindNatObj(string natName)
		{
			ManagementScope standardCimv2Scope = WmiBaseUtils.GetStandardCimv2Scope();
			string query = StringUtilities.InvariantCultureFormat("SELECT * from MSFT_NetNat WHERE Name = '{0}'", new object[]
			{
				natName
			});
			return WmiBaseUtils.GetQueryHelper(standardCimv2Scope, query).Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008885 File Offset: 0x00006A85
		private static ManagementObject CreateNatObj(string natName, IPSubnet subnet)
		{
			ManagementObject managementObject = WmiBaseUtils.CreateManagementObject(WmiBaseUtils.GetStandardCimv2Scope(), "MSFT_NetNat");
			managementObject["Name"] = natName;
			managementObject["InternalIpInterfaceAddressPrefix"] = subnet.ToString();
			managementObject.Put();
			return managementObject;
		}

		// Token: 0x04000054 RID: 84
		private ManagementObject natObj;
	}
}
