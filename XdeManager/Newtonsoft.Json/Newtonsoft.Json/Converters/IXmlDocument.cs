using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EA RID: 234
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06000CA8 RID: 3240
		IXmlNode CreateComment(string text);

		// Token: 0x06000CA9 RID: 3241
		IXmlNode CreateTextNode(string text);

		// Token: 0x06000CAA RID: 3242
		IXmlNode CreateCDataSection(string data);

		// Token: 0x06000CAB RID: 3243
		IXmlNode CreateWhitespace(string text);

		// Token: 0x06000CAC RID: 3244
		IXmlNode CreateSignificantWhitespace(string text);

		// Token: 0x06000CAD RID: 3245
		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		// Token: 0x06000CAE RID: 3246
		IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset);

		// Token: 0x06000CAF RID: 3247
		IXmlNode CreateProcessingInstruction(string target, string data);

		// Token: 0x06000CB0 RID: 3248
		IXmlElement CreateElement(string elementName);

		// Token: 0x06000CB1 RID: 3249
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x06000CB2 RID: 3250
		IXmlNode CreateAttribute(string name, string value);

		// Token: 0x06000CB3 RID: 3251
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000CB4 RID: 3252
		IXmlElement DocumentElement { get; }
	}
}
