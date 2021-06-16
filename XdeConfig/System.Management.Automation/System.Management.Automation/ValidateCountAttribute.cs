using System;
using System.Collections;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000423 RID: 1059
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidateCountAttribute : ValidateArgumentsAttribute
	{
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x00100B66 File Offset: 0x000FED66
		public int MinLength
		{
			get
			{
				return this.minLength;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002EEE RID: 12014 RVA: 0x00100B6E File Offset: 0x000FED6E
		public int MaxLength
		{
			get
			{
				return this.maxLength;
			}
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x00100B78 File Offset: 0x000FED78
		protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
		{
			uint num = 0U;
			IList list;
			ICollection collection;
			IEnumerable enumerable;
			if (arguments == null || arguments == AutomationNull.Value)
			{
				num = 0U;
			}
			else if ((list = (arguments as IList)) != null)
			{
				num = (uint)list.Count;
			}
			else if ((collection = (arguments as ICollection)) != null)
			{
				num = (uint)collection.Count;
			}
			else if ((enumerable = (arguments as IEnumerable)) != null)
			{
				IEnumerator enumerator = enumerable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += 1U;
				}
			}
			else
			{
				IEnumerator enumerator2;
				if ((enumerator2 = (arguments as IEnumerator)) == null)
				{
					throw new ValidationMetadataException("NotAnArrayParameter", null, Metadata.ValidateCountNotInArray, new object[0]);
				}
				while (enumerator2.MoveNext())
				{
					num += 1U;
				}
			}
			if ((ulong)num < (ulong)((long)this.minLength))
			{
				throw new ValidationMetadataException("ValidateCountSmallerThanMin", null, Metadata.ValidateCountMinLengthFailure, new object[]
				{
					this.minLength,
					num
				});
			}
			if ((ulong)num > (ulong)((long)this.maxLength))
			{
				throw new ValidationMetadataException("ValidateCountGreaterThanMax", null, Metadata.ValidateCountMaxLengthFailure, new object[]
				{
					this.maxLength,
					num
				});
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x00100C90 File Offset: 0x000FEE90
		public ValidateCountAttribute(int minLength, int maxLength)
		{
			if (minLength < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("minLength", minLength);
			}
			if (maxLength <= 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("maxLength", maxLength);
			}
			if (maxLength < minLength)
			{
				throw new ValidationMetadataException("ValidateRangeMaxLengthSmallerThanMinLength", null, Metadata.ValidateCountMaxLengthSmallerThanMinLength, new object[0]);
			}
			this.minLength = minLength;
			this.maxLength = maxLength;
		}

		// Token: 0x040018AD RID: 6317
		private int minLength;

		// Token: 0x040018AE RID: 6318
		private int maxLength;
	}
}
