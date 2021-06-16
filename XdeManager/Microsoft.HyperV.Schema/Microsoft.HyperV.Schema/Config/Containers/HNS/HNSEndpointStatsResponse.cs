using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000199 RID: 409
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSEndpointStatsResponse
	{
		// Token: 0x06000699 RID: 1689 RVA: 0x00014E56 File Offset: 0x00013056
		public static bool IsJsonDefault(HNSEndpointStatsResponse val)
		{
			return HNSEndpointStatsResponse._default.JsonEquals(val);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00014E64 File Offset: 0x00013064
		public bool JsonEquals(object obj)
		{
			HNSEndpointStatsResponse graph = obj as HNSEndpointStatsResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSEndpointStatsResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040008DE RID: 2270
		private static readonly HNSEndpointStatsResponse _default = new HNSEndpointStatsResponse();

		// Token: 0x040008DF RID: 2271
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008E0 RID: 2272
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008E1 RID: 2273
		[DataMember(EmitDefaultValue = false)]
		public object Output;
	}
}
