using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.HvSocket
{
	// Token: 0x0200008F RID: 143
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HvSocketServiceConfig
	{
		// Token: 0x0600022F RID: 559 RVA: 0x000085B7 File Offset: 0x000067B7
		public static bool IsJsonDefault(HvSocketServiceConfig val)
		{
			return HvSocketServiceConfig._default.JsonEquals(val);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000085C4 File Offset: 0x000067C4
		public bool JsonEquals(object obj)
		{
			HvSocketServiceConfig graph = obj as HvSocketServiceConfig;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HvSocketServiceConfig), new DataContractJsonSerializerSettings
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

		// Token: 0x04000300 RID: 768
		private static readonly HvSocketServiceConfig _default = new HvSocketServiceConfig();

		// Token: 0x04000301 RID: 769
		[DataMember(EmitDefaultValue = false)]
		public string BindSecurityDescriptor;

		// Token: 0x04000302 RID: 770
		[DataMember(EmitDefaultValue = false)]
		public string ConnectSecurityDescriptor;

		// Token: 0x04000303 RID: 771
		[DataMember(EmitDefaultValue = false)]
		public bool AllowWildcardBinds;

		// Token: 0x04000304 RID: 772
		[DataMember(EmitDefaultValue = false)]
		public bool Disabled;
	}
}
