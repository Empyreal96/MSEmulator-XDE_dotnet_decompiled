using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E7 RID: 231
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06000C8E RID: 3214 RVA: 0x00032F3A File Offset: 0x0003113A
		public XmlDeclarationWrapper(XmlDeclaration declaration) : base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00032F4A File Offset: 0x0003114A
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00032F57 File Offset: 0x00031157
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x00032F64 File Offset: 0x00031164
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00032F72 File Offset: 0x00031172
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00032F7F File Offset: 0x0003117F
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x040003E3 RID: 995
		private readonly XmlDeclaration _declaration;
	}
}
