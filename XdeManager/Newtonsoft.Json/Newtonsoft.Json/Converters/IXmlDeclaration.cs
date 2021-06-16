using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EB RID: 235
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000CB5 RID: 3253
		string Version { get; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000CB6 RID: 3254
		// (set) Token: 0x06000CB7 RID: 3255
		string Encoding { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000CB8 RID: 3256
		// (set) Token: 0x06000CB9 RID: 3257
		string Standalone { get; set; }
	}
}
