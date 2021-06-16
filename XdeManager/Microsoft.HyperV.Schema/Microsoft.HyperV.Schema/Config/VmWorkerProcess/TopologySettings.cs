using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F9 RID: 249
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class TopologySettings
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		public static bool IsJsonDefault(TopologySettings val)
		{
			return TopologySettings._default.JsonEquals(val);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public bool JsonEquals(object obj)
		{
			TopologySettings graph = obj as TopologySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TopologySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040004D8 RID: 1240
		private static readonly TopologySettings _default = new TopologySettings();

		// Token: 0x040004D9 RID: 1241
		[DataMember(EmitDefaultValue = false, Name = "high_mmio_gap_mb")]
		public long HighMMIOGapInMB;

		// Token: 0x040004DA RID: 1242
		[DataMember(EmitDefaultValue = false, Name = "high_mmio_gap_base_mb")]
		public long HighMMIOBaseInMB;

		// Token: 0x040004DB RID: 1243
		[DataMember(Name = "low_mmio_gap_mb")]
		public long LowMMIOGapInMB;

		// Token: 0x040004DC RID: 1244
		[DataMember(Name = "direct_file_mapping_mb")]
		public long DirectFileMappingMB;

		// Token: 0x040004DD RID: 1245
		[DataMember(Name = "shared_memory_mb")]
		public long SharedMemoryMB;
	}
}
