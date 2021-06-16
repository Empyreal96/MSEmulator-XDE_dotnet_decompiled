using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000BA RID: 186
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CreateGroupOperation
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000A4D8 File Offset: 0x000086D8
		public static bool IsJsonDefault(CreateGroupOperation val)
		{
			return CreateGroupOperation._default.JsonEquals(val);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A4E8 File Offset: 0x000086E8
		public bool JsonEquals(object obj)
		{
			CreateGroupOperation graph = obj as CreateGroupOperation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CreateGroupOperation), new DataContractJsonSerializerSettings
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

		// Token: 0x040003AE RID: 942
		private static readonly CreateGroupOperation _default = new CreateGroupOperation();

		// Token: 0x040003AF RID: 943
		[DataMember]
		public Guid GroupId;

		// Token: 0x040003B0 RID: 944
		[DataMember]
		public uint LogicalProcessorCount;

		// Token: 0x040003B1 RID: 945
		[DataMember(EmitDefaultValue = false)]
		public uint[] LogicalProcessors;
	}
}
