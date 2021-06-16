using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000120 RID: 288
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbRequest
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x0000ECC4 File Offset: 0x0000CEC4
		public static bool IsJsonDefault(VSmbRequest val)
		{
			return VSmbRequest._default.JsonEquals(val);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000ECD4 File Offset: 0x0000CED4
		public bool JsonEquals(object obj)
		{
			VSmbRequest graph = obj as VSmbRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x040005BE RID: 1470
		private static readonly VSmbRequest _default = new VSmbRequest();

		// Token: 0x040005BF RID: 1471
		[DataMember]
		public byte[] DataBuffer;

		// Token: 0x040005C0 RID: 1472
		[DataMember]
		public ulong AsyncId;
	}
}
