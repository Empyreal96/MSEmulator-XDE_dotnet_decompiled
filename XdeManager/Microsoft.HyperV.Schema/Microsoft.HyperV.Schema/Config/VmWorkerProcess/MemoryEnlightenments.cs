using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F2 RID: 242
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryEnlightenments
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0000CA54 File Offset: 0x0000AC54
		public static bool IsJsonDefault(MemoryEnlightenments val)
		{
			return MemoryEnlightenments._default.JsonEquals(val);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000CA64 File Offset: 0x0000AC64
		public bool JsonEquals(object obj)
		{
			MemoryEnlightenments graph = obj as MemoryEnlightenments;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryEnlightenments), new DataContractJsonSerializerSettings
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

		// Token: 0x040004A8 RID: 1192
		private static readonly MemoryEnlightenments _default = new MemoryEnlightenments();

		// Token: 0x040004A9 RID: 1193
		[DataMember(EmitDefaultValue = false, Name = "hot_hint_enabled")]
		public bool HotHintEnabled;

		// Token: 0x040004AA RID: 1194
		[DataMember(EmitDefaultValue = false, Name = "cold_hint_enabled")]
		public bool ColdHintEnabled;

		// Token: 0x040004AB RID: 1195
		[DataMember(EmitDefaultValue = false, Name = "epf_enabled")]
		public bool EpfEnabled;
	}
}
