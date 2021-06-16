using System;
using System.Collections;
using System.Collections.Specialized;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000630 RID: 1584
	internal static class HashtableOps
	{
		// Token: 0x060044BA RID: 17594 RVA: 0x0016FE0C File Offset: 0x0016E00C
		internal static void AddKeyValuePair(IDictionary hashtable, object key, object value, IScriptExtent errorExtent)
		{
			key = PSObject.Base(key);
			if (key == null)
			{
				throw InterpreterError.NewInterpreterException(hashtable, typeof(RuntimeException), errorExtent, "InvalidNullKey", ParserStrings.InvalidNullKey, new object[0]);
			}
			if (hashtable.Contains(key))
			{
				string text = PSObject.ToStringParser(null, key);
				if (text.Length > 40)
				{
					text = text.Substring(0, 40) + "...";
				}
				throw InterpreterError.NewInterpreterException(hashtable, typeof(RuntimeException), errorExtent, "DuplicateKeyInHashLiteral", ParserStrings.DuplicateKeyInHashLiteral, new object[]
				{
					text
				});
			}
			hashtable.Add(key, value);
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0016FEA8 File Offset: 0x0016E0A8
		internal static object Add(IDictionary lvalDict, IDictionary rvalDict)
		{
			IDictionary dictionary;
			if (lvalDict is OrderedDictionary)
			{
				dictionary = new OrderedDictionary(StringComparer.CurrentCultureIgnoreCase);
			}
			else
			{
				dictionary = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
			}
			foreach (object key in lvalDict.Keys)
			{
				dictionary.Add(key, lvalDict[key]);
			}
			foreach (object key2 in rvalDict.Keys)
			{
				dictionary.Add(key2, rvalDict[key2]);
			}
			return dictionary;
		}
	}
}
