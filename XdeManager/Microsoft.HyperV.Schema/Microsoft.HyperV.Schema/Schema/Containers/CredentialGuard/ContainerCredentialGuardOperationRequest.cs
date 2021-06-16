using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Containers.CredentialGuard
{
	// Token: 0x020000A4 RID: 164
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCredentialGuardOperationRequest
	{
		// Token: 0x0600027F RID: 639 RVA: 0x000094B0 File Offset: 0x000076B0
		public static bool IsJsonDefault(ContainerCredentialGuardOperationRequest val)
		{
			return ContainerCredentialGuardOperationRequest._default.JsonEquals(val);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000094C0 File Offset: 0x000076C0
		public bool JsonEquals(object obj)
		{
			ContainerCredentialGuardOperationRequest graph = obj as ContainerCredentialGuardOperationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCredentialGuardOperationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00009568 File Offset: 0x00007768
		// (set) Token: 0x06000282 RID: 642 RVA: 0x00009582 File Offset: 0x00007782
		[DataMember(Name = "Operation")]
		private string _Operation
		{
			get
			{
				ContainerCredentialGuardModifyOperation operation = this.Operation;
				return this.Operation.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Operation = ContainerCredentialGuardModifyOperation.AddInstance;
				}
				this.Operation = (ContainerCredentialGuardModifyOperation)Enum.Parse(typeof(ContainerCredentialGuardModifyOperation), value, true);
			}
		}

		// Token: 0x04000352 RID: 850
		private static readonly ContainerCredentialGuardOperationRequest _default = new ContainerCredentialGuardOperationRequest();

		// Token: 0x04000353 RID: 851
		public ContainerCredentialGuardModifyOperation Operation;

		// Token: 0x04000354 RID: 852
		[DataMember]
		public object OperationDetails;
	}
}
