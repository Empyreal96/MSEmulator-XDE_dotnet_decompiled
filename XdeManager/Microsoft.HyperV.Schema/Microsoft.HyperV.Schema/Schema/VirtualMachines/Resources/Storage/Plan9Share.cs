using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x0200003D RID: 61
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Plan9Share
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00004A64 File Offset: 0x00002C64
		public static bool IsJsonDefault(Plan9Share val)
		{
			return Plan9Share._default.JsonEquals(val);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004A74 File Offset: 0x00002C74
		public bool JsonEquals(object obj)
		{
			Plan9Share graph = obj as Plan9Share;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Plan9Share), new DataContractJsonSerializerSettings
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004B1C File Offset: 0x00002D1C
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004B2C File Offset: 0x00002D2C
		[DataMember(EmitDefaultValue = false, Name = "Flags")]
		private ulong _Flags
		{
			get
			{
				Plan9ShareFlags flags = this.Flags;
				return (ulong)((long)this.Flags);
			}
			set
			{
				this.Flags = (Plan9ShareFlags)value;
			}
		}

		// Token: 0x04000139 RID: 313
		private static readonly Plan9Share _default = new Plan9Share();

		// Token: 0x0400013A RID: 314
		[DataMember]
		public string Name;

		// Token: 0x0400013B RID: 315
		[DataMember(EmitDefaultValue = false)]
		public string AccessName;

		// Token: 0x0400013C RID: 316
		[DataMember]
		public string Path;

		// Token: 0x0400013D RID: 317
		[DataMember]
		public uint Port;

		// Token: 0x0400013E RID: 318
		public Plan9ShareFlags Flags;

		// Token: 0x0400013F RID: 319
		[DataMember(EmitDefaultValue = false)]
		public string[] AllowedFiles;
	}
}
