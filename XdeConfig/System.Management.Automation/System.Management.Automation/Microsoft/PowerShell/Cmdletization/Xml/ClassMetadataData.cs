using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009CC RID: 2508
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class ClassMetadataData
	{
		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06005C1D RID: 23581 RVA: 0x001ECE4D File Offset: 0x001EB04D
		// (set) Token: 0x06005C1E RID: 23582 RVA: 0x001ECE55 File Offset: 0x001EB055
		[XmlAttribute]
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06005C1F RID: 23583 RVA: 0x001ECE5E File Offset: 0x001EB05E
		// (set) Token: 0x06005C20 RID: 23584 RVA: 0x001ECE66 File Offset: 0x001EB066
		[XmlText]
		public string Value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}

		// Token: 0x04003147 RID: 12615
		private string nameField;

		// Token: 0x04003148 RID: 12616
		private string valueField;
	}
}
