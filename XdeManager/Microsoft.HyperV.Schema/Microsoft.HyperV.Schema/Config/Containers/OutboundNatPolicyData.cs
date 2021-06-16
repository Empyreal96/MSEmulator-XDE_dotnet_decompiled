using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000181 RID: 385
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class OutboundNatPolicyData
	{
		// Token: 0x06000623 RID: 1571 RVA: 0x000139FB File Offset: 0x00011BFB
		public static bool IsJsonDefault(OutboundNatPolicyData val)
		{
			return OutboundNatPolicyData._default.JsonEquals(val);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00013A08 File Offset: 0x00011C08
		public bool JsonEquals(object obj)
		{
			OutboundNatPolicyData graph = obj as OutboundNatPolicyData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(OutboundNatPolicyData), new DataContractJsonSerializerSettings
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00013AB0 File Offset: 0x00011CB0
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00013ADA File Offset: 0x00011CDA
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

		// Token: 0x0400082C RID: 2092
		private static readonly OutboundNatPolicyData _default = new OutboundNatPolicyData();

		// Token: 0x0400082D RID: 2093
		public PolicyType Type;

		// Token: 0x0400082E RID: 2094
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x0400082F RID: 2095
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;

		// Token: 0x04000830 RID: 2096
		[DataMember(EmitDefaultValue = false)]
		public string[] ExceptionList;

		// Token: 0x04000831 RID: 2097
		[DataMember(EmitDefaultValue = false)]
		public string VIP;
	}
}
