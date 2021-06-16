using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F7 RID: 247
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaNodeTopology
	{
		// Token: 0x060003C1 RID: 961 RVA: 0x0000CF0A File Offset: 0x0000B10A
		public static bool IsJsonDefault(NumaNodeTopology val)
		{
			return NumaNodeTopology._default.JsonEquals(val);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000CF18 File Offset: 0x0000B118
		public bool JsonEquals(object obj)
		{
			NumaNodeTopology graph = obj as NumaNodeTopology;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaNodeTopology), new DataContractJsonSerializerSettings
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

		// Token: 0x040004CF RID: 1231
		private static readonly NumaNodeTopology _default = new NumaNodeTopology();

		// Token: 0x040004D0 RID: 1232
		[DataMember]
		public uint VirtualNodeNumber;

		// Token: 0x040004D1 RID: 1233
		[DataMember]
		public uint PhysicalNodeNumber;

		// Token: 0x040004D2 RID: 1234
		[DataMember]
		public uint CountOfProcessors;

		// Token: 0x040004D3 RID: 1235
		[DataMember]
		public ulong CountOfMemoryBlocks;
	}
}
