using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000E8 RID: 232
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GlobalDeviceSettings
	{
		// Token: 0x0600036B RID: 875 RVA: 0x0000C14C File Offset: 0x0000A34C
		public static bool IsJsonDefault(GlobalDeviceSettings val)
		{
			return GlobalDeviceSettings._default.JsonEquals(val);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C15C File Offset: 0x0000A35C
		public bool JsonEquals(object obj)
		{
			GlobalDeviceSettings graph = obj as GlobalDeviceSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GlobalDeviceSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x0400046D RID: 1133
		private static readonly GlobalDeviceSettings _default = new GlobalDeviceSettings();

		// Token: 0x0400046E RID: 1134
		[DataMember(Name = "generation_id")]
		public string GenerationId;

		// Token: 0x0400046F RID: 1135
		[DataMember(Name = "storage_allow_full_scsi_command_set")]
		public bool AllowFullScsiCmdSet;

		// Token: 0x04000470 RID: 1136
		[DataMember(Name = "lock_on_disconnect")]
		public bool ConsoleLockOnDisconnect;

		// Token: 0x04000471 RID: 1137
		[DataMember]
		public bool PreallocatedResources;

		// Token: 0x04000472 RID: 1138
		[DataMember(Name = "host_resource_protection_enabled")]
		public bool HostResourceProtectionEnabled;

		// Token: 0x04000473 RID: 1139
		[DataMember(EmitDefaultValue = false, Name = "cpu_architecture")]
		public uint? CpuArchitecture;
	}
}
