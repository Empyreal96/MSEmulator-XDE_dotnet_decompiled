using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000195 RID: 405
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSEndpointResponse
	{
		// Token: 0x06000685 RID: 1669 RVA: 0x00014ACE File Offset: 0x00012CCE
		public static bool IsJsonDefault(HNSEndpointResponse val)
		{
			return HNSEndpointResponse._default.JsonEquals(val);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00014ADC File Offset: 0x00012CDC
		public bool JsonEquals(object obj)
		{
			HNSEndpointResponse graph = obj as HNSEndpointResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSEndpointResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040008CC RID: 2252
		private static readonly HNSEndpointResponse _default = new HNSEndpointResponse();

		// Token: 0x040008CD RID: 2253
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008CE RID: 2254
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008CF RID: 2255
		[DataMember(EmitDefaultValue = false)]
		public uint ErrorCode;

		// Token: 0x040008D0 RID: 2256
		[DataMember(EmitDefaultValue = false)]
		public HNSEndpoint[] Output;
	}
}
