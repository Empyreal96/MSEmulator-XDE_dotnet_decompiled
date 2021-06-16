using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandLine.Core;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine
{
	// Token: 0x0200004F RID: 79
	public static class UnParserExtensions
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x00007D46 File Offset: 0x00005F46
		public static string FormatCommandLine<T>(this Parser parser, T options)
		{
			return parser.FormatCommandLine(options, delegate(UnParserSettings config)
			{
			});
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007D70 File Offset: 0x00005F70
		public static string FormatCommandLine<T>(this Parser parser, T options, Action<UnParserSettings> configuration)
		{
			UnParserExtensions.<>c__DisplayClass1_0<T> CS$<>8__locals1 = new UnParserExtensions.<>c__DisplayClass1_0<T>();
			CS$<>8__locals1.options = options;
			if (CS$<>8__locals1.options == null)
			{
				throw new ArgumentNullException("options");
			}
			CS$<>8__locals1.settings = new UnParserSettings();
			configuration(CS$<>8__locals1.settings);
			CS$<>8__locals1.settings.Consumed = true;
			Type type = CS$<>8__locals1.options.GetType();
			CS$<>8__locals1.builder = new StringBuilder();
			type.GetVerbSpecification().MapValueOrDefault((VerbAttribute verb) => CS$<>8__locals1.builder.Append(verb.Name).Append(' '), CS$<>8__locals1.builder);
			var source = (from info in type.GetSpecifications((PropertyInfo pi) => new
			{
				Specification = Specification.FromProperty(pi),
				Value = pi.GetValue(CS$<>8__locals1.options, null).NormalizeValue(),
				PropertyValue = pi.GetValue(CS$<>8__locals1.options, null)
			})
			where !info.PropertyValue.IsEmpty()
			select info).Memorize();
			var enumerable = from i in source
			where i.Specification.Tag == SpecificationType.Option
			select i into info
			let o = (OptionSpecification)info.Specification
			where o.TargetType != TargetType.Switch || (o.TargetType == TargetType.Switch && (bool)info.Value)
			where !o.Hidden || CS$<>8__locals1.settings.ShowHidden
			orderby o.UniqueName()
			select info;
			CS$<>8__locals1.shortSwitches = from info in enumerable
			let o = (OptionSpecification)info.Specification
			where o.TargetType == TargetType.Switch
			where o.ShortName.Length > 0
			orderby o.UniqueName()
			select info;
			var source2 = CS$<>8__locals1.settings.GroupSwitches ? (from info in enumerable
			where !CS$<>8__locals1.shortSwitches.Contains(info)
			select info) : enumerable;
			var source3 = from i in source
			where i.Specification.Tag == SpecificationType.Value
			select i into info
			let v = (ValueSpecification)info.Specification
			orderby v.Index
			select info;
			UnParserExtensions.<>c__DisplayClass1_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			StringBuilder builder;
			if (!CS$<>8__locals1.settings.GroupSwitches || !CS$<>8__locals1.shortSwitches.Any())
			{
				builder = CS$<>8__locals1.builder;
			}
			else
			{
				builder = CS$<>8__locals1.builder.Append('-').Append(string.Join(string.Empty, (from info in CS$<>8__locals1.shortSwitches
				select ((OptionSpecification)info.Specification).ShortName).ToArray<string>())).Append(' ');
			}
			CS$<>8__locals2.builder = builder;
			source2.ForEach(delegate(opt)
			{
				CS$<>8__locals1.builder.Append(UnParserExtensions.FormatOption((OptionSpecification)opt.Specification, opt.Value, CS$<>8__locals1.settings)).Append(' ');
			});
			CS$<>8__locals1.builder.AppendWhen(source3.Any() && parser.Settings.EnableDashDash, new string[]
			{
				"-- "
			});
			source3.ForEach(delegate(val)
			{
				CS$<>8__locals1.builder.Append(UnParserExtensions.FormatValue(val.Specification, val.Value)).Append(' ');
			});
			return CS$<>8__locals1.builder.ToString().TrimEnd(new char[]
			{
				' '
			});
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008158 File Offset: 0x00006358
		private static string FormatValue(Specification spec, object value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TargetType targetType = spec.TargetType;
			if (targetType != TargetType.Scalar)
			{
				if (targetType == TargetType.Sequence)
				{
					char sep = spec.SeperatorOrSpace();
					Func<object, object> func = delegate(object v)
					{
						if (sep != ' ')
						{
							return v;
						}
						return UnParserExtensions.FormatWithQuotesIfString(v);
					};
					foreach (object arg in ((IEnumerable)value))
					{
						stringBuilder.Append(func(arg)).Append(sep);
					}
					stringBuilder.TrimEndIfMatch(sep);
				}
			}
			else
			{
				stringBuilder.Append(UnParserExtensions.FormatWithQuotesIfString(value));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000081F4 File Offset: 0x000063F4
		private static object FormatWithQuotesIfString(object value)
		{
			Func<string, string> doubQt = delegate(string v)
			{
				if (!v.Contains("\""))
				{
					return v;
				}
				return v.Replace("\"", "\\\"");
			};
			return (value as string).ToMaybe<string>().MapValueOrDefault(delegate(string v)
			{
				if (!v.Contains(' ') && !v.Contains("\""))
				{
					return v;
				}
				return "\"".JoinTo(new string[]
				{
					doubQt(v),
					"\""
				});
			}, value);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008249 File Offset: 0x00006449
		private static char SeperatorOrSpace(this Specification spec)
		{
			return (spec as OptionSpecification).ToMaybe<OptionSpecification>().MapValueOrDefault(delegate(OptionSpecification o)
			{
				if (o.Separator == '\0')
				{
					return ' ';
				}
				return o.Separator;
			}, ' ');
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000827C File Offset: 0x0000647C
		private static string FormatOption(OptionSpecification spec, object value, UnParserSettings settings)
		{
			return new StringBuilder().Append(spec.FormatName(settings)).AppendWhen(spec.TargetType > TargetType.Switch, new string[]
			{
				UnParserExtensions.FormatValue(spec, value)
			}).ToString();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000082B4 File Offset: 0x000064B4
		private static string FormatName(this OptionSpecification optionSpec, UnParserSettings settings)
		{
			bool flag = (optionSpec.LongName.Length > 0 && !settings.PreferShortName) || optionSpec.ShortName.Length == 0;
			return new StringBuilder(flag ? "--".JoinTo(new string[]
			{
				optionSpec.LongName
			}) : "-".JoinTo(new string[]
			{
				optionSpec.ShortName
			})).AppendWhen(optionSpec.TargetType > TargetType.Switch, new string[]
			{
				(flag && settings.UseEqualToken) ? "=" : " "
			}).ToString();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008358 File Offset: 0x00006558
		private static object NormalizeValue(this object value)
		{
			return value;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000835C File Offset: 0x0000655C
		private static bool IsEmpty(this object value)
		{
			return value == null || (value is ValueType && value.Equals(value.GetType().GetDefaultValue())) || (value is string && ((string)value).Length == 0) || (value is IEnumerable && !((IEnumerable)value).GetEnumerator().MoveNext());
		}
	}
}
