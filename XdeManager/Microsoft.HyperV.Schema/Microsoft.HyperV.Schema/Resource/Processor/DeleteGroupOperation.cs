using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000BB RID: 187
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DeleteGroupOperation
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x0000A5A4 File Offset: 0x000087A4
		public static bool IsJsonDefault(DeleteGroupOperation val)
		{
			return DeleteGroupOperation._default.JsonEquals(val);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000A5B4 File Offset: 0x000087B4
		public bool JsonEquals(object obj)
		{
			DeleteGroupOperation graph = obj as DeleteGroupOperation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DeleteGroupOperation), new DataContractJsonSerializerSettings
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

		// Token: 0x040003B2 RID: 946
		private static readonly DeleteGroupOperation _default = new DeleteGroupOperation();

		// Token: 0x040003B3 RID: 947
		[DataMember]
		public Guid GroupId;
	}
}
