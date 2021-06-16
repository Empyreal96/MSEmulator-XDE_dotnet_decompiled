using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000B4 RID: 180
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CpuGroupAffinity
	{
		// Token: 0x060002BB RID: 699 RVA: 0x00009FF4 File Offset: 0x000081F4
		public static bool IsJsonDefault(CpuGroupAffinity val)
		{
			return CpuGroupAffinity._default.JsonEquals(val);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A004 File Offset: 0x00008204
		public bool JsonEquals(object obj)
		{
			CpuGroupAffinity graph = obj as CpuGroupAffinity;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CpuGroupAffinity), new DataContractJsonSerializerSettings
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

		// Token: 0x04000399 RID: 921
		private static readonly CpuGroupAffinity _default = new CpuGroupAffinity();

		// Token: 0x0400039A RID: 922
		[DataMember]
		public uint LogicalProcessorCount;

		// Token: 0x0400039B RID: 923
		[DataMember(EmitDefaultValue = false)]
		public uint[] LogicalProcessors;
	}
}
