using System;
using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EE RID: 238
	internal interface IXmlNode
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000CC1 RID: 3265
		XmlNodeType NodeType { get; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000CC2 RID: 3266
		string LocalName { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000CC3 RID: 3267
		List<IXmlNode> ChildNodes { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000CC4 RID: 3268
		List<IXmlNode> Attributes { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000CC5 RID: 3269
		IXmlNode ParentNode { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CC6 RID: 3270
		// (set) Token: 0x06000CC7 RID: 3271
		string Value { get; set; }

		// Token: 0x06000CC8 RID: 3272
		IXmlNode AppendChild(IXmlNode newChild);

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CC9 RID: 3273
		string NamespaceUri { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CCA RID: 3274
		object WrappedNode { get; }
	}
}
