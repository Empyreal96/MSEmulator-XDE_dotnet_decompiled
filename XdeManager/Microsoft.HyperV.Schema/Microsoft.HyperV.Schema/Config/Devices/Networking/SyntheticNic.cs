using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Networking
{
	// Token: 0x02000146 RID: 326
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SyntheticNic
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x00010CD0 File Offset: 0x0000EED0
		public static bool IsJsonDefault(SyntheticNic val)
		{
			return SyntheticNic._default.JsonEquals(val);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		public bool JsonEquals(object obj)
		{
			SyntheticNic graph = obj as SyntheticNic;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SyntheticNic), new DataContractJsonSerializerSettings
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

		// Token: 0x040006AA RID: 1706
		private static readonly SyntheticNic _default = new SyntheticNic();

		// Token: 0x040006AB RID: 1707
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x040006AC RID: 1708
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x040006AD RID: 1709
		[DataMember]
		public string FriendlyName;

		// Token: 0x040006AE RID: 1710
		[DataMember]
		public bool IsConnected;

		// Token: 0x040006AF RID: 1711
		[DataMember]
		public bool? ClusterMonitored;

		// Token: 0x040006B0 RID: 1712
		[DataMember(EmitDefaultValue = false)]
		public bool DeviceNamingEnabled;

		// Token: 0x040006B1 RID: 1713
		[DataMember]
		public bool MacAddressIsStatic;

		// Token: 0x040006B2 RID: 1714
		[DataMember(EmitDefaultValue = false)]
		public string MacAddress;

		// Token: 0x040006B3 RID: 1715
		[DataMember]
		public string PortName;

		// Token: 0x040006B4 RID: 1716
		[DataMember]
		public string SwitchName;

		// Token: 0x040006B5 RID: 1717
		[DataMember]
		public Guid ChannelInstanceGuid;

		// Token: 0x040006B6 RID: 1718
		[DataMember]
		public Guid VpciInstanceGuid;

		// Token: 0x040006B7 RID: 1719
		[DataMember(EmitDefaultValue = false)]
		public bool AllowPacketDirect;

		// Token: 0x040006B8 RID: 1720
		[DataMember(EmitDefaultValue = false)]
		public Connection Connection;

		// Token: 0x040006B9 RID: 1721
		[DataMember(EmitDefaultValue = false)]
		public bool InterruptModerationDisabled;

		// Token: 0x040006BA RID: 1722
		[DataMember(EmitDefaultValue = false)]
		public uint MediaType;

		// Token: 0x040006BB RID: 1723
		[DataMember(EmitDefaultValue = false)]
		public bool NumaAwarePlacement;
	}
}
