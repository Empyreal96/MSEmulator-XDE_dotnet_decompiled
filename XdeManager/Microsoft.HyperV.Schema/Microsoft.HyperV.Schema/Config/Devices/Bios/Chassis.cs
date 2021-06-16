using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Bios
{
	// Token: 0x0200015F RID: 351
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Chassis
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x00011BAC File Offset: 0x0000FDAC
		public static bool IsJsonDefault(Chassis val)
		{
			return Chassis._default.JsonEquals(val);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00011BBC File Offset: 0x0000FDBC
		public bool JsonEquals(object obj)
		{
			Chassis graph = obj as Chassis;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Chassis), new DataContractJsonSerializerSettings
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

		// Token: 0x0400073C RID: 1852
		private static readonly Chassis _default = new Chassis();

		// Token: 0x0400073D RID: 1853
		[DataMember(Name = "serial_number")]
		public string SerialNumber;

		// Token: 0x0400073E RID: 1854
		[DataMember(Name = "asset_tag")]
		public string AssetTag;
	}
}
