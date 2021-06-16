using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F2 RID: 242
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000334E1 File Offset: 0x000316E1
		private XText Text
		{
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000334EE File Offset: 0x000316EE
		public XTextWrapper(XText text) : base(text)
		{
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x000334F7 File Offset: 0x000316F7
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x00033504 File Offset: 0x00031704
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00033512 File Offset: 0x00031712
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
