using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000121 RID: 289
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbIncomingMessage
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x0000ED90 File Offset: 0x0000CF90
		public static bool IsJsonDefault(VSmbIncomingMessage val)
		{
			return VSmbIncomingMessage._default.JsonEquals(val);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		public bool JsonEquals(object obj)
		{
			VSmbIncomingMessage graph = obj as VSmbIncomingMessage;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbIncomingMessage), new DataContractJsonSerializerSettings
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

		// Token: 0x040005C1 RID: 1473
		private static readonly VSmbIncomingMessage _default = new VSmbIncomingMessage();

		// Token: 0x040005C2 RID: 1474
		[DataMember]
		public byte[] SegmentLengthBuffer;

		// Token: 0x040005C3 RID: 1475
		[DataMember]
		public byte[] DataBuffer;
	}
}
