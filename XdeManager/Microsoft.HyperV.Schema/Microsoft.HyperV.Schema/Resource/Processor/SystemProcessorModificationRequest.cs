using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000BE RID: 190
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SystemProcessorModificationRequest
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000A84F File Offset: 0x00008A4F
		public static bool IsJsonDefault(SystemProcessorModificationRequest val)
		{
			return SystemProcessorModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000A85C File Offset: 0x00008A5C
		public bool JsonEquals(object obj)
		{
			SystemProcessorModificationRequest graph = obj as SystemProcessorModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SystemProcessorModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x040003BB RID: 955
		private static readonly SystemProcessorModificationRequest _default = new SystemProcessorModificationRequest();

		// Token: 0x040003BC RID: 956
		[DataMember]
		public Guid GroupId;
	}
}
