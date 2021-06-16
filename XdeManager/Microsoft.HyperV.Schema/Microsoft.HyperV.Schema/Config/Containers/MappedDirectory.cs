using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Config.Containers
{
	// Token: 0x02000168 RID: 360
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedDirectory
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x00011FE4 File Offset: 0x000101E4
		public static bool IsJsonDefault(MappedDirectory val)
		{
			return MappedDirectory._default.JsonEquals(val);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00011FF4 File Offset: 0x000101F4
		public bool JsonEquals(object obj)
		{
			MappedDirectory graph = obj as MappedDirectory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MappedDirectory), new DataContractJsonSerializerSettings
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0001209C File Offset: 0x0001029C
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x000120C6 File Offset: 0x000102C6
		[DataMember(EmitDefaultValue = false, Name = "Cache")]
		private string _Cache
		{
			get
			{
				if (this.Cache == CacheMode.Unspecified)
				{
					return null;
				}
				return this.Cache.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Cache = CacheMode.Unspecified;
				}
				this.Cache = (CacheMode)Enum.Parse(typeof(CacheMode), value, true);
			}
		}

		// Token: 0x04000776 RID: 1910
		private static readonly MappedDirectory _default = new MappedDirectory();

		// Token: 0x04000777 RID: 1911
		[DataMember(IsRequired = true)]
		public string HostPath;

		// Token: 0x04000778 RID: 1912
		[DataMember(IsRequired = true)]
		public string ContainerPath;

		// Token: 0x04000779 RID: 1913
		[DataMember(EmitDefaultValue = false)]
		public uint Port;

		// Token: 0x0400077A RID: 1914
		[DataMember(EmitDefaultValue = false)]
		public bool ReadOnly;

		// Token: 0x0400077B RID: 1915
		[DataMember(EmitDefaultValue = false)]
		public bool CreateInUtilityVM;

		// Token: 0x0400077C RID: 1916
		[DataMember(EmitDefaultValue = false)]
		public ulong? IOPSMaximum;

		// Token: 0x0400077D RID: 1917
		[DataMember(EmitDefaultValue = false)]
		public ulong? BandwidthMaximum;

		// Token: 0x0400077E RID: 1918
		public CacheMode Cache;

		// Token: 0x0400077F RID: 1919
		[DataMember(EmitDefaultValue = false)]
		public bool LinuxMetadata;

		// Token: 0x04000780 RID: 1920
		[DataMember(EmitDefaultValue = false)]
		public bool CaseSensitive;
	}
}
