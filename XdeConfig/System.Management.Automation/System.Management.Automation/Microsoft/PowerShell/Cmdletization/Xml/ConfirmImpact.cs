using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009BD RID: 2493
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[Serializable]
	public enum ConfirmImpact
	{
		// Token: 0x04003125 RID: 12581
		None,
		// Token: 0x04003126 RID: 12582
		Low,
		// Token: 0x04003127 RID: 12583
		Medium,
		// Token: 0x04003128 RID: 12584
		High
	}
}
