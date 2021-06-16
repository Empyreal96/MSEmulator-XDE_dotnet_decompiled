using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.System
{
	// Token: 0x0200019E RID: 414
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SiloProperties
	{
		// Token: 0x060006A1 RID: 1697 RVA: 0x00014FEC File Offset: 0x000131EC
		public static bool IsJsonDefault(SiloProperties val)
		{
			return SiloProperties._default.JsonEquals(val);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00014FFC File Offset: 0x000131FC
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

		// Token: 0x0400091A RID: 2330
		private static readonly SiloProperties _default = new SiloProperties();

		// Token: 0x0400091B RID: 2331
		[DataMember]
		public bool Enabled;

		// Token: 0x0400091C RID: 2332
		[DataMember(EmitDefaultValue = false)]
		public string JobName;
	}
}
