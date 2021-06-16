using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Vpci
{
	// Token: 0x02000110 RID: 272
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class AllocatedDeviceInfo
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x0000E623 File Offset: 0x0000C823
		public static bool IsJsonDefault(AllocatedDeviceInfo val)
		{
			return AllocatedDeviceInfo._default.JsonEquals(val);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000E630 File Offset: 0x0000C830
		public bool JsonEquals(object obj)
		{
			AllocatedDeviceInfo graph = obj as AllocatedDeviceInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(AllocatedDeviceInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x04000560 RID: 1376
		private static readonly AllocatedDeviceInfo _default = new AllocatedDeviceInfo();

		// Token: 0x04000561 RID: 1377
		[DataMember(IsRequired = true)]
		public string DevicePath;

		// Token: 0x04000562 RID: 1378
		[DataMember(IsRequired = true)]
		public long DeviceLuid;

		// Token: 0x04000563 RID: 1379
		[DataMember(IsRequired = true)]
		public long VfLuid;

		// Token: 0x04000564 RID: 1380
		[DataMember(IsRequired = true)]
		public Guid HwGuid;

		// Token: 0x04000565 RID: 1381
		[DataMember(IsRequired = true)]
		public byte[] ConfigBuffer;
	}
}
