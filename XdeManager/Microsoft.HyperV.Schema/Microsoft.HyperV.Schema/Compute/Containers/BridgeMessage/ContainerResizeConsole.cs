using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B3 RID: 435
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerResizeConsole
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x00016383 File Offset: 0x00014583
		public static bool IsJsonDefault(ContainerResizeConsole val)
		{
			return ContainerResizeConsole._default.JsonEquals(val);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00016390 File Offset: 0x00014590
		public bool JsonEquals(object obj)
		{
			ContainerResizeConsole graph = obj as ContainerResizeConsole;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerResizeConsole), new DataContractJsonSerializerSettings
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

		// Token: 0x040009A5 RID: 2469
		private static readonly ContainerResizeConsole _default = new ContainerResizeConsole();

		// Token: 0x040009A6 RID: 2470
		[DataMember]
		public string ContainerId;

		// Token: 0x040009A7 RID: 2471
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009A8 RID: 2472
		[DataMember]
		public uint ProcessId;

		// Token: 0x040009A9 RID: 2473
		[DataMember]
		public ushort Height;

		// Token: 0x040009AA RID: 2474
		[DataMember]
		public ushort Width;
	}
}
