using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000EC RID: 236
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DebugSettings
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		public static bool IsJsonDefault(DebugSettings val)
		{
			return DebugSettings._default.JsonEquals(val);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
		public bool JsonEquals(object obj)
		{
			DebugSettings graph = obj as DebugSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DebugSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x0400047F RID: 1151
		private static readonly DebugSettings _default = new DebugSettings();

		// Token: 0x04000480 RID: 1152
		[DataMember(EmitDefaultValue = false)]
		public int Enabled;
	}
}
