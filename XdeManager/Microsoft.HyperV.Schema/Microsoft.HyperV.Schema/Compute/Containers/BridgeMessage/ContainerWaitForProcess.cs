using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B4 RID: 436
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerWaitForProcess
	{
		// Token: 0x0600070F RID: 1807 RVA: 0x0001644C File Offset: 0x0001464C
		public static bool IsJsonDefault(ContainerWaitForProcess val)
		{
			return ContainerWaitForProcess._default.JsonEquals(val);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001645C File Offset: 0x0001465C
		public bool JsonEquals(object obj)
		{
			ContainerWaitForProcess graph = obj as ContainerWaitForProcess;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerWaitForProcess), new DataContractJsonSerializerSettings
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

		// Token: 0x040009AB RID: 2475
		private static readonly ContainerWaitForProcess _default = new ContainerWaitForProcess();

		// Token: 0x040009AC RID: 2476
		[DataMember]
		public string ContainerId;

		// Token: 0x040009AD RID: 2477
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009AE RID: 2478
		[DataMember]
		public uint ProcessId;

		// Token: 0x040009AF RID: 2479
		[DataMember]
		public uint TimeoutInMs;
	}
}
