using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Text
{
	// Token: 0x02000056 RID: 86
	internal static class ExampleExtensions
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000885C File Offset: 0x00006A5C
		public static IEnumerable<UnParserSettings> GetFormatStylesOrDefault(this Example example)
		{
			if (!example.FormatStyles.Any<UnParserSettings>())
			{
				return new UnParserSettings[]
				{
					new UnParserSettings
					{
						Consumed = true
					}
				};
			}
			return example.FormatStyles;
		}
	}
}
