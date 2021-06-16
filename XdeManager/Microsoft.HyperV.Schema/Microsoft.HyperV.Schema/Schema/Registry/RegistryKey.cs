using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Registry
{
	// Token: 0x0200007B RID: 123
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RegistryKey
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x00007857 File Offset: 0x00005A57
		public static bool IsJsonDefault(RegistryKey val)
		{
			return RegistryKey._default.JsonEquals(val);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007864 File Offset: 0x00005A64
		public bool JsonEquals(object obj)
		{
			RegistryKey graph = obj as RegistryKey;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RegistryKey), new DataContractJsonSerializerSettings
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000790C File Offset: 0x00005B0C
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007926 File Offset: 0x00005B26
		[DataMember(Name = "Hive")]
		private string _Hive
		{
			get
			{
				RegistryHive hive = this.Hive;
				return this.Hive.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Hive = RegistryHive.System;
				}
				this.Hive = (RegistryHive)Enum.Parse(typeof(RegistryHive), value, true);
			}
		}

		// Token: 0x040002A8 RID: 680
		private static readonly RegistryKey _default = new RegistryKey();

		// Token: 0x040002A9 RID: 681
		public RegistryHive Hive;

		// Token: 0x040002AA RID: 682
		[DataMember]
		public string Name;

		// Token: 0x040002AB RID: 683
		[DataMember(EmitDefaultValue = false)]
		public bool Volatile;
	}
}
