using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009A9 RID: 2473
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[XmlRoot(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", IsNullable = false)]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[Serializable]
	internal class PowerShellMetadata
	{
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06005B32 RID: 23346 RVA: 0x001EC690 File Offset: 0x001EA890
		// (set) Token: 0x06005B33 RID: 23347 RVA: 0x001EC698 File Offset: 0x001EA898
		public ClassMetadata Class
		{
			get
			{
				return this.classField;
			}
			set
			{
				this.classField = value;
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06005B34 RID: 23348 RVA: 0x001EC6A1 File Offset: 0x001EA8A1
		// (set) Token: 0x06005B35 RID: 23349 RVA: 0x001EC6A9 File Offset: 0x001EA8A9
		[XmlArrayItem("Enum", IsNullable = false)]
		public EnumMetadataEnum[] Enums
		{
			get
			{
				return this.enumsField;
			}
			set
			{
				this.enumsField = value;
			}
		}

		// Token: 0x040030D8 RID: 12504
		private ClassMetadata classField;

		// Token: 0x040030D9 RID: 12505
		private EnumMetadataEnum[] enumsField;
	}
}
