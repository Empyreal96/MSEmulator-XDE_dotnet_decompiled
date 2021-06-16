using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B4 RID: 2484
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class CmdletParameterMetadataValidateCount
	{
		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x001ECA4B File Offset: 0x001EAC4B
		// (set) Token: 0x06005BA4 RID: 23460 RVA: 0x001ECA53 File Offset: 0x001EAC53
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

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06005BA5 RID: 23461 RVA: 0x001ECA5C File Offset: 0x001EAC5C
		// (set) Token: 0x06005BA6 RID: 23462 RVA: 0x001ECA64 File Offset: 0x001EAC64
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

		// Token: 0x0400310B RID: 12555
		private string minField;

		// Token: 0x0400310C RID: 12556
		private string maxField;
	}
}
