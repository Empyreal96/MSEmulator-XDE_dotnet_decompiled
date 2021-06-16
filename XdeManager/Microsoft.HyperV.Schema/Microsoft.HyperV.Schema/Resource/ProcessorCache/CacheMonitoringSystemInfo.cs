using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Responses.System;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000AC RID: 172
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheMonitoringSystemInfo
	{
		// Token: 0x0600029B RID: 667 RVA: 0x00009A00 File Offset: 0x00007C00
		public static bool IsJsonDefault(CacheMonitoringSystemInfo val)
		{
			return CacheMonitoringSystemInfo._default.JsonEquals(val);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00009A10 File Offset: 0x00007C10
		public bool JsonEquals(object obj)
		{
			CacheMonitoringSystemInfo graph = obj as CacheMonitoringSystemInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheMonitoringSystemInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00009AB8 File Offset: 0x00007CB8
		// (set) Token: 0x0600029E RID: 670 RVA: 0x00009AC0 File Offset: 0x00007CC0
		[DataMember(Name = "RootCacheStats")]
		private CacheQueryStatsResponse _RootCacheStats
		{
			get
			{
				return this.RootCacheStats;
			}
			set
			{
				if (value != null)
				{
					this.RootCacheStats = value;
				}
			}
		}

		// Token: 0x0400037A RID: 890
		private static readonly CacheMonitoringSystemInfo _default = new CacheMonitoringSystemInfo();

		// Token: 0x0400037B RID: 891
		[DataMember]
		public bool L3CmtSupported;

		// Token: 0x0400037C RID: 892
		[DataMember]
		public bool L3OccupancyMonitoringSupported;

		// Token: 0x0400037D RID: 893
		[DataMember]
		public bool L3TotalBwMonitoringSupported;

		// Token: 0x0400037E RID: 894
		[DataMember]
		public bool L3LocalBwMonitoringSupported;

		// Token: 0x0400037F RID: 895
		[DataMember]
		public uint MaxRmid;

		// Token: 0x04000380 RID: 896
		[DataMember]
		public uint RootRmid;

		// Token: 0x04000381 RID: 897
		public CacheQueryStatsResponse RootCacheStats = new CacheQueryStatsResponse();
	}
}
