using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020008A0 RID: 2208
	internal static class StringToBase64Converter
	{
		// Token: 0x06005481 RID: 21633 RVA: 0x001BEB84 File Offset: 0x001BCD84
		internal static string StringToBase64String(string input)
		{
			if (input == null)
			{
				throw PSTraceSource.NewArgumentNullException("input");
			}
			return Convert.ToBase64String(Encoding.Unicode.GetBytes(input.ToCharArray()));
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x001BEBB8 File Offset: 0x001BCDB8
		internal static string Base64ToString(string base64)
		{
			if (string.IsNullOrEmpty(base64))
			{
				throw PSTraceSource.NewArgumentNullException("base64");
			}
			return new string(Encoding.Unicode.GetChars(Convert.FromBase64String(base64)));
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x001BEBF0 File Offset: 0x001BCDF0
		internal static object[] Base64ToArgsConverter(string base64)
		{
			if (string.IsNullOrEmpty(base64))
			{
				throw PSTraceSource.NewArgumentNullException("base64");
			}
			string s = new string(Encoding.Unicode.GetChars(Convert.FromBase64String(base64)));
			XmlReader reader = XmlReader.Create(new StringReader(s), InternalDeserializer.XmlReaderSettingsForCliXml);
			Deserializer deserializer = new Deserializer(reader);
			object obj = deserializer.Deserialize();
			if (!deserializer.Done())
			{
				throw PSTraceSource.NewArgumentException("args");
			}
			PSObject psobject = obj as PSObject;
			if (psobject == null)
			{
				throw PSTraceSource.NewArgumentException("args");
			}
			ArrayList arrayList = psobject.BaseObject as ArrayList;
			if (arrayList == null)
			{
				throw PSTraceSource.NewArgumentException("args");
			}
			return arrayList.ToArray();
		}
	}
}
