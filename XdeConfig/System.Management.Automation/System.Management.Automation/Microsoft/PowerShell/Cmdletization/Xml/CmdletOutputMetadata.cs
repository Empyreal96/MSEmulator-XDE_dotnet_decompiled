using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C4 RID: 2500
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class CmdletOutputMetadata
	{
		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x001ECD38 File Offset: 0x001EAF38
		// (set) Token: 0x06005BFD RID: 23549 RVA: 0x001ECD40 File Offset: 0x001EAF40
		public object ErrorCode
		{
			get
			{
				return this.errorCodeField;
			}
			set
			{
				this.errorCodeField = value;
			}
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x001ECD49 File Offset: 0x001EAF49
		// (set) Token: 0x06005BFF RID: 23551 RVA: 0x001ECD51 File Offset: 0x001EAF51
		[XmlAttribute]
		public string PSName
		{
			get
			{
				return this.pSNameField;
			}
			set
			{
				this.pSNameField = value;
			}
		}

		// Token: 0x04003135 RID: 12597
		private object errorCodeField;

		// Token: 0x04003136 RID: 12598
		private string pSNameField;
	}
}
