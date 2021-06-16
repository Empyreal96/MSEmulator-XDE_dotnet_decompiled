using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001AE RID: 430
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MessageBase
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x00015EB0 File Offset: 0x000140B0
		public static bool IsJsonDefault(MessageBase val)
		{
			return MessageBase._default.JsonEquals(val);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00015EC0 File Offset: 0x000140C0
		public bool JsonEquals(object obj)
		{
			MessageBase graph = obj as MessageBase;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MessageBase), new DataContractJsonSerializerSettings
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

		// Token: 0x0400098D RID: 2445
		private static readonly MessageBase _default = new MessageBase();

		// Token: 0x0400098E RID: 2446
		[DataMember]
		public string ContainerId;

		// Token: 0x0400098F RID: 2447
		[DataMember]
		public Guid ActivityId;
	}
}
