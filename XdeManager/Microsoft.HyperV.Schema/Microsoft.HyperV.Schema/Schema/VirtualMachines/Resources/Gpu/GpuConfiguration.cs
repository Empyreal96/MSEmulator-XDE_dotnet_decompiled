using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Gpu
{
	// Token: 0x02000044 RID: 68
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GpuConfiguration
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00005043 File Offset: 0x00003243
		public static bool IsJsonDefault(GpuConfiguration val)
		{
			return GpuConfiguration._default.JsonEquals(val);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005050 File Offset: 0x00003250
		public bool JsonEquals(object obj)
		{
			GpuConfiguration graph = obj as GpuConfiguration;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GpuConfiguration), new DataContractJsonSerializerSettings
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

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000050F8 File Offset: 0x000032F8
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005122 File Offset: 0x00003322
		[DataMember(EmitDefaultValue = false, Name = "AssignmentMode")]
		private string _AssignmentMode
		{
			get
			{
				if (this.AssignmentMode == GpuAssignmentMode.Disabled)
				{
					return null;
				}
				return this.AssignmentMode.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.AssignmentMode = GpuAssignmentMode.Disabled;
				}
				this.AssignmentMode = (GpuAssignmentMode)Enum.Parse(typeof(GpuAssignmentMode), value, true);
			}
		}

		// Token: 0x04000159 RID: 345
		private static readonly GpuConfiguration _default = new GpuConfiguration();

		// Token: 0x0400015A RID: 346
		public GpuAssignmentMode AssignmentMode;

		// Token: 0x0400015B RID: 347
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<string, ushort> AssignmentRequest;

		// Token: 0x0400015C RID: 348
		[DataMember(EmitDefaultValue = false)]
		public bool AllowVendorExtension;
	}
}
