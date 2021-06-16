using System;

namespace CommandLine.Core
{
	// Token: 0x0200006B RID: 107
	internal static class NameExtensions
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000AE82 File Offset: 0x00009082
		public static bool MatchName(this string value, string shortName, string longName, StringComparer comparer)
		{
			if (value.Length != 1)
			{
				return comparer.Equals(value, longName);
			}
			return comparer.Equals(value, shortName);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000AE9E File Offset: 0x0000909E
		public static NameInfo FromOptionSpecification(this OptionSpecification specification)
		{
			return new NameInfo(specification.ShortName, specification.LongName);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000AEB1 File Offset: 0x000090B1
		public static NameInfo FromSpecification(this Specification specification)
		{
			if (specification.Tag == SpecificationType.Option)
			{
				return ((OptionSpecification)specification).FromOptionSpecification();
			}
			return NameInfo.EmptyName;
		}
	}
}
