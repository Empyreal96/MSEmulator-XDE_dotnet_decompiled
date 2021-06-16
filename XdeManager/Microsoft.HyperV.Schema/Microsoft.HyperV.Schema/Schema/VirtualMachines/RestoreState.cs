using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x02000012 RID: 18
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RestoreState
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002D60 File Offset: 0x00000F60
		public static bool IsJsonDefault(RestoreState val)
		{
			return RestoreState._default.JsonEquals(val);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D70 File Offset: 0x00000F70
		public bool JsonEquals(object obj)
		{
			RestoreState graph = obj as RestoreState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RestoreState), new DataContractJsonSerializerSettings
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

		// Token: 0x0400005F RID: 95
		private static readonly RestoreState _default = new RestoreState();

		// Token: 0x04000060 RID: 96
		[DataMember(EmitDefaultValue = false)]
		public string RuntimeStateFilePath;

		// Token: 0x04000061 RID: 97
		[DataMember(EmitDefaultValue = false)]
		public string SaveStateFilePath;

		// Token: 0x04000062 RID: 98
		[DataMember(EmitDefaultValue = false)]
		public string TemplateSystemId;
	}
}
