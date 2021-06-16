using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices
{
	// Token: 0x02000109 RID: 265
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Device
	{
		// Token: 0x06000437 RID: 1079 RVA: 0x0000E0BA File Offset: 0x0000C2BA
		public static bool IsJsonDefault(Device val)
		{
			return Device._default.JsonEquals(val);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		public bool JsonEquals(object obj)
		{
			Device graph = obj as Device;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Device), new DataContractJsonSerializerSettings
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

		// Token: 0x04000542 RID: 1346
		private static readonly Device _default = new Device();

		// Token: 0x04000543 RID: 1347
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000544 RID: 1348
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;
	}
}
