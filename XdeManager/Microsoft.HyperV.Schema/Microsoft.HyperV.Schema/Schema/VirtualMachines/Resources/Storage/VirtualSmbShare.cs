using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x0200003B RID: 59
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualSmbShare
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00004884 File Offset: 0x00002A84
		public static bool IsJsonDefault(VirtualSmbShare val)
		{
			return VirtualSmbShare._default.JsonEquals(val);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004894 File Offset: 0x00002A94
		public bool JsonEquals(object obj)
		{
			VirtualSmbShare graph = obj as VirtualSmbShare;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualSmbShare), new DataContractJsonSerializerSettings
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000493C File Offset: 0x00002B3C
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000494C File Offset: 0x00002B4C
		[DataMember(EmitDefaultValue = false, Name = "Flags")]
		private ulong _Flags
		{
			get
			{
				VSmbShareFlags flags = this.Flags;
				return (ulong)((long)this.Flags);
			}
			set
			{
				this.Flags = (VSmbShareFlags)value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004956 File Offset: 0x00002B56
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000496D File Offset: 0x00002B6D
		[DataMember(EmitDefaultValue = false, Name = "Options")]
		private VirtualSmbShareOptions _Options
		{
			get
			{
				if (!VirtualSmbShareOptions.IsJsonDefault(this.Options))
				{
					return this.Options;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Options = value;
				}
			}
		}

		// Token: 0x0400012E RID: 302
		private static readonly VirtualSmbShare _default = new VirtualSmbShare();

		// Token: 0x0400012F RID: 303
		[DataMember]
		public string Name;

		// Token: 0x04000130 RID: 304
		[DataMember]
		public string Path;

		// Token: 0x04000131 RID: 305
		public VSmbShareFlags Flags;

		// Token: 0x04000132 RID: 306
		[DataMember(EmitDefaultValue = false)]
		public string[] AllowedFiles;

		// Token: 0x04000133 RID: 307
		[DataMember(EmitDefaultValue = false)]
		public uint[] AllowedFsctls;

		// Token: 0x04000134 RID: 308
		[DataMember(EmitDefaultValue = false)]
		public VirtualSmbAlternateDataStream[] AutoCreateAlternateDataStreams;

		// Token: 0x04000135 RID: 309
		public VirtualSmbShareOptions Options = new VirtualSmbShareOptions();
	}
}
