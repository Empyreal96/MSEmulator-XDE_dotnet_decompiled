using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EC RID: 236
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000CBA RID: 3258
		string Name { get; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000CBB RID: 3259
		string System { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000CBC RID: 3260
		string Public { get; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000CBD RID: 3261
		string InternalSubset { get; }
	}
}
