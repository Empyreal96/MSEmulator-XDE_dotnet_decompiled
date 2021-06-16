using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.HvSocket;

namespace HCS.Config.Devices.IC
{
	// Token: 0x0200014F RID: 335
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HvSocketSystemWpConfig
	{
		// Token: 0x06000545 RID: 1349 RVA: 0x0001127E File Offset: 0x0000F47E
		public static bool IsJsonDefault(HvSocketSystemWpConfig val)
		{
			return HvSocketSystemWpConfig._default.JsonEquals(val);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001128C File Offset: 0x0000F48C
		public bool JsonEquals(object obj)
		{
			HvSocketSystemWpConfig graph = obj as HvSocketSystemWpConfig;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HvSocketSystemWpConfig), new DataContractJsonSerializerSettings
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

		// Token: 0x040006E5 RID: 1765
		private static readonly HvSocketSystemWpConfig _default = new HvSocketSystemWpConfig();

		// Token: 0x040006E6 RID: 1766
		[DataMember(EmitDefaultValue = false)]
		public string DefaultBindSecurityDescriptor;

		// Token: 0x040006E7 RID: 1767
		[DataMember(EmitDefaultValue = false)]
		public string DefaultConnectSecurityDescriptor;

		// Token: 0x040006E8 RID: 1768
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<Guid, HvSocketServiceConfig> ServiceTable;
	}
}
