using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.Resources
{
	// Token: 0x02000095 RID: 149
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Memory
	{
		// Token: 0x06000241 RID: 577 RVA: 0x00008928 File Offset: 0x00006B28
		public static bool IsJsonDefault(Memory val)
		{
			return Memory._default.JsonEquals(val);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00008938 File Offset: 0x00006B38
		public bool JsonEquals(object obj)
		{
			Memory graph = obj as Memory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Memory), new DataContractJsonSerializerSettings
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

		// Token: 0x04000318 RID: 792
		private static readonly Memory _default = new Memory();

		// Token: 0x04000319 RID: 793
		[DataMember]
		public ulong SizeInMB;

		// Token: 0x0400031A RID: 794
		[DataMember]
		public ulong Maximum;
	}
}
