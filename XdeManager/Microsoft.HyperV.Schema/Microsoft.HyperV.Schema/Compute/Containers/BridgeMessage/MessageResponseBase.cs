using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001BA RID: 442
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MessageResponseBase
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x00016958 File Offset: 0x00014B58
		public static bool IsJsonDefault(MessageResponseBase val)
		{
			return MessageResponseBase._default.JsonEquals(val);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00016968 File Offset: 0x00014B68
		public bool JsonEquals(object obj)
		{
			MessageResponseBase graph = obj as MessageResponseBase;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MessageResponseBase), new DataContractJsonSerializerSettings
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

		// Token: 0x040009CB RID: 2507
		private static readonly MessageResponseBase _default = new MessageResponseBase();

		// Token: 0x040009CC RID: 2508
		[DataMember]
		public long Result;

		// Token: 0x040009CD RID: 2509
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009CE RID: 2510
		[DataMember(EmitDefaultValue = false)]
		public ErrorRecord[] ErrorRecords;
	}
}
