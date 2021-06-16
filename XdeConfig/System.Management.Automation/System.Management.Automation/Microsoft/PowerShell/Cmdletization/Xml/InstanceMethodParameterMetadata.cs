using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C5 RID: 2501
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[Serializable]
	internal class InstanceMethodParameterMetadata : CommonMethodParameterMetadata
	{
		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06005C01 RID: 23553 RVA: 0x001ECD62 File Offset: 0x001EAF62
		// (set) Token: 0x06005C02 RID: 23554 RVA: 0x001ECD6A File Offset: 0x001EAF6A
		public CmdletParameterMetadataForInstanceMethodParameter CmdletParameterMetadata
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

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06005C03 RID: 23555 RVA: 0x001ECD73 File Offset: 0x001EAF73
		// (set) Token: 0x06005C04 RID: 23556 RVA: 0x001ECD7B File Offset: 0x001EAF7B
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

		// Token: 0x04003137 RID: 12599
		private CmdletParameterMetadataForInstanceMethodParameter cmdletParameterMetadataField;

		// Token: 0x04003138 RID: 12600
		private CmdletOutputMetadata cmdletOutputMetadataField;
	}
}
