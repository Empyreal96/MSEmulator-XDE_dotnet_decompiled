using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x020000A2 RID: 162
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardAddInstanceRequest
	{
		// Token: 0x06000275 RID: 629 RVA: 0x000092D4 File Offset: 0x000074D4
		public static bool IsJsonDefault(ContainerCredentialGuardAddInstanceRequest val)
		{
			return ContainerCredentialGuardAddInstanceRequest._default.JsonEquals(val);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000092E4 File Offset: 0x000074E4
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardAddInstanceRequest graph = obj as ContainerCredentialGuardAddInstanceRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardAddInstanceRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000938C File Offset: 0x0000758C
		// (set) Token: 0x06000278 RID: 632 RVA: 0x000093A6 File Offset: 0x000075A6
		[DataMember(Name = "Transport")]
		private string _Transport
		{
			get
			{
				ContainerCredentialGuardTransport transport = this.Transport;
				return this.Transport.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Transport = ContainerCredentialGuardTransport.LRPC;
				}
				this.Transport = (ContainerCredentialGuardTransport)Enum.Parse(typeof(ContainerCredentialGuardTransport), value, true);
			}
		}

		// Token: 0x0400034C RID: 844
		private static readonly ContainerCredentialGuardAddInstanceRequest _default = new ContainerCredentialGuardAddInstanceRequest();

		// Token: 0x0400034D RID: 845
		[DataMember]
		public string Id;

		// Token: 0x0400034E RID: 846
		[DataMember]
		public string CredentialSpec;

		// Token: 0x0400034F RID: 847
		public ContainerCredentialGuardTransport Transport;
	}
}
