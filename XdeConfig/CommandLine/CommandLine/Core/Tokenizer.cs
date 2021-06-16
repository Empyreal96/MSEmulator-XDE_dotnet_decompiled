using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine.Infrastructure;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Core
{
	// Token: 0x02000082 RID: 130
	internal static class Tokenizer
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000C0DC File Offset: 0x0000A2DC
		public static Result<IEnumerable<Token>, Error> Tokenize(IEnumerable<string> arguments, Func<string, NameLookupResult> nameLookup)
		{
			return Tokenizer.Tokenize(arguments, nameLookup, (IEnumerable<Token> tokens) => tokens);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000C104 File Offset: 0x0000A304
		public static Result<IEnumerable<Token>, Error> Tokenize(IEnumerable<string> arguments, Func<string, NameLookupResult> nameLookup, Func<IEnumerable<Token>, IEnumerable<Token>> normalize)
		{
			List<Error> list = new List<Error>();
			Action<Error> onError = new Action<Error>(list.Add);
			IEnumerable<Token> arg2 = arguments.SelectMany(delegate(string arg)
			{
				if (!arg.StartsWith("-", StringComparison.Ordinal))
				{
					return new Token[]
					{
						Token.Value(arg)
					};
				}
				if (!arg.StartsWith("--", StringComparison.Ordinal))
				{
					return Tokenizer.TokenizeShortName(arg, nameLookup);
				}
				return Tokenizer.TokenizeLongName(arg, onError);
			}, (string arg, Token token) => token).Memorize<Token>();
			IEnumerable<Token> source = normalize(arg2).Memorize<Token>();
			IEnumerable<Token> unkTokens = (from t in source
			where t.IsName() && nameLookup(t.Text) == NameLookupResult.NoOptionFound
			select t).Memorize<Token>();
			return Result.Succeed<IEnumerable<Token>, Error>(from x in source
			where !unkTokens.Contains(x)
			select x, list.Concat(unkTokens.Select((Token t) => new UnknownOptionError(t.Text))));
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		public static Result<IEnumerable<Token>, Error> PreprocessDashDash(IEnumerable<string> arguments, Func<IEnumerable<string>, Result<IEnumerable<Token>, Error>> tokenizer)
		{
			if (arguments.Any((string arg) => arg.EqualsOrdinal("--")))
			{
				Result<IEnumerable<Token>, Error> result = tokenizer(arguments.TakeWhile((string arg) => !arg.EqualsOrdinal("--")));
				IEnumerable<Token> values = arguments.SkipWhile((string arg) => !arg.EqualsOrdinal("--")).Skip(1).Select(new Func<string, Token>(Token.Value));
				return result.Map((IEnumerable<Token> tokens) => tokens.Concat(values));
			}
			return tokenizer(arguments);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000C2A4 File Offset: 0x0000A4A4
		public static Result<IEnumerable<Token>, Error> ExplodeOptionList(Result<IEnumerable<Token>, Error> tokenizerResult, Func<string, Maybe<char>> optionSequenceWithSeparatorLookup)
		{
			IEnumerable<Token> source = tokenizerResult.SucceededWith<IEnumerable<Token>, Error>().Memorize<Token>();
			IEnumerable<Tuple<int, char>> replaces = source.Select((Token t, int i) => optionSequenceWithSeparatorLookup(t.Text).MapValueOrDefault((char sep) => Tuple.Create<int, char>(i + 1, sep), Tuple.Create<int, char>(-1, '\0'))).SkipWhile((Tuple<int, char> x) => x.Item1 < 0).Memorize<Tuple<int, char>>();
			return Result.Succeed<IEnumerable<Token>, Error>(source.Select((Token t, int i) => replaces.FirstOrDefault((Tuple<int, char> x) => x.Item1 == i).ToMaybe<Tuple<int, char>>().MapValueOrDefault((Tuple<int, char> r) => t.Text.Split(new char[]
			{
				r.Item2
			}).Select(new Func<string, Token>(Token.Value)), Enumerable.Empty<Token>().Concat(new Token[]
			{
				t
			}))).SelectMany((IEnumerable<Token> x) => x), tokenizerResult.SuccessfulMessages<IEnumerable<Token>, Error>());
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000C34C File Offset: 0x0000A54C
		public static IEnumerable<Token> Normalize(IEnumerable<Token> tokens, Func<string, bool> nameLookup)
		{
			Func<Token, bool> <>9__7;
			IEnumerable<int> indexes = from i in tokens.Select(delegate(Token t, int i)
			{
				Maybe<Token> maybe = tokens.ElementAtOrDefault(i - 1).ToMaybe<Token>();
				if (t.IsValue() && ((Value)t).ExplicitlyAssigned)
				{
					Maybe<Token> maybe2 = maybe;
					Func<Token, bool> func;
					if ((func = <>9__7) == null)
					{
						func = (<>9__7 = ((Token p) => p.IsName() && !nameLookup(p.Text)));
					}
					if (maybe2.MapValueOrDefault(func, false))
					{
						return Maybe.Just<int>(i);
					}
				}
				return Maybe.Nothing<int>();
			})
			where i.IsJust<int>()
			select i.FromJustOrFail(null);
			IEnumerable<Token> toExclude = from t in tokens.Select(delegate(Token t, int i)
			{
				if (!indexes.Contains(i))
				{
					return Maybe.Nothing<Token>();
				}
				return Maybe.Just<Token>(t);
			})
			where t.IsJust<Token>()
			select t.FromJustOrFail(null);
			return from t in tokens
			where !toExclude.Contains(t)
			select t;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000C44E File Offset: 0x0000A64E
		public static Func<IEnumerable<string>, IEnumerable<OptionSpecification>, Result<IEnumerable<Token>, Error>> ConfigureTokenizer(StringComparer nameComparer, bool ignoreUnknownArguments, bool enableDashDash)
		{
			Tokenizer.<>c__DisplayClass5_0 CS$<>8__locals1 = new Tokenizer.<>c__DisplayClass5_0();
			CS$<>8__locals1.ignoreUnknownArguments = ignoreUnknownArguments;
			CS$<>8__locals1.nameComparer = nameComparer;
			CS$<>8__locals1.enableDashDash = enableDashDash;
			return delegate(IEnumerable<string> arguments, IEnumerable<OptionSpecification> optionSpecs)
			{
				Tokenizer.<>c__DisplayClass5_1 CS$<>8__locals2 = new Tokenizer.<>c__DisplayClass5_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.optionSpecs = optionSpecs;
				Tokenizer.<>c__DisplayClass5_1 CS$<>8__locals3 = CS$<>8__locals2;
				Func<IEnumerable<Token>, IEnumerable<Token>> normalize;
				if (!CS$<>8__locals1.ignoreUnknownArguments)
				{
					normalize = ((IEnumerable<Token> toks) => toks);
				}
				else
				{
					normalize = delegate(IEnumerable<Token> toks)
					{
						Func<string, bool> nameLookup;
						if ((nameLookup = CS$<>8__locals2.<>9__6) == null)
						{
							nameLookup = (CS$<>8__locals2.<>9__6 = ((string name) => NameLookup.Contains(name, CS$<>8__locals2.optionSpecs, CS$<>8__locals2.CS$<>8__locals1.nameComparer) > NameLookupResult.NoOptionFound));
						}
						return Tokenizer.Normalize(toks, nameLookup);
					};
				}
				CS$<>8__locals3.normalize = normalize;
				return Tokenizer.ExplodeOptionList(CS$<>8__locals1.enableDashDash ? Tokenizer.PreprocessDashDash(arguments, delegate(IEnumerable<string> args)
				{
					Func<string, NameLookupResult> nameLookup;
					if ((nameLookup = CS$<>8__locals2.<>9__7) == null)
					{
						nameLookup = (CS$<>8__locals2.<>9__7 = ((string name) => NameLookup.Contains(name, CS$<>8__locals2.optionSpecs, CS$<>8__locals2.CS$<>8__locals1.nameComparer)));
					}
					return Tokenizer.Tokenize(args, nameLookup, CS$<>8__locals2.normalize);
				}) : Tokenizer.Tokenize(arguments, (string name) => NameLookup.Contains(name, CS$<>8__locals2.optionSpecs, CS$<>8__locals2.CS$<>8__locals1.nameComparer), CS$<>8__locals2.normalize), (string name) => NameLookup.HavingSeparator(name, CS$<>8__locals2.optionSpecs, CS$<>8__locals2.CS$<>8__locals1.nameComparer));
			};
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000C475 File Offset: 0x0000A675
		private static IEnumerable<Token> TokenizeShortName(string value, Func<string, NameLookupResult> nameLookup)
		{
			if (value.Length > 1 && value[0] == '-' && value[1] != '-')
			{
				string text = value.Substring(1);
				if (char.IsDigit(text[0]))
				{
					yield return Token.Value(value);
					yield break;
				}
				if (value.Length == 2)
				{
					yield return Token.Name(text);
					yield break;
				}
				int i = 0;
				string text2 = text;
				for (int j = 0; j < text2.Length; j++)
				{
					string text3 = new string(text2[j], 1);
					NameLookupResult r = nameLookup(text3);
					if (i > 0 && r == NameLookupResult.NoOptionFound)
					{
						break;
					}
					int num = i;
					i = num + 1;
					yield return Token.Name(text3);
					if (r == NameLookupResult.OtherOptionFound)
					{
						break;
					}
				}
				text2 = null;
				if (i < text.Length)
				{
					yield return Token.Value(text.Substring(i));
				}
				text = null;
			}
			yield break;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000C48C File Offset: 0x0000A68C
		private static IEnumerable<Token> TokenizeLongName(string value, Action<Error> onError)
		{
			if (value.Length > 2 && value.StartsWith("--", StringComparison.Ordinal))
			{
				string text = value.Substring(2);
				int num = text.IndexOf('=');
				if (num <= 0)
				{
					yield return Token.Name(text);
					yield break;
				}
				if (num == 1)
				{
					onError(new BadFormatTokenError(value));
					yield break;
				}
				Match tokenMatch = Regex.Match(text, "^([^=]+)=([^ ].*)$");
				if (!tokenMatch.Success)
				{
					onError(new BadFormatTokenError(value));
					yield break;
				}
				yield return Token.Name(tokenMatch.Groups[1].Value);
				yield return Token.Value(tokenMatch.Groups[2].Value, true);
				tokenMatch = null;
			}
			yield break;
		}
	}
}
