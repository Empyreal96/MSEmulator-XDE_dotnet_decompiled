using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200001B RID: 27
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ComPort
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000032C0 File Offset: 0x000014C0
		public static bool IsJsonDefault(ComPort val)
		{
			return ComPort._default.JsonEquals(val);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000032D0 File Offset: 0x000014D0
		public bool JsonEquals(object obj)
		{
			ComPort graph = obj as ComPort;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ComPort), new DataContractJsonSerializerSettings
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

		// Token: 0x04000091 RID: 145
		private static readonly ComPort _default = new ComPort();

		// Token: 0x04000092 RID: 146
		[DataMember(EmitDefaultValue = false)]
		public string NamedPipe;

		// Token: 0x04000093 RID: 147
		[DataMember(EmitDefaultValue = false)]
		public bool OptimizeForDebugger;
	}
}
