using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F5 RID: 245
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessorEnlightenments
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0000CD34 File Offset: 0x0000AF34
		public static bool IsJsonDefault(ProcessorEnlightenments val)
		{
			return ProcessorEnlightenments._default.JsonEquals(val);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000CD44 File Offset: 0x0000AF44
		public bool JsonEquals(object obj)
		{
			ProcessorEnlightenments graph = obj as ProcessorEnlightenments;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessorEnlightenments), new DataContractJsonSerializerSettings
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

		// Token: 0x040004B6 RID: 1206
		private static readonly ProcessorEnlightenments _default = new ProcessorEnlightenments();

		// Token: 0x040004B7 RID: 1207
		[DataMember(EmitDefaultValue = false, Name = "scheduler_assist_enabled")]
		public bool SchedulerAssistEnabled;
	}
}
