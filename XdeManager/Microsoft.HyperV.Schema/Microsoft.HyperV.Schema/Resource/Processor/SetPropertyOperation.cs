using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000BC RID: 188
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SetPropertyOperation
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0000A670 File Offset: 0x00008870
		public static bool IsJsonDefault(SetPropertyOperation val)
		{
			return SetPropertyOperation._default.JsonEquals(val);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000A680 File Offset: 0x00008880
		public bool JsonEquals(object obj)
		{
			SetPropertyOperation graph = obj as SetPropertyOperation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SetPropertyOperation), new DataContractJsonSerializerSettings
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

		// Token: 0x040003B4 RID: 948
		private static readonly SetPropertyOperation _default = new SetPropertyOperation();

		// Token: 0x040003B5 RID: 949
		[DataMember]
		public Guid GroupId;

		// Token: 0x040003B6 RID: 950
		[DataMember]
		public uint PropertyCode;

		// Token: 0x040003B7 RID: 951
		[DataMember]
		public ulong PropertyValue;
	}
}
