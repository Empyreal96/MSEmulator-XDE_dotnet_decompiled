using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000B1 RID: 177
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryBwAllocationSystemInfo
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x00009E5C File Offset: 0x0000805C
		public static bool IsJsonDefault(MemoryBwAllocationSystemInfo val)
		{
			return MemoryBwAllocationSystemInfo._default.JsonEquals(val);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00009E6C File Offset: 0x0000806C
		public bool JsonEquals(object obj)
		{
			MemoryBwAllocationSystemInfo graph = obj as MemoryBwAllocationSystemInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryBwAllocationSystemInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x0400038C RID: 908
		private static readonly MemoryBwAllocationSystemInfo _default = new MemoryBwAllocationSystemInfo();

		// Token: 0x0400038D RID: 909
		[DataMember]
		public bool MbaSupported;

		// Token: 0x0400038E RID: 910
		[DataMember]
		public bool MbaLinear;

		// Token: 0x0400038F RID: 911
		[DataMember]
		public uint MaxMbaCos;

		// Token: 0x04000390 RID: 912
		[DataMember]
		public uint MaxMbaThrottleValue;

		// Token: 0x04000391 RID: 913
		[DataMember]
		public uint[] MbaCosSlots;
	}
}
