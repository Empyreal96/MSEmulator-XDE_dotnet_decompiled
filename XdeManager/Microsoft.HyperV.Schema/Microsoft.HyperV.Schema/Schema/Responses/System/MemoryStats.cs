using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000058 RID: 88
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryStats
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00005BC8 File Offset: 0x00003DC8
		public static bool IsJsonDefault(MemoryStats val)
		{
			return MemoryStats._default.JsonEquals(val);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005BD8 File Offset: 0x00003DD8
		public bool JsonEquals(object obj)
		{
			MemoryStats graph = obj as MemoryStats;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryStats), new DataContractJsonSerializerSettings
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

		// Token: 0x040001C7 RID: 455
		private static readonly MemoryStats _default = new MemoryStats();

		// Token: 0x040001C8 RID: 456
		[DataMember]
		public ulong MemoryUsageCommitBytes;

		// Token: 0x040001C9 RID: 457
		[DataMember]
		public ulong MemoryUsageCommitPeakBytes;

		// Token: 0x040001CA RID: 458
		[DataMember]
		public ulong MemoryUsagePrivateWorkingSetBytes;
	}
}
