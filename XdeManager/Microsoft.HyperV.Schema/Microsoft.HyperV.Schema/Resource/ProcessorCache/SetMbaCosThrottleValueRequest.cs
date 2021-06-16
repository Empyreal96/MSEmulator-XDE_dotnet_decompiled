using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000B2 RID: 178
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SetMbaCosThrottleValueRequest
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x00009F28 File Offset: 0x00008128
		public static bool IsJsonDefault(SetMbaCosThrottleValueRequest val)
		{
			return SetMbaCosThrottleValueRequest._default.JsonEquals(val);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00009F38 File Offset: 0x00008138
		public bool JsonEquals(object obj)
		{
			SetMbaCosThrottleValueRequest graph = obj as SetMbaCosThrottleValueRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SetMbaCosThrottleValueRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x04000392 RID: 914
		private static readonly SetMbaCosThrottleValueRequest _default = new SetMbaCosThrottleValueRequest();

		// Token: 0x04000393 RID: 915
		[DataMember]
		public uint CosIndex;

		// Token: 0x04000394 RID: 916
		[DataMember]
		public uint CosThrottleValue;
	}
}
