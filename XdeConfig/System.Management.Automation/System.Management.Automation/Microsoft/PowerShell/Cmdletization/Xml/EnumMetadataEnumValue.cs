using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009CE RID: 2510
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class EnumMetadataEnumValue
	{
		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x06005C2D RID: 23597 RVA: 0x001ECED4 File Offset: 0x001EB0D4
		// (set) Token: 0x06005C2E RID: 23598 RVA: 0x001ECEDC File Offset: 0x001EB0DC
		[XmlAttribute]
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x06005C2F RID: 23599 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		// (set) Token: 0x06005C30 RID: 23600 RVA: 0x001ECEED File Offset: 0x001EB0ED
		[XmlAttribute(DataType = "integer")]
		public string Value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}

		// Token: 0x0400314E RID: 12622
		private string nameField;

		// Token: 0x0400314F RID: 12623
		private string valueField;
	}
}
