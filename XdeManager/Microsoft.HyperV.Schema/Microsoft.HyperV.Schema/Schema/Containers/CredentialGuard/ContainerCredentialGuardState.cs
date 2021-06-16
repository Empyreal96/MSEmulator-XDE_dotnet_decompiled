using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x0200009E RID: 158
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardState
	{
		// Token: 0x06000261 RID: 609 RVA: 0x00008F47 File Offset: 0x00007147
		public static bool IsJsonDefault(ContainerCredentialGuardState val)
		{
			return ContainerCredentialGuardState._default.JsonEquals(val);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00008F54 File Offset: 0x00007154
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardState graph = obj as ContainerCredentialGuardState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardState), new DataContractJsonSerializerSettings
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00008FFC File Offset: 0x000071FC
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00009016 File Offset: 0x00007216
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

		// Token: 0x0400033E RID: 830
		private static readonly ContainerCredentialGuardState _default = new ContainerCredentialGuardState();

		// Token: 0x0400033F RID: 831
		[DataMember]
		public byte[] Cookie;

		// Token: 0x04000340 RID: 832
		[DataMember]
		public string RpcEndpoint;

		// Token: 0x04000341 RID: 833
		public ContainerCredentialGuardTransport Transport;

		// Token: 0x04000342 RID: 834
		[DataMember]
		public string CredentialSpec;
	}
}
