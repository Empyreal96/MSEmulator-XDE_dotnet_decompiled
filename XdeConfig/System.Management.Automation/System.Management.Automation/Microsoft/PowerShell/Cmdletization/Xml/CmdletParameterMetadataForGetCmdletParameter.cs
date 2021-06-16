using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B2 RID: 2482
	[XmlInclude(typeof(CmdletParameterMetadataForGetCmdletFilteringParameter))]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CmdletParameterMetadataForGetCmdletParameter : CmdletParameterMetadata
	{
		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06005B93 RID: 23443 RVA: 0x001EC9C4 File Offset: 0x001EABC4
		// (set) Token: 0x06005B94 RID: 23444 RVA: 0x001EC9CC File Offset: 0x001EABCC
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

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06005B95 RID: 23445 RVA: 0x001EC9D5 File Offset: 0x001EABD5
		// (set) Token: 0x06005B96 RID: 23446 RVA: 0x001EC9DD File Offset: 0x001EABDD
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

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06005B97 RID: 23447 RVA: 0x001EC9E6 File Offset: 0x001EABE6
		// (set) Token: 0x06005B98 RID: 23448 RVA: 0x001EC9EE File Offset: 0x001EABEE
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

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06005B99 RID: 23449 RVA: 0x001EC9F7 File Offset: 0x001EABF7
		// (set) Token: 0x06005B9A RID: 23450 RVA: 0x001EC9FF File Offset: 0x001EABFF
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

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x001ECA08 File Offset: 0x001EAC08
		// (set) Token: 0x06005B9C RID: 23452 RVA: 0x001ECA10 File Offset: 0x001EAC10
		[XmlAttribute]
		public string[] CmdletParameterSets
		{
			get
			{
				return this.cmdletParameterSetsField;
			}
			set
			{
				this.cmdletParameterSetsField = value;
			}
		}

		// Token: 0x04003104 RID: 12548
		private bool valueFromPipelineField;

		// Token: 0x04003105 RID: 12549
		private bool valueFromPipelineFieldSpecified;

		// Token: 0x04003106 RID: 12550
		private bool valueFromPipelineByPropertyNameField;

		// Token: 0x04003107 RID: 12551
		private bool valueFromPipelineByPropertyNameFieldSpecified;

		// Token: 0x04003108 RID: 12552
		private string[] cmdletParameterSetsField;
	}
}
