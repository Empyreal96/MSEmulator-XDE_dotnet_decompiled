using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Serial
{
	// Token: 0x02000140 RID: 320
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SerialController
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x000108D4 File Offset: 0x0000EAD4
		public static bool IsJsonDefault(SerialController val)
		{
			return SerialController._default.JsonEquals(val);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000108E4 File Offset: 0x0000EAE4
		public bool JsonEquals(object obj)
		{
			SerialController graph = obj as SerialController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SerialController), new DataContractJsonSerializerSettings
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

		// Token: 0x04000686 RID: 1670
		private static readonly SerialController _default = new SerialController();

		// Token: 0x04000687 RID: 1671
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000688 RID: 1672
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000689 RID: 1673
		[DataMember(Name = "port")]
		public Port[] Ports;
	}
}
