using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Options
{
	// Token: 0x0200008C RID: 140
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ExportLayerOptions
	{
		// Token: 0x0600021F RID: 543 RVA: 0x000082A9 File Offset: 0x000064A9
		public static bool IsJsonDefault(ExportLayerOptions val)
		{
			return ExportLayerOptions._default.JsonEquals(val);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000082B8 File Offset: 0x000064B8
		public bool JsonEquals(object obj)
		{
			ExportLayerOptions graph = obj as ExportLayerOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ExportLayerOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x040002F7 RID: 759
		private static readonly ExportLayerOptions _default = new ExportLayerOptions();

		// Token: 0x040002F8 RID: 760
		[DataMember(EmitDefaultValue = false)]
		public bool IsWritableLayer;
	}
}
