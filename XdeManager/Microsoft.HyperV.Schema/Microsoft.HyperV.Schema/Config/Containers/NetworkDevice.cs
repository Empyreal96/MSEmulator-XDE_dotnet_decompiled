using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000178 RID: 376
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkDevice
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x00013020 File Offset: 0x00011220
		public static bool IsJsonDefault(NetworkDevice val)
		{
			return NetworkDevice._default.JsonEquals(val);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00013030 File Offset: 0x00011230
		public bool JsonEquals(object obj)
		{
			NetworkDevice graph = obj as NetworkDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x000130D8 File Offset: 0x000112D8
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x000130EF File Offset: 0x000112EF
		[DataMember(EmitDefaultValue = false, Name = "Connection")]
		private NetworkConnection _Connection
		{
			get
			{
				if (!NetworkConnection.IsJsonDefault(this.Connection))
				{
					return this.Connection;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Connection = value;
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000130FB File Offset: 0x000112FB
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x00013112 File Offset: 0x00011312
		[DataMember(EmitDefaultValue = false, Name = "Settings")]
		private NetworkSettings _Settings
		{
			get
			{
				if (!NetworkSettings.IsJsonDefault(this.Settings))
				{
					return this.Settings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Settings = value;
				}
			}
		}

		// Token: 0x040007FF RID: 2047
		private static readonly NetworkDevice _default = new NetworkDevice();

		// Token: 0x04000800 RID: 2048
		[DataMember]
		public string DeviceType;

		// Token: 0x04000801 RID: 2049
		public NetworkConnection Connection = new NetworkConnection();

		// Token: 0x04000802 RID: 2050
		public NetworkSettings Settings = new NetworkSettings();
	}
}
