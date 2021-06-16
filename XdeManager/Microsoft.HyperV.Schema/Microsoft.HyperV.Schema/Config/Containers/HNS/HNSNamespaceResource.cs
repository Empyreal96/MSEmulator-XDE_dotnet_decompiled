using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000190 RID: 400
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNamespaceResource
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x00014648 File Offset: 0x00012848
		public static bool IsJsonDefault(HNSNamespaceResource val)
		{
			return HNSNamespaceResource._default.JsonEquals(val);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00014658 File Offset: 0x00012858
		public bool JsonEquals(object obj)
		{
			HNSNamespaceResource graph = obj as HNSNamespaceResource;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNamespaceResource), new DataContractJsonSerializerSettings
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

		// Token: 0x040008A5 RID: 2213
		private static readonly HNSNamespaceResource _default = new HNSNamespaceResource();

		// Token: 0x040008A6 RID: 2214
		[DataMember(IsRequired = true)]
		public string Type;

		// Token: 0x040008A7 RID: 2215
		[DataMember(IsRequired = true)]
		public object Data;
	}
}
