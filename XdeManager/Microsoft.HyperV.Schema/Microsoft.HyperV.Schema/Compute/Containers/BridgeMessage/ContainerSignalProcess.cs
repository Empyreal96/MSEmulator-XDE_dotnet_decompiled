using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B5 RID: 437
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerSignalProcess
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x00016518 File Offset: 0x00014718
		public static bool IsJsonDefault(ContainerSignalProcess val)
		{
			return ContainerSignalProcess._default.JsonEquals(val);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00016528 File Offset: 0x00014728
		public bool JsonEquals(object obj)
		{
			ContainerSignalProcess graph = obj as ContainerSignalProcess;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerSignalProcess), new DataContractJsonSerializerSettings
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

		// Token: 0x040009B0 RID: 2480
		private static readonly ContainerSignalProcess _default = new ContainerSignalProcess();

		// Token: 0x040009B1 RID: 2481
		[DataMember]
		public string ContainerId;

		// Token: 0x040009B2 RID: 2482
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009B3 RID: 2483
		[DataMember]
		public uint ProcessId;

		// Token: 0x040009B4 RID: 2484
		[DataMember(EmitDefaultValue = false)]
		public object Options;
	}
}
