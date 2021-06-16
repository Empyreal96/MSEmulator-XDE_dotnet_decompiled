using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.Service
{
	// Token: 0x02000073 RID: 115
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ModificationRequest
	{
		// Token: 0x060001CB RID: 459 RVA: 0x000073CD File Offset: 0x000055CD
		public static bool IsJsonDefault(ModificationRequest val)
		{
			return ModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000073DC File Offset: 0x000055DC
		public bool JsonEquals(object obj)
		{
			ModificationRequest graph = obj as ModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007484 File Offset: 0x00005684
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000749E File Offset: 0x0000569E
		[DataMember(Name = "PropertyType")]
		private string _PropertyType
		{
			get
			{
				PropertyType propertyType = this.PropertyType;
				return this.PropertyType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.PropertyType = PropertyType.Basic;
				}
				this.PropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), value, true);
			}
		}

		// Token: 0x04000276 RID: 630
		private static readonly ModificationRequest _default = new ModificationRequest();

		// Token: 0x04000277 RID: 631
		public PropertyType PropertyType;

		// Token: 0x04000278 RID: 632
		[DataMember(EmitDefaultValue = false)]
		public object Settings;
	}
}
