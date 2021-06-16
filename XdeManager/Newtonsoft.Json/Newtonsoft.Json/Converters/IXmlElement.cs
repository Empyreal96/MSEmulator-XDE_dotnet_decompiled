using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000ED RID: 237
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x06000CBE RID: 3262
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x06000CBF RID: 3263
		string GetPrefixOfNamespace(string namespaceUri);

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000CC0 RID: 3264
		bool IsEmpty { get; }
	}
}
