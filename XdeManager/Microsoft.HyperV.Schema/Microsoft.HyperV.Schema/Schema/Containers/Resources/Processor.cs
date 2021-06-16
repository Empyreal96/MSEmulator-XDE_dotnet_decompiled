using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x02000094 RID: 148
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Processor
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000885F File Offset: 0x00006A5F
		public static bool IsJsonDefault(Processor val)
		{
			return Processor._default.JsonEquals(val);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000886C File Offset: 0x00006A6C
		public bool JsonEquals(object obj)
		{
			Processor graph = obj as Processor;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Processor), new DataContractJsonSerializerSettings
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

		// Token: 0x04000314 RID: 788
		private static readonly Processor _default = new Processor();

		// Token: 0x04000315 RID: 789
		[DataMember]
		public uint Count;

		// Token: 0x04000316 RID: 790
		[DataMember(EmitDefaultValue = false)]
		public ulong Maximum;

		// Token: 0x04000317 RID: 791
		[DataMember(EmitDefaultValue = false)]
		public ulong Weight;
	}
}
