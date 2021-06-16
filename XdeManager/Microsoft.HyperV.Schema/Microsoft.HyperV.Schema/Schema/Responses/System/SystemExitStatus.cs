using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000061 RID: 97
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SystemExitStatus
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000658B File Offset: 0x0000478B
		public static bool IsJsonDefault(SystemExitStatus val)
		{
			return SystemExitStatus._default.JsonEquals(val);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006598 File Offset: 0x00004798
		public bool JsonEquals(object obj)
		{
			SystemExitStatus graph = obj as SystemExitStatus;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SystemExitStatus), new DataContractJsonSerializerSettings
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

		// Token: 0x0400020B RID: 523
		private static readonly SystemExitStatus _default = new SystemExitStatus();

		// Token: 0x0400020C RID: 524
		[DataMember]
		public int Status;
	}
}
