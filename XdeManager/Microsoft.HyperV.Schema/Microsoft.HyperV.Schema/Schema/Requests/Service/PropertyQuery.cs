using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Requests.Service
{
	// Token: 0x02000072 RID: 114
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class PropertyQuery
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x00007241 File Offset: 0x00005441
		public static bool IsJsonDefault(PropertyQuery val)
		{
			return PropertyQuery._default.JsonEquals(val);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007250 File Offset: 0x00005450
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x000072F8 File Offset: 0x000054F8
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00007350 File Offset: 0x00005550
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
						this.PropertyTypes[i] = PropertyType.Basic;
					}
					else
					{
						this.PropertyTypes[i] = (PropertyType)Enum.Parse(typeof(PropertyType), value[i], true);
					}
				}
			}
		}

		// Token: 0x04000274 RID: 628
		private static readonly PropertyQuery _default = new PropertyQuery();

		// Token: 0x04000275 RID: 629
		public PropertyType[] PropertyTypes;
	}
}
