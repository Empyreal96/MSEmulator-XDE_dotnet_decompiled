using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000B8 RID: 184
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessorTopology
	{
		// Token: 0x060002CD RID: 717 RVA: 0x0000A340 File Offset: 0x00008540
		public static bool IsJsonDefault(ProcessorTopology val)
		{
			return ProcessorTopology._default.JsonEquals(val);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A350 File Offset: 0x00008550
		public bool JsonEquals(object obj)
		{
			ProcessorTopology graph = obj as ProcessorTopology;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessorTopology), new DataContractJsonSerializerSettings
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

		// Token: 0x040003A9 RID: 937
		private static readonly ProcessorTopology _default = new ProcessorTopology();

		// Token: 0x040003AA RID: 938
		[DataMember]
		public uint LogicalProcessorCount;

		// Token: 0x040003AB RID: 939
		[DataMember]
		public LogicalProcessor[] LogicalProcessors;
	}
}
