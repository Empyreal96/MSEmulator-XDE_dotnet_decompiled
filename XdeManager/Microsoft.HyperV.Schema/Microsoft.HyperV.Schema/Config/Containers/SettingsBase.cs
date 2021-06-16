using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200016E RID: 366
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SettingsBase
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x00012609 File Offset: 0x00010809
		public static bool IsJsonDefault(SettingsBase val)
		{
			return SettingsBase._default.JsonEquals(val);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00012618 File Offset: 0x00010818
		public bool JsonEquals(object obj)
		{
			SettingsBase graph = obj as SettingsBase;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SettingsBase), new DataContractJsonSerializerSettings
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000126C0 File Offset: 0x000108C0
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x000126DA File Offset: 0x000108DA
		[DataMember(IsRequired = true, Name = "SystemType")]
		private string _SystemType
		{
			get
			{
				SystemType systemType = this.SystemType;
				return this.SystemType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.SystemType = SystemType.Container;
				}
				this.SystemType = (SystemType)Enum.Parse(typeof(SystemType), value, true);
			}
		}

		// Token: 0x040007B5 RID: 1973
		private static readonly SettingsBase _default = new SettingsBase();

		// Token: 0x040007B6 RID: 1974
		public SystemType SystemType;
	}
}
