using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.HvSocket;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x0200009F RID: 159
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardHvSocketServiceConfig
	{
		// Token: 0x06000267 RID: 615 RVA: 0x00009057 File Offset: 0x00007257
		public static bool IsJsonDefault(ContainerCredentialGuardHvSocketServiceConfig val)
		{
			return ContainerCredentialGuardHvSocketServiceConfig._default.JsonEquals(val);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009064 File Offset: 0x00007264
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardHvSocketServiceConfig graph = obj as ContainerCredentialGuardHvSocketServiceConfig;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardHvSocketServiceConfig), new DataContractJsonSerializerSettings
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

		// Token: 0x04000343 RID: 835
		private static readonly ContainerCredentialGuardHvSocketServiceConfig _default = new ContainerCredentialGuardHvSocketServiceConfig();

		// Token: 0x04000344 RID: 836
		[DataMember]
		public Guid ServiceId;

		// Token: 0x04000345 RID: 837
		[DataMember(EmitDefaultValue = false)]
		public HvSocketServiceConfig ServiceConfig;
	}
}
