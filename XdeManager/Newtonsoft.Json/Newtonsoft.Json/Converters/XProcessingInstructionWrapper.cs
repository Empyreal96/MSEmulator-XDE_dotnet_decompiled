using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F4 RID: 244
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00033585 File Offset: 0x00031785
		private XProcessingInstruction ProcessingInstruction
		{
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00033592 File Offset: 0x00031792
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : base(processingInstruction)
		{
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0003359B File Offset: 0x0003179B
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000335A8 File Offset: 0x000317A8
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x000335B5 File Offset: 0x000317B5
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value;
			}
		}
	}
}
