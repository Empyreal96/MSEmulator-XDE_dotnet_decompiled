using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009AC RID: 2476
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[Serializable]
	internal class GetCmdletParameters
	{
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06005B4F RID: 23375 RVA: 0x001EC785 File Offset: 0x001EA985
		// (set) Token: 0x06005B50 RID: 23376 RVA: 0x001EC78D File Offset: 0x001EA98D
		[XmlArrayItem("Property", IsNullable = false)]
		public PropertyMetadata[] QueryableProperties
		{
			get
			{
				return this.queryablePropertiesField;
			}
			set
			{
				this.queryablePropertiesField = value;
			}
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06005B51 RID: 23377 RVA: 0x001EC796 File Offset: 0x001EA996
		// (set) Token: 0x06005B52 RID: 23378 RVA: 0x001EC79E File Offset: 0x001EA99E
		[XmlArrayItem(IsNullable = false)]
		public Association[] QueryableAssociations
		{
			get
			{
				return this.queryableAssociationsField;
			}
			set
			{
				this.queryableAssociationsField = value;
			}
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06005B53 RID: 23379 RVA: 0x001EC7A7 File Offset: 0x001EA9A7
		// (set) Token: 0x06005B54 RID: 23380 RVA: 0x001EC7AF File Offset: 0x001EA9AF
		[XmlArrayItem("Option", IsNullable = false)]
		public QueryOption[] QueryOptions
		{
			get
			{
				return this.queryOptionsField;
			}
			set
			{
				this.queryOptionsField = value;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06005B55 RID: 23381 RVA: 0x001EC7B8 File Offset: 0x001EA9B8
		// (set) Token: 0x06005B56 RID: 23382 RVA: 0x001EC7C0 File Offset: 0x001EA9C0
		[XmlAttribute]
		public string DefaultCmdletParameterSet
		{
			get
			{
				return this.defaultCmdletParameterSetField;
			}
			set
			{
				this.defaultCmdletParameterSetField = value;
			}
		}

		// Token: 0x040030E5 RID: 12517
		private PropertyMetadata[] queryablePropertiesField;

		// Token: 0x040030E6 RID: 12518
		private Association[] queryableAssociationsField;

		// Token: 0x040030E7 RID: 12519
		private QueryOption[] queryOptionsField;

		// Token: 0x040030E8 RID: 12520
		private string defaultCmdletParameterSetField;
	}
}
