using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000AB RID: 171
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheAllocationSystemInfo
	{
		// Token: 0x06000297 RID: 663 RVA: 0x00009934 File Offset: 0x00007B34
		public static bool IsJsonDefault(CacheAllocationSystemInfo val)
		{
			return CacheAllocationSystemInfo._default.JsonEquals(val);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00009944 File Offset: 0x00007B44
		public bool JsonEquals(object obj)
		{
			CacheAllocationSystemInfo graph = obj as CacheAllocationSystemInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheAllocationSystemInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x04000373 RID: 883
		private static readonly CacheAllocationSystemInfo _default = new CacheAllocationSystemInfo();

		// Token: 0x04000374 RID: 884
		[DataMember]
		public bool L3CatSupported;

		// Token: 0x04000375 RID: 885
		[DataMember]
		public uint L3CosBitmaskWidth;

		// Token: 0x04000376 RID: 886
		[DataMember]
		public uint L3CosBitmaskHwReserved;

		// Token: 0x04000377 RID: 887
		[DataMember]
		public uint NumL3CosSlots;

		// Token: 0x04000378 RID: 888
		[DataMember]
		public uint[] L3CosSlots;

		// Token: 0x04000379 RID: 889
		[DataMember]
		public uint RootCos;
	}
}
