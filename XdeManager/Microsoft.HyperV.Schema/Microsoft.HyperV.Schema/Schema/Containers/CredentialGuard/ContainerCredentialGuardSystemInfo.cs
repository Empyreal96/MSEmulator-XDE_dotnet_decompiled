using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x020000A1 RID: 161
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardSystemInfo
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000920B File Offset: 0x0000740B
		public static bool IsJsonDefault(ContainerCredentialGuardSystemInfo val)
		{
			return ContainerCredentialGuardSystemInfo._default.JsonEquals(val);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009218 File Offset: 0x00007418
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardSystemInfo graph = obj as ContainerCredentialGuardSystemInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardSystemInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x0400034A RID: 842
		private static readonly ContainerCredentialGuardSystemInfo _default = new ContainerCredentialGuardSystemInfo();

		// Token: 0x0400034B RID: 843
		[DataMember]
		public ContainerCredentialGuardInstance[] Instances;
	}
}
