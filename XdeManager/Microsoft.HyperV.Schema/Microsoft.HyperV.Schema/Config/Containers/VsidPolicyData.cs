using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200017F RID: 383
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VsidPolicyData
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x000137BB File Offset: 0x000119BB
		public static bool IsJsonDefault(VsidPolicyData val)
		{
			return VsidPolicyData._default.JsonEquals(val);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000137C8 File Offset: 0x000119C8
		public bool JsonEquals(object obj)
		{
			VsidPolicyData graph = obj as VsidPolicyData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VsidPolicyData), new DataContractJsonSerializerSettings
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00013870 File Offset: 0x00011A70
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001389A File Offset: 0x00011A9A
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

		// Token: 0x04000820 RID: 2080
		private static readonly VsidPolicyData _default = new VsidPolicyData();

		// Token: 0x04000821 RID: 2081
		public PolicyType Type;

		// Token: 0x04000822 RID: 2082
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x04000823 RID: 2083
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;

		// Token: 0x04000824 RID: 2084
		[DataMember]
		public uint VSID;
	}
}
