using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.VmHosted
{
	// Token: 0x02000184 RID: 388
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkAdapter
	{
		// Token: 0x06000631 RID: 1585 RVA: 0x00013CB0 File Offset: 0x00011EB0
		public static bool IsJsonDefault(NetworkAdapter val)
		{
			return NetworkAdapter._default.JsonEquals(val);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00013CC0 File Offset: 0x00011EC0
		public bool JsonEquals(object obj)
		{
			NetworkAdapter graph = obj as NetworkAdapter;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkAdapter), new DataContractJsonSerializerSettings
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

		// Token: 0x0400083B RID: 2107
		private static readonly NetworkAdapter _default = new NetworkAdapter();

		// Token: 0x0400083C RID: 2108
		[DataMember]
		public Guid AdapterInstanceId;

		// Token: 0x0400083D RID: 2109
		[DataMember]
		public bool FirewallEnabled;

		// Token: 0x0400083E RID: 2110
		[DataMember]
		public bool NatEnabled;

		// Token: 0x0400083F RID: 2111
		[DataMember(EmitDefaultValue = false)]
		public string MacAddress;

		// Token: 0x04000840 RID: 2112
		[DataMember(EmitDefaultValue = false)]
		public string AllocatedIpAddress;

		// Token: 0x04000841 RID: 2113
		[DataMember(EmitDefaultValue = false)]
		public string HostIpAddress;

		// Token: 0x04000842 RID: 2114
		[DataMember(EmitDefaultValue = false)]
		public byte HostIpPrefixLength;

		// Token: 0x04000843 RID: 2115
		[DataMember(EmitDefaultValue = false)]
		public string HostDnsServerList;

		// Token: 0x04000844 RID: 2116
		[DataMember(EmitDefaultValue = false)]
		public string HostDnsSuffix;

		// Token: 0x04000845 RID: 2117
		[DataMember(EmitDefaultValue = false)]
		public bool EnableLowMetric;

		// Token: 0x04000846 RID: 2118
		[DataMember(EmitDefaultValue = false)]
		public ushort EncapOverhead;

		// Token: 0x04000847 RID: 2119
		[DataMember(EmitDefaultValue = false)]
		public uint MediaType;

		// Token: 0x04000848 RID: 2120
		[DataMember(EmitDefaultValue = false)]
		public Guid AdapterGuid;

		// Token: 0x04000849 RID: 2121
		[DataMember(EmitDefaultValue = false)]
		public ulong AdapterLuid;

		// Token: 0x0400084A RID: 2122
		[DataMember(EmitDefaultValue = false)]
		public uint Index;

		// Token: 0x0400084B RID: 2123
		[DataMember(EmitDefaultValue = false)]
		public string Alias;

		// Token: 0x0400084C RID: 2124
		[DataMember(EmitDefaultValue = false)]
		public string Description;
	}
}
