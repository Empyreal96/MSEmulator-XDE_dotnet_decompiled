using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001BE RID: 446
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerWaitForProcessResponse
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public static bool IsJsonDefault(ContainerWaitForProcessResponse val)
		{
			return ContainerWaitForProcessResponse._default.JsonEquals(val);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00016CC4 File Offset: 0x00014EC4
		public bool JsonEquals(object obj)
		{
			ContainerWaitForProcessResponse graph = obj as ContainerWaitForProcessResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerWaitForProcessResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040009E0 RID: 2528
		private static readonly ContainerWaitForProcessResponse _default = new ContainerWaitForProcessResponse();

		// Token: 0x040009E1 RID: 2529
		[DataMember]
		public long Result;

		// Token: 0x040009E2 RID: 2530
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009E3 RID: 2531
		[DataMember(EmitDefaultValue = false)]
		public ErrorRecord[] ErrorRecords;

		// Token: 0x040009E4 RID: 2532
		[DataMember]
		public uint ExitCode;
	}
}
