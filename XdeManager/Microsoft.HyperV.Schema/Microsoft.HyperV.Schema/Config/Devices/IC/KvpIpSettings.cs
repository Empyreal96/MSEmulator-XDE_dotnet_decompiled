using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.IC
{
	// Token: 0x02000153 RID: 339
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class KvpIpSettings
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x000115BC File Offset: 0x0000F7BC
		public static bool IsJsonDefault(KvpIpSettings val)
		{
			return KvpIpSettings._default.JsonEquals(val);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000115CC File Offset: 0x0000F7CC
		public bool JsonEquals(object obj)
		{
			KvpIpSettings graph = obj as KvpIpSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(KvpIpSettings), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x040006F8 RID: 1784
		private static readonly KvpIpSettings _default = new KvpIpSettings();

		// Token: 0x040006F9 RID: 1785
		[DataMember(Name = "adapter_id")]
		public string PnpId;

		// Token: 0x040006FA RID: 1786
		[DataMember(Name = "address_family")]
		public KvpAddressFamily AddressFamily;

		// Token: 0x040006FB RID: 1787
		[DataMember(Name = "dhcp_enabled")]
		public bool DhcpEnabled;

		// Token: 0x040006FC RID: 1788
		[DataMember(Name = "ip_address")]
		public string IpAddressList;

		// Token: 0x040006FD RID: 1789
		[DataMember(Name = "subnet")]
		public string SubnetList;

		// Token: 0x040006FE RID: 1790
		[DataMember(Name = "gateway")]
		public string GatewayList;

		// Token: 0x040006FF RID: 1791
		[DataMember(Name = "dns_server")]
		public string DnsServerList;
	}
}
