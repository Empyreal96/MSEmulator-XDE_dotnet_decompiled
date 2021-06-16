using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F4 RID: 244
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaProcessors
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x0000CC68 File Offset: 0x0000AE68
		public static bool IsJsonDefault(NumaProcessors val)
		{
			return NumaProcessors._default.JsonEquals(val);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000CC78 File Offset: 0x0000AE78
		public bool JsonEquals(object obj)
		{
			NumaProcessors graph = obj as NumaProcessors;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaProcessors), new DataContractJsonSerializerSettings
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

		// Token: 0x040004B3 RID: 1203
		private static readonly NumaProcessors _default = new NumaProcessors();

		// Token: 0x040004B4 RID: 1204
		[DataMember(Name = "count_per_node")]
		public uint MaximumProcessorsPerNode;

		// Token: 0x040004B5 RID: 1205
		[DataMember(Name = "node_per_socket")]
		public uint MaximumNodesPerSocket;
	}
}
