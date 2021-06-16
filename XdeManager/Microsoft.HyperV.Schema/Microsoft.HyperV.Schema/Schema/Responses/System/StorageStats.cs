using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x02000059 RID: 89
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class StorageStats
	{
		// Token: 0x06000153 RID: 339 RVA: 0x00005C94 File Offset: 0x00003E94
		public static bool IsJsonDefault(StorageStats val)
		{
			return StorageStats._default.JsonEquals(val);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public bool JsonEquals(object obj)
		{
			StorageStats graph = obj as StorageStats;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(StorageStats), new DataContractJsonSerializerSettings
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

		// Token: 0x040001CB RID: 459
		private static readonly StorageStats _default = new StorageStats();

		// Token: 0x040001CC RID: 460
		[DataMember]
		public ulong ReadCountNormalized;

		// Token: 0x040001CD RID: 461
		[DataMember]
		public ulong ReadSizeBytes;

		// Token: 0x040001CE RID: 462
		[DataMember]
		public ulong WriteCountNormalized;

		// Token: 0x040001CF RID: 463
		[DataMember]
		public ulong WriteSizeBytes;
	}
}
