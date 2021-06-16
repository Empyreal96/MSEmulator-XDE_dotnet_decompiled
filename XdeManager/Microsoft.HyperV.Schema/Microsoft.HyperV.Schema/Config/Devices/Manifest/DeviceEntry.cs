using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Manifest
{
	// Token: 0x0200014A RID: 330
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class DeviceEntry
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x00011000 File Offset: 0x0000F200
		public static bool IsJsonDefault(DeviceEntry val)
		{
			return DeviceEntry._default.JsonEquals(val);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00011010 File Offset: 0x0000F210
		public bool JsonEquals(object obj)
		{
			DeviceEntry graph = obj as DeviceEntry;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(DeviceEntry), new DataContractJsonSerializerSettings
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

		// Token: 0x040006CD RID: 1741
		private static readonly DeviceEntry _default = new DeviceEntry();

		// Token: 0x040006CE RID: 1742
		[DataMember(Name = "device")]
		public Guid DeviceClass;

		// Token: 0x040006CF RID: 1743
		[DataMember(Name = "instance")]
		public Guid DeviceInstance;

		// Token: 0x040006D0 RID: 1744
		[DataMember(Name = "name")]
		public string Name;

		// Token: 0x040006D1 RID: 1745
		[DataMember(Name = "flags")]
		public uint Flags;
	}
}
