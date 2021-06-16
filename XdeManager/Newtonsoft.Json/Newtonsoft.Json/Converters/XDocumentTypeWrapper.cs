using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F0 RID: 240
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000CD3 RID: 3283 RVA: 0x000332F9 File Offset: 0x000314F9
		public XDocumentTypeWrapper(XDocumentType documentType) : base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00033309 File Offset: 0x00031509
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00033316 File Offset: 0x00031516
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00033323 File Offset: 0x00031523
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00033330 File Offset: 0x00031530
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0003333D File Offset: 0x0003153D
		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040003E9 RID: 1001
		private readonly XDocumentType _documentType;
	}
}
