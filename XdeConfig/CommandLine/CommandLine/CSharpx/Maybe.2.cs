using System;

namespace CSharpx
{
	// Token: 0x0200002A RID: 42
	internal static class Maybe
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00004CF2 File Offset: 0x00002EF2
		public static Maybe<T> Nothing<T>()
		{
			return new Nothing<T>();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004CF9 File Offset: 0x00002EF9
		public static Just<T> Just<T>(T value)
		{
			return new Just<T>(value);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004D04 File Offset: 0x00002F04
		public static Maybe<T> Return<T>(T value)
		{
			if (!object.Equals(value, default(T)))
			{
				return Maybe.Just<T>(value);
			}
			return Maybe.Nothing<T>();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004D38 File Offset: 0x00002F38
		public static Maybe<T2> Bind<T1, T2>(Maybe<T1> maybe, Func<T1, Maybe<T2>> func)
		{
			T1 arg;
			if (!maybe.MatchJust(out arg))
			{
				return Maybe.Nothing<T2>();
			}
			return func(arg);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004D5C File Offset: 0x00002F5C
		public static Maybe<T2> Map<T1, T2>(Maybe<T1> maybe, Func<T1, T2> func)
		{
			T1 arg;
			if (!maybe.MatchJust(out arg))
			{
				return Maybe.Nothing<T2>();
			}
			return Maybe.Just<T2>(func(arg));
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004D88 File Offset: 0x00002F88
		public static Maybe<Tuple<T1, T2>> Merge<T1, T2>(Maybe<T1> first, Maybe<T2> second)
		{
			T1 item;
			T2 item2;
			if (first.MatchJust(out item) && second.MatchJust(out item2))
			{
				return Maybe.Just<Tuple<T1, T2>>(Tuple.Create<T1, T2>(item, item2));
			}
			return Maybe.Nothing<Tuple<T1, T2>>();
		}
	}
}
