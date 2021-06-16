using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Common.Resources
{
	// Token: 0x0200000A RID: 10
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class StorageQoS
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000026DE File Offset: 0x000008DE
		public static bool IsJsonDefault(StorageQoS val)
		{
			return StorageQoS._default.JsonEquals(val);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000026EC File Offset: 0x000008EC
		public bool JsonEquals(object obj)
		{
			StorageQoS graph = obj as StorageQoS;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(StorageQoS), new DataContractJsonSerializerSettings
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

		// Token: 0x04000033 RID: 51
		private static readonly StorageQoS _default = new StorageQoS();

		// Token: 0x04000034 RID: 52
		[DataMember]
		public ulong IopsMaximum;

		// Token: 0x04000035 RID: 53
		[DataMember]
		public ulong BandwidthMaximum;
	}
}
