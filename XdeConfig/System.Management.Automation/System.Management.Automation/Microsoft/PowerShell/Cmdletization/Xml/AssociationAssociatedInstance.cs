using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B0 RID: 2480
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class AssociationAssociatedInstance
	{
		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06005B6F RID: 23407 RVA: 0x001EC893 File Offset: 0x001EAA93
		// (set) Token: 0x06005B70 RID: 23408 RVA: 0x001EC89B File Offset: 0x001EAA9B
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

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06005B71 RID: 23409 RVA: 0x001EC8A4 File Offset: 0x001EAAA4
		// (set) Token: 0x06005B72 RID: 23410 RVA: 0x001EC8AC File Offset: 0x001EAAAC
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

		// Token: 0x040030F3 RID: 12531
		private TypeMetadata typeField;

		// Token: 0x040030F4 RID: 12532
		private CmdletParameterMetadataForGetCmdletFilteringParameter cmdletParameterMetadataField;
	}
}
