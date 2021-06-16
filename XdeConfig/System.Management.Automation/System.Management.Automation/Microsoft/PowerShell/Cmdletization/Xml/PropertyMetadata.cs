using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009AD RID: 2477
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[Serializable]
	internal class PropertyMetadata
	{
		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06005B58 RID: 23384 RVA: 0x001EC7D1 File Offset: 0x001EA9D1
		// (set) Token: 0x06005B59 RID: 23385 RVA: 0x001EC7D9 File Offset: 0x001EA9D9
		public TypeMetadata Type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06005B5A RID: 23386 RVA: 0x001EC7E2 File Offset: 0x001EA9E2
		// (set) Token: 0x06005B5B RID: 23387 RVA: 0x001EC7EA File Offset: 0x001EA9EA
		[XmlElement("MinValueQuery", typeof(PropertyQuery))]
		[XmlElement("ExcludeQuery", typeof(WildcardablePropertyQuery))]
		[XmlElement("MaxValueQuery", typeof(PropertyQuery))]
		[XmlChoiceIdentifier("ItemsElementName")]
		[XmlElement("RegularQuery", typeof(WildcardablePropertyQuery))]
		public PropertyQuery[] Items
		{
			get
			{
				return this.itemsField;
			}
			set
			{
				this.itemsField = value;
			}
		}

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x001EC7F3 File Offset: 0x001EA9F3
		// (set) Token: 0x06005B5D RID: 23389 RVA: 0x001EC7FB File Offset: 0x001EA9FB
		[XmlIgnore]
		[XmlElement("ItemsElementName")]
		public ItemsChoiceType[] ItemsElementName
		{
			get
			{
				return this.itemsElementNameField;
			}
			set
			{
				this.itemsElementNameField = value;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x001EC804 File Offset: 0x001EAA04
		// (set) Token: 0x06005B5F RID: 23391 RVA: 0x001EC80C File Offset: 0x001EAA0C
		[XmlAttribute]
		public string PropertyName
		{
			get
			{
				return this.propertyNameField;
			}
			set
			{
				this.propertyNameField = value;
			}
		}

		// Token: 0x040030E9 RID: 12521
		private TypeMetadata typeField;

		// Token: 0x040030EA RID: 12522
		private PropertyQuery[] itemsField;

		// Token: 0x040030EB RID: 12523
		private ItemsChoiceType[] itemsElementNameField;

		// Token: 0x040030EC RID: 12524
		private string propertyNameField;
	}
}
