using System;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EF RID: 239
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0003329A File Offset: 0x0003149A
		internal XDeclaration Declaration { get; }

		// Token: 0x06000CCC RID: 3276 RVA: 0x000332A2 File Offset: 0x000314A2
		public XDeclarationWrapper(XDeclaration declaration) : base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x000332B2 File Offset: 0x000314B2
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000332B6 File Offset: 0x000314B6
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x000332C3 File Offset: 0x000314C3
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x000332D0 File Offset: 0x000314D0
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x000332DE File Offset: 0x000314DE
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x000332EB File Offset: 0x000314EB
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
