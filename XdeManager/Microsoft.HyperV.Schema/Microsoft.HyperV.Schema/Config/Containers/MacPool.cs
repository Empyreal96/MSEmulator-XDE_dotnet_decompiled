using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200017A RID: 378
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MacPool
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x00013214 File Offset: 0x00011414
		public static bool IsJsonDefault(MacPool val)
		{
			return MacPool._default.JsonEquals(val);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00013224 File Offset: 0x00011424
		public bool JsonEquals(object obj)
		{
			MacPool graph = obj as MacPool;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MacPool), new DataContractJsonSerializerSettings
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

		// Token: 0x04000808 RID: 2056
		private static readonly MacPool _default = new MacPool();

		// Token: 0x04000809 RID: 2057
		[DataMember(EmitDefaultValue = false)]
		public string StartMacAddress;

		// Token: 0x0400080A RID: 2058
		[DataMember(EmitDefaultValue = false)]
		public string EndMacAddress;
	}
}
