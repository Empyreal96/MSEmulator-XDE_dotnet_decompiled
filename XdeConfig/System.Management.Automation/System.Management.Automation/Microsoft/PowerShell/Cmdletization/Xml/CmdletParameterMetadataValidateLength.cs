using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B5 RID: 2485
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CmdletParameterMetadataValidateLength
	{
		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x001ECA75 File Offset: 0x001EAC75
		// (set) Token: 0x06005BA9 RID: 23465 RVA: 0x001ECA7D File Offset: 0x001EAC7D
		[XmlAttribute(DataType = "nonNegativeInteger")]
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

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06005BAA RID: 23466 RVA: 0x001ECA86 File Offset: 0x001EAC86
		// (set) Token: 0x06005BAB RID: 23467 RVA: 0x001ECA8E File Offset: 0x001EAC8E
		[XmlAttribute(DataType = "nonNegativeInteger")]
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

		// Token: 0x0400310D RID: 12557
		private string minField;

		// Token: 0x0400310E RID: 12558
		private string maxField;
	}
}
