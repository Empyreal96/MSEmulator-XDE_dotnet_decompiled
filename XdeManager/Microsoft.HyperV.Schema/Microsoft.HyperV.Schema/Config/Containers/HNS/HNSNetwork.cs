using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x0200018D RID: 397
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSNetwork
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x0001439C File Offset: 0x0001259C
		public static bool IsJsonDefault(HNSNetwork val)
		{
			return HNSNetwork._default.JsonEquals(val);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000143AC File Offset: 0x000125AC
		public bool JsonEquals(object obj)
		{
			HNSNetwork graph = obj as HNSNetwork;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSNetwork), new DataContractJsonSerializerSettings
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00014454 File Offset: 0x00012654
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x0001446B File Offset: 0x0001266B
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00014477 File Offset: 0x00012677
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00014487 File Offset: 0x00012687
		[DataMember(EmitDefaultValue = false, Name = "Flags")]
		private ulong _Flags
		{
			get
			{
				NetworkFlags flags = this.Flags;
				return (ulong)((long)this.Flags);
			}
			set
			{
				this.Flags = (NetworkFlags)value;
			}
		}

		// Token: 0x04000889 RID: 2185
		private static readonly HNSNetwork _default = new HNSNetwork();

		// Token: 0x0400088A RID: 2186
		[DataMember(EmitDefaultValue = false)]
		public string ID;

		// Token: 0x0400088B RID: 2187
		[DataMember(EmitDefaultValue = false)]
		public string Name;

		// Token: 0x0400088C RID: 2188
		[DataMember(EmitDefaultValue = false)]
		public string Type;

		// Token: 0x0400088D RID: 2189
		[DataMember(EmitDefaultValue = false)]
		public string NetworkAdapterName;

		// Token: 0x0400088E RID: 2190
		[DataMember(EmitDefaultValue = false)]
		public string SourceMac;

		// Token: 0x0400088F RID: 2191
		[DataMember(EmitDefaultValue = false)]
		public Policy[] Policies;

		// Token: 0x04000890 RID: 2192
		[DataMember(EmitDefaultValue = false)]
		public MacPool[] MacPools;

		// Token: 0x04000891 RID: 2193
		[DataMember(EmitDefaultValue = false)]
		public Subnet[] Subnets;

		// Token: 0x04000892 RID: 2194
		[DataMember(EmitDefaultValue = false)]
		public string DNSSuffix;

		// Token: 0x04000893 RID: 2195
		[DataMember(EmitDefaultValue = false)]
		public string DNSServerList;

		// Token: 0x04000894 RID: 2196
		[DataMember(EmitDefaultValue = false)]
		public string DNSDomain;

		// Token: 0x04000895 RID: 2197
		[DataMember(EmitDefaultValue = false)]
		public ushort ExternalNicIndex;

		// Token: 0x04000896 RID: 2198
		[DataMember(EmitDefaultValue = false)]
		public string AllocationType;

		// Token: 0x04000897 RID: 2199
		[DataMember(EmitDefaultValue = false)]
		public bool IsolateSwitch;

		// Token: 0x04000898 RID: 2200
		[DataMember(EmitDefaultValue = false)]
		public uint CurrentEndpointCount;

		// Token: 0x04000899 RID: 2201
		[DataMember(EmitDefaultValue = false)]
		public HNSNetworkSwitchExtension[] Extensions;

		// Token: 0x0400089A RID: 2202
		[DataMember(EmitDefaultValue = false)]
		public uint State;

		// Token: 0x0400089B RID: 2203
		[DataMember(EmitDefaultValue = false)]
		public string Owner;

		// Token: 0x0400089C RID: 2204
		[DataMember(EmitDefaultValue = false)]
		public bool IPv6;

		// Token: 0x0400089D RID: 2205
		[DataMember(EmitDefaultValue = false)]
		public object AdditionalParams;

		// Token: 0x0400089E RID: 2206
		[DataMember(EmitDefaultValue = false)]
		public string ExternalInterfaceConstraint;

		// Token: 0x0400089F RID: 2207
		public HNSInterfaceConstraint InterfaceConstraint = new HNSInterfaceConstraint();

		// Token: 0x040008A0 RID: 2208
		public NetworkFlags Flags;
	}
}
