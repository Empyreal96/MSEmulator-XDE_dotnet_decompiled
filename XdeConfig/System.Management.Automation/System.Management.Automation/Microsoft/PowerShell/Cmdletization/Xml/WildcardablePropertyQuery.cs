using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009CA RID: 2506
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class WildcardablePropertyQuery : PropertyQuery
	{
		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06005C18 RID: 23576 RVA: 0x001ECE23 File Offset: 0x001EB023
		// (set) Token: 0x06005C19 RID: 23577 RVA: 0x001ECE2B File Offset: 0x001EB02B
		[XmlAttribute]
		public bool AllowGlobbing
		{
			get
			{
				return this.allowGlobbingField;
			}
			set
			{
				this.allowGlobbingField = value;
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06005C1A RID: 23578 RVA: 0x001ECE34 File Offset: 0x001EB034
		// (set) Token: 0x06005C1B RID: 23579 RVA: 0x001ECE3C File Offset: 0x001EB03C
		[XmlIgnore]
		public bool AllowGlobbingSpecified
		{
			get
			{
				return this.allowGlobbingFieldSpecified;
			}
			set
			{
				this.allowGlobbingFieldSpecified = value;
			}
		}

		// Token: 0x04003140 RID: 12608
		private bool allowGlobbingField;

		// Token: 0x04003141 RID: 12609
		private bool allowGlobbingFieldSpecified;
	}
}
