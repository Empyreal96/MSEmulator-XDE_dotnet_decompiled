using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Core
{
	// Token: 0x02000069 RID: 105
	internal static class InstanceChooser
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0000ABE8 File Offset: 0x00008DE8
		public static ParserResult<object> Choose(Func<IEnumerable<string>, IEnumerable<OptionSpecification>, Result<IEnumerable<Token>, Error>> tokenizer, IEnumerable<Type> types, IEnumerable<string> arguments, StringComparer nameComparer, bool ignoreValueCase, CultureInfo parsingCulture, bool autoHelp, bool autoVersion, IEnumerable<ErrorType> nonFatalErrors)
		{
			Func<ParserResult<object>> func = delegate()
			{
				string firstArg = arguments.First<string>();
				Func<string, bool> func2 = (string command) => nameComparer.Equals(command, firstArg) || nameComparer.Equals("--" + command, firstArg);
				IEnumerable<Tuple<Verb, Type>> verbs = Verb.SelectFromTypes(types);
				if (autoHelp && func2("help"))
				{
					return InstanceChooser.MakeNotParsed(types, new Error[]
					{
						InstanceChooser.MakeHelpVerbRequestedError(verbs, arguments.Skip(1).FirstOrDefault<string>() ?? string.Empty, nameComparer)
					});
				}
				if (!autoVersion || !func2("version"))
				{
					return InstanceChooser.MatchVerb(tokenizer, verbs, arguments, nameComparer, ignoreValueCase, parsingCulture, autoHelp, autoVersion, nonFatalErrors);
				}
				return InstanceChooser.MakeNotParsed(types, new Error[]
				{
					new VersionRequestedError()
				});
			};
			if (!arguments.Any<string>())
			{
				return InstanceChooser.MakeNotParsed(types, new Error[]
				{
					new NoVerbSelectedError()
				});
			}
			return func();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000AC7C File Offset: 0x00008E7C
		private static ParserResult<object> MatchVerb(Func<IEnumerable<string>, IEnumerable<OptionSpecification>, Result<IEnumerable<Token>, Error>> tokenizer, IEnumerable<Tuple<Verb, Type>> verbs, IEnumerable<string> arguments, StringComparer nameComparer, bool ignoreValueCase, CultureInfo parsingCulture, bool autoHelp, bool autoVersion, IEnumerable<ErrorType> nonFatalErrors)
		{
			if (!verbs.Any((Tuple<Verb, Type> a) => nameComparer.Equals(a.Item1.Name, arguments.First<string>())))
			{
				return InstanceChooser.MakeNotParsed(from v in verbs
				select v.Item2, new Error[]
				{
					new BadVerbSelectedError(arguments.First<string>())
				});
			}
			Func<Tuple<Verb, Type>, bool> <>9__3;
			return InstanceBuilder.Build<object>(Maybe.Just<Func<object>>(delegate
			{
				IEnumerable<Tuple<Verb, Type>> verbs2 = verbs;
				Func<Tuple<Verb, Type>, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((Tuple<Verb, Type> v) => nameComparer.Equals(v.Item1.Name, arguments.First<string>())));
				}
				return verbs2.Single(predicate).Item2.AutoDefault();
			}), tokenizer, arguments.Skip(1), nameComparer, ignoreValueCase, parsingCulture, autoHelp, autoVersion, nonFatalErrors);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000AD3C File Offset: 0x00008F3C
		private static HelpVerbRequestedError MakeHelpVerbRequestedError(IEnumerable<Tuple<Verb, Type>> verbs, string verb, StringComparer nameComparer)
		{
			if (verb.Length <= 0)
			{
				return new HelpVerbRequestedError(null, null, false);
			}
			return verbs.SingleOrDefault((Tuple<Verb, Type> v) => nameComparer.Equals(v.Item1.Name, verb)).ToMaybe<Tuple<Verb, Type>>().MapValueOrDefault((Tuple<Verb, Type> v) => new HelpVerbRequestedError(v.Item1.Name, v.Item2, true), new HelpVerbRequestedError(null, null, false));
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000ADB7 File Offset: 0x00008FB7
		private static NotParsed<object> MakeNotParsed(IEnumerable<Type> types, params Error[] errors)
		{
			return new NotParsed<object>(TypeInfo.Create(typeof(NullInstance), types), errors);
		}
	}
}
