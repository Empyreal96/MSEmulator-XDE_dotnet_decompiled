using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F5 RID: 245
	internal class XContainerWrapper : XObjectWrapper
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000335C3 File Offset: 0x000317C3
		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x000335D0 File Offset: 0x000317D0
		public XContainerWrapper(XContainer container) : base(container)
		{
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x000335DC File Offset: 0x000317DC
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					if (!this.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>();
						foreach (XNode node in this.Container.Nodes())
						{
							this._childNodes.Add(XContainerWrapper.WrapNode(node));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00033668 File Offset: 0x00031868
		protected virtual bool HasChildNodes
		{
			get
			{
				return this.Container.LastNode != null;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00033678 File Offset: 0x00031878
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0003369C File Offset: 0x0003189C
		internal static IXmlNode WrapNode(XObject node)
		{
			XDocument document;
			if ((document = (node as XDocument)) != null)
			{
				return new XDocumentWrapper(document);
			}
			XElement element;
			if ((element = (node as XElement)) != null)
			{
				return new XElementWrapper(element);
			}
			XContainer container;
			if ((container = (node as XContainer)) != null)
			{
				return new XContainerWrapper(container);
			}
			XProcessingInstruction processingInstruction;
			if ((processingInstruction = (node as XProcessingInstruction)) != null)
			{
				return new XProcessingInstructionWrapper(processingInstruction);
			}
			XText text;
			if ((text = (node as XText)) != null)
			{
				return new XTextWrapper(text);
			}
			XComment text2;
			if ((text2 = (node as XComment)) != null)
			{
				return new XCommentWrapper(text2);
			}
			XAttribute attribute;
			if ((attribute = (node as XAttribute)) != null)
			{
				return new XAttributeWrapper(attribute);
			}
			XDocumentType documentType;
			if ((documentType = (node as XDocumentType)) != null)
			{
				return new XDocumentTypeWrapper(documentType);
			}
			return new XObjectWrapper(node);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0003373F File Offset: 0x0003193F
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			this._childNodes = null;
			return newChild;
		}

		// Token: 0x040003EA RID: 1002
		private List<IXmlNode> _childNodes;
	}
}
