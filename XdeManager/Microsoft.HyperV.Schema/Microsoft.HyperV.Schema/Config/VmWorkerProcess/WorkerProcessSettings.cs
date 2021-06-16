using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x02000101 RID: 257
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class WorkerProcessSettings
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x0000D83F File Offset: 0x0000BA3F
		public static bool IsJsonDefault(WorkerProcessSettings val)
		{
			return WorkerProcessSettings._default.JsonEquals(val);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000D84C File Offset: 0x0000BA4C
		public bool JsonEquals(object obj)
		{
			WorkerProcessSettings graph = obj as WorkerProcessSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(WorkerProcessSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x0400050A RID: 1290
		private static readonly WorkerProcessSettings _default = new WorkerProcessSettings();

		// Token: 0x0400050B RID: 1291
		[DataMember(EmitDefaultValue = false)]
		public string SlpDataPath;

		// Token: 0x0400050C RID: 1292
		[DataMember(EmitDefaultValue = false)]
		public string MemoryDumpFilePath;

		// Token: 0x0400050D RID: 1293
		[DataMember(EmitDefaultValue = false)]
		public string BinFilePath;

		// Token: 0x0400050E RID: 1294
		[DataMember(EmitDefaultValue = false)]
		public string VsvFilePath;
	}
}
