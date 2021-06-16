using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000193 RID: 403
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNetworkResponse
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x00014908 File Offset: 0x00012B08
		public static bool IsJsonDefault(HNSNetworkResponse val)
		{
			return HNSNetworkResponse._default.JsonEquals(val);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00014918 File Offset: 0x00012B18
		public bool JsonEquals(object obj)
		{
			HNSNetworkResponse graph = obj as HNSNetworkResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNetworkResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040008C4 RID: 2244
		private static readonly HNSNetworkResponse _default = new HNSNetworkResponse();

		// Token: 0x040008C5 RID: 2245
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008C6 RID: 2246
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008C7 RID: 2247
		[DataMember(EmitDefaultValue = false)]
		public HNSNetwork[] Output;
	}
}
