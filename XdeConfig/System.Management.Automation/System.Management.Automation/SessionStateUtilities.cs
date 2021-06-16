using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x02000840 RID: 2112
	internal static class SessionStateUtilities
	{
		// Token: 0x06005167 RID: 20839 RVA: 0x001B2060 File Offset: 0x001B0260
		internal static Collection<T> ConvertArrayToCollection<T>(T[] array)
		{
			Collection<T> collection = new Collection<T>();
			if (array != null)
			{
				foreach (T item in array)
				{
					collection.Add(item);
				}
			}
			return collection;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x001B2098 File Offset: 0x001B0298
		internal static Collection<T> ConvertListToCollection<T>(List<T> list)
		{
			Collection<T> collection = new Collection<T>();
			if (list != null)
			{
				foreach (T item in list)
				{
					collection.Add(item);
				}
			}
			return collection;
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001B20F0 File Offset: 0x001B02F0
		internal static bool CollectionContainsValue(IEnumerable collection, object value, IComparer comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			bool result = false;
			foreach (object obj in collection)
			{
				if (comparer != null)
				{
					if (comparer.Compare(obj, value) == 0)
					{
						result = true;
						break;
					}
				}
				else if (obj.Equals(value))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001B2168 File Offset: 0x001B0368
		internal static Collection<WildcardPattern> CreateWildcardsFromStrings(string[] globPatterns, WildcardOptions options)
		{
			Collection<string> globPatterns2 = SessionStateUtilities.ConvertArrayToCollection<string>(globPatterns);
			return SessionStateUtilities.CreateWildcardsFromStrings(globPatterns2, options);
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x001B2184 File Offset: 0x001B0384
		internal static Collection<WildcardPattern> CreateWildcardsFromStrings(Collection<string> globPatterns, WildcardOptions options)
		{
			Collection<WildcardPattern> collection = new Collection<WildcardPattern>();
			if (globPatterns != null && globPatterns.Count > 0)
			{
				foreach (string text in globPatterns)
				{
					if (!string.IsNullOrEmpty(text))
					{
						collection.Add(new WildcardPattern(text, options));
					}
				}
			}
			return collection;
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x001B21F0 File Offset: 0x001B03F0
		internal static bool MatchesAnyWildcardPattern(string text, IEnumerable<WildcardPattern> patterns, bool defaultValue)
		{
			bool result = false;
			bool flag = false;
			if (patterns != null)
			{
				foreach (WildcardPattern wildcardPattern in patterns)
				{
					flag = true;
					if (wildcardPattern.IsMatch(text))
					{
						result = true;
						break;
					}
				}
			}
			if (!flag)
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x001B2250 File Offset: 0x001B0450
		internal static FileMode GetFileModeFromOpenMode(OpenMode openMode)
		{
			FileMode result = FileMode.Create;
			switch (openMode)
			{
			case OpenMode.Add:
				result = FileMode.Append;
				break;
			case OpenMode.New:
				result = FileMode.CreateNew;
				break;
			case OpenMode.Overwrite:
				result = FileMode.Create;
				break;
			}
			return result;
		}
	}
}
