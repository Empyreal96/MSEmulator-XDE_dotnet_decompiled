using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Core;
using CommandLine.Infrastructure;

namespace CommandLine
{
	// Token: 0x02000040 RID: 64
	internal static class ErrorExtensions
	{
		// Token: 0x06000140 RID: 320 RVA: 0x000053CD File Offset: 0x000035CD
		public static ParserResult<T> ToParserResult<T>(this IEnumerable<Error> errors, T instance)
		{
			if (!errors.Any<Error>())
			{
				return new Parsed<T>(instance);
			}
			return new NotParsed<T>(instance.GetType().ToTypeInfo(), errors);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000053F8 File Offset: 0x000035F8
		public static IEnumerable<Error> OnlyMeaningfulOnes(this IEnumerable<Error> errors)
		{
			return from e in errors
			where !e.StopsProcessing
			where e.Tag != ErrorType.UnknownOptionError || !((UnknownOptionError)e).Token.EqualsOrdinalIgnoreCase("help")
			select e;
		}
	}
}
