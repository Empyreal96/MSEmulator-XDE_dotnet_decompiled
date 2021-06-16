using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers
{
	// Token: 0x020001A8 RID: 424
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessStatus
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x00015B2C File Offset: 0x00013D2C
		public static bool IsJsonDefault(ProcessStatus val)
		{
			return ProcessStatus._default.JsonEquals(val);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00015B3C File Offset: 0x00013D3C
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

		// Token: 0x0400096C RID: 2412
		private static readonly ProcessStatus _default = new ProcessStatus();

		// Token: 0x0400096D RID: 2413
		[DataMember]
		public uint ProcessId;

		// Token: 0x0400096E RID: 2414
		[DataMember]
		public bool Exited;

		// Token: 0x0400096F RID: 2415
		[DataMember]
		public uint ExitCode;

		// Token: 0x04000970 RID: 2416
		[DataMember]
		public int LastWaitResult;
	}
}
