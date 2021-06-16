using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000131 RID: 305
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMMapping
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public static bool IsJsonDefault(VPMEMMapping val)
		{
			return VPMEMMapping._default.JsonEquals(val);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000FB54 File Offset: 0x0000DD54
		public bool JsonEquals(object obj)
		{
			VPMEMMapping graph = obj as VPMEMMapping;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMMapping), new DataContractJsonSerializerSettings
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

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x0000FC16 File Offset: 0x0000DE16
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

		// Token: 0x0400063C RID: 1596
		private static readonly VPMEMMapping _default = new VPMEMMapping();

		// Token: 0x0400063D RID: 1597
		[DataMember(Name = "locator")]
		public string Locator;

		// Token: 0x0400063E RID: 1598
		public VPMEMImageFormat ImageFormat;
	}
}
