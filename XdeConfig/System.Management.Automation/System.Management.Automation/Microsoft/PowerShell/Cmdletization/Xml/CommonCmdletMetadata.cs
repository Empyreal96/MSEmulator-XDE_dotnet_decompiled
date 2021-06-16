using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009BC RID: 2492
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[DesignerCategory("code")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CommonCmdletMetadata
	{
		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06005BCF RID: 23503 RVA: 0x001ECBBD File Offset: 0x001EADBD
		// (set) Token: 0x06005BD0 RID: 23504 RVA: 0x001ECBC5 File Offset: 0x001EADC5
		public ObsoleteAttributeMetadata Obsolete
		{
			get
			{
				return this.obsoleteField;
			}
			set
			{
				this.obsoleteField = value;
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x001ECBCE File Offset: 0x001EADCE
		// (set) Token: 0x06005BD2 RID: 23506 RVA: 0x001ECBD6 File Offset: 0x001EADD6
		[XmlAttribute]
		public string Verb
		{
			get
			{
				return this.verbField;
			}
			set
			{
				this.verbField = value;
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x001ECBDF File Offset: 0x001EADDF
		// (set) Token: 0x06005BD4 RID: 23508 RVA: 0x001ECBE7 File Offset: 0x001EADE7
		[XmlAttribute]
		public string Noun
		{
			get
			{
				return this.nounField;
			}
			set
			{
				this.nounField = value;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x001ECBF0 File Offset: 0x001EADF0
		// (set) Token: 0x06005BD6 RID: 23510 RVA: 0x001ECBF8 File Offset: 0x001EADF8
		[XmlAttribute]
		public string[] Aliases
		{
			get
			{
				return this.aliasesField;
			}
			set
			{
				this.aliasesField = value;
			}
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x001ECC01 File Offset: 0x001EAE01
		// (set) Token: 0x06005BD8 RID: 23512 RVA: 0x001ECC09 File Offset: 0x001EAE09
		[XmlAttribute]
		public ConfirmImpact ConfirmImpact
		{
			get
			{
				return this.confirmImpactField;
			}
			set
			{
				this.confirmImpactField = value;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x001ECC12 File Offset: 0x001EAE12
		// (set) Token: 0x06005BDA RID: 23514 RVA: 0x001ECC1A File Offset: 0x001EAE1A
		[XmlIgnore]
		public bool ConfirmImpactSpecified
		{
			get
			{
				return this.confirmImpactFieldSpecified;
			}
			set
			{
				this.confirmImpactFieldSpecified = value;
			}
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06005BDB RID: 23515 RVA: 0x001ECC23 File Offset: 0x001EAE23
		// (set) Token: 0x06005BDC RID: 23516 RVA: 0x001ECC2B File Offset: 0x001EAE2B
		[XmlAttribute(DataType = "anyURI")]
		public string HelpUri
		{
			get
			{
				return this.helpUriField;
			}
			set
			{
				this.helpUriField = value;
			}
		}

		// Token: 0x0400311D RID: 12573
		private ObsoleteAttributeMetadata obsoleteField;

		// Token: 0x0400311E RID: 12574
		private string verbField;

		// Token: 0x0400311F RID: 12575
		private string nounField;

		// Token: 0x04003120 RID: 12576
		private string[] aliasesField;

		// Token: 0x04003121 RID: 12577
		private ConfirmImpact confirmImpactField;

		// Token: 0x04003122 RID: 12578
		private bool confirmImpactFieldSpecified;

		// Token: 0x04003123 RID: 12579
		private string helpUriField;
	}
}
