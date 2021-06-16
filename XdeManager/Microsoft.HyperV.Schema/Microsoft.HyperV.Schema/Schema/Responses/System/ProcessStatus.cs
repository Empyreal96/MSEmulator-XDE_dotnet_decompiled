using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000062 RID: 98
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessStatus
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00006654 File Offset: 0x00004854
		public static bool IsJsonDefault(ProcessStatus val)
		{
			return ProcessStatus._default.JsonEquals(val);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006664 File Offset: 0x00004864
		public bool JsonEquals(object obj)
		{
			ProcessStatus graph = obj as ProcessStatus;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessStatus), new DataContractJsonSerializerSettings
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

		// Token: 0x0400020D RID: 525
		private static readonly ProcessStatus _default = new ProcessStatus();

		// Token: 0x0400020E RID: 526
		[DataMember]
		public uint ProcessId;

		// Token: 0x0400020F RID: 527
		[DataMember]
		public bool Exited;

		// Token: 0x04000210 RID: 528
		[DataMember]
		public uint ExitCode;

		// Token: 0x04000211 RID: 529
		[DataMember]
		public int LastWaitResult;
	}
}
