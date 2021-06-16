using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200017B RID: 379
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Policy
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x000132E0 File Offset: 0x000114E0
		public static bool IsJsonDefault(Policy val)
		{
			return Policy._default.JsonEquals(val);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000132F0 File Offset: 0x000114F0
		public bool JsonEquals(object obj)
		{
			Policy graph = obj as Policy;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Policy), new DataContractJsonSerializerSettings
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

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00013398 File Offset: 0x00011598
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x000133C2 File Offset: 0x000115C2
		[DataMember(EmitDefaultValue = false, Name = "Type")]
		private string _Type
		{
			get
			{
				if (this.Type == PolicyType.NAT)
				{
					return null;
				}
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = PolicyType.NAT;
				}
				this.Type = (PolicyType)Enum.Parse(typeof(PolicyType), value, true);
			}
		}

		// Token: 0x0400080B RID: 2059
		private static readonly Policy _default = new Policy();

		// Token: 0x0400080C RID: 2060
		public PolicyType Type;

		// Token: 0x0400080D RID: 2061
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x0400080E RID: 2062
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;
	}
}
