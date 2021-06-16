using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000E7 RID: 231
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class StorageSettings
	{
		// Token: 0x06000367 RID: 871 RVA: 0x0000C080 File Offset: 0x0000A280
		public static bool IsJsonDefault(StorageSettings val)
		{
			return StorageSettings._default.JsonEquals(val);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000C090 File Offset: 0x0000A290
		public bool JsonEquals(object obj)
		{
			StorageSettings graph = obj as StorageSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(StorageSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x04000469 RID: 1129
		private static readonly StorageSettings _default = new StorageSettings();

		// Token: 0x0400046A RID: 1130
		[DataMember]
		public ushort VPCPerChannel;

		// Token: 0x0400046B RID: 1131
		[DataMember]
		public ushort ThreadsPerChannel;

		// Token: 0x0400046C RID: 1132
		[DataMember]
		public bool DisableInterruptBatching;
	}
}
