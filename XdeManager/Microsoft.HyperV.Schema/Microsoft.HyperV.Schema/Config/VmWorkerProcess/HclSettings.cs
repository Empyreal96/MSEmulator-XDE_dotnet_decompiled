using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000FA RID: 250
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HclSettings
	{
		// Token: 0x060003CD RID: 973 RVA: 0x0000D16C File Offset: 0x0000B36C
		public static bool IsJsonDefault(HclSettings val)
		{
			return HclSettings._default.JsonEquals(val);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000D17C File Offset: 0x0000B37C
		public bool JsonEquals(object obj)
		{
			HclSettings graph = obj as HclSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HclSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040004DE RID: 1246
		private static readonly HclSettings _default = new HclSettings();

		// Token: 0x040004DF RID: 1247
		[DataMember(EmitDefaultValue = false, Name = "enabled")]
		public bool Enabled;

		// Token: 0x040004E0 RID: 1248
		[DataMember(EmitDefaultValue = false, Name = "debug_host")]
		public string DebugHost;

		// Token: 0x040004E1 RID: 1249
		[DataMember(EmitDefaultValue = false, Name = "debug_port")]
		public long DebugPort;

		// Token: 0x040004E2 RID: 1250
		[DataMember(EmitDefaultValue = false, Name = "scsi_device_id")]
		public Guid ScsiDeviceId;
	}
}
