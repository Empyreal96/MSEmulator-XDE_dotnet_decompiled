using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000C7 RID: 199
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaNodeMemory
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0000AD14 File Offset: 0x00008F14
		public static bool IsJsonDefault(NumaNodeMemory val)
		{
			return NumaNodeMemory._default.JsonEquals(val);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000AD24 File Offset: 0x00008F24
		public bool JsonEquals(object obj)
		{
			NumaNodeMemory graph = obj as NumaNodeMemory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaNodeMemory), new DataContractJsonSerializerSettings
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

		// Token: 0x040003DF RID: 991
		private static readonly NumaNodeMemory _default = new NumaNodeMemory();

		// Token: 0x040003E0 RID: 992
		[DataMember]
		public ulong TotalConsumableMemoryInPages;

		// Token: 0x040003E1 RID: 993
		[DataMember]
		public ulong AvailableMemoryInPages;
	}
}
