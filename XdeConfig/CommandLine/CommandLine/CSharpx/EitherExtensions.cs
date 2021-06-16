using System;

namespace CSharpx
{
	// Token: 0x02000024 RID: 36
	internal static class EitherExtensions
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00004658 File Offset: 0x00002858
		public static void Match<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TLeft> ifLeft, Action<TRight> ifRight)
		{
			TLeft obj;
			if (either.MatchLeft(out obj))
			{
				ifLeft(obj);
				return;
			}
			ifRight(((Right<TLeft, TRight>)either).Value);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004688 File Offset: 0x00002888
		public static Either<string, TRight> ToEither<TRight>(this TRight value)
		{
			return Either.Return<TRight>(value);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004690 File Offset: 0x00002890
		public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(this Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TResult>> func)
		{
			return Either.Bind<TLeft, TRight, TResult>(either, func);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004699 File Offset: 0x00002899
		public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(this Either<TLeft, TRight> either, Func<TRight, TResult> func)
		{
			return Either.Map<TLeft, TRight, TResult>(either, func);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000046A2 File Offset: 0x000028A2
		public static Either<TLeft1, TRight1> Bimap<TLeft, TRight, TLeft1, TRight1>(this Either<TLeft, TRight> either, Func<TLeft, TLeft1> mapLeft, Func<TRight, TRight1> mapRight)
		{
			return Either.Bimap<TLeft, TRight, TLeft1, TRight1>(either, mapLeft, mapRight);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000046AC File Offset: 0x000028AC
		public static bool IsLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
		{
			return either.Tag == EitherType.Left;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000046B7 File Offset: 0x000028B7
		public static bool IsRight<TLeft, TRight>(this Either<TLeft, TRight> either)
		{
			return either.Tag == EitherType.Right;
		}
	}
}
