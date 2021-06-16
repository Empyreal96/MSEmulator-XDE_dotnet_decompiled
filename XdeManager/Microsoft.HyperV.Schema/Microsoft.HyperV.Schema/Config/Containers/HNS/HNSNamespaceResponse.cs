using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000197 RID: 407
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNamespaceResponse
	{
		// Token: 0x0600068F RID: 1679 RVA: 0x00014C92 File Offset: 0x00012E92
		public static bool IsJsonDefault(HNSNamespaceResponse val)
		{
			return HNSNamespaceResponse._default.JsonEquals(val);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00014CA0 File Offset: 0x00012EA0
		public bool JsonEquals(object obj)
		{
			HNSNamespaceResponse graph = obj as HNSNamespaceResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNamespaceResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040008D6 RID: 2262
		private static readonly HNSNamespaceResponse _default = new HNSNamespaceResponse();

		// Token: 0x040008D7 RID: 2263
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008D8 RID: 2264
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008D9 RID: 2265
		[DataMember(EmitDefaultValue = false)]
		public HNSNamespace[] Output;
	}
}
