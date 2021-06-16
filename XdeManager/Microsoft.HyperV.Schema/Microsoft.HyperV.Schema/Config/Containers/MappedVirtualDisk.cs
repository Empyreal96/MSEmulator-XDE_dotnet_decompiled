using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;

namespace HCS.Config.Containers
{
	// Token: 0x0200016A RID: 362
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedVirtualDisk
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x000121D0 File Offset: 0x000103D0
		public static bool IsJsonDefault(MappedVirtualDisk val)
		{
			return MappedVirtualDisk._default.JsonEquals(val);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000121E0 File Offset: 0x000103E0
		public bool JsonEquals(object obj)
		{
			MappedVirtualDisk graph = obj as MappedVirtualDisk;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MappedVirtualDisk), new DataContractJsonSerializerSettings
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00012288 File Offset: 0x00010488
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x000122B2 File Offset: 0x000104B2
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

		// Token: 0x04000785 RID: 1925
		private static readonly MappedVirtualDisk _default = new MappedVirtualDisk();

		// Token: 0x04000786 RID: 1926
		[DataMember(EmitDefaultValue = false)]
		public string HostPath;

		// Token: 0x04000787 RID: 1927
		[DataMember(EmitDefaultValue = false)]
		public string ContainerPath;

		// Token: 0x04000788 RID: 1928
		[DataMember(EmitDefaultValue = false)]
		public byte Lun;

		// Token: 0x04000789 RID: 1929
		[DataMember(EmitDefaultValue = false)]
		public bool CreateInUtilityVM;

		// Token: 0x0400078A RID: 1930
		[DataMember(EmitDefaultValue = false)]
		public bool ReadOnly;

		// Token: 0x0400078B RID: 1931
		[DataMember(EmitDefaultValue = false)]
		public bool AttachOnly;

		// Token: 0x0400078C RID: 1932
		[DataMember(EmitDefaultValue = false)]
		public bool OverwriteIfExists;

		// Token: 0x0400078D RID: 1933
		public CacheMode Cache;
	}
}
