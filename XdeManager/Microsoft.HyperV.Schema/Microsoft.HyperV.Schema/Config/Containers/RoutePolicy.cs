using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000180 RID: 384
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RoutePolicy
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x000138DB File Offset: 0x00011ADB
		public static bool IsJsonDefault(RoutePolicy val)
		{
			return RoutePolicy._default.JsonEquals(val);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000138E8 File Offset: 0x00011AE8
		public bool JsonEquals(object obj)
		{
			RoutePolicy graph = obj as RoutePolicy;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RoutePolicy), new DataContractJsonSerializerSettings
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00013990 File Offset: 0x00011B90
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x000139BA File Offset: 0x00011BBA
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

		// Token: 0x04000825 RID: 2085
		private static readonly RoutePolicy _default = new RoutePolicy();

		// Token: 0x04000826 RID: 2086
		public PolicyType Type;

		// Token: 0x04000827 RID: 2087
		[DataMember(EmitDefaultValue = false)]
		public Guid ExtensionID;

		// Token: 0x04000828 RID: 2088
		[DataMember(EmitDefaultValue = false)]
		public bool Enable;

		// Token: 0x04000829 RID: 2089
		[DataMember(IsRequired = true)]
		public string DestinationPrefix;

		// Token: 0x0400082A RID: 2090
		[DataMember(EmitDefaultValue = false)]
		public string NextHop;

		// Token: 0x0400082B RID: 2091
		[DataMember(IsRequired = true)]
		public bool EncapEnabled;
	}
}
