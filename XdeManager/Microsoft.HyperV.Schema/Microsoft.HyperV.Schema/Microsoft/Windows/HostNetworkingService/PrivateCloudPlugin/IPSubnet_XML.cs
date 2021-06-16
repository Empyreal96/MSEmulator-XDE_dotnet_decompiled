using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Microsoft.Windows.HostNetworkingService.PrivateCloudPlugin
{
	// Token: 0x020000D9 RID: 217
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class IPSubnet_XML
	{
		// Token: 0x0600034F RID: 847 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public static bool IsJsonDefault(IPSubnet_XML val)
		{
			return IPSubnet_XML._default.JsonEquals(val);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000BBC8 File Offset: 0x00009DC8
		public bool JsonEquals(object obj)
		{
			IPSubnet_XML graph = obj as IPSubnet_XML;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IPSubnet_XML), new DataContractJsonSerializerSettings
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

		// Token: 0x04000433 RID: 1075
		private static readonly IPSubnet_XML _default = new IPSubnet_XML();

		// Token: 0x04000434 RID: 1076
		[DataMember(IsRequired = true, Name = "Prefix")]
		public string prefix;

		// Token: 0x04000435 RID: 1077
		[DataMember(IsRequired = true, Name = "IPAddress")]
		public IPAddress_XML[] ipAddresses;
	}
}
