using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpx
{
	// Token: 0x0200002B RID: 43
	internal static class MaybeExtensions
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00004DBC File Offset: 0x00002FBC
		public static void Match<T>(this Maybe<T> maybe, Action<T> ifJust, Action ifNothing)
		{
			T obj;
			if (maybe.MatchJust(out obj))
			{
				ifJust(obj);
				return;
			}
			ifNothing();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004DE4 File Offset: 0x00002FE4
		public static void Match<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, Action<T1, T2> ifJust, Action ifNothing)
		{
			T1 arg;
			T2 arg2;
			if (maybe.MatchJust(out arg, out arg2))
			{
				ifJust(arg, arg2);
				return;
			}
			ifNothing();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004E0C File Offset: 0x0000300C
		public static bool MatchJust<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, out T1 value1, out T2 value2)
		{
			Tuple<T1, T2> tuple;
			if (maybe.MatchJust(out tuple))
			{
				value1 = tuple.Item1;
				value2 = tuple.Item2;
				return true;
			}
			value1 = default(T1);
			value2 = default(T2);
			return false;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004E4C File Offset: 0x0000304C
		public static Maybe<T> ToMaybe<T>(this T value)
		{
			return Maybe.Return<T>(value);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004E54 File Offset: 0x00003054
		public static Maybe<T2> Bind<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func)
		{
			return Maybe.Bind<T1, T2>(maybe, func);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004E5D File Offset: 0x0000305D
		public static Maybe<T2> Map<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func)
		{
			return Maybe.Map<T1, T2>(maybe, func);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004E66 File Offset: 0x00003066
		public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, TResult> selector)
		{
			return Maybe.Map<TSource, TResult>(maybe, selector);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004E70 File Offset: 0x00003070
		public static Maybe<TResult> SelectMany<TSource, TValue, TResult>(this Maybe<TSource> maybe, Func<TSource, Maybe<TValue>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
		{
			return maybe.Bind((TSource sourceValue) => valueSelector(sourceValue).Map((TValue resultValue) => resultSelector(sourceValue, resultValue)));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004EA4 File Offset: 0x000030A4
		public static void Do<T>(this Maybe<T> maybe, Action<T> action)
		{
			T obj;
			if (maybe.MatchJust(out obj))
			{
				action(obj);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004EC4 File Offset: 0x000030C4
		public static void Do<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, Action<T1, T2> action)
		{
			T1 arg;
			T2 arg2;
			if (maybe.MatchJust(out arg, out arg2))
			{
				action(arg, arg2);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004EE5 File Offset: 0x000030E5
		public static bool IsJust<T>(this Maybe<T> maybe)
		{
			return maybe.Tag == MaybeType.Just;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004EF0 File Offset: 0x000030F0
		public static bool IsNothing<T>(this Maybe<T> maybe)
		{
			return maybe.Tag == MaybeType.Nothing;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004EFC File Offset: 0x000030FC
		public static T FromJust<T>(this Maybe<T> maybe)
		{
			T result;
			if (maybe.MatchJust(out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004F20 File Offset: 0x00003120
		public static T FromJustOrFail<T>(this Maybe<T> maybe, Exception exceptionToThrow = null)
		{
			T result;
			if (maybe.MatchJust(out result))
			{
				return result;
			}
			throw exceptionToThrow ?? new ArgumentException("Value empty.");
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004F48 File Offset: 0x00003148
		public static T GetValueOrDefault<T>(this Maybe<T> maybe, T noneValue)
		{
			T result;
			if (!maybe.MatchJust(out result))
			{
				return noneValue;
			}
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004F64 File Offset: 0x00003164
		public static T2 MapValueOrDefault<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func, T2 noneValue)
		{
			T1 arg;
			if (!maybe.MatchJust(out arg))
			{
				return noneValue;
			}
			return func(arg);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004F84 File Offset: 0x00003184
		public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> maybe)
		{
			T t;
			if (maybe.MatchJust(out t))
			{
				return Enumerable.Empty<T>().Concat(new T[]
				{
					t
				});
			}
			return Enumerable.Empty<T>();
		}
	}
}
