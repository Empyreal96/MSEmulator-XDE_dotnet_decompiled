using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x020000A3 RID: 163
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardRemoveInstanceRequest
	{
		// Token: 0x0600027B RID: 635 RVA: 0x000093E7 File Offset: 0x000075E7
		public static bool IsJsonDefault(ContainerCredentialGuardRemoveInstanceRequest val)
		{
			return ContainerCredentialGuardRemoveInstanceRequest._default.JsonEquals(val);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000093F4 File Offset: 0x000075F4
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardRemoveInstanceRequest graph = obj as ContainerCredentialGuardRemoveInstanceRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardRemoveInstanceRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x04000350 RID: 848
		private static readonly ContainerCredentialGuardRemoveInstanceRequest _default = new ContainerCredentialGuardRemoveInstanceRequest();

		// Token: 0x04000351 RID: 849
		[DataMember]
		public string Id;
	}
}
