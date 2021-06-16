using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B8 RID: 2488
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CmdletParameterMetadataForInstanceMethodParameter : CmdletParameterMetadata
	{
		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06005BB5 RID: 23477 RVA: 0x001ECAE2 File Offset: 0x001EACE2
		// (set) Token: 0x06005BB6 RID: 23478 RVA: 0x001ECAEA File Offset: 0x001EACEA
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

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06005BB7 RID: 23479 RVA: 0x001ECAF3 File Offset: 0x001EACF3
		// (set) Token: 0x06005BB8 RID: 23480 RVA: 0x001ECAFB File Offset: 0x001EACFB
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

		// Token: 0x04003112 RID: 12562
		private bool valueFromPipelineByPropertyNameField;

		// Token: 0x04003113 RID: 12563
		private bool valueFromPipelineByPropertyNameFieldSpecified;
	}
}
