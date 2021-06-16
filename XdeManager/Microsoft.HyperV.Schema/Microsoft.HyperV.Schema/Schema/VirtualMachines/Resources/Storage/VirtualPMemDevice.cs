using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000041 RID: 65
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualPMemDevice
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00004E03 File Offset: 0x00003003
		public static bool IsJsonDefault(VirtualPMemDevice val)
		{
			return VirtualPMemDevice._default.JsonEquals(val);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004E10 File Offset: 0x00003010
		public bool JsonEquals(object obj)
		{
			VirtualPMemDevice graph = obj as VirtualPMemDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualPMemDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004EB8 File Offset: 0x000030B8
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004EE2 File Offset: 0x000030E2
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

		// Token: 0x04000149 RID: 329
		private static readonly VirtualPMemDevice _default = new VirtualPMemDevice();

		// Token: 0x0400014A RID: 330
		[DataMember]
		public string HostPath;

		// Token: 0x0400014B RID: 331
		[DataMember(EmitDefaultValue = false)]
		public bool ReadOnly;

		// Token: 0x0400014C RID: 332
		public VirtualPMemImageFormat ImageFormat;

		// Token: 0x0400014D RID: 333
		[DataMember(EmitDefaultValue = false)]
		public ulong SizeBytes;

		// Token: 0x0400014E RID: 334
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<ulong, VirtualPMemMapping> Mappings;
	}
}
