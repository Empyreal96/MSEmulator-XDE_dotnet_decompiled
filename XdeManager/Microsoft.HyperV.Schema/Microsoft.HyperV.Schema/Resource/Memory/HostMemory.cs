using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000C9 RID: 201
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HostMemory
	{
		// Token: 0x06000307 RID: 775 RVA: 0x0000AEAC File Offset: 0x000090AC
		public static bool IsJsonDefault(HostMemory val)
		{
			return HostMemory._default.JsonEquals(val);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000AEBC File Offset: 0x000090BC
		public bool JsonEquals(object obj)
		{
			HostMemory graph = obj as HostMemory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HostMemory), new DataContractJsonSerializerSettings
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

		// Token: 0x040003E5 RID: 997
		private static readonly HostMemory _default = new HostMemory();

		// Token: 0x040003E6 RID: 998
		[DataMember]
		public ulong TotalMemoryInPages;

		// Token: 0x040003E7 RID: 999
		[DataMember]
		public ulong TotalConsumableMemoryInPages;

		// Token: 0x040003E8 RID: 1000
		[DataMember]
		public ulong AvailableMemoryInPages;

		// Token: 0x040003E9 RID: 1001
		[DataMember]
		public ulong VmOverheadInPages;

		// Token: 0x040003EA RID: 1002
		[DataMember]
		public ulong VmOverheadPerGBInPages;

		// Token: 0x040003EB RID: 1003
		[DataMember]
		public ulong SystemOverheadInPages;

		// Token: 0x040003EC RID: 1004
		[DataMember]
		public bool NumaSpanningEnabled;
	}
}
