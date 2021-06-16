using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000132 RID: 306
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMDevice
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x0000FC57 File Offset: 0x0000DE57
		public static bool IsJsonDefault(VPMEMDevice val)
		{
			return VPMEMDevice._default.JsonEquals(val);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000FC64 File Offset: 0x0000DE64
		public bool JsonEquals(object obj)
		{
			VPMEMDevice graph = obj as VPMEMDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000FD0C File Offset: 0x0000DF0C
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0000FD26 File Offset: 0x0000DF26
		[DataMember(Name = "image_format")]
		private string _ImageFormat
		{
			get
			{
				VPMEMImageFormat imageFormat = this.ImageFormat;
				return this.ImageFormat.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ImageFormat = VPMEMImageFormat.Default;
				}
				this.ImageFormat = (VPMEMImageFormat)Enum.Parse(typeof(VPMEMImageFormat), value, true);
			}
		}

		// Token: 0x0400063F RID: 1599
		private static readonly VPMEMDevice _default = new VPMEMDevice();

		// Token: 0x04000640 RID: 1600
		[DataMember(Name = "locator")]
		public string Locator;

		// Token: 0x04000641 RID: 1601
		[DataMember(Name = "readonly")]
		public bool ReadOnly;

		// Token: 0x04000642 RID: 1602
		public VPMEMImageFormat ImageFormat;

		// Token: 0x04000643 RID: 1603
		[DataMember(EmitDefaultValue = false, Name = "rfic")]
		public ushort RFIC;

		// Token: 0x04000644 RID: 1604
		[DataMember(Name = "size_bytes")]
		public ulong SizeBytes;

		// Token: 0x04000645 RID: 1605
		[DataMember(EmitDefaultValue = false, Name = "mappings")]
		public Dictionary<ulong, VPMEMMapping> Mappings;
	}
}
