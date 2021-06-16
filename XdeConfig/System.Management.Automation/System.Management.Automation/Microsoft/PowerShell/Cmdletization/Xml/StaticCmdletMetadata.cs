using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009BE RID: 2494
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[Serializable]
	internal class StaticCmdletMetadata
	{
		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06005BDE RID: 23518 RVA: 0x001ECC3C File Offset: 0x001EAE3C
		// (set) Token: 0x06005BDF RID: 23519 RVA: 0x001ECC44 File Offset: 0x001EAE44
		public StaticCmdletMetadataCmdletMetadata CmdletMetadata
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

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x001ECC4D File Offset: 0x001EAE4D
		// (set) Token: 0x06005BE1 RID: 23521 RVA: 0x001ECC55 File Offset: 0x001EAE55
		[XmlElement("Method")]
		public StaticMethodMetadata[] Method
		{
			get
			{
				return this.methodField;
			}
			set
			{
				this.methodField = value;
			}
		}

		// Token: 0x04003129 RID: 12585
		private StaticCmdletMetadataCmdletMetadata cmdletMetadataField;

		// Token: 0x0400312A RID: 12586
		private StaticMethodMetadata[] methodField;
	}
}
