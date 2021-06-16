using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009C0 RID: 2496
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[XmlInclude(typeof(StaticMethodMetadata))]
	[DesignerCategory("code")]
	[XmlInclude(typeof(InstanceMethodMetadata))]
	[DebuggerStepThrough]
	[Serializable]
	internal class CommonMethodMetadata
	{
		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x001ECC7F File Offset: 0x001EAE7F
		// (set) Token: 0x06005BE7 RID: 23527 RVA: 0x001ECC87 File Offset: 0x001EAE87
		public CommonMethodMetadataReturnValue ReturnValue
		{
			get
			{
				return this.returnValueField;
			}
			set
			{
				this.returnValueField = value;
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x001ECC90 File Offset: 0x001EAE90
		// (set) Token: 0x06005BE9 RID: 23529 RVA: 0x001ECC98 File Offset: 0x001EAE98
		[XmlAttribute]
		public string MethodName
		{
			get
			{
				return this.methodNameField;
			}
			set
			{
				this.methodNameField = value;
			}
		}

		// Token: 0x0400312C RID: 12588
		private CommonMethodMetadataReturnValue returnValueField;

		// Token: 0x0400312D RID: 12589
		private string methodNameField;
	}
}
