using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.System
{
	// Token: 0x0200006E RID: 110
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ModifySettingRequest
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00006E18 File Offset: 0x00005018
		public static bool IsJsonDefault(ModifySettingRequest val)
		{
			return ModifySettingRequest._default.JsonEquals(val);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006E28 File Offset: 0x00005028
		public bool JsonEquals(object obj)
		{
			ModifySettingRequest graph = obj as ModifySettingRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ModifySettingRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00006ED0 File Offset: 0x000050D0
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00006EEA File Offset: 0x000050EA
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

		// Token: 0x0400025C RID: 604
		private static readonly ModifySettingRequest _default = new ModifySettingRequest();

		// Token: 0x0400025D RID: 605
		[DataMember(EmitDefaultValue = false)]
		public string ResourcePath;

		// Token: 0x0400025E RID: 606
		public ModifyRequestType RequestType;

		// Token: 0x0400025F RID: 607
		[DataMember(EmitDefaultValue = false)]
		public object Settings;

		// Token: 0x04000260 RID: 608
		[DataMember(EmitDefaultValue = false)]
		public object GuestRequest;
	}
}
