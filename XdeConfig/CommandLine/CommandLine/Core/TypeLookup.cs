using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000087 RID: 135
	internal static class TypeLookup
	{
		// Token: 0x06000323 RID: 803 RVA: 0x0000C870 File Offset: 0x0000AA70
		public static Maybe<TypeDescriptor> FindTypeDescriptorAndSibling(string name, IEnumerable<OptionSpecification> specifications, StringComparer comparer)
		{
			return specifications.SingleOrDefault((OptionSpecification a) => name.MatchName(a.ShortName, a.LongName, comparer)).ToMaybe<OptionSpecification>().Map(delegate(OptionSpecification first)
			{
				TypeDescriptor descriptor = TypeDescriptor.Create(first.TargetType, first.Max, default(TypeDescriptor));
				Maybe<TypeDescriptor> nextValue = specifications.SkipWhile((OptionSpecification s) => s.Equals(first)).Take(1).SingleOrDefault((OptionSpecification x) => x.IsValue()).ToMaybe<OptionSpecification>().Map((OptionSpecification second) => TypeDescriptor.Create(second.TargetType, second.Max, default(TypeDescriptor)));
				return descriptor.WithNextValue(nextValue);
			});
		}
	}
}
