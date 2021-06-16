using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000173 RID: 371
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemorySettings
	{
		// Token: 0x060005D5 RID: 1493 RVA: 0x00012BB5 File Offset: 0x00010DB5
		public static bool IsJsonDefault(MemorySettings val)
		{
			return MemorySettings._default.JsonEquals(val);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00012BC4 File Offset: 0x00010DC4
		public bool JsonEquals(object obj)
		{
			MemorySettings graph = obj as MemorySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemorySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040007EE RID: 2030
		private static readonly MemorySettings _default = new MemorySettings();

		// Token: 0x040007EF RID: 2031
		[DataMember(EmitDefaultValue = false)]
		public long MemoryMaximumInMB;
	}
}
