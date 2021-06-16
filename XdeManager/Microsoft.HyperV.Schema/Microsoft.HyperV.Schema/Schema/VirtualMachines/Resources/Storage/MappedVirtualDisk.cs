using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x0200003F RID: 63
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedVirtualDisk
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00004C14 File Offset: 0x00002E14
		public static bool IsJsonDefault(MappedVirtualDisk val)
		{
			return MappedVirtualDisk._default.JsonEquals(val);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004C24 File Offset: 0x00002E24
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

		// Token: 0x04000142 RID: 322
		private static readonly MappedVirtualDisk _default = new MappedVirtualDisk();

		// Token: 0x04000143 RID: 323
		[DataMember(EmitDefaultValue = false)]
		public string ContainerPath;

		// Token: 0x04000144 RID: 324
		[DataMember(EmitDefaultValue = false)]
		public byte Lun;

		// Token: 0x04000145 RID: 325
		[DataMember(EmitDefaultValue = false)]
		public bool OverwriteIfExists;
	}
}
