using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F6 RID: 246
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x06000D01 RID: 3329 RVA: 0x0003375A File Offset: 0x0003195A
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00033769 File Offset: 0x00031969
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00033771 File Offset: 0x00031971
		public virtual XmlNodeType NodeType
		{
			get
			{
				return this._xmlObject.NodeType;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0003377E File Offset: 0x0003197E
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00033781 File Offset: 0x00031981
		public virtual List<IXmlNode> ChildNodes
		{
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00033788 File Offset: 0x00031988
		public virtual List<IXmlNode> Attributes
		{
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0003378F File Offset: 0x0003198F
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00033792 File Offset: 0x00031992
		// (set) Token: 0x06000D09 RID: 3337 RVA: 0x00033795 File Offset: 0x00031995
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0003379C File Offset: 0x0003199C
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x000337A3 File Offset: 0x000319A3
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040003EB RID: 1003
		private readonly XObject _xmlObject;
	}
}
