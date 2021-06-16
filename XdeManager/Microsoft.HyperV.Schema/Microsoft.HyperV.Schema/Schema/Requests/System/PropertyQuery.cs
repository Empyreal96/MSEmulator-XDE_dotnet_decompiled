using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.System
{
	// Token: 0x0200006F RID: 111
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class PropertyQuery
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00006F2B File Offset: 0x0000512B
		public static bool IsJsonDefault(PropertyQuery val)
		{
			return PropertyQuery._default.JsonEquals(val);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006F38 File Offset: 0x00005138
		public bool JsonEquals(object obj)
		{
			PropertyQuery graph = obj as PropertyQuery;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(PropertyQuery), new DataContractJsonSerializerSettings
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00006FE0 File Offset: 0x000051E0
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00007038 File Offset: 0x00005238
		[DataMember(EmitDefaultValue = false, Name = "PropertyTypes")]
		private string[] _PropertyTypes
		{
			get
			{
				if (this.PropertyTypes == null)
				{
					return null;
				}
				string[] array = new string[this.PropertyTypes.Length];
				for (int i = 0; i < array.Length; i++)
				{
					PropertyType propertyType = this.PropertyTypes[i];
					array[i] = this.PropertyTypes[i].ToString();
				}
				return array;
			}
			set
			{
				if (value == null)
				{
					this.PropertyTypes = null;
					return;
				}
				this.PropertyTypes = new PropertyType[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					if (string.IsNullOrEmpty(value[i]))
					{
						this.PropertyTypes[i] = PropertyType.Memory;
					}
					else
					{
						this.PropertyTypes[i] = (PropertyType)Enum.Parse(typeof(PropertyType), value[i], true);
					}
				}
			}
		}

		// Token: 0x04000261 RID: 609
		private static readonly PropertyQuery _default = new PropertyQuery();

		// Token: 0x04000262 RID: 610
		public PropertyType[] PropertyTypes;
	}
}
