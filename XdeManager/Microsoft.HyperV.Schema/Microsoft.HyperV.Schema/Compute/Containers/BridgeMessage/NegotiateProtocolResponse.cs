using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001BB RID: 443
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NegotiateProtocolResponse
	{
		// Token: 0x0600072D RID: 1837 RVA: 0x00016A24 File Offset: 0x00014C24
		public static bool IsJsonDefault(NegotiateProtocolResponse val)
		{
			return NegotiateProtocolResponse._default.JsonEquals(val);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00016A34 File Offset: 0x00014C34
		public bool JsonEquals(object obj)
		{
			NegotiateProtocolResponse graph = obj as NegotiateProtocolResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NegotiateProtocolResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00016ADC File Offset: 0x00014CDC
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00016AF3 File Offset: 0x00014CF3
		[DataMember(EmitDefaultValue = false, Name = "Capabilities")]
		private GcsCapabilities _Capabilities
		{
			get
			{
				if (!GcsCapabilities.IsJsonDefault(this.Capabilities))
				{
					return this.Capabilities;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Capabilities = value;
				}
			}
		}

		// Token: 0x040009CF RID: 2511
		private static readonly NegotiateProtocolResponse _default = new NegotiateProtocolResponse();

		// Token: 0x040009D0 RID: 2512
		[DataMember]
		public long Result;

		// Token: 0x040009D1 RID: 2513
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009D2 RID: 2514
		[DataMember(EmitDefaultValue = false)]
		public ErrorRecord[] ErrorRecords;

		// Token: 0x040009D3 RID: 2515
		[DataMember(EmitDefaultValue = false)]
		public uint Version;

		// Token: 0x040009D4 RID: 2516
		public GcsCapabilities Capabilities = new GcsCapabilities();
	}
}
