using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Chipset
{
	// Token: 0x02000158 RID: 344
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestEmulationDevice
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x00011A16 File Offset: 0x0000FC16
		public static bool IsJsonDefault(GuestEmulationDevice val)
		{
			return GuestEmulationDevice._default.JsonEquals(val);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00011A24 File Offset: 0x0000FC24
		public bool JsonEquals(object obj)
		{
			GuestEmulationDevice graph = obj as GuestEmulationDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestEmulationDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x04000721 RID: 1825
		private static readonly GuestEmulationDevice _default = new GuestEmulationDevice();

		// Token: 0x04000722 RID: 1826
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000723 RID: 1827
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;
	}
}
