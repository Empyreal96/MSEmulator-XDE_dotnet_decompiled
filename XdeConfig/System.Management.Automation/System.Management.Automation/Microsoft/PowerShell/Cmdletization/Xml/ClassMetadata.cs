using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009AA RID: 2474
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[Serializable]
	internal class ClassMetadata
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x001EC6BA File Offset: 0x001EA8BA
		// (set) Token: 0x06005B38 RID: 23352 RVA: 0x001EC6C2 File Offset: 0x001EA8C2
		public string Version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06005B39 RID: 23353 RVA: 0x001EC6CB File Offset: 0x001EA8CB
		// (set) Token: 0x06005B3A RID: 23354 RVA: 0x001EC6D3 File Offset: 0x001EA8D3
		public string DefaultNoun
		{
			get
			{
				return this.defaultNounField;
			}
			set
			{
				this.defaultNounField = value;
			}
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06005B3B RID: 23355 RVA: 0x001EC6DC File Offset: 0x001EA8DC
		// (set) Token: 0x06005B3C RID: 23356 RVA: 0x001EC6E4 File Offset: 0x001EA8E4
		public ClassMetadataInstanceCmdlets InstanceCmdlets
		{
			get
			{
				return this.instanceCmdletsField;
			}
			set
			{
				this.instanceCmdletsField = value;
			}
		}

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x06005B3D RID: 23357 RVA: 0x001EC6ED File Offset: 0x001EA8ED
		// (set) Token: 0x06005B3E RID: 23358 RVA: 0x001EC6F5 File Offset: 0x001EA8F5
		[XmlArrayItem("Cmdlet", IsNullable = false)]
		public StaticCmdletMetadata[] StaticCmdlets
		{
			get
			{
				return this.staticCmdletsField;
			}
			set
			{
				this.staticCmdletsField = value;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x001EC6FE File Offset: 0x001EA8FE
		// (set) Token: 0x06005B40 RID: 23360 RVA: 0x001EC706 File Offset: 0x001EA906
		[XmlArrayItem("Data", IsNullable = false)]
		public ClassMetadataData[] CmdletAdapterPrivateData
		{
			get
			{
				return this.cmdletAdapterPrivateDataField;
			}
			set
			{
				this.cmdletAdapterPrivateDataField = value;
			}
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06005B41 RID: 23361 RVA: 0x001EC70F File Offset: 0x001EA90F
		// (set) Token: 0x06005B42 RID: 23362 RVA: 0x001EC717 File Offset: 0x001EA917
		[XmlAttribute]
		public string CmdletAdapter
		{
			get
			{
				return this.cmdletAdapterField;
			}
			set
			{
				this.cmdletAdapterField = value;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06005B43 RID: 23363 RVA: 0x001EC720 File Offset: 0x001EA920
		// (set) Token: 0x06005B44 RID: 23364 RVA: 0x001EC728 File Offset: 0x001EA928
		[XmlAttribute]
		public string ClassName
		{
			get
			{
				return this.classNameField;
			}
			set
			{
				this.classNameField = value;
			}
		}

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06005B45 RID: 23365 RVA: 0x001EC731 File Offset: 0x001EA931
		// (set) Token: 0x06005B46 RID: 23366 RVA: 0x001EC739 File Offset: 0x001EA939
		[XmlAttribute]
		public string ClassVersion
		{
			get
			{
				return this.classVersionField;
			}
			set
			{
				this.classVersionField = value;
			}
		}

		// Token: 0x040030DA RID: 12506
		private string versionField;

		// Token: 0x040030DB RID: 12507
		private string defaultNounField;

		// Token: 0x040030DC RID: 12508
		private ClassMetadataInstanceCmdlets instanceCmdletsField;

		// Token: 0x040030DD RID: 12509
		private StaticCmdletMetadata[] staticCmdletsField;

		// Token: 0x040030DE RID: 12510
		private ClassMetadataData[] cmdletAdapterPrivateDataField;

		// Token: 0x040030DF RID: 12511
		private string cmdletAdapterField;

		// Token: 0x040030E0 RID: 12512
		private string classNameField;

		// Token: 0x040030E1 RID: 12513
		private string classVersionField;
	}
}
