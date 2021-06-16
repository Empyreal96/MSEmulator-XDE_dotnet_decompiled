using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F7 RID: 247
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000337A6 File Offset: 0x000319A6
		private XAttribute Attribute
		{
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000337B3 File Offset: 0x000319B3
		public XAttributeWrapper(XAttribute attribute) : base(attribute)
		{
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000337BC File Offset: 0x000319BC
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x000337C9 File Offset: 0x000319C9
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x000337D7 File Offset: 0x000319D7
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000337E9 File Offset: 0x000319E9
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x000337FB File Offset: 0x000319FB
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
