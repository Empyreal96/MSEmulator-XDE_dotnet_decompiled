using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x0200007A RID: 122
	internal static class SpecificationPropertyExtensions
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000BD90 File Offset: 0x00009F90
		public static SpecificationProperty WithSpecification(this SpecificationProperty specProp, Specification newSpecification)
		{
			if (newSpecification == null)
			{
				throw new ArgumentNullException("newSpecification");
			}
			return SpecificationProperty.Create(newSpecification, specProp.Property, specProp.Value);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000BDB2 File Offset: 0x00009FB2
		public static SpecificationProperty WithValue(this SpecificationProperty specProp, Maybe<object> newValue)
		{
			if (newValue == null)
			{
				throw new ArgumentNullException("newValue");
			}
			return SpecificationProperty.Create(specProp.Specification, specProp.Property, newValue);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BDD4 File Offset: 0x00009FD4
		public static Type GetConversionType(this SpecificationProperty specProp)
		{
			if (specProp.Specification.TargetType == TargetType.Sequence)
			{
				return specProp.Property.PropertyType.GetTypeInfo().GetGenericArguments().SingleOrDefault<Type>().ToMaybe<Type>().FromJustOrFail(new InvalidOperationException("Sequence properties should be of type IEnumerable<T>."));
			}
			return specProp.Property.PropertyType;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BE2C File Offset: 0x0000A02C
		public static IEnumerable<Error> Validate(this IEnumerable<SpecificationProperty> specProps, IEnumerable<Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>>> rules)
		{
			return rules.SelectMany((Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>> rule) => rule(specProps));
		}
	}
}
