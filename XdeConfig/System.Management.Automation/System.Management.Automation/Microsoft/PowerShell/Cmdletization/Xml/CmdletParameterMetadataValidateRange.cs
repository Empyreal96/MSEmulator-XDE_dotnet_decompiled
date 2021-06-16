using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B6 RID: 2486
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[Serializable]
	internal class CmdletParameterMetadataValidateRange
	{
		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06005BAD RID: 23469 RVA: 0x001ECA9F File Offset: 0x001EAC9F
		// (set) Token: 0x06005BAE RID: 23470 RVA: 0x001ECAA7 File Offset: 0x001EACA7
		[XmlAttribute(DataType = "integer")]
		public string Min
		{
			get
			{
				return this.minField;
			}
			set
			{
				this.minField = value;
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06005BAF RID: 23471 RVA: 0x001ECAB0 File Offset: 0x001EACB0
		// (set) Token: 0x06005BB0 RID: 23472 RVA: 0x001ECAB8 File Offset: 0x001EACB8
		[XmlAttribute(DataType = "integer")]
		public string Max
		{
			get
			{
				return this.maxField;
			}
			set
			{
				this.maxField = value;
			}
		}

		// Token: 0x0400310F RID: 12559
		private string minField;

		// Token: 0x04003110 RID: 12560
		private string maxField;
	}
}
