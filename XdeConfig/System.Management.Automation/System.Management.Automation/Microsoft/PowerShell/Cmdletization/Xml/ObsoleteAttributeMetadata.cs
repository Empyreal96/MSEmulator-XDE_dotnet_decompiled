using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B7 RID: 2487
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class ObsoleteAttributeMetadata
	{
		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x001ECAC9 File Offset: 0x001EACC9
		// (set) Token: 0x06005BB3 RID: 23475 RVA: 0x001ECAD1 File Offset: 0x001EACD1
		[XmlAttribute]
		public string Message
		{
			get
			{
				return this.messageField;
			}
			set
			{
				this.messageField = value;
			}
		}

		// Token: 0x04003111 RID: 12561
		private string messageField;
	}
}
