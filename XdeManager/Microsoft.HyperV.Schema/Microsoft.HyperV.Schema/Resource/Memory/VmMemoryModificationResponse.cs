using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000CC RID: 204
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VmMemoryModificationResponse
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0000B173 File Offset: 0x00009373
		public static bool IsJsonDefault(VmMemoryModificationResponse val)
		{
			return VmMemoryModificationResponse._default.JsonEquals(val);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000B180 File Offset: 0x00009380
		public bool JsonEquals(object obj)
		{
			VmMemoryModificationResponse graph = obj as VmMemoryModificationResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VmMemoryModificationResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040003F5 RID: 1013
		private static readonly VmMemoryModificationResponse _default = new VmMemoryModificationResponse();

		// Token: 0x040003F6 RID: 1014
		[DataMember]
		public ulong AssignedPageCount;
	}
}
