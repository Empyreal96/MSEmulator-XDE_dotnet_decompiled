using System;
using System.Collections.Generic;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000078 RID: 120
	internal static class SpecificationGuards
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000BC51 File Offset: 0x00009E51
		private static Func<Specification, bool> GuardAgainstScalarWithRange()
		{
			return (Specification spec) => spec.TargetType == TargetType.Scalar && (spec.Min.IsJust<int>() || spec.Max.IsJust<int>());
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000BC72 File Offset: 0x00009E72
		private static Func<Specification, bool> GuardAgainstSequenceWithWrongRange()
		{
			return delegate(Specification spec)
			{
				if (spec.TargetType == TargetType.Sequence)
				{
					return spec.HavingRange((int min, int max) => min > max);
				}
				return false;
			};
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000BC93 File Offset: 0x00009E93
		private static Func<Specification, bool> GuardAgainstOneCharLongName()
		{
			return (Specification spec) => spec.IsOption() && ((OptionSpecification)spec).LongName.Length == 1;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		private static Func<Specification, bool> GuardAgainstSequenceWithZeroRange()
		{
			return delegate(Specification spec)
			{
				if (spec.TargetType != TargetType.Sequence)
				{
					return false;
				}
				if (!spec.HavingMin((int min) => min == 0))
				{
					return spec.HavingMax((int max) => max == 0);
				}
				return true;
			};
		}

		// Token: 0x040000DE RID: 222
		public static readonly IEnumerable<Tuple<Func<Specification, bool>, string>> Lookup = new List<Tuple<Func<Specification, bool>, string>>
		{
			Tuple.Create<Func<Specification, bool>, string>(SpecificationGuards.GuardAgainstScalarWithRange(), "Scalar option specifications do not support range specification."),
			Tuple.Create<Func<Specification, bool>, string>(SpecificationGuards.GuardAgainstSequenceWithWrongRange(), "Bad range in sequence option specifications."),
			Tuple.Create<Func<Specification, bool>, string>(SpecificationGuards.GuardAgainstSequenceWithZeroRange(), "Zero is not allowed in range of sequence option specifications."),
			Tuple.Create<Func<Specification, bool>, string>(SpecificationGuards.GuardAgainstOneCharLongName(), "Long name should be longer than one character.")
		};
	}
}
