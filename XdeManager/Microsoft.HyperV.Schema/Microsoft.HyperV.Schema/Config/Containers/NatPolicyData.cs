using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200017C RID: 380
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NatPolicyData
	{
		// Token: 0x06000603 RID: 1539 RVA: 0x00013403 File Offset: 0x00011603
		public static bool IsJsonDefault(NatPolicyData val)
		{
			return NatPolicyData._default.JsonEquals(val);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00013410 File Offset: 0x00011610
		public bool JsonEquals(object obj)
		{
			NatPolicyData graph = obj as NatPolicyData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NatPolicyData), new DataContractJsonSerializerSettings
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

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000134B8 File Offset: 0x000116B8
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x000134E2 File Offset: 0x000116E2
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00013510 File Offset: 0x00011710
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0001353A File Offset: 0x0001173A
		[DataMember(EmitDefaultValue = false, Name = "Protocol")]
		private string _Protocol
		{
			get
			{
				if (this.Protocol == NatPortProtocol.TCP)
				{
					return null;
				}
				return this.Protocol.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Protocol = NatPortProtocol.TCP;
				}
				this.Protocol = (NatPortProtocol)Enum.Parse(typeof(NatPortProtocol), value, true);
			}
		}

		// Token: 0x0400080F RID: 2063
		private static readonly NatPolicyData _default = new NatPolicyData();

		// Token: 0x04000810 RID: 2064
		public PolicyType Type;

		// Token: 0x04000811 RID: 2065
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x04000812 RID: 2066
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;

		// Token: 0x04000813 RID: 2067
		public NatPortProtocol Protocol;

		// Token: 0x04000814 RID: 2068
		[DataMember(EmitDefaultValue = false)]
		public ushort InternalPort;

		// Token: 0x04000815 RID: 2069
		[DataMember(EmitDefaultValue = false)]
		public ushort ExternalPort;
	}
}
