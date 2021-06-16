using System;

namespace CSharpx
{
	// Token: 0x02000023 RID: 35
	internal static class Either
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x000044AB File Offset: 0x000026AB
		public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
		{
			return new Left<TLeft, TRight>(value);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000044B3 File Offset: 0x000026B3
		public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
		{
			return new Right<TLeft, TRight>(value);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000044BB File Offset: 0x000026BB
		public static Either<string, TRight> Return<TRight>(TRight value)
		{
			return Either.Right<string, TRight>(value);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000044C3 File Offset: 0x000026C3
		public static Either<string, TRight> Fail<TRight>(string message)
		{
			throw new Exception(message);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000044CC File Offset: 0x000026CC
		public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TResult>> func)
		{
			TRight arg;
			if (either.MatchRight(out arg))
			{
				return func(arg);
			}
			return Either.Left<TLeft, TResult>(either.GetLeft<TLeft, TRight>());
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000044F8 File Offset: 0x000026F8
		public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(Either<TLeft, TRight> either, Func<TRight, TResult> func)
		{
			TRight arg;
			if (either.MatchRight(out arg))
			{
				return Either.Right<TLeft, TResult>(func(arg));
			}
			return Either.Left<TLeft, TResult>(either.GetLeft<TLeft, TRight>());
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004528 File Offset: 0x00002728
		public static Either<TLeft1, TRight1> Bimap<TLeft, TRight, TLeft1, TRight1>(Either<TLeft, TRight> either, Func<TLeft, TLeft1> mapLeft, Func<TRight, TRight1> mapRight)
		{
			TRight arg;
			if (either.MatchRight(out arg))
			{
				return Either.Right<TLeft1, TRight1>(mapRight(arg));
			}
			return Either.Left<TLeft1, TRight1>(mapLeft(either.GetLeft<TLeft, TRight>()));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000455D File Offset: 0x0000275D
		public static Either<TLeft, TResult> Select<TLeft, TRight, TResult>(this Either<TLeft, TRight> either, Func<TRight, TResult> selector)
		{
			return Either.Map<TLeft, TRight, TResult>(either, selector);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004566 File Offset: 0x00002766
		public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TResult>(this Either<TLeft, TRight> result, Func<TRight, Either<TLeft, TResult>> func)
		{
			return Either.Bind<TLeft, TRight, TResult>(result, func);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004570 File Offset: 0x00002770
		public static TRight GetOrFail<TLeft, TRight>(Either<TLeft, TRight> either)
		{
			TRight result;
			if (either.MatchRight(out result))
			{
				return result;
			}
			throw new ArgumentException("either", string.Format("The either value was Left {0}", either));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000045A0 File Offset: 0x000027A0
		public static TLeft GetLeftOrDefault<TLeft, TRight>(Either<TLeft, TRight> either, TLeft @default)
		{
			TLeft result;
			if (!either.MatchLeft(out result))
			{
				return @default;
			}
			return result;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000045BC File Offset: 0x000027BC
		public static TRight GetRightOrDefault<TLeft, TRight>(Either<TLeft, TRight> either, TRight @default)
		{
			TRight result;
			if (!either.MatchRight(out result))
			{
				return @default;
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000045D8 File Offset: 0x000027D8
		public static Either<Exception, TRight> Try<TRight>(Func<TRight> func)
		{
			Either<Exception, TRight> result;
			try
			{
				result = new Right<Exception, TRight>(func());
			}
			catch (Exception value)
			{
				result = new Left<Exception, TRight>(value);
			}
			return result;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000460C File Offset: 0x0000280C
		public static Either<Exception, TRight> Cast<TRight>(object obj)
		{
			return Either.Try<TRight>(() => (TRight)((object)obj));
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000462A File Offset: 0x0000282A
		public static Either<TLeft, TRight> OfMaybe<TLeft, TRight>(Maybe<TRight> maybe, TLeft left)
		{
			if (maybe.Tag == MaybeType.Just)
			{
				return Either.Right<TLeft, TRight>(((Just<TRight>)maybe).Value);
			}
			return Either.Left<TLeft, TRight>(left);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000464B File Offset: 0x0000284B
		private static TLeft GetLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
		{
			return ((Left<TLeft, TRight>)either).Value;
		}
	}
}
