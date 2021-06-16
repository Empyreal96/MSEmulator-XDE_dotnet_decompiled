using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x02000194 RID: 404
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSSingleNetworkResponse
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x000149D4 File Offset: 0x00012BD4
		public static bool IsJsonDefault(HNSSingleNetworkResponse val)
		{
			return HNSSingleNetworkResponse._default.JsonEquals(val);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000149E4 File Offset: 0x00012BE4
		public bool JsonEquals(object obj)
		{
			HNSSingleNetworkResponse graph = obj as HNSSingleNetworkResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSSingleNetworkResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00014A8C File Offset: 0x00012C8C
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x00014AA3 File Offset: 0x00012CA3
		[DataMember(EmitDefaultValue = false, Name = "Output")]
		private HNSNetwork _Output
		{
			get
			{
				if (!HNSNetwork.IsJsonDefault(this.Output))
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

		// Token: 0x040008C8 RID: 2248
		private static readonly HNSSingleNetworkResponse _default = new HNSSingleNetworkResponse();

		// Token: 0x040008C9 RID: 2249
		[DataMember(IsRequired = true)]
		public bool Success;

		// Token: 0x040008CA RID: 2250
		[DataMember(EmitDefaultValue = false)]
		public string Error;

		// Token: 0x040008CB RID: 2251
		public HNSNetwork Output = new HNSNetwork();
	}
}
