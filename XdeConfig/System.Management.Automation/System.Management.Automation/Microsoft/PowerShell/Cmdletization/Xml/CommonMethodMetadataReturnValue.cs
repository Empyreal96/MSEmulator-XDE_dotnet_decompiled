using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C6 RID: 2502
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CommonMethodMetadataReturnValue
	{
		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06005C06 RID: 23558 RVA: 0x001ECD8C File Offset: 0x001EAF8C
		// (set) Token: 0x06005C07 RID: 23559 RVA: 0x001ECD94 File Offset: 0x001EAF94
		public TypeMetadata Type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06005C08 RID: 23560 RVA: 0x001ECD9D File Offset: 0x001EAF9D
		// (set) Token: 0x06005C09 RID: 23561 RVA: 0x001ECDA5 File Offset: 0x001EAFA5
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

		// Token: 0x04003139 RID: 12601
		private TypeMetadata typeField;

		// Token: 0x0400313A RID: 12602
		private CmdletOutputMetadata cmdletOutputMetadataField;
	}
}
