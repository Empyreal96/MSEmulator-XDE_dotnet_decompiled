using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Microsoft.Windows.HostNetworkingService.PrivateCloudPlugin
{
	// Token: 0x020000DA RID: 218
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Interface_XML
	{
		// Token: 0x06000353 RID: 851 RVA: 0x0000BC84 File Offset: 0x00009E84
		public static bool IsJsonDefault(Interface_XML val)
		{
			return Interface_XML._default.JsonEquals(val);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000BC94 File Offset: 0x00009E94
		public bool JsonEquals(object obj)
		{
			Interface_XML graph = obj as Interface_XML;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Interface_XML), new DataContractJsonSerializerSettings
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

		// Token: 0x04000436 RID: 1078
		private static readonly Interface_XML _default = new Interface_XML();

		// Token: 0x04000437 RID: 1079
		[DataMember(IsRequired = true, Name = "MacAddress")]
		public string macAddr;

		// Token: 0x04000438 RID: 1080
		[DataMember(Name = "IsPrimary")]
		public bool isPrimary;

		// Token: 0x04000439 RID: 1081
		[DataMember(IsRequired = true, Name = "IPSubnet")]
		public IPSubnet_XML[] subnets;
	}
}
