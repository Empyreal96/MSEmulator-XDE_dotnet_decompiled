using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Networking
{
	// Token: 0x02000148 RID: 328
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class EmulatedNicDevice
	{
		// Token: 0x0600052F RID: 1327 RVA: 0x00010E68 File Offset: 0x0000F068
		public static bool IsJsonDefault(EmulatedNicDevice val)
		{
			return EmulatedNicDevice._default.JsonEquals(val);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00010E78 File Offset: 0x0000F078
		public bool JsonEquals(object obj)
		{
			EmulatedNicDevice graph = obj as EmulatedNicDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(EmulatedNicDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x040006C5 RID: 1733
		private static readonly EmulatedNicDevice _default = new EmulatedNicDevice();

		// Token: 0x040006C6 RID: 1734
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x040006C7 RID: 1735
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x040006C8 RID: 1736
		[DataMember(Name = "ethernet_card")]
		public EmulatedNic[] EthernetCard;
	}
}
