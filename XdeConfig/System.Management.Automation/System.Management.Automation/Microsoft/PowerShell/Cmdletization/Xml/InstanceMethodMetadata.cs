using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C7 RID: 2503
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class InstanceMethodMetadata : CommonMethodMetadata
	{
		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06005C0B RID: 23563 RVA: 0x001ECDB6 File Offset: 0x001EAFB6
		// (set) Token: 0x06005C0C RID: 23564 RVA: 0x001ECDBE File Offset: 0x001EAFBE
		[XmlArrayItem("Parameter", IsNullable = false)]
		public InstanceMethodParameterMetadata[] Parameters
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

		// Token: 0x0400313B RID: 12603
		private InstanceMethodParameterMetadata[] parametersField;
	}
}
