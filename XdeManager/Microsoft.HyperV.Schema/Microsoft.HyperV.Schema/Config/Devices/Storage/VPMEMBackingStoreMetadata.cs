using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000139 RID: 313
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMBackingStoreMetadata
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0001029C File Offset: 0x0000E49C
		public static bool IsJsonDefault(VPMEMBackingStoreMetadata val)
		{
			return VPMEMBackingStoreMetadata._default.JsonEquals(val);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000102AC File Offset: 0x0000E4AC
		public bool JsonEquals(object obj)
		{
			VPMEMBackingStoreMetadata graph = obj as VPMEMBackingStoreMetadata;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMBackingStoreMetadata), new DataContractJsonSerializerSettings
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00010354 File Offset: 0x0000E554
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0001036E File Offset: 0x0000E56E
		[DataMember(Name = "Format")]
		private string _Format
		{
			get
			{
				VPMEMImageFormat format = this.Format;
				return this.Format.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Format = VPMEMImageFormat.Default;
				}
				this.Format = (VPMEMImageFormat)Enum.Parse(typeof(VPMEMImageFormat), value, true);
			}
		}

		// Token: 0x0400065F RID: 1631
		private static readonly VPMEMBackingStoreMetadata _default = new VPMEMBackingStoreMetadata();

		// Token: 0x04000660 RID: 1632
		public VPMEMImageFormat Format;

		// Token: 0x04000661 RID: 1633
		[DataMember]
		public ulong VirtualSize;

		// Token: 0x04000662 RID: 1634
		[DataMember]
		public VPMEMVHD2Metadata Vhd2Metadata;
	}
}
