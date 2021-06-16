using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000072 RID: 114
	internal static class Scalar
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000B7AC File Offset: 0x000099AC
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
					if (info.TargetType != TargetType.Scalar)
					{
						return new Token[0];
					}
					return new Token[]
					{
						f,
						s
					};
				}, new Token[0]);
			})
			from t in tseq
			select t;
		}
	}
}
