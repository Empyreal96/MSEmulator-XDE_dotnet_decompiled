using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Compute
{
	// Token: 0x02000048 RID: 72
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CpuGroup
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00005283 File Offset: 0x00003483
		public static bool IsJsonDefault(CpuGroup val)
		{
			return CpuGroup._default.JsonEquals(val);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005290 File Offset: 0x00003490
		public bool JsonEquals(object obj)
		{
			CpuGroup graph = obj as CpuGroup;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CpuGroup), new DataContractJsonSerializerSettings
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

		// Token: 0x04000172 RID: 370
		private static readonly CpuGroup _default = new CpuGroup();

		// Token: 0x04000173 RID: 371
		[DataMember]
		public Guid Id;
	}
}
