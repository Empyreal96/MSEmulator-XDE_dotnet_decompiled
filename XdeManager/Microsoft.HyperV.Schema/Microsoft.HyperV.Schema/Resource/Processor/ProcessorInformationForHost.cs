using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000C0 RID: 192
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessorInformationForHost
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		public static bool IsJsonDefault(ProcessorInformationForHost val)
		{
			return ProcessorInformationForHost._default.JsonEquals(val);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		public bool JsonEquals(object obj)
		{
			ProcessorInformationForHost graph = obj as ProcessorInformationForHost;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessorInformationForHost), new DataContractJsonSerializerSettings
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

		// Token: 0x040003C0 RID: 960
		private static readonly ProcessorInformationForHost _default = new ProcessorInformationForHost();

		// Token: 0x040003C1 RID: 961
		[DataMember]
		public uint NumaNodeCount;

		// Token: 0x040003C2 RID: 962
		[DataMember(EmitDefaultValue = false)]
		public NumaNodeProcessor[] NumaNodes;
	}
}
