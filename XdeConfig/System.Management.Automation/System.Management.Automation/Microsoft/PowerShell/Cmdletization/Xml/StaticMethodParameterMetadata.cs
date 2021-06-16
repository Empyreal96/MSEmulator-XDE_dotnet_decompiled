using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C3 RID: 2499
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class StaticMethodParameterMetadata : CommonMethodParameterMetadata
	{
		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x001ECD0E File Offset: 0x001EAF0E
		// (set) Token: 0x06005BF8 RID: 23544 RVA: 0x001ECD16 File Offset: 0x001EAF16
		public CmdletParameterMetadataForStaticMethodParameter CmdletParameterMetadata
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

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x001ECD1F File Offset: 0x001EAF1F
		// (set) Token: 0x06005BFA RID: 23546 RVA: 0x001ECD27 File Offset: 0x001EAF27
		public CmdletOutputMetadata CmdletOutputMetadata
		{
			get
			{
				return this.cmdletOutputMetadataField;
			}
			set
			{
				this.cmdletOutputMetadataField = value;
			}
		}

		// Token: 0x04003133 RID: 12595
		private CmdletParameterMetadataForStaticMethodParameter cmdletParameterMetadataField;

		// Token: 0x04003134 RID: 12596
		private CmdletOutputMetadata cmdletOutputMetadataField;
	}
}
