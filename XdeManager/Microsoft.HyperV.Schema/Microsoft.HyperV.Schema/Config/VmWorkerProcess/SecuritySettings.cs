using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000E9 RID: 233
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SecuritySettings
	{
		// Token: 0x0600036F RID: 879 RVA: 0x0000C218 File Offset: 0x0000A418
		public static bool IsJsonDefault(SecuritySettings val)
		{
			return SecuritySettings._default.JsonEquals(val);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C228 File Offset: 0x0000A428
		public bool JsonEquals(object obj)
		{
			SecuritySettings graph = obj as SecuritySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SecuritySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x04000474 RID: 1140
		private static readonly SecuritySettings _default = new SecuritySettings();

		// Token: 0x04000475 RID: 1141
		[DataMember(EmitDefaultValue = false, Name = "shielding_requested")]
		public bool Shielding;

		// Token: 0x04000476 RID: 1142
		[DataMember(EmitDefaultValue = false, Name = "tpm_enabled")]
		public bool Tpm;

		// Token: 0x04000477 RID: 1143
		[DataMember(EmitDefaultValue = false, Name = "ksd_enabled")]
		public bool Ksd;

		// Token: 0x04000478 RID: 1144
		[DataMember(EmitDefaultValue = false, Name = "encrypt_state_migration")]
		public bool EncryptStateMigration;

		// Token: 0x04000479 RID: 1145
		[DataMember(EmitDefaultValue = false, Name = "vbs_opt_out")]
		public bool VbsOptOut;

		// Token: 0x0400047A RID: 1146
		[DataMember(EmitDefaultValue = false, Name = "data_protection_requested")]
		public bool DataProtection;
	}
}
