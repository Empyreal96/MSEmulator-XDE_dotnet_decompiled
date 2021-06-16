using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E5 RID: 229
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x06000C7C RID: 3196 RVA: 0x00032DB6 File Offset: 0x00030FB6
		public XmlDocumentWrapper(XmlDocument document) : base(document)
		{
			this._document = document;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00032DC6 File Offset: 0x00030FC6
		public IXmlNode CreateComment(string data)
		{
			return new XmlNodeWrapper(this._document.CreateComment(data));
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00032DD9 File Offset: 0x00030FD9
		public IXmlNode CreateTextNode(string text)
		{
			return new XmlNodeWrapper(this._document.CreateTextNode(text));
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00032DEC File Offset: 0x00030FEC
		public IXmlNode CreateCDataSection(string data)
		{
			return new XmlNodeWrapper(this._document.CreateCDataSection(data));
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00032DFF File Offset: 0x00030FFF
		public IXmlNode CreateWhitespace(string text)
		{
			return new XmlNodeWrapper(this._document.CreateWhitespace(text));
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00032E12 File Offset: 0x00031012
		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XmlNodeWrapper(this._document.CreateSignificantWhitespace(text));
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00032E25 File Offset: 0x00031025
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XmlDeclarationWrapper(this._document.CreateXmlDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00032E3A File Offset: 0x0003103A
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(this._document.CreateDocumentType(name, publicId, systemId, null));
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00032E50 File Offset: 0x00031050
		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XmlNodeWrapper(this._document.CreateProcessingInstruction(target, data));
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00032E64 File Offset: 0x00031064
		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(this._document.CreateElement(elementName));
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00032E77 File Offset: 0x00031077
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00032E8B File Offset: 0x0003108B
		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(name))
			{
				Value = value
			};
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00032EA5 File Offset: 0x000310A5
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			return new XmlNodeWrapper(this._document.CreateAttribute(qualifiedName, namespaceUri))
			{
				Value = value
			};
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00032EC0 File Offset: 0x000310C0
		public IXmlElement DocumentElement
		{
			get
			{
				if (this._document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(this._document.DocumentElement);
			}
		}

		// Token: 0x040003E1 RID: 993
		private readonly XmlDocument _document;
	}
}
