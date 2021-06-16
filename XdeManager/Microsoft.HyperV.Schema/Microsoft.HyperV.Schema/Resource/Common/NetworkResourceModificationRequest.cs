using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Common
{
	// Token: 0x020000D5 RID: 213
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkResourceModificationRequest
	{
		// Token: 0x0600033D RID: 829 RVA: 0x0000B847 File Offset: 0x00009A47
		public static bool IsJsonDefault(NetworkResourceModificationRequest val)
		{
			return NetworkResourceModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000B854 File Offset: 0x00009A54
		public bool JsonEquals(object obj)
		{
			NetworkResourceModificationRequest graph = obj as NetworkResourceModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkResourceModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000B8FC File Offset: 0x00009AFC
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000B916 File Offset: 0x00009B16
		[DataMember(Name = "RequestType")]
		private string _RequestType
		{
			get
			{
				NetworkRequestType requestType = this.RequestType;
				return this.RequestType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.RequestType = NetworkRequestType.PreAdd;
				}
				this.RequestType = (NetworkRequestType)Enum.Parse(typeof(NetworkRequestType), value, true);
			}
		}

		// Token: 0x04000423 RID: 1059
		private static readonly NetworkResourceModificationRequest _default = new NetworkResourceModificationRequest();

		// Token: 0x04000424 RID: 1060
		public NetworkRequestType RequestType;

		// Token: 0x04000425 RID: 1061
		[DataMember(EmitDefaultValue = false)]
		public Guid AdapterInstanceId;

		// Token: 0x04000426 RID: 1062
		[DataMember(EmitDefaultValue = false)]
		public object Settings;
	}
}
