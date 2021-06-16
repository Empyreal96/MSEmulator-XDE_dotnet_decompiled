using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F8 RID: 248
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NumaSettings
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0000CFD4 File Offset: 0x0000B1D4
		public static bool IsJsonDefault(NumaSettings val)
		{
			return NumaSettings._default.JsonEquals(val);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		public bool JsonEquals(object obj)
		{
			NumaSettings graph = obj as NumaSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NumaSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040004D4 RID: 1236
		private static readonly NumaSettings _default = new NumaSettings();

		// Token: 0x040004D5 RID: 1237
		[DataMember(Name = "enabled")]
		public bool Enabled;

		// Token: 0x040004D6 RID: 1238
		[DataMember(EmitDefaultValue = false)]
		public byte RequestedVNodeCount;

		// Token: 0x040004D7 RID: 1239
		[DataMember(EmitDefaultValue = false)]
		public NumaNodeTopology[] TopologyArray;
	}
}
