using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x0200007B RID: 123
	internal static class SpecificationPropertyRules
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0000BE58 File Offset: 0x0000A058
		public static IEnumerable<Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>>> Lookup(IEnumerable<Token> tokens)
		{
			return new List<Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>>>
			{
				SpecificationPropertyRules.EnforceMutuallyExclusiveSet(),
				SpecificationPropertyRules.EnforceRequired(),
				SpecificationPropertyRules.EnforceRange(),
				SpecificationPropertyRules.EnforceSingle(tokens)
			};
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BE8C File Offset: 0x0000A08C
		private static Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>> EnforceMutuallyExclusiveSet()
		{
			return delegate(IEnumerable<SpecificationProperty> specProps)
			{
				IEnumerable<OptionSpecification> source = from sp in specProps
				where sp.Specification.IsOption()
				where sp.Value.IsJust<object>()
				let o = (OptionSpecification)sp.Specification
				where o.SetName.Length > 0
				select o;
				if ((from o in source
				group o by o.SetName into g
				select g).Count<IGrouping<string, OptionSpecification>>() > 1)
				{
					return from o in source
					select new MutuallyExclusiveSetError(o.FromOptionSpecification(), o.SetName);
				}
				return Enumerable.Empty<Error>();
			};
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000BEAD File Offset: 0x0000A0AD
		private static Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>> EnforceRequired()
		{
			return delegate(IEnumerable<SpecificationProperty> specProps)
			{
				IEnumerable<Specification> enumerable = from sp in specProps
				where sp.Specification.IsOption()
				where sp.Specification.Required
				where sp.Value.IsJust<object>()
				let o = (OptionSpecification)sp.Specification
				where o.SetName.Length > 0
				select sp.Specification;
				IEnumerable<string> setWithRequiredValue = (from s in enumerable
				let o = (OptionSpecification)s
				where o.SetName.Length > 0
				select o.SetName).Distinct<string>();
				return from sp in (from sp in specProps
				where sp.Specification.IsOption()
				where sp.Specification.Required
				where sp.Value.IsNothing<object>()
				let o = (OptionSpecification)sp.Specification
				where o.SetName.Length > 0
				where setWithRequiredValue.ContainsIfNotEmpty(o.SetName)
				select sp.Specification).Except(enumerable).Concat(from sp in specProps
				where sp.Specification.IsOption()
				where sp.Specification.Required
				where sp.Value.IsNothing<object>()
				let o = (OptionSpecification)sp.Specification
				where o.SetName.Length == 0
				select sp.Specification).Concat(from sp in specProps
				where sp.Specification.IsValue()
				where sp.Specification.Required
				where sp.Value.IsNothing<object>()
				select sp.Specification)
				select new MissingRequiredOptionError(sp.FromSpecification());
			};
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000BECE File Offset: 0x0000A0CE
		private static Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>> EnforceRange()
		{
			return delegate(IEnumerable<SpecificationProperty> specProps)
			{
				IEnumerable<SpecificationProperty> source = from sp in specProps
				where sp.Specification.TargetType == TargetType.Sequence
				where sp.Value.IsJust<object>()
				where (sp.Specification.Min.IsJust<int>() && ((Array)sp.Value.FromJustOrFail(null)).Length < sp.Specification.Min.FromJustOrFail(null)) || (sp.Specification.Max.IsJust<int>() && ((Array)sp.Value.FromJustOrFail(null)).Length > sp.Specification.Max.FromJustOrFail(null))
				select sp;
				if (source.Any<SpecificationProperty>())
				{
					return from s in source
					select new SequenceOutOfRangeError(s.Specification.FromSpecification());
				}
				return Enumerable.Empty<Error>();
			};
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000BEEF File Offset: 0x0000A0EF
		private static Func<IEnumerable<SpecificationProperty>, IEnumerable<Error>> EnforceSingle(IEnumerable<Token> tokens)
		{
			return delegate(IEnumerable<SpecificationProperty> specProps)
			{
				IEnumerable<OptionSpecification> inner = from sp in specProps
				where sp.Specification.IsOption()
				where sp.Value.IsJust<object>()
				select (OptionSpecification)sp.Specification;
				var first = from t in tokens
				where t.IsName()
				join o in inner on t.Text equals o.ShortName into to
				from o in to.DefaultIfEmpty<OptionSpecification>()
				where o != null
				select new
				{
					o.ShortName,
					o.LongName
				};
				var second = from t in tokens
				where t.IsName()
				join o in inner on t.Text equals o.LongName into to
				from o in to.DefaultIfEmpty<OptionSpecification>()
				where o != null
				select new
				{
					o.ShortName,
					o.LongName
				};
				return from x in first.Concat(second)
				group x by x into g
				let count = g.Count()
				select new
				{
					Value = g.Key,
					Count = count
				} into y
				where y.Count > 1
				select new RepeatedOptionError(new NameInfo(y.Value.ShortName, y.Value.LongName));
			};
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000BF08 File Offset: 0x0000A108
		private static bool ContainsIfNotEmpty<T>(this IEnumerable<T> sequence, T value)
		{
			return !sequence.Any<T>() || sequence.Contains(value);
		}
	}
}
