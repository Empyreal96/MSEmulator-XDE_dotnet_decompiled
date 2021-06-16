using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200001D RID: 29
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtioSerial
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00003458 File Offset: 0x00001658
		public static bool IsJsonDefault(VirtioSerial val)
		{
			return VirtioSerial._default.JsonEquals(val);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003468 File Offset: 0x00001668
		public bool JsonEquals(object obj)
		{
			VirtioSerial graph = obj as VirtioSerial;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtioSerial), new DataContractJsonSerializerSettings
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

		// Token: 0x04000097 RID: 151
		private static readonly VirtioSerial _default = new VirtioSerial();

		// Token: 0x04000098 RID: 152
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<uint, VirtioSerialPort> Ports;
	}
}
