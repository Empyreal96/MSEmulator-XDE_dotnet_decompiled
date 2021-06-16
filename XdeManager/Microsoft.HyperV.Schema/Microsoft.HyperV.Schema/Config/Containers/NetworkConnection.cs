using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000176 RID: 374
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkConnection
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00012E5C File Offset: 0x0001105C
		public static bool IsJsonDefault(NetworkConnection val)
		{
			return NetworkConnection._default.JsonEquals(val);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00012E6C File Offset: 0x0001106C
		public bool JsonEquals(object obj)
		{
			NetworkConnection graph = obj as NetworkConnection;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkConnection), new DataContractJsonSerializerSettings
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00012F14 File Offset: 0x00011114
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00012F2B File Offset: 0x0001112B
		[DataMember(EmitDefaultValue = false, Name = "Nat")]
		private NatSettings _Nat
		{
			get
			{
				if (!NatSettings.IsJsonDefault(this.Nat))
				{
					return this.Nat;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Nat = value;
				}
			}
		}

		// Token: 0x040007F7 RID: 2039
		private static readonly NetworkConnection _default = new NetworkConnection();

		// Token: 0x040007F8 RID: 2040
		[DataMember]
		public string NetworkName;

		// Token: 0x040007F9 RID: 2041
		[DataMember(EmitDefaultValue = false)]
		public string Ip4Address;

		// Token: 0x040007FA RID: 2042
		[DataMember]
		public bool EnableNat;

		// Token: 0x040007FB RID: 2043
		public NatSettings Nat = new NatSettings();

		// Token: 0x040007FC RID: 2044
		[DataMember]
		public ulong? MaximumOutgoingBandwidthInBytes;
	}
}
