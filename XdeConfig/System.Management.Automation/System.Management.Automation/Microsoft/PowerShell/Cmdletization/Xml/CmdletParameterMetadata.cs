using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009B1 RID: 2481
	[XmlInclude(typeof(CmdletParameterMetadataForInstanceMethodParameter))]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[XmlInclude(typeof(CmdletParameterMetadataForStaticMethodParameter))]
	[DesignerCategory("code")]
	[XmlInclude(typeof(CmdletParameterMetadataForGetCmdletFilteringParameter))]
	[XmlInclude(typeof(CmdletParameterMetadataForGetCmdletParameter))]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class CmdletParameterMetadata
	{
		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06005B74 RID: 23412 RVA: 0x001EC8BD File Offset: 0x001EAABD
		// (set) Token: 0x06005B75 RID: 23413 RVA: 0x001EC8C5 File Offset: 0x001EAAC5
		public object AllowEmptyCollection
		{
			get
			{
				return this.allowEmptyCollectionField;
			}
			set
			{
				this.allowEmptyCollectionField = value;
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x001EC8CE File Offset: 0x001EAACE
		// (set) Token: 0x06005B77 RID: 23415 RVA: 0x001EC8D6 File Offset: 0x001EAAD6
		public object AllowEmptyString
		{
			get
			{
				return this.allowEmptyStringField;
			}
			set
			{
				this.allowEmptyStringField = value;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06005B78 RID: 23416 RVA: 0x001EC8DF File Offset: 0x001EAADF
		// (set) Token: 0x06005B79 RID: 23417 RVA: 0x001EC8E7 File Offset: 0x001EAAE7
		public object AllowNull
		{
			get
			{
				return this.allowNullField;
			}
			set
			{
				this.allowNullField = value;
			}
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06005B7A RID: 23418 RVA: 0x001EC8F0 File Offset: 0x001EAAF0
		// (set) Token: 0x06005B7B RID: 23419 RVA: 0x001EC8F8 File Offset: 0x001EAAF8
		public object ValidateNotNull
		{
			get
			{
				return this.validateNotNullField;
			}
			set
			{
				this.validateNotNullField = value;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06005B7C RID: 23420 RVA: 0x001EC901 File Offset: 0x001EAB01
		// (set) Token: 0x06005B7D RID: 23421 RVA: 0x001EC909 File Offset: 0x001EAB09
		public object ValidateNotNullOrEmpty
		{
			get
			{
				return this.validateNotNullOrEmptyField;
			}
			set
			{
				this.validateNotNullOrEmptyField = value;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06005B7E RID: 23422 RVA: 0x001EC912 File Offset: 0x001EAB12
		// (set) Token: 0x06005B7F RID: 23423 RVA: 0x001EC91A File Offset: 0x001EAB1A
		public CmdletParameterMetadataValidateCount ValidateCount
		{
			get
			{
				return this.validateCountField;
			}
			set
			{
				this.validateCountField = value;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x001EC923 File Offset: 0x001EAB23
		// (set) Token: 0x06005B81 RID: 23425 RVA: 0x001EC92B File Offset: 0x001EAB2B
		public CmdletParameterMetadataValidateLength ValidateLength
		{
			get
			{
				return this.validateLengthField;
			}
			set
			{
				this.validateLengthField = value;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06005B82 RID: 23426 RVA: 0x001EC934 File Offset: 0x001EAB34
		// (set) Token: 0x06005B83 RID: 23427 RVA: 0x001EC93C File Offset: 0x001EAB3C
		public CmdletParameterMetadataValidateRange ValidateRange
		{
			get
			{
				return this.validateRangeField;
			}
			set
			{
				this.validateRangeField = value;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06005B84 RID: 23428 RVA: 0x001EC945 File Offset: 0x001EAB45
		// (set) Token: 0x06005B85 RID: 23429 RVA: 0x001EC94D File Offset: 0x001EAB4D
		[XmlArrayItem("AllowedValue", IsNullable = false)]
		public string[] ValidateSet
		{
			get
			{
				return this.validateSetField;
			}
			set
			{
				this.validateSetField = value;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x001EC956 File Offset: 0x001EAB56
		// (set) Token: 0x06005B87 RID: 23431 RVA: 0x001EC95E File Offset: 0x001EAB5E
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

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x001EC967 File Offset: 0x001EAB67
		// (set) Token: 0x06005B89 RID: 23433 RVA: 0x001EC96F File Offset: 0x001EAB6F
		[XmlAttribute]
		public bool IsMandatory
		{
			get
			{
				return this.isMandatoryField;
			}
			set
			{
				this.isMandatoryField = value;
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06005B8A RID: 23434 RVA: 0x001EC978 File Offset: 0x001EAB78
		// (set) Token: 0x06005B8B RID: 23435 RVA: 0x001EC980 File Offset: 0x001EAB80
		[XmlIgnore]
		public bool IsMandatorySpecified
		{
			get
			{
				return this.isMandatoryFieldSpecified;
			}
			set
			{
				this.isMandatoryFieldSpecified = value;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06005B8C RID: 23436 RVA: 0x001EC989 File Offset: 0x001EAB89
		// (set) Token: 0x06005B8D RID: 23437 RVA: 0x001EC991 File Offset: 0x001EAB91
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

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06005B8E RID: 23438 RVA: 0x001EC99A File Offset: 0x001EAB9A
		// (set) Token: 0x06005B8F RID: 23439 RVA: 0x001EC9A2 File Offset: 0x001EABA2
		[XmlAttribute]
		public string PSName
		{
			get
			{
				return this.pSNameField;
			}
			set
			{
				this.pSNameField = value;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06005B90 RID: 23440 RVA: 0x001EC9AB File Offset: 0x001EABAB
		// (set) Token: 0x06005B91 RID: 23441 RVA: 0x001EC9B3 File Offset: 0x001EABB3
		[XmlAttribute(DataType = "nonNegativeInteger")]
		public string Position
		{
			get
			{
				return this.positionField;
			}
			set
			{
				this.positionField = value;
			}
		}

		// Token: 0x040030F5 RID: 12533
		private object allowEmptyCollectionField;

		// Token: 0x040030F6 RID: 12534
		private object allowEmptyStringField;

		// Token: 0x040030F7 RID: 12535
		private object allowNullField;

		// Token: 0x040030F8 RID: 12536
		private object validateNotNullField;

		// Token: 0x040030F9 RID: 12537
		private object validateNotNullOrEmptyField;

		// Token: 0x040030FA RID: 12538
		private CmdletParameterMetadataValidateCount validateCountField;

		// Token: 0x040030FB RID: 12539
		private CmdletParameterMetadataValidateLength validateLengthField;

		// Token: 0x040030FC RID: 12540
		private CmdletParameterMetadataValidateRange validateRangeField;

		// Token: 0x040030FD RID: 12541
		private string[] validateSetField;

		// Token: 0x040030FE RID: 12542
		private ObsoleteAttributeMetadata obsoleteField;

		// Token: 0x040030FF RID: 12543
		private bool isMandatoryField;

		// Token: 0x04003100 RID: 12544
		private bool isMandatoryFieldSpecified;

		// Token: 0x04003101 RID: 12545
		private string[] aliasesField;

		// Token: 0x04003102 RID: 12546
		private string pSNameField;

		// Token: 0x04003103 RID: 12547
		private string positionField;
	}
}
