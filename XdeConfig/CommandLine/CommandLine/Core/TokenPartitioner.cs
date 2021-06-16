using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000083 RID: 131
	internal static class TokenPartitioner
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
		public static Tuple<IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable<string>, IEnumerable<Token>> Partition(IEnumerable<Token> tokens, Func<string, Maybe<TypeDescriptor>> typeLookup)
		{
			IEqualityComparer<Token> @default = ReferenceEqualityComparer.Default;
			IEnumerable<Token> enumerable = tokens.Memorize<Token>();
			HashSet<Token> switches = new HashSet<Token>(Switch.Partition(enumerable, typeLookup), @default);
			HashSet<Token> scalars = new HashSet<Token>(Scalar.Partition(enumerable, typeLookup), @default);
			HashSet<Token> sequences = new HashSet<Token>(Sequence.Partition(enumerable, typeLookup), @default);
			IEnumerable<Token> enumerable2 = (from t in enumerable
			where !switches.Contains(t)
			where !scalars.Contains(t)
			where !sequences.Contains(t)
			select t).Memorize<Token>();
			IEnumerable<Token> enumerable3 = (from v in enumerable2
			where v.IsValue()
			select v).Memorize<Token>();
			IEnumerable<Token> item = enumerable2.Except(enumerable3, ReferenceEqualityComparer.Default).Memorize<Token>();
			return Tuple.Create<IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable<string>, IEnumerable<Token>>(KeyValuePairHelper.ForSwitch(switches).Concat(KeyValuePairHelper.ForScalar(scalars)).Concat(KeyValuePairHelper.ForSequence(sequences)), from t in enumerable3
			select t.Text, item);
		}
	}
}
