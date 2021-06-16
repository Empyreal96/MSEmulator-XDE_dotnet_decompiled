using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.Service
{
	// Token: 0x02000067 RID: 103
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class BasicInformation
	{
		// Token: 0x0600019D RID: 413 RVA: 0x000069D8 File Offset: 0x00004BD8
		public static bool IsJsonDefault(BasicInformation val)
		{
			return BasicInformation._default.JsonEquals(val);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000069E8 File Offset: 0x00004BE8
		public bool JsonEquals(object obj)
		{
			BasicInformation graph = obj as BasicInformation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(BasicInformation), new DataContractJsonSerializerSettings
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

		// Token: 0x04000236 RID: 566
		private static readonly BasicInformation _default = new BasicInformation();

		// Token: 0x04000237 RID: 567
		[DataMember(EmitDefaultValue = false)]
		public Version[] SupportedSchemaVersions;
	}
}
