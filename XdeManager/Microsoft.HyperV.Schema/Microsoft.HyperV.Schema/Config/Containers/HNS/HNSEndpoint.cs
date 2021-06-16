using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000192 RID: 402
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSEndpoint
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x000147E0 File Offset: 0x000129E0
		public static bool IsJsonDefault(HNSEndpoint val)
		{
			return HNSEndpoint._default.JsonEquals(val);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x000147F0 File Offset: 0x000129F0
		public bool JsonEquals(object obj)
		{
			HNSEndpoint graph = obj as HNSEndpoint;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSEndpoint), new DataContractJsonSerializerSettings
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00014898 File Offset: 0x00012A98
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x000148AF File Offset: 0x00012AAF
		[DataMember(EmitDefaultValue = false, Name = "Namespace")]
		private HNSNamespace _Namespace
		{
			get
			{
				if (!HNSNamespace.IsJsonDefault(this.Namespace))
				{
					return this.Namespace;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Namespace = value;
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x000148BB File Offset: 0x00012ABB
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x000148D2 File Offset: 0x00012AD2
		[DataMember(EmitDefaultValue = false, Name = "InterfaceConstraint")]
		private HNSInterfaceConstraint _InterfaceConstraint
		{
			get
			{
				if (!HNSInterfaceConstraint.IsJsonDefault(this.InterfaceConstraint))
				{
					return this.InterfaceConstraint;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.InterfaceConstraint = value;
				}
			}
		}

		// Token: 0x040008AC RID: 2220
		private static readonly HNSEndpoint _default = new HNSEndpoint();

		// Token: 0x040008AD RID: 2221
		[DataMember(EmitDefaultValue = false)]
		public Guid ID;

		// Token: 0x040008AE RID: 2222
		[DataMember(EmitDefaultValue = false)]
		public string Name;

		// Token: 0x040008AF RID: 2223
		[DataMember(EmitDefaultValue = false)]
		public Guid VirtualNetwork;

		// Token: 0x040008B0 RID: 2224
		[DataMember(EmitDefaultValue = false)]
		public string VirtualNetworkName;

		// Token: 0x040008B1 RID: 2225
		[DataMember(EmitDefaultValue = false)]
		public object[] Policies;

		// Token: 0x040008B2 RID: 2226
		[DataMember(EmitDefaultValue = false)]
		public string MacAddress;

		// Token: 0x040008B3 RID: 2227
		[DataMember(EmitDefaultValue = false)]
		public string IPAddress;

		// Token: 0x040008B4 RID: 2228
		[DataMember(EmitDefaultValue = false)]
		public bool IsRemoteEndpoint;

		// Token: 0x040008B5 RID: 2229
		[DataMember(EmitDefaultValue = false)]
		public bool EnableInternalDNS;

		// Token: 0x040008B6 RID: 2230
		[DataMember(EmitDefaultValue = false)]
		public bool DisableICC;

		// Token: 0x040008B7 RID: 2231
		[DataMember(EmitDefaultValue = false)]
		public string DNSServerList;

		// Token: 0x040008B8 RID: 2232
		[DataMember(EmitDefaultValue = false)]
		public string DNSSuffix;

		// Token: 0x040008B9 RID: 2233
		[DataMember(EmitDefaultValue = false)]
		public string DNSDomain;

		// Token: 0x040008BA RID: 2234
		[DataMember(EmitDefaultValue = false)]
		public string PortFriendlyName;

		// Token: 0x040008BB RID: 2235
		[DataMember(EmitDefaultValue = false)]
		public string GatewayAddress;

		// Token: 0x040008BC RID: 2236
		[DataMember(EmitDefaultValue = false)]
		public byte PrefixLength;

		// Token: 0x040008BD RID: 2237
		[DataMember(EmitDefaultValue = false)]
		public string IPv6Address;

		// Token: 0x040008BE RID: 2238
		[DataMember(EmitDefaultValue = false)]
		public byte IPv6PrefixLength;

		// Token: 0x040008BF RID: 2239
		[DataMember(EmitDefaultValue = false)]
		public string GatewayAddressV6;

		// Token: 0x040008C0 RID: 2240
		[DataMember(EmitDefaultValue = false)]
		public bool EnableLowMetric;

		// Token: 0x040008C1 RID: 2241
		public HNSNamespace Namespace = new HNSNamespace();

		// Token: 0x040008C2 RID: 2242
		[DataMember(EmitDefaultValue = false)]
		public ushort EncapOverhead;

		// Token: 0x040008C3 RID: 2243
		public HNSInterfaceConstraint InterfaceConstraint = new HNSInterfaceConstraint();
	}
}
