using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.DeviceAssignment
{
	// Token: 0x02000092 RID: 146
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Device
	{
		// Token: 0x06000237 RID: 567 RVA: 0x0000874C File Offset: 0x0000694C
		public static bool IsJsonDefault(Device val)
		{
			return Device._default.JsonEquals(val);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000875C File Offset: 0x0000695C
		public bool JsonEquals(object obj)
		{
			Device graph = obj as Device;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Device), new DataContractJsonSerializerSettings
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00008804 File Offset: 0x00006A04
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000881E File Offset: 0x00006A1E
		[DataMember(Name = "Type")]
		private string _Type
		{
			get
			{
				DeviceType type = this.Type;
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = DeviceType.ClassGuid;
				}
				this.Type = (DeviceType)Enum.Parse(typeof(DeviceType), value, true);
			}
		}

		// Token: 0x0400030D RID: 781
		private static readonly Device _default = new Device();

		// Token: 0x0400030E RID: 782
		public DeviceType Type;

		// Token: 0x0400030F RID: 783
		[DataMember]
		public Guid InterfaceClassGuid;

		// Token: 0x04000310 RID: 784
		[DataMember]
		public string LocationPath;
	}
}
