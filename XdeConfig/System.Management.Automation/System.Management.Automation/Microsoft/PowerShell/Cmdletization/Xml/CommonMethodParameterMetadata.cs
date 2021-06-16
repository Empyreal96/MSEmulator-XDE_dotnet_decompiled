using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C2 RID: 2498
	[XmlInclude(typeof(StaticMethodParameterMetadata))]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[XmlInclude(typeof(InstanceMethodParameterMetadata))]
	[DebuggerStepThrough]
	[Serializable]
	internal class CommonMethodParameterMetadata
	{
		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06005BF0 RID: 23536 RVA: 0x001ECCD3 File Offset: 0x001EAED3
		// (set) Token: 0x06005BF1 RID: 23537 RVA: 0x001ECCDB File Offset: 0x001EAEDB
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

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06005BF2 RID: 23538 RVA: 0x001ECCE4 File Offset: 0x001EAEE4
		// (set) Token: 0x06005BF3 RID: 23539 RVA: 0x001ECCEC File Offset: 0x001EAEEC
		[XmlAttribute]
		public string ParameterName
		{
			get
			{
				return this.parameterNameField;
			}
			set
			{
				this.parameterNameField = value;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x001ECCF5 File Offset: 0x001EAEF5
		// (set) Token: 0x06005BF5 RID: 23541 RVA: 0x001ECCFD File Offset: 0x001EAEFD
		[XmlAttribute]
		public string DefaultValue
		{
			get
			{
				return this.defaultValueField;
			}
			set
			{
				this.defaultValueField = value;
			}
		}

		// Token: 0x04003130 RID: 12592
		private TypeMetadata typeField;

		// Token: 0x04003131 RID: 12593
		private string parameterNameField;

		// Token: 0x04003132 RID: 12594
		private string defaultValueField;
	}
}
