using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B9 RID: 2489
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CmdletParameterMetadataForStaticMethodParameter : CmdletParameterMetadata
	{
		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06005BBA RID: 23482 RVA: 0x001ECB0C File Offset: 0x001EAD0C
		// (set) Token: 0x06005BBB RID: 23483 RVA: 0x001ECB14 File Offset: 0x001EAD14
		[XmlAttribute]
		public bool ValueFromPipeline
		{
			get
			{
				return this.valueFromPipelineField;
			}
			set
			{
				this.valueFromPipelineField = value;
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x001ECB1D File Offset: 0x001EAD1D
		// (set) Token: 0x06005BBD RID: 23485 RVA: 0x001ECB25 File Offset: 0x001EAD25
		[XmlIgnore]
		public bool ValueFromPipelineSpecified
		{
			get
			{
				return this.valueFromPipelineFieldSpecified;
			}
			set
			{
				this.valueFromPipelineFieldSpecified = value;
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06005BBE RID: 23486 RVA: 0x001ECB2E File Offset: 0x001EAD2E
		// (set) Token: 0x06005BBF RID: 23487 RVA: 0x001ECB36 File Offset: 0x001EAD36
		[XmlAttribute]
		public bool ValueFromPipelineByPropertyName
		{
			get
			{
				return this.valueFromPipelineByPropertyNameField;
			}
			set
			{
				this.valueFromPipelineByPropertyNameField = value;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06005BC0 RID: 23488 RVA: 0x001ECB3F File Offset: 0x001EAD3F
		// (set) Token: 0x06005BC1 RID: 23489 RVA: 0x001ECB47 File Offset: 0x001EAD47
		[XmlIgnore]
		public bool ValueFromPipelineByPropertyNameSpecified
		{
			get
			{
				return this.valueFromPipelineByPropertyNameFieldSpecified;
			}
			set
			{
				this.valueFromPipelineByPropertyNameFieldSpecified = value;
			}
		}

		// Token: 0x04003114 RID: 12564
		private bool valueFromPipelineField;

		// Token: 0x04003115 RID: 12565
		private bool valueFromPipelineFieldSpecified;

		// Token: 0x04003116 RID: 12566
		private bool valueFromPipelineByPropertyNameField;

		// Token: 0x04003117 RID: 12567
		private bool valueFromPipelineByPropertyNameFieldSpecified;
	}
}
