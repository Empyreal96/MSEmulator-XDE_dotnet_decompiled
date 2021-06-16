using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009CD RID: 2509
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class EnumMetadataEnum
	{
		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06005C22 RID: 23586 RVA: 0x001ECE77 File Offset: 0x001EB077
		// (set) Token: 0x06005C23 RID: 23587 RVA: 0x001ECE7F File Offset: 0x001EB07F
		[XmlElement("Value")]
		public EnumMetadataEnumValue[] Value
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

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06005C24 RID: 23588 RVA: 0x001ECE88 File Offset: 0x001EB088
		// (set) Token: 0x06005C25 RID: 23589 RVA: 0x001ECE90 File Offset: 0x001EB090
		[XmlAttribute]
		public string EnumName
		{
			get
			{
				return this.enumNameField;
			}
			set
			{
				this.enumNameField = value;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06005C26 RID: 23590 RVA: 0x001ECE99 File Offset: 0x001EB099
		// (set) Token: 0x06005C27 RID: 23591 RVA: 0x001ECEA1 File Offset: 0x001EB0A1
		[XmlAttribute]
		public string UnderlyingType
		{
			get
			{
				return this.underlyingTypeField;
			}
			set
			{
				this.underlyingTypeField = value;
			}
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06005C28 RID: 23592 RVA: 0x001ECEAA File Offset: 0x001EB0AA
		// (set) Token: 0x06005C29 RID: 23593 RVA: 0x001ECEB2 File Offset: 0x001EB0B2
		[XmlAttribute]
		public bool BitwiseFlags
		{
			get
			{
				return this.bitwiseFlagsField;
			}
			set
			{
				this.bitwiseFlagsField = value;
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06005C2A RID: 23594 RVA: 0x001ECEBB File Offset: 0x001EB0BB
		// (set) Token: 0x06005C2B RID: 23595 RVA: 0x001ECEC3 File Offset: 0x001EB0C3
		[XmlIgnore]
		public bool BitwiseFlagsSpecified
		{
			get
			{
				return this.bitwiseFlagsFieldSpecified;
			}
			set
			{
				this.bitwiseFlagsFieldSpecified = value;
			}
		}

		// Token: 0x04003149 RID: 12617
		private EnumMetadataEnumValue[] valueField;

		// Token: 0x0400314A RID: 12618
		private string enumNameField;

		// Token: 0x0400314B RID: 12619
		private string underlyingTypeField;

		// Token: 0x0400314C RID: 12620
		private bool bitwiseFlagsField;

		// Token: 0x0400314D RID: 12621
		private bool bitwiseFlagsFieldSpecified;
	}
}
