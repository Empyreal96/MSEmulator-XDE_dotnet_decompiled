using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000EB RID: 235
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GlobalSecurity
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0000C3DE File Offset: 0x0000A5DE
		public static bool IsJsonDefault(GlobalSecurity val)
		{
			return GlobalSecurity._default.JsonEquals(val);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C3EC File Offset: 0x0000A5EC
		public bool JsonEquals(object obj)
		{
			GlobalSecurity graph = obj as GlobalSecurity;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GlobalSecurity), new DataContractJsonSerializerSettings
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

		// Token: 0x0400047D RID: 1149
		private static readonly GlobalSecurity _default = new GlobalSecurity();

		// Token: 0x0400047E RID: 1150
		[DataMember(EmitDefaultValue = false, Name = "sd")]
		public string SecurityDescriptor;
	}
}
