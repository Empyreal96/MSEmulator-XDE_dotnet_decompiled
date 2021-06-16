using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000175 RID: 373
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NatSettings
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x00012D93 File Offset: 0x00010F93
		public static bool IsJsonDefault(NatSettings val)
		{
			return NatSettings._default.JsonEquals(val);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00012DA0 File Offset: 0x00010FA0
		public bool JsonEquals(object obj)
		{
			NatSettings graph = obj as NatSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NatSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040007F4 RID: 2036
		private static readonly NatSettings _default = new NatSettings();

		// Token: 0x040007F5 RID: 2037
		[DataMember(EmitDefaultValue = false)]
		public string Name;

		// Token: 0x040007F6 RID: 2038
		[DataMember(EmitDefaultValue = false)]
		public NatPortBinding[] PortBindings;
	}
}
