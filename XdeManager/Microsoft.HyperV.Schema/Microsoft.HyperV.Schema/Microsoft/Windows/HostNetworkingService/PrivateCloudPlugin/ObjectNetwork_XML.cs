using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Microsoft.Windows.HostNetworkingService.PrivateCloudPlugin
{
	// Token: 0x020000DB RID: 219
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ObjectNetwork_XML
	{
		// Token: 0x06000357 RID: 855 RVA: 0x0000BD50 File Offset: 0x00009F50
		public static bool IsJsonDefault(ObjectNetwork_XML val)
		{
			return ObjectNetwork_XML._default.JsonEquals(val);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000BD60 File Offset: 0x00009F60
		public bool JsonEquals(object obj)
		{
			ObjectNetwork_XML graph = obj as ObjectNetwork_XML;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ObjectNetwork_XML), new DataContractJsonSerializerSettings
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

		// Token: 0x0400043A RID: 1082
		private static readonly ObjectNetwork_XML _default = new ObjectNetwork_XML();

		// Token: 0x0400043B RID: 1083
		[DataMember(IsRequired = true, Name = "Interface")]
		public Interface_XML[] interfaces;
	}
}
