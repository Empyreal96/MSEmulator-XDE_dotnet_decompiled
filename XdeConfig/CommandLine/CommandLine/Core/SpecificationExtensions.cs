using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Core
{
	// Token: 0x02000077 RID: 119
	internal static class SpecificationExtensions
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000BABF File Offset: 0x00009CBF
		public static bool IsOption(this Specification specification)
		{
			return specification.Tag == SpecificationType.Option;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000BACA File Offset: 0x00009CCA
		public static bool IsValue(this Specification specification)
		{
			return specification.Tag == SpecificationType.Value;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000BAD8 File Offset: 0x00009CD8
		public static OptionSpecification WithLongName(this OptionSpecification specification, string newLongName)
		{
			return new OptionSpecification(specification.ShortName, newLongName, specification.Required, specification.SetName, specification.Min, specification.Max, specification.Separator, specification.DefaultValue, specification.HelpText, specification.MetaValue, specification.EnumValues, specification.ConversionType, specification.TargetType, specification.Hidden);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000BB39 File Offset: 0x00009D39
		public static string UniqueName(this OptionSpecification specification)
		{
			if (specification.ShortName.Length <= 0)
			{
				return specification.LongName;
			}
			return specification.ShortName;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000BB58 File Offset: 0x00009D58
		public static IEnumerable<Specification> ThrowingValidate(this IEnumerable<Specification> specifications, IEnumerable<Tuple<Func<Specification, bool>, string>> guardsLookup)
		{
			using (IEnumerator<Tuple<Func<Specification, bool>, string>> enumerator = guardsLookup.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tuple<Func<Specification, bool>, string> guard = enumerator.Current;
					if (specifications.Any((Specification spec) => guard.Item1(spec)))
					{
						throw new InvalidOperationException(guard.Item2);
					}
				}
			}
			return specifications;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000BBCC File Offset: 0x00009DCC
		public static bool HavingRange(this Specification specification, Func<int, int, bool> predicate)
		{
			int arg;
			int arg2;
			return specification.Min.MatchJust(out arg) && specification.Max.MatchJust(out arg2) && predicate(arg, arg2);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BC04 File Offset: 0x00009E04
		public static bool HavingMin(this Specification specification, Func<int, bool> predicate)
		{
			int arg;
			return specification.Min.MatchJust(out arg) && predicate(arg);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000BC2C File Offset: 0x00009E2C
		public static bool HavingMax(this Specification specification, Func<int, bool> predicate)
		{
			int arg;
			return specification.Max.MatchJust(out arg) && predicate(arg);
		}
	}
}
