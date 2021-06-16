using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C9 RID: 2505
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[XmlInclude(typeof(WildcardablePropertyQuery))]
	[DesignerCategory("code")]
	[Serializable]
	internal class PropertyQuery
	{
		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06005C15 RID: 23573 RVA: 0x001ECE0A File Offset: 0x001EB00A
		// (set) Token: 0x06005C16 RID: 23574 RVA: 0x001ECE12 File Offset: 0x001EB012
		public CmdletParameterMetadataForGetCmdletFilteringParameter CmdletParameterMetadata
		{
			get
			{
				return this.cmdletParameterMetadataField;
			}
			set
			{
				this.cmdletParameterMetadataField = value;
			}
		}

		// Token: 0x0400313F RID: 12607
		private CmdletParameterMetadataForGetCmdletFilteringParameter cmdletParameterMetadataField;
	}
}
