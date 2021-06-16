using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000F RID: 15
	public class SerializableStringDictionary : StringDictionary, IXmlSerializable
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000328E File Offset: 0x0000148E
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003294 File Offset: 0x00001494
		public void ReadXml(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !(reader.LocalName == base.GetType().Name)))
			{
				string text = reader["Name"];
				if (text == null)
				{
					throw new FormatException();
				}
				string value = reader["Value"];
				this[text] = value;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000032F8 File Offset: 0x000014F8
		public void WriteXml(XmlWriter writer)
		{
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				writer.WriteStartElement("Pair");
				writer.WriteAttributeString("Name", (string)dictionaryEntry.Key);
				writer.WriteAttributeString("Value", (string)dictionaryEntry.Value);
				writer.WriteEndElement();
			}
		}
	}
}
