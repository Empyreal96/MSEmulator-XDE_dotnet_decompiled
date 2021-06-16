using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.HvSocket
{
	// Token: 0x02000090 RID: 144
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HvSocketSystemConfig
	{
		// Token: 0x06000233 RID: 563 RVA: 0x00008680 File Offset: 0x00006880
		public static bool IsJsonDefault(HvSocketSystemConfig val)
		{
			return HvSocketSystemConfig._default.JsonEquals(val);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00008690 File Offset: 0x00006890
		public bool JsonEquals(object obj)
		{
			HvSocketSystemConfig graph = obj as HvSocketSystemConfig;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HvSocketSystemConfig), new DataContractJsonSerializerSettings
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

		// Token: 0x04000305 RID: 773
		private static readonly HvSocketSystemConfig _default = new HvSocketSystemConfig();

		// Token: 0x04000306 RID: 774
		[DataMember(EmitDefaultValue = false)]
		public string DefaultBindSecurityDescriptor;

		// Token: 0x04000307 RID: 775
		[DataMember(EmitDefaultValue = false)]
		public string DefaultConnectSecurityDescriptor;

		// Token: 0x04000308 RID: 776
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<Guid, HvSocketServiceConfig> ServiceTable;
	}
}
