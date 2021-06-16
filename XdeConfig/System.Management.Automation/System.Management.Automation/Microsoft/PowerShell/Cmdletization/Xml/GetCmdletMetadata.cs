using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009BB RID: 2491
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class GetCmdletMetadata
	{
		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06005BCA RID: 23498 RVA: 0x001ECB93 File Offset: 0x001EAD93
		// (set) Token: 0x06005BCB RID: 23499 RVA: 0x001ECB9B File Offset: 0x001EAD9B
		public CommonCmdletMetadata CmdletMetadata
		{
			get
			{
				return this.cmdletMetadataField;
			}
			set
			{
				this.cmdletMetadataField = value;
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06005BCC RID: 23500 RVA: 0x001ECBA4 File Offset: 0x001EADA4
		// (set) Token: 0x06005BCD RID: 23501 RVA: 0x001ECBAC File Offset: 0x001EADAC
		public GetCmdletParameters GetCmdletParameters
		{
			get
			{
				return this.getCmdletParametersField;
			}
			set
			{
				this.getCmdletParametersField = value;
			}
		}

		// Token: 0x0400311B RID: 12571
		private CommonCmdletMetadata cmdletMetadataField;

		// Token: 0x0400311C RID: 12572
		private GetCmdletParameters getCmdletParametersField;
	}
}
