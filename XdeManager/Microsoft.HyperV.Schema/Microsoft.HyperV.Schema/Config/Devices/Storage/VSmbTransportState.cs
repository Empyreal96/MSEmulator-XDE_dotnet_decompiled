using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000123 RID: 291
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbTransportState
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x0000EF28 File Offset: 0x0000D128
		public static bool IsJsonDefault(VSmbTransportState val)
		{
			return VSmbTransportState._default.JsonEquals(val);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000EF38 File Offset: 0x0000D138
		public bool JsonEquals(object obj)
		{
			VSmbTransportState graph = obj as VSmbTransportState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbTransportState), new DataContractJsonSerializerSettings
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0000EFE0 File Offset: 0x0000D1E0
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
		[DataMember(Name = "PartialResponse")]
		private VSmbOutgoingMessage _PartialResponse
		{
			get
			{
				return this.PartialResponse;
			}
			set
			{
				if (value != null)
				{
					this.PartialResponse = value;
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x0000EFFC File Offset: 0x0000D1FC
		[DataMember(Name = "PartialRequest")]
		private VSmbIncomingMessage _PartialRequest
		{
			get
			{
				return this.PartialRequest;
			}
			set
			{
				if (value != null)
				{
					this.PartialRequest = value;
				}
			}
		}

		// Token: 0x040005C6 RID: 1478
		private static readonly VSmbTransportState _default = new VSmbTransportState();

		// Token: 0x040005C7 RID: 1479
		public VSmbOutgoingMessage PartialResponse = new VSmbOutgoingMessage();

		// Token: 0x040005C8 RID: 1480
		[DataMember]
		public VSmbOutgoingMessage[] FullResponses;

		// Token: 0x040005C9 RID: 1481
		public VSmbIncomingMessage PartialRequest = new VSmbIncomingMessage();

		// Token: 0x040005CA RID: 1482
		[DataMember]
		public byte[] PipeInfoBuffer;
	}
}
