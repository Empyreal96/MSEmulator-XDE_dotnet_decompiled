using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.Guest
{
	// Token: 0x02000076 RID: 118
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestModifySettingRequest
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x000074DF File Offset: 0x000056DF
		public static bool IsJsonDefault(GuestModifySettingRequest val)
		{
			return GuestModifySettingRequest._default.JsonEquals(val);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000074EC File Offset: 0x000056EC
		public bool JsonEquals(object obj)
		{
			GuestModifySettingRequest graph = obj as GuestModifySettingRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestModifySettingRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007594 File Offset: 0x00005794
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x000075AE File Offset: 0x000057AE
		[DataMember(Name = "ResourceType")]
		private string _ResourceType
		{
			get
			{
				ModifyResourceType resourceType = this.ResourceType;
				return this.ResourceType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.ResourceType = ModifyResourceType.Network;
				}
				this.ResourceType = (ModifyResourceType)Enum.Parse(typeof(ModifyResourceType), value, true);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000075DB File Offset: 0x000057DB
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x000075F5 File Offset: 0x000057F5
		[DataMember(Name = "RequestType")]
		private string _RequestType
		{
			get
			{
				ModifyRequestType requestType = this.RequestType;
				return this.RequestType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.RequestType = ModifyRequestType.Add;
				}
				this.RequestType = (ModifyRequestType)Enum.Parse(typeof(ModifyRequestType), value, true);
			}
		}

		// Token: 0x0400028D RID: 653
		private static readonly GuestModifySettingRequest _default = new GuestModifySettingRequest();

		// Token: 0x0400028E RID: 654
		public ModifyResourceType ResourceType;

		// Token: 0x0400028F RID: 655
		public ModifyRequestType RequestType;

		// Token: 0x04000290 RID: 656
		[DataMember(EmitDefaultValue = false)]
		public object Settings;

		// Token: 0x04000291 RID: 657
		[DataMember(EmitDefaultValue = false)]
		public object HostedSettings;
	}
}
