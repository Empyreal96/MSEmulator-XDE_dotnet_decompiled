using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Vpci
{
	// Token: 0x0200010F RID: 271
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class FlexIoDevice
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x0000E501 File Offset: 0x0000C701
		public static bool IsJsonDefault(FlexIoDevice val)
		{
			return FlexIoDevice._default.JsonEquals(val);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000E510 File Offset: 0x0000C710
		public bool JsonEquals(object obj)
		{
			FlexIoDevice graph = obj as FlexIoDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FlexIoDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x0000E5E2 File Offset: 0x0000C7E2
		[DataMember(EmitDefaultValue = false, Name = "HostingModel")]
		private string _HostingModel
		{
			get
			{
				if (this.HostingModel == FlexIoDeviceHostingModel.Internal)
				{
					return null;
				}
				return this.HostingModel.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.HostingModel = FlexIoDeviceHostingModel.Internal;
				}
				this.HostingModel = (FlexIoDeviceHostingModel)Enum.Parse(typeof(FlexIoDeviceHostingModel), value, true);
			}
		}

		// Token: 0x04000559 RID: 1369
		private static readonly FlexIoDevice _default = new FlexIoDevice();

		// Token: 0x0400055A RID: 1370
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400055B RID: 1371
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400055C RID: 1372
		[DataMember(IsRequired = true)]
		public Guid EmulatorId;

		// Token: 0x0400055D RID: 1373
		[DataMember(EmitDefaultValue = false)]
		public string[] EmulatorConfiguration;

		// Token: 0x0400055E RID: 1374
		[DataMember(EmitDefaultValue = false)]
		public Guid InstanceGuid;

		// Token: 0x0400055F RID: 1375
		public FlexIoDeviceHostingModel HostingModel;
	}
}
