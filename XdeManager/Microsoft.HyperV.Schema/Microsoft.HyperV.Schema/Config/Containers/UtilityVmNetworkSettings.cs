using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200016C RID: 364
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class UtilityVmNetworkSettings
	{
		// Token: 0x060005A5 RID: 1445 RVA: 0x000123BC File Offset: 0x000105BC
		public static bool IsJsonDefault(UtilityVmNetworkSettings val)
		{
			return UtilityVmNetworkSettings._default.JsonEquals(val);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000123CC File Offset: 0x000105CC
		public bool JsonEquals(object obj)
		{
			UtilityVmNetworkSettings graph = obj as UtilityVmNetworkSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(UtilityVmNetworkSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x04000790 RID: 1936
		private static readonly UtilityVmNetworkSettings _default = new UtilityVmNetworkSettings();

		// Token: 0x04000791 RID: 1937
		[DataMember(EmitDefaultValue = false)]
		public string SwitchName;

		// Token: 0x04000792 RID: 1938
		[DataMember(EmitDefaultValue = false)]
		public string PortName;
	}
}
