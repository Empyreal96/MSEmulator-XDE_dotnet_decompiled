using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace DiscUtils
{
	// Token: 0x02000024 RID: 36
	internal static class Plist
	{
		// Token: 0x06000181 RID: 385 RVA: 0x00004060 File Offset: 0x00002260
		internal static Dictionary<string, object> Parse(Stream stream)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			using (XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings
			{
				DtdProcessing = DtdProcessing.Ignore
			}))
			{
				xmlDocument.Load(xmlReader);
			}
			XmlElement documentElement = xmlDocument.DocumentElement;
			if (documentElement.Name != "plist")
			{
				throw new InvalidDataException("XML document is not a plist");
			}
			return Plist.ParseDictionary(documentElement.FirstChild);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000040E0 File Offset: 0x000022E0
		internal static void Write(Stream stream, Dictionary<string, object> plist)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
			xmlDocument.AppendChild(newChild);
			XmlDocumentType newChild2 = xmlDocument.CreateDocumentType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
			xmlDocument.AppendChild(newChild2);
			XmlElement xmlElement = xmlDocument.CreateElement("plist");
			xmlElement.SetAttribute("Version", "1.0");
			xmlDocument.AppendChild(xmlElement);
			xmlDocument.DocumentElement.SetAttribute("Version", "1.0");
			xmlElement.AppendChild(Plist.CreateNode(xmlDocument, plist));
			using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
			{
				Indent = true,
				Encoding = Encoding.UTF8
			}))
			{
				xmlDocument.Save(xmlWriter);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000041C8 File Offset: 0x000023C8
		private static object ParseNode(XmlNode xmlNode)
		{
			string name = xmlNode.Name;
			if (name != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1278716217U)
				{
					if (num != 184981848U)
					{
						if (num != 398550328U)
						{
							if (num == 1278716217U)
							{
								if (name == "dict")
								{
									return Plist.ParseDictionary(xmlNode);
								}
							}
						}
						else if (name == "string")
						{
							return Plist.ParseString(xmlNode);
						}
					}
					else if (name == "false")
					{
						return false;
					}
				}
				else if (num <= 2321067302U)
				{
					if (num != 1303515621U)
					{
						if (num == 2321067302U)
						{
							if (name == "array")
							{
								return Plist.ParseArray(xmlNode);
							}
						}
					}
					else if (name == "true")
					{
						return true;
					}
				}
				else if (num != 3218261061U)
				{
					if (num == 3631407781U)
					{
						if (name == "data")
						{
							return Plist.ParseData(xmlNode);
						}
					}
				}
				else if (name == "integer")
				{
					return Plist.ParseInteger(xmlNode);
				}
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000042E8 File Offset: 0x000024E8
		private static XmlNode CreateNode(XmlDocument xmlDoc, object obj)
		{
			if (obj is Dictionary<string, object>)
			{
				return Plist.CreateDictionary(xmlDoc, (Dictionary<string, object>)obj);
			}
			if (obj is string)
			{
				XmlText newChild = xmlDoc.CreateTextNode((string)obj);
				XmlElement xmlElement = xmlDoc.CreateElement("string");
				xmlElement.AppendChild(newChild);
				return xmlElement;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00004338 File Offset: 0x00002538
		private static XmlNode CreateDictionary(XmlDocument xmlDoc, Dictionary<string, object> dict)
		{
			XmlElement xmlElement = xmlDoc.CreateElement("dict");
			foreach (KeyValuePair<string, object> keyValuePair in dict)
			{
				XmlText newChild = xmlDoc.CreateTextNode(keyValuePair.Key);
				XmlElement xmlElement2 = xmlDoc.CreateElement("key");
				xmlElement2.AppendChild(newChild);
				xmlElement.AppendChild(xmlElement2);
				XmlNode newChild2 = Plist.CreateNode(xmlDoc, keyValuePair.Value);
				xmlElement.AppendChild(newChild2);
			}
			return xmlElement;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000043D4 File Offset: 0x000025D4
		private static Dictionary<string, object> ParseDictionary(XmlNode xmlNode)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			for (XmlNode xmlNode2 = xmlNode.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
			{
				if (xmlNode2.Name != "key")
				{
					throw new InvalidDataException("Invalid plist, expected dictionary key");
				}
				string innerText = xmlNode2.InnerText;
				xmlNode2 = xmlNode2.NextSibling;
				dictionary.Add(innerText, Plist.ParseNode(xmlNode2));
			}
			return dictionary;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00004434 File Offset: 0x00002634
		private static object ParseArray(XmlNode xmlNode)
		{
			List<object> list = new List<object>();
			for (XmlNode xmlNode2 = xmlNode.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
			{
				list.Add(Plist.ParseNode(xmlNode2));
			}
			return list;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00004467 File Offset: 0x00002667
		private static object ParseString(XmlNode xmlNode)
		{
			return xmlNode.InnerText;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000446F File Offset: 0x0000266F
		private static object ParseData(XmlNode xmlNode)
		{
			return Convert.FromBase64String(xmlNode.InnerText);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000447C File Offset: 0x0000267C
		private static object ParseInteger(XmlNode xmlNode)
		{
			return int.Parse(xmlNode.InnerText, CultureInfo.InvariantCulture);
		}
	}
}
