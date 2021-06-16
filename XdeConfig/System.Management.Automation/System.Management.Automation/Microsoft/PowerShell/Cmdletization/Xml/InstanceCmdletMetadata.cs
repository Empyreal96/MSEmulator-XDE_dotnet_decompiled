using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C8 RID: 2504
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[Serializable]
	internal class InstanceCmdletMetadata
	{
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06005C0E RID: 23566 RVA: 0x001ECDCF File Offset: 0x001EAFCF
		// (set) Token: 0x06005C0F RID: 23567 RVA: 0x001ECDD7 File Offset: 0x001EAFD7
		public CommonCmdletMetadata CmdletMetadata
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

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06005C10 RID: 23568 RVA: 0x001ECDE0 File Offset: 0x001EAFE0
		// (set) Token: 0x06005C11 RID: 23569 RVA: 0x001ECDE8 File Offset: 0x001EAFE8
		public InstanceMethodMetadata Method
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

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06005C12 RID: 23570 RVA: 0x001ECDF1 File Offset: 0x001EAFF1
		// (set) Token: 0x06005C13 RID: 23571 RVA: 0x001ECDF9 File Offset: 0x001EAFF9
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

		// Token: 0x0400313C RID: 12604
		private CommonCmdletMetadata cmdletMetadataField;

		// Token: 0x0400313D RID: 12605
		private InstanceMethodMetadata methodField;

		// Token: 0x0400313E RID: 12606
		private GetCmdletParameters getCmdletParametersField;
	}
}
