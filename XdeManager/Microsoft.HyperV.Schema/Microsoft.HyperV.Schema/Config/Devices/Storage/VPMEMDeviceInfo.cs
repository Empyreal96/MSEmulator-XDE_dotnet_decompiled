using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200013C RID: 316
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMDeviceInfo
	{
		// Token: 0x06000501 RID: 1281 RVA: 0x00010560 File Offset: 0x0000E760
		public static bool IsJsonDefault(VPMEMDeviceInfo val)
		{
			return VPMEMDeviceInfo._default.JsonEquals(val);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00010570 File Offset: 0x0000E770
		public bool JsonEquals(object obj)
		{
			VPMEMDeviceInfo graph = obj as VPMEMDeviceInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMDeviceInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x04000672 RID: 1650
		private static readonly VPMEMDeviceInfo _default = new VPMEMDeviceInfo();

		// Token: 0x04000673 RID: 1651
		[DataMember]
		public uint NFITHandle;

		// Token: 0x04000674 RID: 1652
		[DataMember]
		public string Locator;

		// Token: 0x04000675 RID: 1653
		[DataMember]
		public bool ReadOnly;

		// Token: 0x04000676 RID: 1654
		[DataMember]
		public ulong GPABase;

		// Token: 0x04000677 RID: 1655
		[DataMember]
		public ulong Size;

		// Token: 0x04000678 RID: 1656
		[DataMember]
		public uint HealthStatus;

		// Token: 0x04000679 RID: 1657
		[DataMember]
		public uint HealthInfo;

		// Token: 0x0400067A RID: 1658
		[DataMember]
		public bool GuestInjectedErrors;
	}
}
