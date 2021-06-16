using System;
using System.IO;
using System.Xml;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x02000368 RID: 872
	internal static class ServerOperationHelpers
	{
		// Token: 0x06002B15 RID: 11029 RVA: 0x000EC9C0 File Offset: 0x000EABC0
		internal static byte[] ExtractEncodedXmlElement(string xmlBuffer, string xmlTag)
		{
			if (xmlBuffer == null || xmlTag == null)
			{
				return new byte[1];
			}
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.CheckCharacters = false;
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.XmlResolver = null;
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
			xmlReaderSettings.MaxCharactersFromEntities = 1024L;
			xmlReaderSettings.DtdProcessing = DtdProcessing.Prohibit;
			XmlReader xmlReader = XmlReader.Create(new StringReader(xmlBuffer), xmlReaderSettings);
			if (XmlNodeType.Element == xmlReader.MoveToContent())
			{
				string s = xmlReader.ReadElementContentAsString(xmlTag, xmlReader.NamespaceURI);
				return Convert.FromBase64String(s);
			}
			return new byte[1];
		}
	}
}
