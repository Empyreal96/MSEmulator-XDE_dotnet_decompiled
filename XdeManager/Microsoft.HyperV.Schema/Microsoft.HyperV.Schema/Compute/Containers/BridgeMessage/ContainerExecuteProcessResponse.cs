using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001BD RID: 445
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerExecuteProcessResponse
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public static bool IsJsonDefault(ContainerExecuteProcessResponse val)
		{
			return ContainerExecuteProcessResponse._default.JsonEquals(val);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00016BF8 File Offset: 0x00014DF8
		public bool JsonEquals(object obj)
		{
			ContainerExecuteProcessResponse graph = obj as ContainerExecuteProcessResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerExecuteProcessResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040009DB RID: 2523
		private static readonly ContainerExecuteProcessResponse _default = new ContainerExecuteProcessResponse();

		// Token: 0x040009DC RID: 2524
		[DataMember]
		public long Result;

		// Token: 0x040009DD RID: 2525
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009DE RID: 2526
		[DataMember(EmitDefaultValue = false)]
		public ErrorRecord[] ErrorRecords;

		// Token: 0x040009DF RID: 2527
		[DataMember]
		public uint ProcessId;
	}
}
