using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Storage
{
	// Token: 0x020000A8 RID: 168
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class StorageResource
	{
		// Token: 0x0600028D RID: 653 RVA: 0x00009758 File Offset: 0x00007958
		public static bool IsJsonDefault(StorageResource val)
		{
			return StorageResource._default.JsonEquals(val);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009768 File Offset: 0x00007968
		public bool JsonEquals(object obj)
		{
			StorageResource graph = obj as StorageResource;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(StorageResource), new DataContractJsonSerializerSettings
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00009810 File Offset: 0x00007A10
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000982A File Offset: 0x00007A2A
		[DataMember(Name = "CachingMode")]
		private string _CachingMode
		{
			get
			{
				DiskCachingMode cachingMode = this.CachingMode;
				return this.CachingMode.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.CachingMode = DiskCachingMode.Default;
				}
				this.CachingMode = (DiskCachingMode)Enum.Parse(typeof(DiskCachingMode), value, true);
			}
		}

		// Token: 0x04000360 RID: 864
		private static readonly StorageResource _default = new StorageResource();

		// Token: 0x04000361 RID: 865
		[DataMember]
		public bool IsDrive;

		// Token: 0x04000362 RID: 866
		[DataMember]
		public uint BusNumber;

		// Token: 0x04000363 RID: 867
		[DataMember]
		public uint DeviceNumber;

		// Token: 0x04000364 RID: 868
		[DataMember]
		public uint AttachmentType;

		// Token: 0x04000365 RID: 869
		[DataMember(EmitDefaultValue = false)]
		public string AttachmentPath;

		// Token: 0x04000366 RID: 870
		[DataMember(EmitDefaultValue = false)]
		public string ParserType;

		// Token: 0x04000367 RID: 871
		[DataMember(EmitDefaultValue = false)]
		public string PoolId;

		// Token: 0x04000368 RID: 872
		[DataMember]
		public bool PrSupported;

		// Token: 0x04000369 RID: 873
		[DataMember]
		public bool IgnoreFlushes;

		// Token: 0x0400036A RID: 874
		public DiskCachingMode CachingMode;
	}
}
