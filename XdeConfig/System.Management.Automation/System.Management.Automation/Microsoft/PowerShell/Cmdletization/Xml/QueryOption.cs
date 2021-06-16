using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009BA RID: 2490
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[Serializable]
	internal class QueryOption
	{
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06005BC3 RID: 23491 RVA: 0x001ECB58 File Offset: 0x001EAD58
		// (set) Token: 0x06005BC4 RID: 23492 RVA: 0x001ECB60 File Offset: 0x001EAD60
		public TypeMetadata Type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06005BC5 RID: 23493 RVA: 0x001ECB69 File Offset: 0x001EAD69
		// (set) Token: 0x06005BC6 RID: 23494 RVA: 0x001ECB71 File Offset: 0x001EAD71
		public CmdletParameterMetadataForGetCmdletParameter CmdletParameterMetadata
		{
			get
			{
				return this.cmdletParameterMetadataField;
			}
			set
			{
				this.cmdletParameterMetadataField = value;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06005BC7 RID: 23495 RVA: 0x001ECB7A File Offset: 0x001EAD7A
		// (set) Token: 0x06005BC8 RID: 23496 RVA: 0x001ECB82 File Offset: 0x001EAD82
		[XmlAttribute]
		public string OptionName
		{
			get
			{
				return this.optionNameField;
			}
			set
			{
				this.optionNameField = value;
			}
		}

		// Token: 0x04003118 RID: 12568
		private TypeMetadata typeField;

		// Token: 0x04003119 RID: 12569
		private CmdletParameterMetadataForGetCmdletParameter cmdletParameterMetadataField;

		// Token: 0x0400311A RID: 12570
		private string optionNameField;
	}
}
