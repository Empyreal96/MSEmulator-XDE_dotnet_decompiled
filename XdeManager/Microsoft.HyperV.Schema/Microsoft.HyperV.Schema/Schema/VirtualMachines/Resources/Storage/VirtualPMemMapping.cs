using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000040 RID: 64
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualPMemMapping
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00004CE0 File Offset: 0x00002EE0
		public static bool IsJsonDefault(VirtualPMemMapping val)
		{
			return VirtualPMemMapping._default.JsonEquals(val);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004CF0 File Offset: 0x00002EF0
		public bool JsonEquals(object obj)
		{
			VirtualPMemMapping graph = obj as VirtualPMemMapping;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualPMemMapping), new DataContractJsonSerializerSettings
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004D98 File Offset: 0x00002F98
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004DC2 File Offset: 0x00002FC2
		[DataMember(EmitDefaultValue = false, Name = "ImageFormat")]
		private string _ImageFormat
		{
			get
			{
				if (this.ImageFormat == VirtualPMemImageFormat.Vhdx)
				{
					return null;
				}
				return this.ImageFormat.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ImageFormat = VirtualPMemImageFormat.Vhdx;
				}
				this.ImageFormat = (VirtualPMemImageFormat)Enum.Parse(typeof(VirtualPMemImageFormat), value, true);
			}
		}

		// Token: 0x04000146 RID: 326
		private static readonly VirtualPMemMapping _default = new VirtualPMemMapping();

		// Token: 0x04000147 RID: 327
		[DataMember]
		public string HostPath;

		// Token: 0x04000148 RID: 328
		public VirtualPMemImageFormat ImageFormat;
	}
}
