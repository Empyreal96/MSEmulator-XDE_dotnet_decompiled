using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200017E RID: 382
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VlanPolicyData
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x0001369B File Offset: 0x0001189B
		public static bool IsJsonDefault(VlanPolicyData val)
		{
			return VlanPolicyData._default.JsonEquals(val);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000136A8 File Offset: 0x000118A8
		public bool JsonEquals(object obj)
		{
			VlanPolicyData graph = obj as VlanPolicyData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VlanPolicyData), new DataContractJsonSerializerSettings
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00013750 File Offset: 0x00011950
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x0001377A File Offset: 0x0001197A
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

		// Token: 0x0400081B RID: 2075
		private static readonly VlanPolicyData _default = new VlanPolicyData();

		// Token: 0x0400081C RID: 2076
		public PolicyType Type;

		// Token: 0x0400081D RID: 2077
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x0400081E RID: 2078
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;

		// Token: 0x0400081F RID: 2079
		[DataMember]
		public uint VLAN;
	}
}
