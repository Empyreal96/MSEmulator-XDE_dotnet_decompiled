using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000E6 RID: 230
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CriticalErrorPolicy
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		public static bool IsJsonDefault(CriticalErrorPolicy val)
		{
			return CriticalErrorPolicy._default.JsonEquals(val);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000BFC4 File Offset: 0x0000A1C4
		public bool JsonEquals(object obj)
		{
			CriticalErrorPolicy graph = obj as CriticalErrorPolicy;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CriticalErrorPolicy), new DataContractJsonSerializerSettings
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

		// Token: 0x04000466 RID: 1126
		private static readonly CriticalErrorPolicy _default = new CriticalErrorPolicy();

		// Token: 0x04000467 RID: 1127
		[DataMember(Name = "action")]
		public CriticalErrorAction Action;

		// Token: 0x04000468 RID: 1128
		[DataMember(Name = "timeout")]
		public long Timeout;
	}
}
