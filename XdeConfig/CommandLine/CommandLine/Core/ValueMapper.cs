using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Infrastructure;
using CSharpx;
using RailwaySharp.ErrorHandling;

namespace CommandLine.Core
{
	// Token: 0x02000088 RID: 136
	internal static class ValueMapper
	{
		// Token: 0x06000324 RID: 804 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
		public static Result<IEnumerable<SpecificationProperty>, Error> MapValues(IEnumerable<SpecificationProperty> specProps, IEnumerable<string> values, Func<IEnumerable<string>, Type, bool, Maybe<object>> converter)
		{
			IEnumerable<Tuple<SpecificationProperty, Maybe<Error>>> source = ValueMapper.MapValuesImpl(specProps, values, converter);
			return Result.Succeed<IEnumerable<SpecificationProperty>, Error>(from pe in source
			select pe.Item1, source.Select((Tuple<SpecificationProperty, Maybe<Error>> pe) => pe.Item2).OfType<Just<Error>>().Select((Just<Error> e) => e.Value));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000C956 File Offset: 0x0000AB56
		private static IEnumerable<Tuple<SpecificationProperty, Maybe<Error>>> MapValuesImpl(IEnumerable<SpecificationProperty> specProps, IEnumerable<string> values, Func<IEnumerable<string>, Type, bool, Maybe<object>> converter)
		{
			if (specProps.Empty<SpecificationProperty>())
			{
				yield break;
			}
			SpecificationProperty pt = specProps.First<SpecificationProperty>();
			IEnumerable<string> taken = values.Take(pt.Specification.CountOfMaxNumberOfValues().GetValueOrDefault(values.Count<string>()));
			if (taken.Empty<string>())
			{
				yield return Tuple.Create<SpecificationProperty, Maybe<Error>>(pt, pt.Specification.MakeErrorInCaseOfMinConstraint());
				yield break;
			}
			Maybe<SpecificationProperty> maybe = specProps.Skip(1).FirstOrDefault((SpecificationProperty s) => s.Specification.IsValue()).ToMaybe<SpecificationProperty>();
			if (pt.Specification.Max.IsJust<int>() && maybe.IsNothing<SpecificationProperty>() && values.Skip(taken.Count<string>()).Any<string>())
			{
				yield return Tuple.Create<SpecificationProperty, Maybe<Error>>(pt, Maybe.Just<Error>(new SequenceOutOfRangeError(NameInfo.EmptyName)));
				yield break;
			}
			yield return converter(taken, pt.Property.PropertyType, pt.Specification.TargetType != TargetType.Sequence).MapValueOrDefault((object converted) => Tuple.Create<SpecificationProperty, Maybe<Error>>(pt.WithValue(Maybe.Just<object>(converted)), Maybe.Nothing<Error>()), Tuple.Create<SpecificationProperty, Maybe<Error>>(pt, Maybe.Just<Error>(new BadFormatConversionError(NameInfo.EmptyName))));
			foreach (Tuple<SpecificationProperty, Maybe<Error>> tuple in ValueMapper.MapValuesImpl(specProps.Skip(1), values.Skip(taken.Count<string>()), converter))
			{
				yield return tuple;
			}
			IEnumerator<Tuple<SpecificationProperty, Maybe<Error>>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000C974 File Offset: 0x0000AB74
		private static Maybe<int> CountOfMaxNumberOfValues(this Specification specification)
		{
			TargetType targetType = specification.TargetType;
			if (targetType != TargetType.Scalar)
			{
				if (targetType == TargetType.Sequence)
				{
					if (specification.Max.IsJust<int>())
					{
						return Maybe.Just<int>(specification.Max.FromJustOrFail(null));
					}
				}
				return Maybe.Nothing<int>();
			}
			return Maybe.Just<int>(1);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000C9BD File Offset: 0x0000ABBD
		private static Maybe<Error> MakeErrorInCaseOfMinConstraint(this Specification specification)
		{
			if (!specification.Min.IsJust<int>())
			{
				return Maybe.Nothing<Error>();
			}
			return Maybe.Just<Error>(new SequenceOutOfRangeError(NameInfo.EmptyName));
		}
	}
}
