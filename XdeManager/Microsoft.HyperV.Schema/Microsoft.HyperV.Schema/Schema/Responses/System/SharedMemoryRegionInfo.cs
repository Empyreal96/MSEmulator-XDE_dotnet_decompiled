using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x0200005C RID: 92
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SharedMemoryRegionInfo
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00006043 File Offset: 0x00004243
		public static bool IsJsonDefault(SharedMemoryRegionInfo val)
		{
			return SharedMemoryRegionInfo._default.JsonEquals(val);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006050 File Offset: 0x00004250
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

		// Token: 0x040001E0 RID: 480
		private static readonly SharedMemoryRegionInfo _default = new SharedMemoryRegionInfo();

		// Token: 0x040001E1 RID: 481
		[DataMember]
		public string SectionName;

		// Token: 0x040001E2 RID: 482
		[DataMember]
		public ulong GuestPhysicalAddress;
	}
}
