using System;
using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E9 RID: 233
	internal class XmlNodeWrapper : IXmlNode
	{
		// Token: 0x06000C9A RID: 3226 RVA: 0x00032FD8 File Offset: 0x000311D8
		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00032FE7 File Offset: 0x000311E7
		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00032FEF File Offset: 0x000311EF
		public XmlNodeType NodeType
		{
			get
			{
				return this._node.NodeType;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x00032FFC File Offset: 0x000311FC
		public virtual string LocalName
		{
			get
			{
				return this._node.LocalName;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0003300C File Offset: 0x0003120C
		public List<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					if (!this._node.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
						foreach (object obj in this._node.ChildNodes)
						{
							XmlNode node = (XmlNode)obj;
							this._childNodes.Add(XmlNodeWrapper.WrapNode(node));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x000330BC File Offset: 0x000312BC
		protected virtual bool HasChildNodes
		{
			get
			{
				return this._node.HasChildNodes;
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000330CC File Offset: 0x000312CC
		internal static IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType == XmlNodeType.DocumentType)
			{
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			}
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x0003311C File Offset: 0x0003131C
		public List<IXmlNode> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					if (!this.HasAttributes)
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
						foreach (object obj in this._node.Attributes)
						{
							XmlAttribute node = (XmlAttribute)obj;
							this._attributes.Add(XmlNodeWrapper.WrapNode(node));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x000331C4 File Offset: 0x000313C4
		private bool HasAttributes
		{
			get
			{
				XmlElement xmlElement;
				if ((xmlElement = (this._node as XmlElement)) != null)
				{
					return xmlElement.HasAttributes;
				}
				XmlAttributeCollection attributes = this._node.Attributes;
				return attributes != null && attributes.Count > 0;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x00033200 File Offset: 0x00031400
		public IXmlNode ParentNode
		{
			get
			{
				XmlAttribute xmlAttribute;
				XmlNode xmlNode = ((xmlAttribute = (this._node as XmlAttribute)) != null) ? xmlAttribute.OwnerElement : this._node.ParentNode;
				if (xmlNode == null)
				{
					return null;
				}
				return XmlNodeWrapper.WrapNode(xmlNode);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0003323B File Offset: 0x0003143B
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x00033248 File Offset: 0x00031448
		public string Value
		{
			get
			{
				return this._node.Value;
			}
			set
			{
				this._node.Value = value;
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00033258 File Offset: 0x00031458
		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			this._childNodes = null;
			this._attributes = null;
			return newChild;
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0003328D File Offset: 0x0003148D
		public string NamespaceUri
		{
			get
			{
				return this._node.NamespaceURI;
			}
		}

		// Token: 0x040003E5 RID: 997
		private readonly XmlNode _node;

		// Token: 0x040003E6 RID: 998
		private List<IXmlNode> _childNodes;

		// Token: 0x040003E7 RID: 999
		private List<IXmlNode> _attributes;
	}
}
