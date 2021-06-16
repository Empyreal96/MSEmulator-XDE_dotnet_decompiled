using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200001C RID: 28
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtioSerialPort
	{
		// Token: 0x06000073 RID: 115 RVA: 0x0000338C File Offset: 0x0000158C
		public static bool IsJsonDefault(VirtioSerialPort val)
		{
			return VirtioSerialPort._default.JsonEquals(val);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000339C File Offset: 0x0000159C
		public bool JsonEquals(object obj)
		{
			VirtioSerialPort graph = obj as VirtioSerialPort;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtioSerialPort), new DataContractJsonSerializerSettings
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

		// Token: 0x04000094 RID: 148
		private static readonly VirtioSerialPort _default = new VirtioSerialPort();

		// Token: 0x04000095 RID: 149
		[DataMember(EmitDefaultValue = false)]
		public string NamedPipe;

		// Token: 0x04000096 RID: 150
		[DataMember(EmitDefaultValue = false)]
		public string Name;
	}
}
