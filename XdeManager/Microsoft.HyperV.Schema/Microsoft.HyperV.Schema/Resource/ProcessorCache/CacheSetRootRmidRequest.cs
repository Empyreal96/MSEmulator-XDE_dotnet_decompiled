using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000B0 RID: 176
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheSetRootRmidRequest
	{
		// Token: 0x060002AF RID: 687 RVA: 0x00009D90 File Offset: 0x00007F90
		public static bool IsJsonDefault(CacheSetRootRmidRequest val)
		{
			return CacheSetRootRmidRequest._default.JsonEquals(val);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00009DA0 File Offset: 0x00007FA0
		public bool JsonEquals(object obj)
		{
			CacheSetRootRmidRequest graph = obj as CacheSetRootRmidRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheSetRootRmidRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x0400038A RID: 906
		private static readonly CacheSetRootRmidRequest _default = new CacheSetRootRmidRequest();

		// Token: 0x0400038B RID: 907
		[DataMember]
		public uint Rmid;
	}
}
