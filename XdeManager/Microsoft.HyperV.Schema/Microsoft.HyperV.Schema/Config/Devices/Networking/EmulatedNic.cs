using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Networking
{
	// Token: 0x02000147 RID: 327
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class EmulatedNic
	{
		// Token: 0x0600052B RID: 1323 RVA: 0x00010D9C File Offset: 0x0000EF9C
		public static bool IsJsonDefault(EmulatedNic val)
		{
			return EmulatedNic._default.JsonEquals(val);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00010DAC File Offset: 0x0000EFAC
		public bool JsonEquals(object obj)
		{
			EmulatedNic graph = obj as EmulatedNic;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(EmulatedNic), new DataContractJsonSerializerSettings
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

		// Token: 0x040006BC RID: 1724
		private static readonly EmulatedNic _default = new EmulatedNic();

		// Token: 0x040006BD RID: 1725
		[DataMember(Name = "is_attached")]
		public bool IsAttached;

		// Token: 0x040006BE RID: 1726
		[DataMember(Name = "mac_address")]
		public byte[] MacAddress;

		// Token: 0x040006BF RID: 1727
		[DataMember(Name = "mac_address_is_static")]
		public bool MacAddressIsStatic;

		// Token: 0x040006C0 RID: 1728
		[DataMember(Name = "cluster_monitored")]
		public bool ClusterMonitored;

		// Token: 0x040006C1 RID: 1729
		[DataMember(Name = "friendly_name")]
		public string FriendlyName;

		// Token: 0x040006C2 RID: 1730
		[DataMember(Name = "virtual_switch_name")]
		public string SwitchName;

		// Token: 0x040006C3 RID: 1731
		[DataMember(Name = "virtual_port_name")]
		public string PortName;

		// Token: 0x040006C4 RID: 1732
		[DataMember(EmitDefaultValue = false)]
		public Connection Connection;
	}
}
