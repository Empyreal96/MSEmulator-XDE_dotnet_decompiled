using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000876 RID: 2166
	internal static class EnumerableExtensions
	{
		// Token: 0x060052F5 RID: 21237 RVA: 0x001B9A90 File Offset: 0x001B7C90
		internal static IEnumerable<T> Append<T>(this IEnumerable<T> collection, T element)
		{
			foreach (T t in collection)
			{
				yield return t;
			}
			yield return element;
			yield break;
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x001B9C6C File Offset: 0x001B7E6C
		internal static IEnumerable<T> Prepend<T>(this IEnumerable<T> collection, T element)
		{
			yield return element;
			foreach (T t in collection)
			{
				yield return t;
			}
			yield break;
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x001B9C90 File Offset: 0x001B7E90
		internal static int SequenceGetHashCode<T>(this IEnumerable<T> xs) where T : class
		{
			if (xs == null)
			{
				return 82460653;
			}
			int num = 41;
			foreach (T t in xs)
			{
				num *= 59;
				if (t != null)
				{
					num += t.GetHashCode();
				}
			}
			return num;
		}
	}
}
