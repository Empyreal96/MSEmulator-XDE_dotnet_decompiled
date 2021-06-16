using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F3 RID: 243
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00033533 File Offset: 0x00031733
		private XComment Text
		{
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00033540 File Offset: 0x00031740
		public XCommentWrapper(XComment text) : base(text)
		{
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00033549 File Offset: 0x00031749
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x00033556 File Offset: 0x00031756
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

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00033564 File Offset: 0x00031764
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
