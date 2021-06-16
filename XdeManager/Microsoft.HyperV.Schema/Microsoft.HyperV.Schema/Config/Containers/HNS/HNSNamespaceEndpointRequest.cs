using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x0200018F RID: 399
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNamespaceEndpointRequest
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x0001457C File Offset: 0x0001277C
		public static bool IsJsonDefault(HNSNamespaceEndpointRequest val)
		{
			return HNSNamespaceEndpointRequest._default.JsonEquals(val);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001458C File Offset: 0x0001278C
		public bool JsonEquals(object obj)
		{
			HNSNamespaceEndpointRequest graph = obj as HNSNamespaceEndpointRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNamespaceEndpointRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x040008A3 RID: 2211
		private static readonly HNSNamespaceEndpointRequest _default = new HNSNamespaceEndpointRequest();

		// Token: 0x040008A4 RID: 2212
		[DataMember]
		public Guid Id;
	}
}
