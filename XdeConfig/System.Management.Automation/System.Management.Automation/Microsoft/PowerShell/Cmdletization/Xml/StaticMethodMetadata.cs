using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C1 RID: 2497
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[Serializable]
	internal class StaticMethodMetadata : CommonMethodMetadata
	{
		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06005BEB RID: 23531 RVA: 0x001ECCA9 File Offset: 0x001EAEA9
		// (set) Token: 0x06005BEC RID: 23532 RVA: 0x001ECCB1 File Offset: 0x001EAEB1
		[XmlArrayItem("Parameter", IsNullable = false)]
		public StaticMethodParameterMetadata[] Parameters
		{
			get
			{
				return this.parametersField;
			}
			set
			{
				this.parametersField = value;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x001ECCBA File Offset: 0x001EAEBA
		// (set) Token: 0x06005BEE RID: 23534 RVA: 0x001ECCC2 File Offset: 0x001EAEC2
		[XmlAttribute]
		public string CmdletParameterSet
		{
			get
			{
				return this.cmdletParameterSetField;
			}
			set
			{
				this.cmdletParameterSetField = value;
			}
		}

		// Token: 0x0400312E RID: 12590
		private StaticMethodParameterMetadata[] parametersField;

		// Token: 0x0400312F RID: 12591
		private string cmdletParameterSetField;
	}
}
