using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Microsoft.Windows.HostNetworkingService.PrivateCloudPlugin
{
	// Token: 0x020000D6 RID: 214
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ObjectNetwork_JSON
	{
		// Token: 0x06000343 RID: 835 RVA: 0x0000B957 File Offset: 0x00009B57
		public static bool IsJsonDefault(ObjectNetwork_JSON val)
		{
			return ObjectNetwork_JSON._default.JsonEquals(val);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000B964 File Offset: 0x00009B64
		public bool JsonEquals(object obj)
		{
			ObjectNetwork_JSON graph = obj as ObjectNetwork_JSON;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ObjectNetwork_JSON), new DataContractJsonSerializerSettings
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

		// Token: 0x04000427 RID: 1063
		private static readonly ObjectNetwork_JSON _default = new ObjectNetwork_JSON();

		// Token: 0x04000428 RID: 1064
		[DataMember(IsRequired = true)]
		public string isolation;

		// Token: 0x04000429 RID: 1065
		[DataMember(EmitDefaultValue = false)]
		public Objectips_JSON[] ips;
	}
}
