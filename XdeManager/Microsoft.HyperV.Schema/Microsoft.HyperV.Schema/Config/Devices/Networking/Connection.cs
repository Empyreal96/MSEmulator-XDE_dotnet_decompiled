using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Networking
{
	// Token: 0x02000145 RID: 325
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Connection
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x00010C04 File Offset: 0x0000EE04
		public static bool IsJsonDefault(Connection val)
		{
			return Connection._default.JsonEquals(val);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00010C14 File Offset: 0x0000EE14
		public bool JsonEquals(object obj)
		{
			Connection graph = obj as Connection;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Connection), new DataContractJsonSerializerSettings
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

		// Token: 0x0400069C RID: 1692
		private static readonly Connection _default = new Connection();

		// Token: 0x0400069D RID: 1693
		[DataMember(EmitDefaultValue = false)]
		public string AltPortName;

		// Token: 0x0400069E RID: 1694
		[DataMember(EmitDefaultValue = false)]
		public string AltSwitchName;

		// Token: 0x0400069F RID: 1695
		[DataMember(EmitDefaultValue = false)]
		public string AuthorizationScope;

		// Token: 0x040006A0 RID: 1696
		[DataMember(EmitDefaultValue = false)]
		public ulong ChimneyOffloadWeight;

		// Token: 0x040006A1 RID: 1697
		[DataMember]
		public List<Guid> Features;

		// Token: 0x040006A2 RID: 1698
		[DataMember(Name = "Feature_")]
		public Dictionary<Guid, Feature> FeaturesMap;

		// Token: 0x040006A3 RID: 1699
		[DataMember(EmitDefaultValue = false)]
		public List<string> HostResources;

		// Token: 0x040006A4 RID: 1700
		[DataMember(EmitDefaultValue = false)]
		public bool PreventIPSpoofing;

		// Token: 0x040006A5 RID: 1701
		[DataMember(EmitDefaultValue = false)]
		public List<string> AllowedIPv4Addresses;

		// Token: 0x040006A6 RID: 1702
		[DataMember(EmitDefaultValue = false)]
		public List<string> AllowedIPv6Addresses;

		// Token: 0x040006A7 RID: 1703
		[DataMember]
		public string PoolId;

		// Token: 0x040006A8 RID: 1704
		[DataMember]
		public string TestReplicaPoolId;

		// Token: 0x040006A9 RID: 1705
		[DataMember]
		public string TestReplicaSwitchName;
	}
}
