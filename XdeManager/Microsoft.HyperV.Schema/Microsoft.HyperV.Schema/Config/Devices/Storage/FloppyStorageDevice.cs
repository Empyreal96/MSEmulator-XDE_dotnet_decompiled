using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000130 RID: 304
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class FloppyStorageDevice
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x0000FA78 File Offset: 0x0000DC78
		public static bool IsJsonDefault(FloppyStorageDevice val)
		{
			return FloppyStorageDevice._default.JsonEquals(val);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public bool JsonEquals(object obj)
		{
			FloppyStorageDevice graph = obj as FloppyStorageDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FloppyStorageDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x04000638 RID: 1592
		private static readonly FloppyStorageDevice _default = new FloppyStorageDevice();

		// Token: 0x04000639 RID: 1593
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400063A RID: 1594
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400063B RID: 1595
		[DataMember(Name = "controller")]
		public FloppyController[] Controllers;
	}
}
