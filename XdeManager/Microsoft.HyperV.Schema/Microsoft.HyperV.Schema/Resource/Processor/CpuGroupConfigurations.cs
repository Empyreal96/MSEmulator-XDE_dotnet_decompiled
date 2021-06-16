using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000B9 RID: 185
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CpuGroupConfigurations
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000A40C File Offset: 0x0000860C
		public static bool IsJsonDefault(CpuGroupConfigurations val)
		{
			return CpuGroupConfigurations._default.JsonEquals(val);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000A41C File Offset: 0x0000861C
		public bool JsonEquals(object obj)
		{
			CpuGroupConfigurations graph = obj as CpuGroupConfigurations;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CpuGroupConfigurations), new DataContractJsonSerializerSettings
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

		// Token: 0x040003AC RID: 940
		private static readonly CpuGroupConfigurations _default = new CpuGroupConfigurations();

		// Token: 0x040003AD RID: 941
		[DataMember(EmitDefaultValue = false)]
		public CpuGroupConfig[] CpuGroups;
	}
}
