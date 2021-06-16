using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000CF RID: 207
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryBalancingEnableOperation
	{
		// Token: 0x06000327 RID: 807 RVA: 0x0000B45F File Offset: 0x0000965F
		public static bool IsJsonDefault(MemoryBalancingEnableOperation val)
		{
			return MemoryBalancingEnableOperation._default.JsonEquals(val);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000B46C File Offset: 0x0000966C
		public bool JsonEquals(object obj)
		{
			MemoryBalancingEnableOperation graph = obj as MemoryBalancingEnableOperation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryBalancingEnableOperation), new DataContractJsonSerializerSettings
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

		// Token: 0x04000400 RID: 1024
		private static readonly MemoryBalancingEnableOperation _default = new MemoryBalancingEnableOperation();

		// Token: 0x04000401 RID: 1025
		[DataMember]
		public bool EnableMemoryBalancing;
	}
}
