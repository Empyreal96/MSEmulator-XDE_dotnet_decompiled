using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009AF RID: 2479
	[DebuggerStepThrough]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[Serializable]
	internal class Association
	{
		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06005B66 RID: 23398 RVA: 0x001EC847 File Offset: 0x001EAA47
		// (set) Token: 0x06005B67 RID: 23399 RVA: 0x001EC84F File Offset: 0x001EAA4F
		public AssociationAssociatedInstance AssociatedInstance
		{
			get
			{
				return this.associatedInstanceField;
			}
			set
			{
				this.associatedInstanceField = value;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x001EC858 File Offset: 0x001EAA58
		// (set) Token: 0x06005B69 RID: 23401 RVA: 0x001EC860 File Offset: 0x001EAA60
		[XmlAttribute("Association")]
		public string Association1
		{
			get
			{
				return this.association1Field;
			}
			set
			{
				this.association1Field = value;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06005B6A RID: 23402 RVA: 0x001EC869 File Offset: 0x001EAA69
		// (set) Token: 0x06005B6B RID: 23403 RVA: 0x001EC871 File Offset: 0x001EAA71
		[XmlAttribute]
		public string SourceRole
		{
			get
			{
				return this.sourceRoleField;
			}
			set
			{
				this.sourceRoleField = value;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06005B6C RID: 23404 RVA: 0x001EC87A File Offset: 0x001EAA7A
		// (set) Token: 0x06005B6D RID: 23405 RVA: 0x001EC882 File Offset: 0x001EAA82
		[XmlAttribute]
		public string ResultRole
		{
			get
			{
				return this.resultRoleField;
			}
			set
			{
				this.resultRoleField = value;
			}
		}

		// Token: 0x040030EF RID: 12527
		private AssociationAssociatedInstance associatedInstanceField;

		// Token: 0x040030F0 RID: 12528
		private string association1Field;

		// Token: 0x040030F1 RID: 12529
		private string sourceRoleField;

		// Token: 0x040030F2 RID: 12530
		private string resultRoleField;
	}
}
