using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x0200018E RID: 398
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNamespaceRequest
	{
		// Token: 0x06000663 RID: 1635 RVA: 0x000144B0 File Offset: 0x000126B0
		public static bool IsJsonDefault(HNSNamespaceRequest val)
		{
			return HNSNamespaceRequest._default.JsonEquals(val);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000144C0 File Offset: 0x000126C0
		public bool JsonEquals(object obj)
		{
			HNSNamespaceRequest graph = obj as HNSNamespaceRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNamespaceRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x040008A1 RID: 2209
		private static readonly HNSNamespaceRequest _default = new HNSNamespaceRequest();

		// Token: 0x040008A2 RID: 2210
		[DataMember]
		public bool IsDefault;
	}
}
