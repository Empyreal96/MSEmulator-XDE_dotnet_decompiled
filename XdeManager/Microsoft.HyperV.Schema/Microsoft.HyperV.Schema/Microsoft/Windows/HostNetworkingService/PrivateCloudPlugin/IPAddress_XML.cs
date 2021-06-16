using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Microsoft.Windows.HostNetworkingService.PrivateCloudPlugin
{
	// Token: 0x020000D8 RID: 216
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class IPAddress_XML
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000BAEC File Offset: 0x00009CEC
		public static bool IsJsonDefault(IPAddress_XML val)
		{
			return IPAddress_XML._default.JsonEquals(val);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public bool JsonEquals(object obj)
		{
			IPAddress_XML graph = obj as IPAddress_XML;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IPAddress_XML), new DataContractJsonSerializerSettings
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

		// Token: 0x04000430 RID: 1072
		private static readonly IPAddress_XML _default = new IPAddress_XML();

		// Token: 0x04000431 RID: 1073
		[DataMember(IsRequired = true, Name = "Address")]
		public string addr;

		// Token: 0x04000432 RID: 1074
		[DataMember(Name = "IsPrimary")]
		public bool isPrimary;
	}
}
