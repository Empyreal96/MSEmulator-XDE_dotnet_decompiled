using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200075A RID: 1882
	internal static class CollectionExtension
	{
		// Token: 0x06004B12 RID: 19218 RVA: 0x001892D0 File Offset: 0x001874D0
		internal static bool TrueForAll<T>(this IEnumerable<T> collection, Predicate<T> predicate)
		{
			foreach (T obj in collection)
			{
				if (!predicate(obj))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x00189324 File Offset: 0x00187524
		internal static U[] Map<T, U>(this ICollection<T> collection, Func<T, U> select)
		{
			int num = collection.Count;
			U[] array = new U[num];
			num = 0;
			foreach (T arg in collection)
			{
				array[num++] = select(arg);
			}
			return array;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x00189388 File Offset: 0x00187588
		internal static int ListHashCode<T>(this IEnumerable<T> list)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int num = 6551;
			foreach (T obj in list)
			{
				num ^= (num << 5 ^ @default.GetHashCode(obj));
			}
			return num;
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x001893E4 File Offset: 0x001875E4
		internal static bool ListEquals<T>(this ICollection<T> first, ICollection<T> second)
		{
			if (first.Count != second.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			IEnumerator<T> enumerator = first.GetEnumerator();
			IEnumerator<T> enumerator2 = second.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator2.MoveNext();
				if (!@default.Equals(enumerator.Current, enumerator2.Current))
				{
					return false;
				}
			}
			return true;
		}
	}
}
