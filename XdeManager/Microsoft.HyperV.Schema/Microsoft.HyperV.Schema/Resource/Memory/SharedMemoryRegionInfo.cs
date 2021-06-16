using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000C6 RID: 198
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SharedMemoryRegionInfo
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000AC48 File Offset: 0x00008E48
		public static bool IsJsonDefault(SharedMemoryRegionInfo val)
		{
			return SharedMemoryRegionInfo._default.JsonEquals(val);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000AC58 File Offset: 0x00008E58
		public bool JsonEquals(object obj)
		{
			SharedMemoryRegionInfo graph = obj as SharedMemoryRegionInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SharedMemoryRegionInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x040003DC RID: 988
		private static readonly SharedMemoryRegionInfo _default = new SharedMemoryRegionInfo();

		// Token: 0x040003DD RID: 989
		[DataMember]
		public string SectionName;

		// Token: 0x040003DE RID: 990
		[DataMember]
		public ulong GuestPhysicalAddress;
	}
}
