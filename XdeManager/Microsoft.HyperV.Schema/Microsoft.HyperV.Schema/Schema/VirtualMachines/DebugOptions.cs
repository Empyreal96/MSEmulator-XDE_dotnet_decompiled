using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x02000010 RID: 16
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DebugOptions
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002BCB File Offset: 0x00000DCB
		public static bool IsJsonDefault(DebugOptions val)
		{
			return DebugOptions._default.JsonEquals(val);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public bool JsonEquals(object obj)
		{
			DebugOptions graph = obj as DebugOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DebugOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x04000059 RID: 89
		private static readonly DebugOptions _default = new DebugOptions();

		// Token: 0x0400005A RID: 90
		[DataMember(EmitDefaultValue = false)]
		public string BugcheckSavedStateFileName;
	}
}
