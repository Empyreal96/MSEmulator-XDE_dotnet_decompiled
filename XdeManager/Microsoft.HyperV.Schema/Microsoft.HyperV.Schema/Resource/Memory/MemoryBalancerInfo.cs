using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000C8 RID: 200
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryBalancerInfo
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public static bool IsJsonDefault(MemoryBalancerInfo val)
		{
			return MemoryBalancerInfo._default.JsonEquals(val);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public bool JsonEquals(object obj)
		{
			MemoryBalancerInfo graph = obj as MemoryBalancerInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryBalancerInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x040003E2 RID: 994
		private static readonly MemoryBalancerInfo _default = new MemoryBalancerInfo();

		// Token: 0x040003E3 RID: 995
		[DataMember]
		public ulong AvailableMemoryInPages;

		// Token: 0x040003E4 RID: 996
		[DataMember]
		public ulong AvailableMemoryForStartingVmsInPages;
	}
}
