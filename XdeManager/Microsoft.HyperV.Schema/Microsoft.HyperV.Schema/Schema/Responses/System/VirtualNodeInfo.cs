using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000053 RID: 83
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualNodeInfo
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000057B2 File Offset: 0x000039B2
		public static bool IsJsonDefault(VirtualNodeInfo val)
		{
			return VirtualNodeInfo._default.JsonEquals(val);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000057C0 File Offset: 0x000039C0
		public bool JsonEquals(object obj)
		{
			VirtualNodeInfo graph = obj as VirtualNodeInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualNodeInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x040001AB RID: 427
		private static readonly VirtualNodeInfo _default = new VirtualNodeInfo();

		// Token: 0x040001AC RID: 428
		[DataMember]
		public byte VirtualNodeIndex;

		// Token: 0x040001AD RID: 429
		[DataMember]
		public byte PhysicalNodeNumber;

		// Token: 0x040001AE RID: 430
		[DataMember]
		public uint VirtualProcessorCount;

		// Token: 0x040001AF RID: 431
		[DataMember]
		public ulong MemoryUsageInPages;
	}
}
