using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009AE RID: 2478
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://schemas.microsoft.com/cmdlets-over-objects/2009/11")]
	[GeneratedCode("xsd", "4.0.30319.17929")]
	[DebuggerStepThrough]
	[Serializable]
	internal class TypeMetadata
	{
		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x001EC81D File Offset: 0x001EAA1D
		// (set) Token: 0x06005B62 RID: 23394 RVA: 0x001EC825 File Offset: 0x001EAA25
		[XmlAttribute]
		public string PSType
		{
			get
			{
				return this.pSTypeField;
			}
			set
			{
				this.pSTypeField = value;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06005B63 RID: 23395 RVA: 0x001EC82E File Offset: 0x001EAA2E
		// (set) Token: 0x06005B64 RID: 23396 RVA: 0x001EC836 File Offset: 0x001EAA36
		[XmlAttribute]
		public string ETSType
		{
			get
			{
				return this.eTSTypeField;
			}
			set
			{
				this.eTSTypeField = value;
			}
		}

		// Token: 0x040030ED RID: 12525
		private string pSTypeField;

		// Token: 0x040030EE RID: 12526
		private string eTSTypeField;
	}
}
