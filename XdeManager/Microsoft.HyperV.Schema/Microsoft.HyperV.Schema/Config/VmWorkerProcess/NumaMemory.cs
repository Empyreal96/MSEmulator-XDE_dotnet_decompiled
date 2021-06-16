using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F0 RID: 240
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaMemory
	{
		// Token: 0x06000399 RID: 921 RVA: 0x0000C8BC File Offset: 0x0000AABC
		public static bool IsJsonDefault(NumaMemory val)
		{
			return NumaMemory._default.JsonEquals(val);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000C8CC File Offset: 0x0000AACC
		public bool JsonEquals(object obj)
		{
			NumaMemory graph = obj as NumaMemory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaMemory), new DataContractJsonSerializerSettings
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

		// Token: 0x040004A1 RID: 1185
		private static readonly NumaMemory _default = new NumaMemory();

		// Token: 0x040004A2 RID: 1186
		[DataMember(Name = "max_size_per_node")]
		public ulong MaxSizePerNode;
	}
}
