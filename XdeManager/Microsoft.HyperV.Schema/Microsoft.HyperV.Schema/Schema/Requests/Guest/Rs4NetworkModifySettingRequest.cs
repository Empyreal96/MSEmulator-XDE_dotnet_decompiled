using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.Guest
{
	// Token: 0x02000078 RID: 120
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Rs4NetworkModifySettingRequest
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00007747 File Offset: 0x00005947
		public static bool IsJsonDefault(Rs4NetworkModifySettingRequest val)
		{
			return Rs4NetworkModifySettingRequest._default.JsonEquals(val);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00007754 File Offset: 0x00005954
		public bool JsonEquals(object obj)
		{
			Rs4NetworkModifySettingRequest graph = obj as Rs4NetworkModifySettingRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Rs4NetworkModifySettingRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000077FC File Offset: 0x000059FC
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00007816 File Offset: 0x00005A16
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

		// Token: 0x04000296 RID: 662
		private static readonly Rs4NetworkModifySettingRequest _default = new Rs4NetworkModifySettingRequest();

		// Token: 0x04000297 RID: 663
		public NetworkModifyRequestType RequestType;

		// Token: 0x04000298 RID: 664
		[DataMember(EmitDefaultValue = false)]
		public Guid AdapterInstanceID;

		// Token: 0x04000299 RID: 665
		[DataMember(EmitDefaultValue = false)]
		public object Settings;
	}
}
