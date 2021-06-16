using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x0200002D RID: 45
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class KernelIntegration
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00004200 File Offset: 0x00002400
		public static bool IsJsonDefault(KernelIntegration val)
		{
			return KernelIntegration._default.JsonEquals(val);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004210 File Offset: 0x00002410
		public bool JsonEquals(object obj)
		{
			KernelIntegration graph = obj as KernelIntegration;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(KernelIntegration), new DataContractJsonSerializerSettings
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

		// Token: 0x040000CC RID: 204
		private static readonly KernelIntegration _default = new KernelIntegration();
	}
}
