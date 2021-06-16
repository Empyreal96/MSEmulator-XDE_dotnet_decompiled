using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Core
{
	// Token: 0x0200006E RID: 110
	internal static class OptionMapper
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000AF84 File Offset: 0x00009184
		public static Result<IEnumerable<SpecificationProperty>, Error> MapValues(IEnumerable<SpecificationProperty> propertyTuples, IEnumerable<KeyValuePair<string, IEnumerable<string>>> options, Func<IEnumerable<string>, Type, bool, Maybe<object>> converter, StringComparer comparer)
		{
			IEnumerable<Tuple<SpecificationProperty, Maybe<Error>>> source = propertyTuples.Select(delegate(SpecificationProperty pt)
			{
				Maybe<KeyValuePair<string, IEnumerable<string>>> maybe = options.FirstOrDefault((KeyValuePair<string, IEnumerable<string>> s) => s.Key.MatchName(((OptionSpecification)pt.Specification).ShortName, ((OptionSpecification)pt.Specification).LongName, comparer)).ToMaybe<KeyValuePair<string, IEnumerable<string>>>();
				if (!maybe.IsJust<KeyValuePair<string, IEnumerable<string>>>())
				{
					return Tuple.Create<SpecificationProperty, Maybe<Error>>(pt, Maybe.Nothing<Error>());
				}
				return maybe.SelectMany(delegate(KeyValuePair<string, IEnumerable<string>> sequence)
				{
					Func<IEnumerable<string>, Type, bool, Maybe<object>> converter2 = converter;
					KeyValuePair<string, IEnumerable<string>> keyValuePair = sequence;
					return converter2(keyValuePair.Value, pt.Property.PropertyType, pt.Specification.TargetType != TargetType.Sequence);
				}, (KeyValuePair<string, IEnumerable<string>> sequence, object converted) => Tuple.Create<SpecificationProperty, Maybe<Error>>(pt.WithValue(Maybe.Just<object>(converted)), Maybe.Nothing<Error>())).GetValueOrDefault(Tuple.Create<SpecificationProperty, Maybe<Error>>(pt, Maybe.Just<Error>(new BadFormatConversionError(((OptionSpecification)pt.Specification).FromOptionSpecification()))));
			}).Memorize<Tuple<SpecificationProperty, Maybe<Error>>>();
			return Result.Succeed<IEnumerable<SpecificationProperty>, Error>(from se in source
			select se.Item1, source.Select((Tuple<SpecificationProperty, Maybe<Error>> se) => se.Item2).OfType<Just<Error>>().Select((Just<Error> se) => se.Value));
		}
	}
}
