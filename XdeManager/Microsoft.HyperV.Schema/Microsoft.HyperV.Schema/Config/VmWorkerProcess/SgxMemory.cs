using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F1 RID: 241
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SgxMemory
	{
		// Token: 0x0600039D RID: 925 RVA: 0x0000C988 File Offset: 0x0000AB88
		public static bool IsJsonDefault(SgxMemory val)
		{
			return SgxMemory._default.JsonEquals(val);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000C998 File Offset: 0x0000AB98
		public bool JsonEquals(object obj)
		{
			SgxMemory graph = obj as SgxMemory;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SgxMemory), new DataContractJsonSerializerSettings
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

		// Token: 0x040004A3 RID: 1187
		private static readonly SgxMemory _default = new SgxMemory();

		// Token: 0x040004A4 RID: 1188
		[DataMember(Name = "enabled")]
		public bool Enabled;

		// Token: 0x040004A5 RID: 1189
		[DataMember(Name = "size")]
		public ulong SizeInMB;

		// Token: 0x040004A6 RID: 1190
		[DataMember(EmitDefaultValue = false, Name = "launch_control_mode")]
		public SgxLaunchControlMode LaunchControlMode;

		// Token: 0x040004A7 RID: 1191
		[DataMember(EmitDefaultValue = false, Name = "launch_control_default")]
		public string LaunchControlDefault;
	}
}
