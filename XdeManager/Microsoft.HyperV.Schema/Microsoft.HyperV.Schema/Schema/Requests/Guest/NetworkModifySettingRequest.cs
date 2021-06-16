using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.Guest
{
	// Token: 0x02000077 RID: 119
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkModifySettingRequest
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x00007636 File Offset: 0x00005836
		public static bool IsJsonDefault(NetworkModifySettingRequest val)
		{
			return NetworkModifySettingRequest._default.JsonEquals(val);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007644 File Offset: 0x00005844
		public bool JsonEquals(object obj)
		{
			NetworkModifySettingRequest graph = obj as NetworkModifySettingRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkModifySettingRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000076EC File Offset: 0x000058EC
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00007706 File Offset: 0x00005906
		[DataMember(Name = "RequestType")]
		private string _RequestType
		{
			get
			{
				NetworkModifyRequestType requestType = this.RequestType;
				return this.RequestType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.RequestType = NetworkModifyRequestType.PreAdd;
				}
				this.RequestType = (NetworkModifyRequestType)Enum.Parse(typeof(NetworkModifyRequestType), value, true);
			}
		}

		// Token: 0x04000292 RID: 658
		private static readonly NetworkModifySettingRequest _default = new NetworkModifySettingRequest();

		// Token: 0x04000293 RID: 659
		public NetworkModifyRequestType RequestType;

		// Token: 0x04000294 RID: 660
		[DataMember(EmitDefaultValue = false)]
		public string AdapterId;

		// Token: 0x04000295 RID: 661
		[DataMember(EmitDefaultValue = false)]
		public object Settings;
	}
}
