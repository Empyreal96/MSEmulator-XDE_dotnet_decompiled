using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009AB RID: 2475
	[DebuggerStepThrough]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[Serializable]
	internal class ClassMetadataInstanceCmdlets
	{
		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06005B48 RID: 23368 RVA: 0x001EC74A File Offset: 0x001EA94A
		// (set) Token: 0x06005B49 RID: 23369 RVA: 0x001EC752 File Offset: 0x001EA952
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

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06005B4A RID: 23370 RVA: 0x001EC75B File Offset: 0x001EA95B
		// (set) Token: 0x06005B4B RID: 23371 RVA: 0x001EC763 File Offset: 0x001EA963
		public GetCmdletMetadata GetCmdlet
		{
			get
			{
				return this.getCmdletField;
			}
			set
			{
				this.getCmdletField = value;
			}
		}

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06005B4C RID: 23372 RVA: 0x001EC76C File Offset: 0x001EA96C
		// (set) Token: 0x06005B4D RID: 23373 RVA: 0x001EC774 File Offset: 0x001EA974
		[XmlElement("Cmdlet")]
		public InstanceCmdletMetadata[] Cmdlet
		{
			get
			{
				return this.cmdletField;
			}
			set
			{
				this.cmdletField = value;
			}
		}

		// Token: 0x040030E2 RID: 12514
		private GetCmdletParameters getCmdletParametersField;

		// Token: 0x040030E3 RID: 12515
		private GetCmdletMetadata getCmdletField;

		// Token: 0x040030E4 RID: 12516
		private InstanceCmdletMetadata[] cmdletField;
	}
}
