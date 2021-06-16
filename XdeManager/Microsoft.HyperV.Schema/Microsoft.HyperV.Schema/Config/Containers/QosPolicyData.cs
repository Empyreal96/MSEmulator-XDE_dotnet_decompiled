using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200017D RID: 381
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class QosPolicyData
	{
		// Token: 0x0600060B RID: 1547 RVA: 0x0001357B File Offset: 0x0001177B
		public static bool IsJsonDefault(QosPolicyData val)
		{
			return QosPolicyData._default.JsonEquals(val);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00013588 File Offset: 0x00011788
		public bool JsonEquals(object obj)
		{
			QosPolicyData graph = obj as QosPolicyData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(QosPolicyData), new DataContractJsonSerializerSettings
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00013630 File Offset: 0x00011830
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x0001365A File Offset: 0x0001185A
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

		// Token: 0x04000816 RID: 2070
		private static readonly QosPolicyData _default = new QosPolicyData();

		// Token: 0x04000817 RID: 2071
		public PolicyType Type;

		// Token: 0x04000818 RID: 2072
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x04000819 RID: 2073
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;

		// Token: 0x0400081A RID: 2074
		[DataMember(EmitDefaultValue = false)]
		public ulong MaximumOutgoingBandwidthInBytes;
	}
}
