using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000AF RID: 175
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheSetRootCosRequest
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00009CC4 File Offset: 0x00007EC4
		public static bool IsJsonDefault(CacheSetRootCosRequest val)
		{
			return CacheSetRootCosRequest._default.JsonEquals(val);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00009CD4 File Offset: 0x00007ED4
		public bool JsonEquals(object obj)
		{
			CacheSetRootCosRequest graph = obj as CacheSetRootCosRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheSetRootCosRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x04000388 RID: 904
		private static readonly CacheSetRootCosRequest _default = new CacheSetRootCosRequest();

		// Token: 0x04000389 RID: 905
		[DataMember]
		public uint CosIndex;
	}
}
