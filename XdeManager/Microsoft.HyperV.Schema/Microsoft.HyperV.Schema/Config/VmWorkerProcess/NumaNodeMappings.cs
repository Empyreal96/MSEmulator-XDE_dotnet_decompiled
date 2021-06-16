using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000FD RID: 253
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaNodeMappings
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		public static bool IsJsonDefault(NumaNodeMappings val)
		{
			return NumaNodeMappings._default.JsonEquals(val);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
		public bool JsonEquals(object obj)
		{
			NumaNodeMappings graph = obj as NumaNodeMappings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaNodeMappings), new DataContractJsonSerializerSettings
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

		// Token: 0x040004ED RID: 1261
		private static readonly NumaNodeMappings _default = new NumaNodeMappings();

		// Token: 0x040004EE RID: 1262
		[DataMember(Name = "virtual_to_physical_count")]
		public long VirtualToPhysicalNodeCount;

		// Token: 0x040004EF RID: 1263
		[DataMember(Name = "virtual_to_physical_node")]
		public long[] VirtualToPhysicalNodes;

		// Token: 0x040004F0 RID: 1264
		[DataMember(Name = "physical_to_virtual_count")]
		public long PhysicalToVirtualNodeArrayElementCount;

		// Token: 0x040004F1 RID: 1265
		[DataMember(Name = "physical_to_virtual_key")]
		public long[] PhysicalToVirtualNodeKeyArray;

		// Token: 0x040004F2 RID: 1266
		[DataMember(Name = "physical_to_virtual_value")]
		public long[] PhysicalToVirtualNodeValueArray;
	}
}
