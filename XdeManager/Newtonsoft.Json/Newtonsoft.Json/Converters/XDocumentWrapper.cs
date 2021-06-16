using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F1 RID: 241
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00033344 File Offset: 0x00031544
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00033351 File Offset: 0x00031551
		public XDocumentWrapper(XDocument document) : base(document)
		{
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0003335C File Offset: 0x0003155C
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				List<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && (childNodes.Count == 0 || childNodes[0].NodeType != XmlNodeType.XmlDeclaration))
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000333AD File Offset: 0x000315AD
		protected override bool HasChildNodes
		{
			get
			{
				return base.HasChildNodes || this.Document.Declaration != null;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x000333C7 File Offset: 0x000315C7
		public IXmlNode CreateComment(string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000333D4 File Offset: 0x000315D4
		public IXmlNode CreateTextNode(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x000333E1 File Offset: 0x000315E1
		public IXmlNode CreateCDataSection(string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000333EE File Offset: 0x000315EE
		public IXmlNode CreateWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000333FB File Offset: 0x000315FB
		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00033408 File Offset: 0x00031608
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00033417 File Offset: 0x00031617
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00033428 File Offset: 0x00031628
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00033436 File Offset: 0x00031636
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00033448 File Offset: 0x00031648
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00033460 File Offset: 0x00031660
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00033473 File Offset: 0x00031673
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			return new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), value));
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0003348C File Offset: 0x0003168C
		public IXmlElement DocumentElement
		{
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x000334B0 File Offset: 0x000316B0
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper;
			if ((xdeclarationWrapper = (newChild as XDeclarationWrapper)) != null)
			{
				this.Document.Declaration = xdeclarationWrapper.Declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
