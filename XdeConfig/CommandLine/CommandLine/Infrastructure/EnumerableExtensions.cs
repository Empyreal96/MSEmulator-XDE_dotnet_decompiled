using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Infrastructure
{
	// Token: 0x0200005E RID: 94
	internal static class EnumerableExtensions
	{
		// Token: 0x0600026E RID: 622 RVA: 0x0000A17C File Offset: 0x0000837C
		public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			int num = -1;
			foreach (TSource arg in source)
			{
				num++;
				if (predicate(arg))
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A1D0 File Offset: 0x000083D0
		public static object ToUntypedArray(this IEnumerable<object> value, Type type)
		{
			Array array = Array.CreateInstance(type, value.Count<object>());
			value.ToArray<object>().CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A1F8 File Offset: 0x000083F8
		public static bool Empty<TSource>(this IEnumerable<TSource> source)
		{
			return !source.Any<TSource>();
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A203 File Offset: 0x00008403
		public static IEnumerable<T[]> Group<T>(this IEnumerable<T> source, int groupSize)
		{
			if (groupSize < 1)
			{
				throw new ArgumentOutOfRangeException("groupSize");
			}
			T[] array = new T[groupSize];
			int num = 0;
			foreach (T t in source)
			{
				array[num++] = t;
				if (num == groupSize)
				{
					yield return array;
					array = new T[groupSize];
					num = 0;
				}
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}
	}
}
