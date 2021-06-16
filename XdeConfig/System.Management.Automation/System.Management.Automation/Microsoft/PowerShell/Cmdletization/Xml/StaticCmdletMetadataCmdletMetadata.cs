using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009BF RID: 2495
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class StaticCmdletMetadataCmdletMetadata : CommonCmdletMetadata
	{
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06005BE3 RID: 23523 RVA: 0x001ECC66 File Offset: 0x001EAE66
		// (set) Token: 0x06005BE4 RID: 23524 RVA: 0x001ECC6E File Offset: 0x001EAE6E
		[XmlAttribute]
		public string DefaultCmdletParameterSet
		{
			get
			{
				return this.defaultCmdletParameterSetField;
			}
			set
			{
				this.defaultCmdletParameterSetField = value;
			}
		}

		// Token: 0x0400312B RID: 12587
		private string defaultCmdletParameterSetField;
	}
}
