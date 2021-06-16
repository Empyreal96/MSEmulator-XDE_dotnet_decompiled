using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000057 RID: 87
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessorStats
	{
		// Token: 0x0600014B RID: 331 RVA: 0x00005AFC File Offset: 0x00003CFC
		public static bool IsJsonDefault(ProcessorStats val)
		{
			return ProcessorStats._default.JsonEquals(val);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005B0C File Offset: 0x00003D0C
		public bool JsonEquals(object obj)
		{
			ProcessorStats graph = obj as ProcessorStats;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessorStats), new DataContractJsonSerializerSettings
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

		// Token: 0x040001C3 RID: 451
		private static readonly ProcessorStats _default = new ProcessorStats();

		// Token: 0x040001C4 RID: 452
		[DataMember]
		public ulong TotalRuntime100ns;

		// Token: 0x040001C5 RID: 453
		[DataMember(EmitDefaultValue = false)]
		public ulong RuntimeUser100ns;

		// Token: 0x040001C6 RID: 454
		[DataMember(EmitDefaultValue = false)]
		public ulong RuntimeKernel100ns;
	}
}
