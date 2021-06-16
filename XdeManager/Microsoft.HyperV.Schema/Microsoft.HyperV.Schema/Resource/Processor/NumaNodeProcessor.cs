using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000BF RID: 191
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaNodeProcessor
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000A918 File Offset: 0x00008B18
		public static bool IsJsonDefault(NumaNodeProcessor val)
		{
			return NumaNodeProcessor._default.JsonEquals(val);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000A928 File Offset: 0x00008B28
		public bool JsonEquals(object obj)
		{
			NumaNodeProcessor graph = obj as NumaNodeProcessor;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaNodeProcessor), new DataContractJsonSerializerSettings
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

		// Token: 0x040003BD RID: 957
		private static readonly NumaNodeProcessor _default = new NumaNodeProcessor();

		// Token: 0x040003BE RID: 958
		[DataMember]
		public uint TotalAssignedProcessors;

		// Token: 0x040003BF RID: 959
		[DataMember]
		public uint TotalAvailableProcessors;
	}
}
