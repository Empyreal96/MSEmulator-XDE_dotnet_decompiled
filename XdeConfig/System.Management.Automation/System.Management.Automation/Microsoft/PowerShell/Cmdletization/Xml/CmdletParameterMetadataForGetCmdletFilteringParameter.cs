using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B3 RID: 2483
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[Serializable]
	internal class CmdletParameterMetadataForGetCmdletFilteringParameter : CmdletParameterMetadataForGetCmdletParameter
	{
		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06005B9E RID: 23454 RVA: 0x001ECA21 File Offset: 0x001EAC21
		// (set) Token: 0x06005B9F RID: 23455 RVA: 0x001ECA29 File Offset: 0x001EAC29
		[XmlAttribute]
		public bool ErrorOnNoMatch
		{
			get
			{
				return this.errorOnNoMatchField;
			}
			set
			{
				this.errorOnNoMatchField = value;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x001ECA32 File Offset: 0x001EAC32
		// (set) Token: 0x06005BA1 RID: 23457 RVA: 0x001ECA3A File Offset: 0x001EAC3A
		[XmlIgnore]
		public bool ErrorOnNoMatchSpecified
		{
			get
			{
				return this.errorOnNoMatchFieldSpecified;
			}
			set
			{
				this.errorOnNoMatchFieldSpecified = value;
			}
		}

		// Token: 0x04003109 RID: 12553
		private bool errorOnNoMatchField;

		// Token: 0x0400310A RID: 12554
		private bool errorOnNoMatchFieldSpecified;
	}
}
