using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine.Core;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Text
{
	// Token: 0x02000059 RID: 89
	public class HelpText
	{
		// Token: 0x06000204 RID: 516 RVA: 0x00008A48 File Offset: 0x00006C48
		private ComparableOption ToComparableOption(Specification spec, int index)
		{
			OptionSpecification optionSpecification = spec as OptionSpecification;
			ValueSpecification valueSpecification = spec as ValueSpecification;
			bool required = optionSpecification != null && optionSpecification.Required;
			return new ComparableOption
			{
				Required = required,
				IsOption = (optionSpecification != null),
				IsValue = (valueSpecification != null),
				LongName = (((optionSpecification != null) ? optionSpecification.LongName : null) ?? ((valueSpecification != null) ? valueSpecification.MetaName : null)),
				ShortName = ((optionSpecification != null) ? optionSpecification.ShortName : null),
				Index = index
			};
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00008AD5 File Offset: 0x00006CD5
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00008ADD File Offset: 0x00006CDD
		public Comparison<ComparableOption> OptionComparison { get; set; }

		// Token: 0x06000207 RID: 519 RVA: 0x00008AE6 File Offset: 0x00006CE6
		public HelpText() : this(SentenceBuilder.Create(), string.Empty, string.Empty)
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00008AFD File Offset: 0x00006CFD
		public HelpText(SentenceBuilder sentenceBuilder) : this(sentenceBuilder, string.Empty, string.Empty)
		{
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00008B10 File Offset: 0x00006D10
		public HelpText(string heading) : this(SentenceBuilder.Create(), heading, string.Empty)
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008B23 File Offset: 0x00006D23
		public HelpText(SentenceBuilder sentenceBuilder, string heading) : this(sentenceBuilder, heading, string.Empty)
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008B32 File Offset: 0x00006D32
		public HelpText(string heading, string copyright) : this(SentenceBuilder.Create(), heading, copyright)
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008B44 File Offset: 0x00006D44
		public HelpText(SentenceBuilder sentenceBuilder, string heading, string copyright)
		{
			if (sentenceBuilder == null)
			{
				throw new ArgumentNullException("sentenceBuilder");
			}
			if (heading == null)
			{
				throw new ArgumentNullException("heading");
			}
			if (copyright == null)
			{
				throw new ArgumentNullException("copyright");
			}
			this.preOptionsHelp = new StringBuilder(128);
			this.postOptionsHelp = new StringBuilder(128);
			try
			{
				this.maximumDisplayWidth = Console.WindowWidth;
				if (this.maximumDisplayWidth < 1)
				{
					this.maximumDisplayWidth = 80;
				}
			}
			catch (IOException)
			{
				this.maximumDisplayWidth = 80;
			}
			this.sentenceBuilder = sentenceBuilder;
			this.heading = heading;
			this.copyright = copyright;
			this.autoHelp = true;
			this.autoVersion = true;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00008C00 File Offset: 0x00006E00
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00008C08 File Offset: 0x00006E08
		public string Heading
		{
			get
			{
				return this.heading;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.heading = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00008C1F File Offset: 0x00006E1F
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00008C27 File Offset: 0x00006E27
		public string Copyright
		{
			get
			{
				return this.copyright;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.copyright = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008C3E File Offset: 0x00006E3E
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00008C46 File Offset: 0x00006E46
		public int MaximumDisplayWidth
		{
			get
			{
				return this.maximumDisplayWidth;
			}
			set
			{
				this.maximumDisplayWidth = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00008C4F File Offset: 0x00006E4F
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00008C57 File Offset: 0x00006E57
		public bool AddDashesToOption
		{
			get
			{
				return this.addDashesToOption;
			}
			set
			{
				this.addDashesToOption = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00008C60 File Offset: 0x00006E60
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00008C68 File Offset: 0x00006E68
		public bool AdditionalNewLineAfterOption
		{
			get
			{
				return this.additionalNewLineAfterOption;
			}
			set
			{
				this.additionalNewLineAfterOption = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00008C71 File Offset: 0x00006E71
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00008C79 File Offset: 0x00006E79
		public bool AddEnumValuesToHelpText
		{
			get
			{
				return this.addEnumValuesToHelpText;
			}
			set
			{
				this.addEnumValuesToHelpText = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00008C82 File Offset: 0x00006E82
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00008C8A File Offset: 0x00006E8A
		public bool AutoHelp
		{
			get
			{
				return this.autoHelp;
			}
			set
			{
				this.autoHelp = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00008C93 File Offset: 0x00006E93
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00008C9B File Offset: 0x00006E9B
		public bool AutoVersion
		{
			get
			{
				return this.autoVersion;
			}
			set
			{
				this.autoVersion = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00008CA4 File Offset: 0x00006EA4
		public SentenceBuilder SentenceBuilder
		{
			get
			{
				return this.sentenceBuilder;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008CAC File Offset: 0x00006EAC
		public static HelpText AutoBuild<T>(ParserResult<T> parserResult, Func<HelpText, HelpText> onError, Func<Example, Example> onExample, bool verbsIndex = false, int maxDisplayWidth = 80)
		{
			HelpText auto = new HelpText
			{
				Heading = HeadingInfo.Empty,
				Copyright = CopyrightInfo.Empty,
				AdditionalNewLineAfterOption = true,
				AddDashesToOption = !verbsIndex,
				MaximumDisplayWidth = maxDisplayWidth
			};
			try
			{
				auto.Heading = HeadingInfo.Default;
				auto.Copyright = CopyrightInfo.Default;
			}
			catch (Exception)
			{
				auto = onError(auto);
			}
			IEnumerable<Error> enumerable = Enumerable.Empty<Error>();
			if (onError != null && parserResult.Tag == ParserResultType.NotParsed)
			{
				enumerable = ((NotParsed<T>)parserResult).Errors;
				if (enumerable.IsHelp() || enumerable.OnlyMeaningfulOnes().Any<Error>())
				{
					auto = onError(auto);
				}
			}
			ReflectionHelper.GetAttribute<AssemblyLicenseAttribute>().Do(delegate(AssemblyLicenseAttribute license)
			{
				license.AddToHelpText(auto, true);
			});
			Maybe<AssemblyUsageAttribute> attribute = ReflectionHelper.GetAttribute<AssemblyUsageAttribute>();
			Maybe<IEnumerable<string>> maybe = HelpText.RenderUsageTextAsLines<T>(parserResult, onExample).ToMaybe<string>();
			if (attribute.IsJust<AssemblyUsageAttribute>() || maybe.IsJust<IEnumerable<string>>())
			{
				string text = auto.SentenceBuilder.UsageHeadingText();
				if (text.Length > 0)
				{
					auto.AddPreOptionsLine(text);
				}
			}
			attribute.Do(delegate(AssemblyUsageAttribute usage)
			{
				usage.AddToHelpText(auto, true);
			});
			maybe.Do(delegate(IEnumerable<string> lines)
			{
				auto.AddPreOptionsLines(lines);
			});
			if (!verbsIndex || !parserResult.TypeInfo.Choices.Any<Type>())
			{
				if (!enumerable.Any((Error e) => e.Tag == ErrorType.NoVerbSelectedError))
				{
					auto.AddOptions<T>(parserResult);
					goto IL_1CA;
				}
			}
			auto.AddDashesToOption = false;
			auto.AddVerbs(parserResult.TypeInfo.Choices.ToArray<Type>());
			IL_1CA:
			return auto;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00008E9C File Offset: 0x0000709C
		public static HelpText AutoBuild<T>(ParserResult<T> parserResult, int maxDisplayWidth = 80)
		{
			if (parserResult.Tag != ParserResultType.NotParsed)
			{
				throw new ArgumentException("Excepting NotParsed<T> type.", "parserResult");
			}
			IEnumerable<Error> errors = ((NotParsed<T>)parserResult).Errors;
			if (errors.Any((Error e) => e.Tag == ErrorType.VersionRequestedError))
			{
				return new HelpText(string.Format("{0}{1}", HeadingInfo.Default, Environment.NewLine))
				{
					MaximumDisplayWidth = maxDisplayWidth
				}.AddPreOptionsLine(Environment.NewLine);
			}
			if (!errors.Any((Error e) => e.Tag == ErrorType.HelpVerbRequestedError))
			{
				return HelpText.AutoBuild<T>(parserResult, (HelpText current) => HelpText.DefaultParsingErrorsHandler<T>(parserResult, current), (Example e) => e, false, maxDisplayWidth);
			}
			HelpVerbRequestedError helpVerbRequestedError = errors.OfType<HelpVerbRequestedError>().Single<HelpVerbRequestedError>();
			NotParsed<object> pr = new NotParsed<object>(TypeInfo.Create(helpVerbRequestedError.Type), Enumerable.Empty<Error>());
			if (!helpVerbRequestedError.Matched)
			{
				return HelpText.AutoBuild<T>(parserResult, (HelpText current) => HelpText.DefaultParsingErrorsHandler<T>(parserResult, current), (Example e) => e, true, maxDisplayWidth);
			}
			return HelpText.AutoBuild<object>(pr, (HelpText current) => HelpText.DefaultParsingErrorsHandler<object>(pr, current), (Example e) => e, false, maxDisplayWidth);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00009038 File Offset: 0x00007238
		public static HelpText DefaultParsingErrorsHandler<T>(ParserResult<T> parserResult, HelpText current)
		{
			if (parserResult == null)
			{
				throw new ArgumentNullException("parserResult");
			}
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			if (((NotParsed<T>)parserResult).Errors.OnlyMeaningfulOnes().Empty<Error>())
			{
				return current;
			}
			IEnumerable<string> enumerable = HelpText.RenderParsingErrorsTextAsLines<T>(parserResult, current.SentenceBuilder.FormatError, current.SentenceBuilder.FormatMutuallyExclusiveSetErrors, 2);
			if (enumerable.Empty<string>())
			{
				return current;
			}
			return current.AddPreOptionsLine(Environment.NewLine + current.SentenceBuilder.ErrorsHeadingText()).AddPreOptionsLines(enumerable);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000090C8 File Offset: 0x000072C8
		public static implicit operator string(HelpText info)
		{
			return info.ToString();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000090D0 File Offset: 0x000072D0
		public HelpText AddPreOptionsLine(string value)
		{
			return this.AddPreOptionsLine(value, this.MaximumDisplayWidth);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000090DF File Offset: 0x000072DF
		public HelpText AddPostOptionsLine(string value)
		{
			return this.AddLine(this.postOptionsHelp, value);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000090EE File Offset: 0x000072EE
		public HelpText AddPreOptionsLines(IEnumerable<string> lines)
		{
			lines.ForEach(delegate(string line)
			{
				this.AddPreOptionsLine(line);
			});
			return this;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009103 File Offset: 0x00007303
		public HelpText AddPostOptionsLines(IEnumerable<string> lines)
		{
			lines.ForEach(delegate(string line)
			{
				this.AddPostOptionsLine(line);
			});
			return this;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00009118 File Offset: 0x00007318
		public HelpText AddPreOptionsText(string text)
		{
			text.Split(new string[]
			{
				Environment.NewLine
			}, StringSplitOptions.None).ForEach(delegate(string line)
			{
				this.AddPreOptionsLine(line);
			});
			return this;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009141 File Offset: 0x00007341
		public HelpText AddPostOptionsText(string text)
		{
			text.Split(new string[]
			{
				Environment.NewLine
			}, StringSplitOptions.None).ForEach(delegate(string line)
			{
				this.AddPostOptionsLine(line);
			});
			return this;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000916A File Offset: 0x0000736A
		public HelpText AddOptions<T>(ParserResult<T> result)
		{
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			return this.AddOptionsImpl(this.GetSpecificationsFromType(result.TypeInfo.Current), this.SentenceBuilder.RequiredWord(), this.MaximumDisplayWidth);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000091A8 File Offset: 0x000073A8
		public HelpText AddVerbs(params Type[] types)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			if (types.Length == 0)
			{
				throw new ArgumentOutOfRangeException("types");
			}
			return this.AddOptionsImpl(this.AdaptVerbsToSpecifications(types), this.SentenceBuilder.RequiredWord(), this.MaximumDisplayWidth);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000091F5 File Offset: 0x000073F5
		public HelpText AddOptions<T>(int maximumLength, ParserResult<T> result)
		{
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			return this.AddOptionsImpl(this.GetSpecificationsFromType(result.TypeInfo.Current), this.SentenceBuilder.RequiredWord(), maximumLength);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000922D File Offset: 0x0000742D
		public HelpText AddVerbs(int maximumLength, params Type[] types)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			if (types.Length == 0)
			{
				throw new ArgumentOutOfRangeException("types");
			}
			return this.AddOptionsImpl(this.AdaptVerbsToSpecifications(types), this.SentenceBuilder.RequiredWord(), maximumLength);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000926A File Offset: 0x0000746A
		public static string RenderParsingErrorsText<T>(ParserResult<T> parserResult, Func<Error, string> formatError, Func<IEnumerable<MutuallyExclusiveSetError>, string> formatMutuallyExclusiveSetErrors, int indent)
		{
			return string.Join(Environment.NewLine, HelpText.RenderParsingErrorsTextAsLines<T>(parserResult, formatError, formatMutuallyExclusiveSetErrors, indent));
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000927F File Offset: 0x0000747F
		public static IEnumerable<string> RenderParsingErrorsTextAsLines<T>(ParserResult<T> parserResult, Func<Error, string> formatError, Func<IEnumerable<MutuallyExclusiveSetError>, string> formatMutuallyExclusiveSetErrors, int indent)
		{
			if (parserResult == null)
			{
				throw new ArgumentNullException("parserResult");
			}
			IEnumerable<Error> meaningfulErrors = ((NotParsed<T>)parserResult).Errors.OnlyMeaningfulOnes();
			if (meaningfulErrors.Empty<Error>())
			{
				yield break;
			}
			foreach (Error arg in from e in meaningfulErrors
			where e.Tag != ErrorType.MutuallyExclusiveSetError
			select e)
			{
				StringBuilder stringBuilder = new StringBuilder(indent.Spaces()).Append(formatError(arg));
				yield return stringBuilder.ToString();
			}
			IEnumerator<Error> enumerator = null;
			string text = formatMutuallyExclusiveSetErrors(meaningfulErrors.OfType<MutuallyExclusiveSetError>());
			if (text.Length > 0)
			{
				string[] array = text.Split(new string[]
				{
					Environment.NewLine
				}, StringSplitOptions.None);
				foreach (string text2 in array)
				{
					yield return text2;
				}
				string[] array2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000092A4 File Offset: 0x000074A4
		public static string RenderUsageText<T>(ParserResult<T> parserResult)
		{
			return HelpText.RenderUsageText<T>(parserResult, (Example example) => example);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000092CB File Offset: 0x000074CB
		public static string RenderUsageText<T>(ParserResult<T> parserResult, Func<Example, Example> mapperFunc)
		{
			return string.Join(Environment.NewLine, HelpText.RenderUsageTextAsLines<T>(parserResult, mapperFunc));
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000092DE File Offset: 0x000074DE
		public static IEnumerable<string> RenderUsageTextAsLines<T>(ParserResult<T> parserResult, Func<Example, Example> mapperFunc)
		{
			if (parserResult == null)
			{
				throw new ArgumentNullException("parserResult");
			}
			Maybe<Tuple<UsageAttribute, IEnumerable<Example>>> usageFromType = HelpText.GetUsageFromType(parserResult.TypeInfo.Current);
			if (usageFromType.MatchNothing())
			{
				yield break;
			}
			Tuple<UsageAttribute, IEnumerable<Example>> tuple = usageFromType.FromJustOrFail(null);
			IEnumerable<Example> item = tuple.Item2;
			string appAlias = tuple.Item1.ApplicationAlias ?? ReflectionHelper.GetAssemblyName();
			foreach (Example arg in item)
			{
				Example example = mapperFunc(arg);
				StringBuilder stringBuilder = new StringBuilder(example.HelpText).Append(':');
				yield return stringBuilder.ToString();
				IEnumerable<UnParserSettings> formatStylesOrDefault = example.GetFormatStylesOrDefault();
				using (IEnumerator<UnParserSettings> enumerator2 = formatStylesOrDefault.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						UnParserSettings s = enumerator2.Current;
						StringBuilder stringBuilder2 = new StringBuilder(2.Spaces()).Append(appAlias).Append(' ').Append(Parser.Default.FormatCommandLine(example.Sample, delegate(UnParserSettings config)
						{
							config.PreferShortName = s.PreferShortName;
							config.GroupSwitches = s.GroupSwitches;
							config.UseEqualToken = s.UseEqualToken;
						}));
						yield return stringBuilder2.ToString();
					}
				}
				IEnumerator<UnParserSettings> enumerator2 = null;
				example = null;
			}
			IEnumerator<Example> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000092F8 File Offset: 0x000074F8
		public override string ToString()
		{
			return new StringBuilder(this.heading.SafeLength() + this.copyright.SafeLength() + this.preOptionsHelp.SafeLength() + this.optionsHelp.SafeLength() + 10).Append(this.heading).AppendWhen(!string.IsNullOrEmpty(this.copyright), new string[]
			{
				Environment.NewLine,
				this.copyright
			}).AppendWhen(this.preOptionsHelp.Length > 0, new string[]
			{
				Environment.NewLine,
				this.preOptionsHelp.ToString()
			}).AppendWhen(this.optionsHelp != null && this.optionsHelp.Length > 0, new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				this.optionsHelp.SafeToString()
			}).AppendWhen(this.postOptionsHelp.Length > 0, new string[]
			{
				Environment.NewLine,
				this.postOptionsHelp.ToString()
			}).ToString();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009418 File Offset: 0x00007618
		internal static void AddLine(StringBuilder builder, string value, int maximumLength)
		{
			if (builder == null)
			{
				throw new ArgumentNullException("builder");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (maximumLength < 1)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			value = value.TrimEnd(Array.Empty<char>());
			builder.AppendWhen(builder.Length > 0, new string[]
			{
				Environment.NewLine
			});
			builder.Append(TextWrapper.WrapAndIndentText(value, 0, maximumLength));
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000948C File Offset: 0x0000768C
		private IEnumerable<Specification> GetSpecificationsFromType(Type type)
		{
			IEnumerable<Specification> specifications = type.GetSpecifications(new Func<PropertyInfo, Specification>(Specification.FromProperty));
			IEnumerable<OptionSpecification> enumerable = specifications.OfType<OptionSpecification>();
			if (this.autoHelp)
			{
				enumerable = enumerable.Concat(new OptionSpecification[]
				{
					this.MakeHelpEntry()
				});
			}
			if (this.autoVersion)
			{
				enumerable = enumerable.Concat(new OptionSpecification[]
				{
					this.MakeVersionEntry()
				});
			}
			IOrderedEnumerable<ValueSpecification> second = from v in specifications.OfType<ValueSpecification>()
			orderby v.Index
			select v;
			return Enumerable.Empty<Specification>().Concat(enumerable).Concat(second);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000952B File Offset: 0x0000772B
		private static Maybe<Tuple<UsageAttribute, IEnumerable<Example>>> GetUsageFromType(Type type)
		{
			return type.GetUsageData().Map(delegate(Tuple<PropertyInfo, UsageAttribute> tuple)
			{
				PropertyInfo item = tuple.Item1;
				UsageAttribute item2 = tuple.Item2;
				IEnumerable<Example> item3 = (IEnumerable<Example>)item.GetValue(null, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null);
				return Tuple.Create<UsageAttribute, IEnumerable<Example>>(item2, item3);
			});
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00009558 File Offset: 0x00007758
		private IEnumerable<Specification> AdaptVerbsToSpecifications(IEnumerable<Type> types)
		{
			IEnumerable<OptionSpecification> enumerable = from verbTuple in Verb.SelectFromTypes(types)
			select OptionSpecification.NewSwitch(string.Empty, verbTuple.Item1.Name, false, verbTuple.Item1.HelpText, string.Empty, verbTuple.Item1.Hidden);
			if (this.autoHelp)
			{
				enumerable = enumerable.Concat(new OptionSpecification[]
				{
					this.MakeHelpEntry()
				});
			}
			if (this.autoVersion)
			{
				enumerable = enumerable.Concat(new OptionSpecification[]
				{
					this.MakeVersionEntry()
				});
			}
			return enumerable;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000095D0 File Offset: 0x000077D0
		private HelpText AddOptionsImpl(IEnumerable<Specification> specifications, string requiredWord, int maximumLength)
		{
			int maxLength = this.GetMaxLength(specifications);
			this.optionsHelp = new StringBuilder(128);
			int remainingSpace = maximumLength - (maxLength + 6);
			if (this.OptionComparison != null)
			{
				int i = -1;
				List<ComparableOption> list = specifications.ToList<Specification>().Select(delegate(Specification s)
				{
					int i = i;
					i++;
					return this.ToComparableOption(s, i);
				}).ToList<ComparableOption>();
				list.Sort(this.OptionComparison);
				using (List<ComparableOption>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ComparableOption comparableOption = enumerator.Current;
						Specification specification = specifications.ElementAt(comparableOption.Index);
						this.AddOption(requiredWord, maxLength, specification, remainingSpace);
					}
					return this;
				}
			}
			specifications.ForEach(delegate(Specification option)
			{
				this.AddOption(requiredWord, maxLength, option, remainingSpace);
			});
			return this;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000096EC File Offset: 0x000078EC
		private OptionSpecification MakeHelpEntry()
		{
			return OptionSpecification.NewSwitch(string.Empty, "help", false, this.sentenceBuilder.HelpCommandText(this.AddDashesToOption), string.Empty, false);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000971A File Offset: 0x0000791A
		private OptionSpecification MakeVersionEntry()
		{
			return OptionSpecification.NewSwitch(string.Empty, "version", false, this.sentenceBuilder.VersionCommandText(this.AddDashesToOption), string.Empty, false);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009748 File Offset: 0x00007948
		private HelpText AddPreOptionsLine(string value, int maximumLength)
		{
			HelpText.AddLine(this.preOptionsHelp, value, maximumLength);
			return this;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009758 File Offset: 0x00007958
		private HelpText AddOption(string requiredWord, int maxLength, Specification specification, int widthOfHelpText)
		{
			if (specification.Hidden)
			{
				return this;
			}
			this.optionsHelp.Append("  ");
			StringBuilder stringBuilder = new StringBuilder(maxLength).BimapIf(specification.Tag == SpecificationType.Option, (StringBuilder it) => it.Append(this.AddOptionName(maxLength, (OptionSpecification)specification)), (StringBuilder it) => it.Append(this.AddValueName(maxLength, (ValueSpecification)specification)));
			this.optionsHelp.Append((stringBuilder.Length < maxLength) ? stringBuilder.ToString().PadRight(maxLength) : stringBuilder.ToString()).Append(4.Spaces());
			string optionHelpText = specification.HelpText;
			if (this.addEnumValuesToHelpText && specification.EnumValues.Any<string>())
			{
				optionHelpText = optionHelpText + " Valid values: " + string.Join(", ", specification.EnumValues);
			}
			specification.DefaultValue.Do(delegate(object defaultValue)
			{
				optionHelpText = "(Default: {0}) ".FormatInvariant(new object[]
				{
					HelpText.FormatDefaultValue<object>(defaultValue)
				}) + optionHelpText;
			});
			if (specification.Required)
			{
				optionHelpText = "{0} ".FormatInvariant(new object[]
				{
					requiredWord
				}) + optionHelpText;
			}
			string value = TextWrapper.WrapAndIndentText(optionHelpText, maxLength + 6, widthOfHelpText).TrimStart(Array.Empty<char>());
			this.optionsHelp.Append(value).Append(Environment.NewLine).AppendWhen(this.additionalNewLineAfterOption, new string[]
			{
				Environment.NewLine
			});
			return this;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000990C File Offset: 0x00007B0C
		private string AddOptionName(int maxLength, OptionSpecification specification)
		{
			return new StringBuilder(maxLength).MapIf(specification.ShortName.Length > 0, (StringBuilder it) => it.AppendWhen(this.addDashesToOption, new char[]
			{
				'-'
			}).AppendFormat("{0}", specification.ShortName).AppendFormatWhen(specification.MetaValue.Length > 0, " {0}", new object[]
			{
				specification.MetaValue
			}).AppendWhen(specification.LongName.Length > 0, new string[]
			{
				", "
			})).MapIf(specification.LongName.Length > 0, (StringBuilder it) => it.AppendWhen(this.addDashesToOption, new string[]
			{
				"--"
			}).AppendFormat("{0}", specification.LongName).AppendFormatWhen(specification.MetaValue.Length > 0, "={0}", new object[]
			{
				specification.MetaValue
			})).ToString();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009980 File Offset: 0x00007B80
		private string AddValueName(int maxLength, ValueSpecification specification)
		{
			return new StringBuilder(maxLength).BimapIf(specification.MetaName.Length > 0, (StringBuilder it) => it.AppendFormat("{0} (pos. {1})", specification.MetaName, specification.Index), (StringBuilder it) => it.AppendFormat("value pos. {0}", specification.Index)).AppendFormatWhen(specification.MetaValue.Length > 0, " {0}", new object[]
			{
				specification.MetaValue
			}).ToString();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009A06 File Offset: 0x00007C06
		private HelpText AddLine(StringBuilder builder, string value)
		{
			HelpText.AddLine(builder, value, this.MaximumDisplayWidth);
			return this;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00009A16 File Offset: 0x00007C16
		private int GetMaxLength(IEnumerable<Specification> specifications)
		{
			return specifications.Aggregate(0, delegate(int length, Specification spec)
			{
				if (spec.Hidden)
				{
					return length;
				}
				int val = (spec.Tag == SpecificationType.Option) ? this.GetMaxOptionLength((OptionSpecification)spec) : this.GetMaxValueLength((ValueSpecification)spec);
				return Math.Max(length, val);
			});
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009A2C File Offset: 0x00007C2C
		private int GetMaxOptionLength(OptionSpecification spec)
		{
			int num = 0;
			bool flag = spec.ShortName.Length > 0;
			bool flag2 = spec.LongName.Length > 0;
			int num2 = 0;
			if (spec.MetaValue.Length > 0)
			{
				num2 = spec.MetaValue.Length + 1;
			}
			if (flag)
			{
				num++;
				if (this.AddDashesToOption)
				{
					num++;
				}
				num += num2;
			}
			if (flag2)
			{
				num += spec.LongName.Length;
				if (this.AddDashesToOption)
				{
					num += 2;
				}
				num += num2;
			}
			if (flag && flag2)
			{
				num += 2;
			}
			return num;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009AB8 File Offset: 0x00007CB8
		private int GetMaxValueLength(ValueSpecification spec)
		{
			int num = 0;
			bool flag = spec.MetaName.Length > 0;
			int num2 = 0;
			if (spec.MetaValue.Length > 0)
			{
				num2 = spec.MetaValue.Length + 1;
			}
			if (flag)
			{
				num += spec.MetaName.Length + spec.Index.ToStringInvariant<int>().Length + 8;
			}
			else
			{
				num += spec.Index.ToStringInvariant<int>().Length + 11;
			}
			return num + num2;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009B34 File Offset: 0x00007D34
		private static string FormatDefaultValue<T>(T value)
		{
			if (value is bool)
			{
				return value.ToStringLocal<T>().ToLowerInvariant();
			}
			if (value is string)
			{
				return value.ToStringLocal<T>();
			}
			IEnumerable enumerable = value as IEnumerable;
			if (enumerable == null)
			{
				return value.ToStringLocal<T>();
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object value2 in enumerable)
			{
				stringBuilder.Append(value2.ToStringLocal<object>()).Append(" ");
			}
			if (stringBuilder.Length <= 0)
			{
				return string.Empty;
			}
			return stringBuilder.ToString(0, stringBuilder.Length - 1);
		}

		// Token: 0x040000A4 RID: 164
		public static Comparison<ComparableOption> RequiredThenAlphaComparison = delegate(ComparableOption attr1, ComparableOption attr2)
		{
			if (attr1.IsOption && attr2.IsOption)
			{
				if (attr1.Required && !attr2.Required)
				{
					return -1;
				}
				if (!attr1.Required && attr2.Required)
				{
					return 1;
				}
				return string.Compare(attr1.LongName, attr2.LongName, StringComparison.Ordinal);
			}
			else
			{
				if (attr1.IsOption && attr2.IsValue)
				{
					return -1;
				}
				return 1;
			}
		};

		// Token: 0x040000A5 RID: 165
		private const int BuilderCapacity = 128;

		// Token: 0x040000A6 RID: 166
		private const int DefaultMaximumLength = 80;

		// Token: 0x040000A7 RID: 167
		private const int OptionToHelpTextSeparatorWidth = 4;

		// Token: 0x040000A8 RID: 168
		private const int OptionPrefixWidth = 2;

		// Token: 0x040000A9 RID: 169
		private const int TotalOptionPadding = 6;

		// Token: 0x040000AA RID: 170
		private readonly StringBuilder preOptionsHelp;

		// Token: 0x040000AB RID: 171
		private readonly StringBuilder postOptionsHelp;

		// Token: 0x040000AC RID: 172
		private readonly SentenceBuilder sentenceBuilder;

		// Token: 0x040000AD RID: 173
		private int maximumDisplayWidth;

		// Token: 0x040000AE RID: 174
		private string heading;

		// Token: 0x040000AF RID: 175
		private string copyright;

		// Token: 0x040000B0 RID: 176
		private bool additionalNewLineAfterOption;

		// Token: 0x040000B1 RID: 177
		private StringBuilder optionsHelp;

		// Token: 0x040000B2 RID: 178
		private bool addDashesToOption;

		// Token: 0x040000B3 RID: 179
		private bool addEnumValuesToHelpText;

		// Token: 0x040000B4 RID: 180
		private bool autoHelp;

		// Token: 0x040000B5 RID: 181
		private bool autoVersion;
	}
}
