using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.Service
{
	// Token: 0x0200006B RID: 107
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ResultError
	{
		// Token: 0x060001AF RID: 431 RVA: 0x00006D4C File Offset: 0x00004F4C
		public static bool IsJsonDefault(ResultError val)
		{
			return ResultError._default.JsonEquals(val);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006D5C File Offset: 0x00004F5C
		public bool JsonEquals(object obj)
		{
			ResultError graph = obj as ResultError;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ResultError), new DataContractJsonSerializerSettings
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

		// Token: 0x04000245 RID: 581
		private static readonly ResultError _default = new ResultError();

		// Token: 0x04000246 RID: 582
		[DataMember]
		public int Error;

		// Token: 0x04000247 RID: 583
		[DataMember]
		public string ErrorMessage;

		// Token: 0x04000248 RID: 584
		[DataMember(EmitDefaultValue = false)]
		public ErrorEvent[] ErrorEvents;
	}
}
