using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.System
{
	// Token: 0x020001A1 RID: 417
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class PropertyQuery
	{
		// Token: 0x060006B7 RID: 1719 RVA: 0x0001545D File Offset: 0x0001365D
		public static bool IsJsonDefault(PropertyQuery val)
		{
			return PropertyQuery._default.JsonEquals(val);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001546C File Offset: 0x0001366C
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00015514 File Offset: 0x00013714
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0001556C File Offset: 0x0001376C
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

		// Token: 0x04000940 RID: 2368
		private static readonly PropertyQuery _default = new PropertyQuery();

		// Token: 0x04000941 RID: 2369
		public PropertyType[] PropertyTypes;
	}
}
