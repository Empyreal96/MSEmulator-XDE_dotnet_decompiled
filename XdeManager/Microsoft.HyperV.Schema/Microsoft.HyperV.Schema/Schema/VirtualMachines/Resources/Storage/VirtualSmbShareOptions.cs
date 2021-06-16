using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000039 RID: 57
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualSmbShareOptions
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000046EC File Offset: 0x000028EC
		public static bool IsJsonDefault(VirtualSmbShareOptions val)
		{
			return VirtualSmbShareOptions._default.JsonEquals(val);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000046FC File Offset: 0x000028FC
		public bool JsonEquals(object obj)
		{
			VirtualSmbShareOptions graph = obj as VirtualSmbShareOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualSmbShareOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x04000114 RID: 276
		private static readonly VirtualSmbShareOptions _default = new VirtualSmbShareOptions();

		// Token: 0x04000115 RID: 277
		[DataMember(EmitDefaultValue = false)]
		public bool ReadOnly;

		// Token: 0x04000116 RID: 278
		[DataMember(EmitDefaultValue = false)]
		public bool ShareRead;

		// Token: 0x04000117 RID: 279
		[DataMember(EmitDefaultValue = false)]
		public bool CacheIo;

		// Token: 0x04000118 RID: 280
		[DataMember(EmitDefaultValue = false)]
		public bool NoOplocks;

		// Token: 0x04000119 RID: 281
		[DataMember(EmitDefaultValue = false)]
		public bool TakeBackupPrivilege;

		// Token: 0x0400011A RID: 282
		[DataMember(EmitDefaultValue = false)]
		public bool UseShareRootIdentity;

		// Token: 0x0400011B RID: 283
		[DataMember(EmitDefaultValue = false)]
		public bool NoDirectmap;

		// Token: 0x0400011C RID: 284
		[DataMember(EmitDefaultValue = false)]
		public bool NoLocks;

		// Token: 0x0400011D RID: 285
		[DataMember(EmitDefaultValue = false)]
		public bool NoDirnotify;

		// Token: 0x0400011E RID: 286
		[DataMember(EmitDefaultValue = false)]
		public bool Test;

		// Token: 0x0400011F RID: 287
		[DataMember(EmitDefaultValue = false)]
		public bool VmSharedMemory;

		// Token: 0x04000120 RID: 288
		[DataMember(EmitDefaultValue = false)]
		public bool RestrictFileAccess;

		// Token: 0x04000121 RID: 289
		[DataMember(EmitDefaultValue = false)]
		public bool ForceLevelIIOplocks;

		// Token: 0x04000122 RID: 290
		[DataMember(EmitDefaultValue = false)]
		public bool ReparseBaseLayer;

		// Token: 0x04000123 RID: 291
		[DataMember(EmitDefaultValue = false)]
		public bool PseudoOplocks;

		// Token: 0x04000124 RID: 292
		[DataMember(EmitDefaultValue = false)]
		public bool NonCacheIo;

		// Token: 0x04000125 RID: 293
		[DataMember(EmitDefaultValue = false)]
		public bool PseudoDirnotify;

		// Token: 0x04000126 RID: 294
		[DataMember(EmitDefaultValue = false)]
		public bool DisableIndexing;

		// Token: 0x04000127 RID: 295
		[DataMember(EmitDefaultValue = false)]
		public bool HideAlternateDataStreams;

		// Token: 0x04000128 RID: 296
		[DataMember(EmitDefaultValue = false)]
		public bool EnableFsctlFiltering;

		// Token: 0x04000129 RID: 297
		[DataMember(EmitDefaultValue = false)]
		public bool AllowNewCreates;

		// Token: 0x0400012A RID: 298
		[DataMember(EmitDefaultValue = false)]
		public bool SingleFileMapping;
	}
}
