using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B9 RID: 441
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ErrorRecord
	{
		// Token: 0x06000725 RID: 1829 RVA: 0x0001688F File Offset: 0x00014A8F
		public static bool IsJsonDefault(ErrorRecord val)
		{
			return ErrorRecord._default.JsonEquals(val);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001689C File Offset: 0x00014A9C
		public bool JsonEquals(object obj)
		{
			ErrorRecord graph = obj as ErrorRecord;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ErrorRecord), new DataContractJsonSerializerSettings
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

		// Token: 0x040009C3 RID: 2499
		private static readonly ErrorRecord _default = new ErrorRecord();

		// Token: 0x040009C4 RID: 2500
		[DataMember]
		public long Result;

		// Token: 0x040009C5 RID: 2501
		[DataMember]
		public string Message;

		// Token: 0x040009C6 RID: 2502
		[DataMember(EmitDefaultValue = false)]
		public string StackTrace;

		// Token: 0x040009C7 RID: 2503
		[DataMember]
		public string ModuleName;

		// Token: 0x040009C8 RID: 2504
		[DataMember]
		public string FileName;

		// Token: 0x040009C9 RID: 2505
		[DataMember]
		public uint Line;

		// Token: 0x040009CA RID: 2506
		[DataMember(EmitDefaultValue = false)]
		public string FunctionName;
	}
}
