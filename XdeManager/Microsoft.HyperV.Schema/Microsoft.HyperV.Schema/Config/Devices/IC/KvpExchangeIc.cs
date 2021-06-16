using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.IC
{
	// Token: 0x02000154 RID: 340
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class KvpExchangeIc
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x00011688 File Offset: 0x0000F888
		public static bool IsJsonDefault(KvpExchangeIc val)
		{
			return KvpExchangeIc._default.JsonEquals(val);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00011698 File Offset: 0x0000F898
		public bool JsonEquals(object obj)
		{
			KvpExchangeIc graph = obj as KvpExchangeIc;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(KvpExchangeIc), new DataContractJsonSerializerSettings
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

		// Token: 0x04000700 RID: 1792
		private static readonly KvpExchangeIc _default = new KvpExchangeIc();

		// Token: 0x04000701 RID: 1793
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000702 RID: 1794
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000703 RID: 1795
		[DataMember]
		public bool Enabled = true;

		// Token: 0x04000704 RID: 1796
		[DataMember(EmitDefaultValue = false)]
		public bool DisableHostKVPItems;

		// Token: 0x04000705 RID: 1797
		[DataMember(EmitDefaultValue = false, Name = "kvp")]
		public KvpEntry[] Entries;

		// Token: 0x04000706 RID: 1798
		[DataMember(EmitDefaultValue = false, Name = "guest_vNIC")]
		public Dictionary<byte, KvpIpSettings> IpSettings;
	}
}
