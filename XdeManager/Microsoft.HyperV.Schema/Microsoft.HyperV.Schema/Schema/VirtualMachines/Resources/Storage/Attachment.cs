using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000037 RID: 55
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Attachment
	{
		// Token: 0x060000CF RID: 207 RVA: 0x000044B8 File Offset: 0x000026B8
		public static bool IsJsonDefault(Attachment val)
		{
			return Attachment._default.JsonEquals(val);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000044C8 File Offset: 0x000026C8
		public bool JsonEquals(object obj)
		{
			Attachment graph = obj as Attachment;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Attachment), new DataContractJsonSerializerSettings
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004570 File Offset: 0x00002770
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x0000458A File Offset: 0x0000278A
		[DataMember(Name = "Type")]
		private string _Type
		{
			get
			{
				AttachmentType type = this.Type;
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = AttachmentType.VirtualDisk;
				}
				this.Type = (AttachmentType)Enum.Parse(typeof(AttachmentType), value, true);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000045B8 File Offset: 0x000027B8
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000045E2 File Offset: 0x000027E2
		[DataMember(EmitDefaultValue = false, Name = "CachingMode")]
		private string _CachingMode
		{
			get
			{
				if (this.CachingMode == CachingMode.Uncached)
				{
					return null;
				}
				return this.CachingMode.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.CachingMode = CachingMode.Uncached;
				}
				this.CachingMode = (CachingMode)Enum.Parse(typeof(CachingMode), value, true);
			}
		}

		// Token: 0x04000108 RID: 264
		private static readonly Attachment _default = new Attachment();

		// Token: 0x04000109 RID: 265
		public AttachmentType Type;

		// Token: 0x0400010A RID: 266
		[DataMember(EmitDefaultValue = false)]
		public string Path;

		// Token: 0x0400010B RID: 267
		[DataMember(EmitDefaultValue = false)]
		public bool IgnoreFlushes;

		// Token: 0x0400010C RID: 268
		public CachingMode CachingMode;

		// Token: 0x0400010D RID: 269
		[DataMember(EmitDefaultValue = false)]
		public bool NoWriteHardening;

		// Token: 0x0400010E RID: 270
		[DataMember(EmitDefaultValue = false)]
		public bool DisableExpansionOptimization;

		// Token: 0x0400010F RID: 271
		[DataMember(EmitDefaultValue = false)]
		public bool IgnoreRelativeLocator;

		// Token: 0x04000110 RID: 272
		[DataMember(EmitDefaultValue = false)]
		public bool CaptureIoAttributionContext;

		// Token: 0x04000111 RID: 273
		[DataMember(EmitDefaultValue = false)]
		public bool ReadOnly;
	}
}
