using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x0200005F RID: 95
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheQueryStatsResponse
	{
		// Token: 0x06000177 RID: 375 RVA: 0x000062A4 File Offset: 0x000044A4
		public static bool IsJsonDefault(CacheQueryStatsResponse val)
		{
			return CacheQueryStatsResponse._default.JsonEquals(val);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000062B4 File Offset: 0x000044B4
		public bool JsonEquals(object obj)
		{
			CacheQueryStatsResponse graph = obj as CacheQueryStatsResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheQueryStatsResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040001EA RID: 490
		private static readonly CacheQueryStatsResponse _default = new CacheQueryStatsResponse();

		// Token: 0x040001EB RID: 491
		[DataMember]
		public ulong L3OccupancyBytes;

		// Token: 0x040001EC RID: 492
		[DataMember]
		public ulong L3TotalBwBytes;

		// Token: 0x040001ED RID: 493
		[DataMember]
		public ulong L3LocalBwBytes;
	}
}
