using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000B5 RID: 181
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CpuGroupProperty
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000A0C0 File Offset: 0x000082C0
		public static bool IsJsonDefault(CpuGroupProperty val)
		{
			return CpuGroupProperty._default.JsonEquals(val);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A0D0 File Offset: 0x000082D0
		public bool JsonEquals(object obj)
		{
			CpuGroupProperty graph = obj as CpuGroupProperty;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CpuGroupProperty), new DataContractJsonSerializerSettings
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

		// Token: 0x0400039C RID: 924
		private static readonly CpuGroupProperty _default = new CpuGroupProperty();

		// Token: 0x0400039D RID: 925
		[DataMember]
		public uint PropertyCode;

		// Token: 0x0400039E RID: 926
		[DataMember]
		public ulong PropertyValue;
	}
}
