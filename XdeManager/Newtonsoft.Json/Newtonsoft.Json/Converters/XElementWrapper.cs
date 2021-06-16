using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F8 RID: 248
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0003381C File Offset: 0x00031A1C
		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00033829 File Offset: 0x00031A29
		public XElementWrapper(XElement element) : base(element)
		{
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00033834 File Offset: 0x00031A34
		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xobjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xobjectWrapper.WrappedNode);
			this._attributes = null;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00033860 File Offset: 0x00031A60
		public override List<IXmlNode> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					if (!this.Element.HasAttributes && !this.HasImplicitNamespaceAttribute(this.NamespaceUri))
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>();
						foreach (XAttribute attribute in this.Element.Attributes())
						{
							this._attributes.Add(new XAttributeWrapper(attribute));
						}
						string namespaceUri = this.NamespaceUri;
						if (this.HasImplicitNamespaceAttribute(namespaceUri))
						{
							this._attributes.Insert(0, new XAttributeWrapper(new XAttribute("xmlns", namespaceUri)));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00033934 File Offset: 0x00031B34
		private bool HasImplicitNamespaceAttribute(string namespaceUri)
		{
			if (!string.IsNullOrEmpty(namespaceUri))
			{
				IXmlNode parentNode = this.ParentNode;
				if (namespaceUri != ((parentNode != null) ? parentNode.NamespaceUri : null) && string.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
				{
					bool flag = false;
					if (this.Element.HasAttributes)
					{
						foreach (XAttribute xattribute in this.Element.Attributes())
						{
							if (xattribute.Name.LocalName == "xmlns" && string.IsNullOrEmpty(xattribute.Name.NamespaceName) && xattribute.Value == namespaceUri)
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00033A04 File Offset: 0x00031C04
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			IXmlNode result = base.AppendChild(newChild);
			this._attributes = null;
			return result;
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00033A14 File Offset: 0x00031C14
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00033A21 File Offset: 0x00031C21
		public override string Value
		{
			get
			{
				return this.Element.Value;
			}
			set
			{
				this.Element.Value = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00033A2F File Offset: 0x00031C2F
		public override string LocalName
		{
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x00033A41 File Offset: 0x00031C41
		public override string NamespaceUri
		{
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00033A53 File Offset: 0x00031C53
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this.Element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x00033A66 File Offset: 0x00031C66
		public bool IsEmpty
		{
			get
			{
				return this.Element.IsEmpty;
			}
		}

		// Token: 0x040003EC RID: 1004
		private List<IXmlNode> _attributes;
	}
}
