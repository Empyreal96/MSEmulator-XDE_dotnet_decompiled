using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009CB RID: 2507
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", IncludeInSchema = false)]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[Serializable]
	public enum ItemsChoiceType
	{
		// Token: 0x04003143 RID: 12611
		ExcludeQuery,
		// Token: 0x04003144 RID: 12612
		MaxValueQuery,
		// Token: 0x04003145 RID: 12613
		MinValueQuery,
		// Token: 0x04003146 RID: 12614
		RegularQuery
	}
}
