using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000133 RID: 307
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RemapVPMEMDevice
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x0000FD67 File Offset: 0x0000DF67
		public static bool IsJsonDefault(RemapVPMEMDevice val)
		{
			return RemapVPMEMDevice._default.JsonEquals(val);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000FD74 File Offset: 0x0000DF74
		public bool JsonEquals(object obj)
		{
			RemapVPMEMDevice graph = obj as RemapVPMEMDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RemapVPMEMDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000FE1C File Offset: 0x0000E01C
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0000FE24 File Offset: 0x0000E024
		[DataMember(Name = "Mapping")]
		private VPMEMMapping _Mapping
		{
			get
			{
				return this.Mapping;
			}
			set
			{
				if (value != null)
				{
					this.Mapping = value;
				}
			}
		}

		// Token: 0x04000646 RID: 1606
		private static readonly RemapVPMEMDevice _default = new RemapVPMEMDevice();

		// Token: 0x04000647 RID: 1607
		[DataMember]
		public uint DeviceHandle;

		// Token: 0x04000648 RID: 1608
		[DataMember]
		public ulong DeviceOffset;

		// Token: 0x04000649 RID: 1609
		public VPMEMMapping Mapping = new VPMEMMapping();
	}
}
