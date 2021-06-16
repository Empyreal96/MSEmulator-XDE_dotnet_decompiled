using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.IC
{
	// Token: 0x02000152 RID: 338
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class KvpEntry
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x000114F2 File Offset: 0x0000F6F2
		public static bool IsJsonDefault(KvpEntry val)
		{
			return KvpEntry._default.JsonEquals(val);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00011500 File Offset: 0x0000F700
		public bool JsonEquals(object obj)
		{
			KvpEntry graph = obj as KvpEntry;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(KvpEntry), new DataContractJsonSerializerSettings
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

		// Token: 0x040006F5 RID: 1781
		private static readonly KvpEntry _default = new KvpEntry();

		// Token: 0x040006F6 RID: 1782
		[DataMember(Name = "key")]
		public string Key;

		// Token: 0x040006F7 RID: 1783
		[DataMember(Name = "value")]
		public string Value;
	}
}
