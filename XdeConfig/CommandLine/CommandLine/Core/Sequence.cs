using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000073 RID: 115
	internal static class Sequence
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x0000B81C File Offset: 0x00009A1C
		public static IEnumerable<Token> Partition(IEnumerable<Token> tokens, Func<string, Maybe<TypeDescriptor>> typeLookup)
		{
			return from tseq in tokens.Pairwise(delegate(Token f, Token s)
			{
				if (!f.IsName() || !s.IsValue())
				{
					return new Token[0];
				}
				return typeLookup(f.Text).MapValueOrDefault(delegate(TypeDescriptor info)
				{
					if (info.TargetType != TargetType.Sequence)
					{
						return new Token[0];
					}
					return new Token[]
					{
						f
					}.Concat(tokens.OfSequence(f, info));
				}, new Token[0]);
			})
			from t in tseq
			select t;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B898 File Offset: 0x00009A98
		private static IEnumerable<Token> OfSequence(this IEnumerable<Token> tokens, Token nameToken, TypeDescriptor info)
		{
			int nameIndex = tokens.IndexOf((Token t) => t.Equals(nameToken));
			if (nameIndex >= 0)
			{
				Func<int, IEnumerable<Token>> <>9__3;
				return info.NextValue.MapValueOrDefault(delegate(TypeDescriptor _)
				{
					Maybe<int> maxItems = info.MaxItems;
					Func<int, IEnumerable<Token>> func;
					if ((func = <>9__3) == null)
					{
						func = (<>9__3 = ((int n) => tokens.Skip(nameIndex + 1).Take(n)));
					}
					return maxItems.MapValueOrDefault(func, tokens.Skip(nameIndex + 1).TakeWhile((Token v) => v.IsValue()));
				}, tokens.Skip(nameIndex + 1).TakeWhile((Token v) => v.IsValue()));
			}
			return new Token[0];
		}
	}
}
