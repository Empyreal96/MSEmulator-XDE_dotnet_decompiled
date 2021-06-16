using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000122 RID: 290
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbOutgoingMessage
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x0000EE5C File Offset: 0x0000D05C
		public static bool IsJsonDefault(VSmbOutgoingMessage val)
		{
			return VSmbOutgoingMessage._default.JsonEquals(val);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000EE6C File Offset: 0x0000D06C
		public bool JsonEquals(object obj)
		{
			VSmbOutgoingMessage graph = obj as VSmbOutgoingMessage;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbOutgoingMessage), new DataContractJsonSerializerSettings
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

		// Token: 0x040005C4 RID: 1476
		private static readonly VSmbOutgoingMessage _default = new VSmbOutgoingMessage();

		// Token: 0x040005C5 RID: 1477
		[DataMember]
		public byte[] DataBuffer;
	}
}
