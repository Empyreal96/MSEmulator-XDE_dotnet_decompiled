using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x0200005E RID: 94
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SiloProperties
	{
		// Token: 0x06000173 RID: 371 RVA: 0x000061D8 File Offset: 0x000043D8
		public static bool IsJsonDefault(SiloProperties val)
		{
			return SiloProperties._default.JsonEquals(val);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000061E8 File Offset: 0x000043E8
		public bool JsonEquals(object obj)
		{
			SiloProperties graph = obj as SiloProperties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SiloProperties), new DataContractJsonSerializerSettings
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

		// Token: 0x040001E7 RID: 487
		private static readonly SiloProperties _default = new SiloProperties();

		// Token: 0x040001E8 RID: 488
		[DataMember]
		public bool Enabled;

		// Token: 0x040001E9 RID: 489
		[DataMember(EmitDefaultValue = false)]
		public string JobName;
	}
}
