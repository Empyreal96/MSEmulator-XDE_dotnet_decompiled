using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Serial
{
	// Token: 0x020000A9 RID: 169
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SerialModificationRequest
	{
		// Token: 0x06000293 RID: 659 RVA: 0x0000986B File Offset: 0x00007A6B
		public static bool IsJsonDefault(SerialModificationRequest val)
		{
			return SerialModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00009878 File Offset: 0x00007A78
		public bool JsonEquals(object obj)
		{
			SerialModificationRequest graph = obj as SerialModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SerialModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x0400036B RID: 875
		private static readonly SerialModificationRequest _default = new SerialModificationRequest();

		// Token: 0x0400036C RID: 876
		[DataMember]
		public byte PortNumber;

		// Token: 0x0400036D RID: 877
		[DataMember]
		public string Connection;
	}
}
