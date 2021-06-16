using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x0200006A RID: 106
	internal static class KeyValuePairHelper
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0000ADCF File Offset: 0x00008FCF
		public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> ForSwitch(IEnumerable<Token> tokens)
		{
			return from t in tokens
			select t.Text.ToKeyValuePair(new string[]
			{
				"true"
			});
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000ADF6 File Offset: 0x00008FF6
		public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> ForScalar(IEnumerable<Token> tokens)
		{
			return from g in tokens.Group(2)
			select g[0].Text.ToKeyValuePair(new string[]
			{
				g[1].Text
			});
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000AE24 File Offset: 0x00009024
		public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> ForSequence(IEnumerable<Token> tokens)
		{
			return tokens.Pairwise(delegate(Token f, Token s)
			{
				if (!f.IsName())
				{
					return string.Empty.ToKeyValuePair(Array.Empty<string>());
				}
				return f.Text.ToKeyValuePair((from x in tokens.SkipWhile((Token t) => !t.Equals(f)).SkipWhile((Token t) => t.Equals(f)).TakeWhile((Token v) => v.IsValue())
				select x.Text).ToArray<string>());
			}).Where(delegate(KeyValuePair<string, IEnumerable<string>> t)
			{
				KeyValuePair<string, IEnumerable<string>> keyValuePair = t;
				if (keyValuePair.Key.Length > 0)
				{
					keyValuePair = t;
					return keyValuePair.Value.Any<string>();
				}
				return false;
			});
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000AE79 File Offset: 0x00009079
		private static KeyValuePair<string, IEnumerable<string>> ToKeyValuePair(this string value, params string[] values)
		{
			return new KeyValuePair<string, IEnumerable<string>>(value, values);
		}
	}
}
