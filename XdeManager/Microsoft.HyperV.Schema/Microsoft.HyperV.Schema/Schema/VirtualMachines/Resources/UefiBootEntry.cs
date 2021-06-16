using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000017 RID: 23
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class UefiBootEntry
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00002EF8 File Offset: 0x000010F8
		public static bool IsJsonDefault(UefiBootEntry val)
		{
			return UefiBootEntry._default.JsonEquals(val);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002F08 File Offset: 0x00001108
		public bool JsonEquals(object obj)
		{
			UefiBootEntry graph = obj as UefiBootEntry;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(UefiBootEntry), new DataContractJsonSerializerSettings
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002FB0 File Offset: 0x000011B0
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002FCA File Offset: 0x000011CA
		[DataMember(Name = "DeviceType")]
		private string _DeviceType
		{
			get
			{
				UefiBootDevice deviceType = this.DeviceType;
				return this.DeviceType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.DeviceType = UefiBootDevice.ScsiDrive;
				}
				this.DeviceType = (UefiBootDevice)Enum.Parse(typeof(UefiBootDevice), value, true);
			}
		}

		// Token: 0x04000079 RID: 121
		private static readonly UefiBootEntry _default = new UefiBootEntry();

		// Token: 0x0400007A RID: 122
		public UefiBootDevice DeviceType;

		// Token: 0x0400007B RID: 123
		[DataMember(EmitDefaultValue = false)]
		public string DevicePath;

		// Token: 0x0400007C RID: 124
		[DataMember(EmitDefaultValue = false)]
		public ushort DiskNumber;

		// Token: 0x0400007D RID: 125
		[DataMember(EmitDefaultValue = false)]
		public string OptionalData;

		// Token: 0x0400007E RID: 126
		[DataMember(EmitDefaultValue = false)]
		public string VmbFsRootPath;
	}
}
