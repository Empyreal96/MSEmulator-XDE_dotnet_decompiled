using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000174 RID: 372
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NatPortBinding
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x00012C80 File Offset: 0x00010E80
		public static bool IsJsonDefault(NatPortBinding val)
		{
			return NatPortBinding._default.JsonEquals(val);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00012C90 File Offset: 0x00010E90
		public bool JsonEquals(object obj)
		{
			NatPortBinding graph = obj as NatPortBinding;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NatPortBinding), new DataContractJsonSerializerSettings
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00012D38 File Offset: 0x00010F38
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x00012D52 File Offset: 0x00010F52
		[DataMember(Name = "Protocol")]
		private string _Protocol
		{
			get
			{
				NatPortProtocol protocol = this.Protocol;
				return this.Protocol.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Protocol = NatPortProtocol.TCP;
				}
				this.Protocol = (NatPortProtocol)Enum.Parse(typeof(NatPortProtocol), value, true);
			}
		}

		// Token: 0x040007F0 RID: 2032
		private static readonly NatPortBinding _default = new NatPortBinding();

		// Token: 0x040007F1 RID: 2033
		public NatPortProtocol Protocol;

		// Token: 0x040007F2 RID: 2034
		[DataMember]
		public ushort InternalPort;

		// Token: 0x040007F3 RID: 2035
		[DataMember]
		public ushort ExternalPort;
	}
}
