using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200001A RID: 26
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Chipset
	{
		// Token: 0x0600006B RID: 107 RVA: 0x000031F4 File Offset: 0x000013F4
		public static bool IsJsonDefault(Chipset val)
		{
			return Chipset._default.JsonEquals(val);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003204 File Offset: 0x00001404
		public bool JsonEquals(object obj)
		{
			Chipset graph = obj as Chipset;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Chipset), new DataContractJsonSerializerSettings
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

		// Token: 0x04000089 RID: 137
		private static readonly Chipset _default = new Chipset();

		// Token: 0x0400008A RID: 138
		[DataMember(EmitDefaultValue = false)]
		public Uefi Uefi;

		// Token: 0x0400008B RID: 139
		[DataMember(EmitDefaultValue = false)]
		public bool IsNumLockDisabled;

		// Token: 0x0400008C RID: 140
		[DataMember(EmitDefaultValue = false)]
		public string BaseBoardSerialNumber;

		// Token: 0x0400008D RID: 141
		[DataMember(EmitDefaultValue = false)]
		public string ChassisSerialNumber;

		// Token: 0x0400008E RID: 142
		[DataMember(EmitDefaultValue = false)]
		public string ChassisAssetTag;

		// Token: 0x0400008F RID: 143
		[DataMember(EmitDefaultValue = false)]
		public bool UseUtc;

		// Token: 0x04000090 RID: 144
		[DataMember(EmitDefaultValue = false)]
		public LinuxKernelDirect LinuxKernelDirect;
	}
}
