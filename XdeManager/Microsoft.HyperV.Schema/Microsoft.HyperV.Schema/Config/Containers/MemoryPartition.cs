using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000170 RID: 368
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryPartition
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x0001282B File Offset: 0x00010A2B
		public static bool IsJsonDefault(MemoryPartition val)
		{
			return MemoryPartition._default.JsonEquals(val);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00012838 File Offset: 0x00010A38
		public bool JsonEquals(object obj)
		{
			MemoryPartition graph = obj as MemoryPartition;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryPartition), new DataContractJsonSerializerSettings
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

		// Token: 0x040007BB RID: 1979
		private static readonly MemoryPartition _default = new MemoryPartition();

		// Token: 0x040007BC RID: 1980
		[DataMember(EmitDefaultValue = false)]
		public string ExistingPartitionName;

		// Token: 0x040007BD RID: 1981
		[DataMember(EmitDefaultValue = false)]
		public ulong SizeInMB;

		// Token: 0x040007BE RID: 1982
		[DataMember(EmitDefaultValue = false)]
		public ulong? ExtraPageFileSizeInMB;

		// Token: 0x040007BF RID: 1983
		[DataMember(EmitDefaultValue = false)]
		public string PageFilePath;
	}
}
