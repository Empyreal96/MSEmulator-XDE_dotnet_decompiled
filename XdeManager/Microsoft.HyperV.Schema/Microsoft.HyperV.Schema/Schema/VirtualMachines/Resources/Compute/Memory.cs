using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Compute
{
	// Token: 0x02000047 RID: 71
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Memory
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00005163 File Offset: 0x00003363
		public static bool IsJsonDefault(Memory val)
		{
			return Memory._default.JsonEquals(val);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005170 File Offset: 0x00003370
		public bool JsonEquals(object obj)
		{
			Memory graph = obj as Memory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Memory), new DataContractJsonSerializerSettings
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005218 File Offset: 0x00003418
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00005242 File Offset: 0x00003442
		[DataMember(EmitDefaultValue = false, Name = "Backing")]
		private string _Backing
		{
			get
			{
				if (this.Backing == MemoryBackingType.Physical)
				{
					return null;
				}
				return this.Backing.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Backing = MemoryBackingType.Physical;
				}
				this.Backing = (MemoryBackingType)Enum.Parse(typeof(MemoryBackingType), value, true);
			}
		}

		// Token: 0x04000163 RID: 355
		private static readonly Memory _default = new Memory();

		// Token: 0x04000164 RID: 356
		[DataMember]
		public ulong SizeInMB;

		// Token: 0x04000165 RID: 357
		public MemoryBackingType Backing;

		// Token: 0x04000166 RID: 358
		[DataMember(EmitDefaultValue = false)]
		public bool AllowOvercommit;

		// Token: 0x04000167 RID: 359
		[DataMember(EmitDefaultValue = false)]
		public MemoryBackingPageSize BackingPageSize;

		// Token: 0x04000168 RID: 360
		[DataMember(EmitDefaultValue = false)]
		public bool PinBackingPages;

		// Token: 0x04000169 RID: 361
		[DataMember(EmitDefaultValue = false)]
		public bool ForbidSmallBackingPages;

		// Token: 0x0400016A RID: 362
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePrivateCompressionStore;

		// Token: 0x0400016B RID: 363
		[DataMember(EmitDefaultValue = false)]
		public bool EnableHotHint;

		// Token: 0x0400016C RID: 364
		[DataMember(EmitDefaultValue = false)]
		public bool EnableColdHint;

		// Token: 0x0400016D RID: 365
		[DataMember(EmitDefaultValue = false)]
		public long SharedMemoryMB;

		// Token: 0x0400016E RID: 366
		[DataMember(EmitDefaultValue = false)]
		public bool DisableSharedMemoryMapping;

		// Token: 0x0400016F RID: 367
		[DataMember(EmitDefaultValue = false)]
		public string[] SharedMemoryAccessSids;

		// Token: 0x04000170 RID: 368
		[DataMember(EmitDefaultValue = false)]
		public bool EnableEpf;

		// Token: 0x04000171 RID: 369
		[DataMember(EmitDefaultValue = false)]
		public bool EnableDeferredCommit;
	}
}
