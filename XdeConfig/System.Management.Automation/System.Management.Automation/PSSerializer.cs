using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000448 RID: 1096
	public class PSSerializer
	{
		// Token: 0x06002FD4 RID: 12244 RVA: 0x00105581 File Offset: 0x00103781
		internal PSSerializer()
		{
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x00105589 File Offset: 0x00103789
		public static string Serialize(object source)
		{
			return PSSerializer.Serialize(source, PSSerializer.mshDefaultSerializationDepth);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x00105598 File Offset: 0x00103798
		public static string Serialize(object source, int depth)
		{
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings
			{
				CloseOutput = true,
				Encoding = Encoding.Unicode,
				Indent = true,
				OmitXmlDeclaration = true
			});
			Serializer serializer = new Serializer(writer, depth, true);
			serializer.Serialize(source);
			serializer.Done();
			return stringBuilder.ToString();
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x001055F8 File Offset: 0x001037F8
		public static object Deserialize(string source)
		{
			object[] array = PSSerializer.DeserializeAsList(source);
			if (array.Length == 0)
			{
				return null;
			}
			if (array.Length == 1)
			{
				return array[0];
			}
			return array;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x00105620 File Offset: 0x00103820
		public static object[] DeserializeAsList(string source)
		{
			List<object> list = new List<object>();
			TextReader input = new StringReader(source);
			XmlReader reader = XmlReader.Create(input, InternalDeserializer.XmlReaderSettingsForCliXml);
			Deserializer deserializer = new Deserializer(reader);
			while (!deserializer.Done())
			{
				object item = deserializer.Deserialize();
				list.Add(item);
			}
			return list.ToArray();
		}

		// Token: 0x040019E5 RID: 6629
		private static int mshDefaultSerializationDepth = 1;
	}
}
