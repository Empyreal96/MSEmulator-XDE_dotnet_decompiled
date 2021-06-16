using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000196 RID: 406
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSSingleEndpointResponse
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x00014B98 File Offset: 0x00012D98
		public static bool IsJsonDefault(HNSSingleEndpointResponse val)
		{
			return HNSSingleEndpointResponse._default.JsonEquals(val);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00014BA8 File Offset: 0x00012DA8
		public bool JsonEquals(object obj)
		{
			HNSSingleEndpointResponse graph = obj as HNSSingleEndpointResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSSingleEndpointResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00014C50 File Offset: 0x00012E50
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00014C67 File Offset: 0x00012E67
		[DataMember(EmitDefaultValue = false, Name = "Output")]
		private HNSEndpoint _Output
		{
			get
			{
				if (!HNSEndpoint.IsJsonDefault(this.Output))
				{
					return this.Output;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Output = value;
				}
			}
		}

		// Token: 0x040008D1 RID: 2257
		private static readonly HNSSingleEndpointResponse _default = new HNSSingleEndpointResponse();

		// Token: 0x040008D2 RID: 2258
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008D3 RID: 2259
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008D4 RID: 2260
		[DataMember(EmitDefaultValue = false)]
		public uint ErrorCode;

		// Token: 0x040008D5 RID: 2261
		public HNSEndpoint Output = new HNSEndpoint();
	}
}
