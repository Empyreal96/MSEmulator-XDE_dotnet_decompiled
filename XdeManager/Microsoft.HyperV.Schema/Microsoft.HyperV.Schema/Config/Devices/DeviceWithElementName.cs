using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices
{
	// Token: 0x0200010A RID: 266
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DeviceWithElementName
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x0000E184 File Offset: 0x0000C384
		public static bool IsJsonDefault(DeviceWithElementName val)
		{
			return DeviceWithElementName._default.JsonEquals(val);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000E194 File Offset: 0x0000C394
		public bool JsonEquals(object obj)
		{
			DeviceWithElementName graph = obj as DeviceWithElementName;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DeviceWithElementName), new DataContractJsonSerializerSettings
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

		// Token: 0x04000545 RID: 1349
		private static readonly DeviceWithElementName _default = new DeviceWithElementName();

		// Token: 0x04000546 RID: 1350
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000547 RID: 1351
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000548 RID: 1352
		[DataMember(EmitDefaultValue = false)]
		public string ElementName;
	}
}
