using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x020000A0 RID: 160
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardInstance
	{
		// Token: 0x0600026B RID: 619 RVA: 0x00009120 File Offset: 0x00007320
		public static bool IsJsonDefault(ContainerCredentialGuardInstance val)
		{
			return ContainerCredentialGuardInstance._default.JsonEquals(val);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009130 File Offset: 0x00007330
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardInstance graph = obj as ContainerCredentialGuardInstance;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardInstance), new DataContractJsonSerializerSettings
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600026D RID: 621 RVA: 0x000091D8 File Offset: 0x000073D8
		// (set) Token: 0x0600026E RID: 622 RVA: 0x000091E0 File Offset: 0x000073E0
		[DataMember(Name = "CredentialGuard")]
		private ContainerCredentialGuardState _CredentialGuard
		{
			get
			{
				return this.CredentialGuard;
			}
			set
			{
				if (value != null)
				{
					this.CredentialGuard = value;
				}
			}
		}

		// Token: 0x04000346 RID: 838
		private static readonly ContainerCredentialGuardInstance _default = new ContainerCredentialGuardInstance();

		// Token: 0x04000347 RID: 839
		[DataMember]
		public string Id;

		// Token: 0x04000348 RID: 840
		public ContainerCredentialGuardState CredentialGuard = new ContainerCredentialGuardState();

		// Token: 0x04000349 RID: 841
		[DataMember(EmitDefaultValue = false)]
		public ContainerCredentialGuardHvSocketServiceConfig HvSocketConfig;
	}
}
