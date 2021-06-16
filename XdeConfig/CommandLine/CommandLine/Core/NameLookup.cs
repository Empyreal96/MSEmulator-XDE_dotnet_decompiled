using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x0200006D RID: 109
	internal static class NameLookup
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x0000AECC File Offset: 0x000090CC
		public static NameLookupResult Contains(string name, IEnumerable<OptionSpecification> specifications, StringComparer comparer)
		{
			OptionSpecification optionSpecification = specifications.FirstOrDefault((OptionSpecification a) => name.MatchName(a.ShortName, a.LongName, comparer));
			if (optionSpecification == null)
			{
				return NameLookupResult.NoOptionFound;
			}
			if (!(optionSpecification.ConversionType == typeof(bool)))
			{
				return NameLookupResult.OtherOptionFound;
			}
			return NameLookupResult.BooleanOptionFound;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000AF20 File Offset: 0x00009120
		public static Maybe<char> HavingSeparator(string name, IEnumerable<OptionSpecification> specifications, StringComparer comparer)
		{
			return specifications.SingleOrDefault((OptionSpecification a) => name.MatchName(a.ShortName, a.LongName, comparer) && a.Separator > '\0').ToMaybe<OptionSpecification>().MapValueOrDefault((OptionSpecification spec) => Maybe.Just<char>(spec.Separator), Maybe.Nothing<char>());
		}
	}
}
