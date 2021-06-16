using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E8 RID: 232
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x00032F8D File Offset: 0x0003118D
		public XmlDocumentTypeWrapper(XmlDocumentType documentType) : base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00032F9D File Offset: 0x0003119D
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00032FAA File Offset: 0x000311AA
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x00032FB7 File Offset: 0x000311B7
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00032FC4 File Offset: 0x000311C4
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00032FD1 File Offset: 0x000311D1
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040003E4 RID: 996
		private readonly XmlDocumentType _documentType;
	}
}
