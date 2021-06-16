using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CSharpx
{
	// Token: 0x02000025 RID: 37
	internal static class EnumerableExtensions
	{
		// Token: 0x060000CE RID: 206 RVA: 0x000046C4 File Offset: 0x000028C4
		private static IEnumerable<TSource> AssertCountImpl<TSource>(IEnumerable<TSource> source, int count, Func<int, int, Exception> errorSelector)
		{
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection == null)
			{
				return EnumerableExtensions.ExpectingCountYieldingImpl<TSource>(source, count, errorSelector);
			}
			if (collection.Count != count)
			{
				throw errorSelector(collection.Count.CompareTo(count), count);
			}
			return source;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004705 File Offset: 0x00002905
		private static IEnumerable<TSource> ExpectingCountYieldingImpl<TSource>(IEnumerable<TSource> source, int count, Func<int, int, Exception> errorSelector)
		{
			int iterations = 0;
			foreach (TSource tsource in source)
			{
				int num = iterations;
				iterations = num + 1;
				if (iterations > count)
				{
					throw errorSelector(1, count);
				}
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			if (iterations != count)
			{
				throw errorSelector(-1, count);
			}
			yield break;
			yield break;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004724 File Offset: 0x00002924
		public static IEnumerable<TResult> Cartesian<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}
			if (second == null)
			{
				throw new ArgumentNullException("second");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
			return from item1 in first
			from item2 in second
			select resultSelector(item1, item2);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004797 File Offset: 0x00002997
		public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return Enumerable.Repeat<TSource>(value, 1).Concat(source);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000047B4 File Offset: 0x000029B4
		public static IEnumerable<T> Concat<T>(this T head, IEnumerable<T> tail)
		{
			if (tail == null)
			{
				throw new ArgumentNullException("tail");
			}
			return tail.Prepend(head);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000047CB File Offset: 0x000029CB
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
		{
			if (head == null)
			{
				throw new ArgumentNullException("head");
			}
			return head.Concat(Enumerable.Repeat<T>(tail, 1));
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000047E8 File Offset: 0x000029E8
		public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, int startIndex, int count)
		{
			if (sequence == null)
			{
				throw new ArgumentNullException("sequence");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return EnumerableExtensions.ExcludeImpl<T>(sequence, startIndex, count);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000481E File Offset: 0x00002A1E
		private static IEnumerable<T> ExcludeImpl<T>(IEnumerable<T> sequence, int startIndex, int count)
		{
			int index = -1;
			int endIndex = startIndex + count;
			using (IEnumerator<T> iter = sequence.GetEnumerator())
			{
				while (iter.MoveNext())
				{
					int num = index + 1;
					index = num;
					if (num >= startIndex)
					{
						break;
					}
					yield return iter.Current;
				}
				do
				{
					int num = index + 1;
					index = num;
					if (num >= endIndex)
					{
						break;
					}
				}
				while (iter.MoveNext());
				IL_F5:
				while (iter.MoveNext())
				{
					!0 ! = iter.Current;
					yield return !;
				}
				goto JumpOutOfTryFinally-3;
				goto IL_F5;
			}
			JumpOutOfTryFinally-3:
			IEnumerator<T> iter = null;
			yield break;
			yield break;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000483C File Offset: 0x00002A3C
		public static IEnumerable<KeyValuePair<int, TSource>> Index<TSource>(this IEnumerable<TSource> source)
		{
			return source.Index(0);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004848 File Offset: 0x00002A48
		public static IEnumerable<KeyValuePair<int, TSource>> Index<TSource>(this IEnumerable<TSource> source, int startIndex)
		{
			return source.Select((TSource item, int index) => new KeyValuePair<int, TSource>(startIndex + index, item));
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004874 File Offset: 0x00002A74
		public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, TResult> folder)
		{
			return EnumerableExtensions.FoldImpl<T, TResult>(source, 1, folder, null, null, null);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004881 File Offset: 0x00002A81
		public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, TResult> folder)
		{
			return EnumerableExtensions.FoldImpl<T, TResult>(source, 2, null, folder, null, null);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000488E File Offset: 0x00002A8E
		public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, TResult> folder)
		{
			return EnumerableExtensions.FoldImpl<T, TResult>(source, 3, null, null, folder, null);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000489B File Offset: 0x00002A9B
		public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, TResult> folder)
		{
			return EnumerableExtensions.FoldImpl<T, TResult>(source, 4, null, null, null, folder);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000048A8 File Offset: 0x00002AA8
		private static TResult FoldImpl<T, TResult>(IEnumerable<T> source, int count, Func<T, TResult> folder1, Func<T, T, TResult> folder2, Func<T, T, T, TResult> folder3, Func<T, T, T, T, TResult> folder4)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((count == 1 && folder1 == null) || (count == 2 && folder2 == null) || (count == 3 && folder3 == null) || (count == 4 && folder4 == null))
			{
				throw new ArgumentNullException("folder");
			}
			T[] array = new T[count];
			foreach (KeyValuePair<int, T> keyValuePair in EnumerableExtensions.AssertCountImpl<KeyValuePair<int, T>>(source.Index<T>(), count, EnumerableExtensions.OnFolderSourceSizeErrorSelector))
			{
				array[keyValuePair.Key] = keyValuePair.Value;
			}
			switch (count)
			{
			case 1:
				return folder1(array[0]);
			case 2:
				return folder2(array[0], array[1]);
			case 3:
				return folder3(array[0], array[1], array[2]);
			case 4:
				return folder4(array[0], array[1], array[2], array[3]);
			default:
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000049D0 File Offset: 0x00002BD0
		private static Exception OnFolderSourceSizeError(int cmp, int count)
		{
			return new Exception(string.Format((cmp < 0) ? "Sequence contains too few elements when exactly {0} {1} expected." : "Sequence contains too many elements when exactly {0} {1} expected.", count.ToString("N0"), (count == 1) ? "was" : "were"));
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004A08 File Offset: 0x00002C08
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			foreach (T obj in source)
			{
				action(obj);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004A6C File Offset: 0x00002C6C
		public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
			return source.PairwiseImpl(resultSelector);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004A91 File Offset: 0x00002C91
		private static IEnumerable<TResult> PairwiseImpl<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
		{
			using (IEnumerator<TSource> e = source.GetEnumerator())
			{
				if (!e.MoveNext())
				{
					yield break;
				}
				TSource arg = e.Current;
				while (e.MoveNext())
				{
					!0 arg2 = e.Current;
					yield return resultSelector(arg, arg2);
					arg = e.Current;
				}
			}
			IEnumerator<TSource> e = null;
			yield break;
			yield break;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004AA8 File Offset: 0x00002CA8
		public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source)
		{
			return source.ToDelimitedString(null);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004AB1 File Offset: 0x00002CB1
		public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return EnumerableExtensions.ToDelimitedStringImpl<TSource>(source, delimiter, (StringBuilder sb, TSource e) => sb.Append(e));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004AE8 File Offset: 0x00002CE8
		private static string ToDelimitedStringImpl<T>(IEnumerable<T> source, string delimiter, Func<StringBuilder, T, StringBuilder> append)
		{
			delimiter = (delimiter ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator);
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (T arg in source)
			{
				if (num++ > 0)
				{
					stringBuilder.Append(delimiter);
				}
				append(stringBuilder, arg);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004B68 File Offset: 0x00002D68
		public static Maybe<T> TryHead<T>(this IEnumerable<T> source)
		{
			Maybe<T> result;
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				result = (enumerator.MoveNext() ? Maybe.Just<T>(enumerator.Current) : Maybe.Nothing<T>());
			}
			return result;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public static Maybe<IEnumerable<T>> ToMaybe<T>(this IEnumerable<T> source)
		{
			Maybe<IEnumerable<T>> result;
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				result = (enumerator.MoveNext() ? Maybe.Just<IEnumerable<T>>(source) : Maybe.Nothing<IEnumerable<T>>());
			}
			return result;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004BFC File Offset: 0x00002DFC
		public static IEnumerable<T> Tail<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> e = source.GetEnumerator())
			{
				if (!e.MoveNext())
				{
					throw new ArgumentException("Source sequence cannot be empty.", "source");
				}
				while (e.MoveNext())
				{
					!0 ! = e.Current;
					yield return !;
				}
			}
			IEnumerator<T> e = null;
			yield break;
			yield break;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004C0C File Offset: 0x00002E0C
		public static IEnumerable<T> TailNoFail<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> e = source.GetEnumerator())
			{
				if (e.MoveNext())
				{
					while (e.MoveNext())
					{
						!0 ! = e.Current;
						yield return !;
					}
				}
			}
			IEnumerator<T> e = null;
			yield break;
			yield break;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004C1C File Offset: 0x00002E1C
		public static IEnumerable<T> Memorize<T>(this IEnumerable<T> source)
		{
			if (!source.GetType().IsArray)
			{
				return source.ToArray<T>();
			}
			return source;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004C40 File Offset: 0x00002E40
		public static IEnumerable<T> Materialize<T>(this IEnumerable<T> source)
		{
			if (source is EnumerableExtensions.MaterializedEnumerable<T> || source.GetType().IsArray)
			{
				return source;
			}
			return new EnumerableExtensions.MaterializedEnumerable<T>(source);
		}

		// Token: 0x0400003D RID: 61
		private static readonly Func<int, int, Exception> OnFolderSourceSizeErrorSelector = new Func<int, int, Exception>(EnumerableExtensions.OnFolderSourceSizeError);

		// Token: 0x02000097 RID: 151
		private class MaterializedEnumerable<T> : IEnumerable<!0>, IEnumerable
		{
			// Token: 0x06000357 RID: 855 RVA: 0x0000CE66 File Offset: 0x0000B066
			public MaterializedEnumerable(IEnumerable<T> enumerable)
			{
				this.inner = ((enumerable as ICollection<T>) ?? enumerable.ToArray<T>());
			}

			// Token: 0x06000358 RID: 856 RVA: 0x0000CE84 File Offset: 0x0000B084
			public IEnumerator<T> GetEnumerator()
			{
				return this.inner.GetEnumerator();
			}

			// Token: 0x06000359 RID: 857 RVA: 0x0000CE91 File Offset: 0x0000B091
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x04000109 RID: 265
			private readonly ICollection<T> inner;
		}
	}
}
