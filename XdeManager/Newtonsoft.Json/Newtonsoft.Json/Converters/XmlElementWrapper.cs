using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E6 RID: 230
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x00032EE1 File Offset: 0x000310E1
		public XmlElementWrapper(XmlElement element) : base(element)
		{
			this._element = element;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00032EF4 File Offset: 0x000310F4
		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00032F1F File Offset: 0x0003111F
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00032F2D File Offset: 0x0003112D
		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		// Token: 0x040003E2 RID: 994
		private readonly XmlElement _element;
	}
}
