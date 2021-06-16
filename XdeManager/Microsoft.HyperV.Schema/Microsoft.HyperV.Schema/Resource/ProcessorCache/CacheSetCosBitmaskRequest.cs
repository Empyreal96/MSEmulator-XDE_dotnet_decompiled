using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000AE RID: 174
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheSetCosBitmaskRequest
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x00009BFB File Offset: 0x00007DFB
		public static bool IsJsonDefault(CacheSetCosBitmaskRequest val)
		{
			return CacheSetCosBitmaskRequest._default.JsonEquals(val);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00009C08 File Offset: 0x00007E08
		public bool JsonEquals(object obj)
		{
			CacheSetCosBitmaskRequest graph = obj as CacheSetCosBitmaskRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheSetCosBitmaskRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x04000385 RID: 901
		private static readonly CacheSetCosBitmaskRequest _default = new CacheSetCosBitmaskRequest();

		// Token: 0x04000386 RID: 902
		[DataMember]
		public uint CosIndex;

		// Token: 0x04000387 RID: 903
		[DataMember]
		public uint CosBitmap;
	}
}
