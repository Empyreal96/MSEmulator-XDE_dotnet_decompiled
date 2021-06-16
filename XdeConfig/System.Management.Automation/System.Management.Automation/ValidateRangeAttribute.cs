using System;

namespace System.Management.Automation
{
	// Token: 0x02000420 RID: 1056
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ValidateRangeAttribute : ValidateEnumeratedArgumentsAttribute
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x001006C6 File Offset: 0x000FE8C6
		public object MinRange
		{
			get
			{
				return this.minRange;
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06002EE1 RID: 12001 RVA: 0x001006CE File Offset: 0x000FE8CE
		public object MaxRange
		{
			get
			{
				return this.maxRange;
			}
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x001006D8 File Offset: 0x000FE8D8
		protected override void ValidateElement(object element)
		{
			if (element == null)
			{
				throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullFailure, new object[0]);
			}
			if (element is PSObject)
			{
				element = ((PSObject)element).BaseObject;
			}
			if (!element.GetType().Equals(this.promotedType))
			{
				object obj;
				if (!LanguagePrimitives.TryConvertTo(element, this.promotedType, out obj))
				{
					throw new ValidationMetadataException("ValidationRangeElementType", null, Metadata.ValidateRangeElementType, new object[]
					{
						element.GetType().Name,
						this.minRange.GetType().Name
					});
				}
				element = obj;
			}
			if (this.minComparable.CompareTo(element) > 0)
			{
				throw new ValidationMetadataException("ValidateRangeTooSmall", null, Metadata.ValidateRangeSmallerThanMinRangeFailure, new object[]
				{
					element.ToString(),
					this.minRange.ToString()
				});
			}
			if (this.maxComparable.CompareTo(element) < 0)
			{
				throw new ValidationMetadataException("ValidateRangeTooBig", null, Metadata.ValidateRangeGreaterThanMaxRangeFailure, new object[]
				{
					element.ToString(),
					this.maxRange.ToString()
				});
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x001007F4 File Offset: 0x000FE9F4
		public ValidateRangeAttribute(object minRange, object maxRange)
		{
			if (minRange == null)
			{
				throw PSTraceSource.NewArgumentNullException("minRange");
			}
			if (maxRange == null)
			{
				throw PSTraceSource.NewArgumentNullException("maxRange");
			}
			if (!maxRange.GetType().Equals(minRange.GetType()))
			{
				bool flag = true;
				this.promotedType = ValidateRangeAttribute.GetCommonType(minRange.GetType(), maxRange.GetType());
				object obj;
				if (this.promotedType != null && LanguagePrimitives.TryConvertTo(minRange, this.promotedType, out obj))
				{
					minRange = obj;
					if (LanguagePrimitives.TryConvertTo(maxRange, this.promotedType, out obj))
					{
						maxRange = obj;
						flag = false;
					}
				}
				if (flag)
				{
					throw new ValidationMetadataException("MinRangeNotTheSameTypeOfMaxRange", null, Metadata.ValidateRangeMinRangeMaxRangeType, new object[]
					{
						minRange.GetType().Name,
						maxRange.GetType().Name
					});
				}
			}
			else
			{
				this.promotedType = minRange.GetType();
			}
			this.minComparable = (minRange as IComparable);
			if (this.minComparable == null)
			{
				throw new ValidationMetadataException("MinRangeNotIComparable", null, Metadata.ValidateRangeNotIComparable, new object[0]);
			}
			this.maxComparable = (maxRange as IComparable);
			if (this.minComparable.CompareTo(maxRange) > 0)
			{
				throw new ValidationMetadataException("MaxRangeSmallerThanMinRange", null, Metadata.ValidateRangeMaxRangeSmallerThanMinRange, new object[0]);
			}
			this.minRange = minRange;
			this.maxRange = maxRange;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00100938 File Offset: 0x000FEB38
		private static Type GetCommonType(Type minType, Type maxType)
		{
			Type result = null;
			TypeCode typeCode = LanguagePrimitives.GetTypeCode(minType);
			TypeCode typeCode2 = LanguagePrimitives.GetTypeCode(maxType);
			TypeCode typeCode3 = (typeCode >= typeCode2) ? typeCode : typeCode2;
			if (typeCode3 <= TypeCode.Int32)
			{
				result = typeof(int);
			}
			else if (typeCode3 <= TypeCode.UInt32)
			{
				result = ((LanguagePrimitives.IsSignedInteger(typeCode) || LanguagePrimitives.IsSignedInteger(typeCode2)) ? typeof(double) : typeof(uint));
			}
			else if (typeCode3 <= TypeCode.Int64)
			{
				result = typeof(long);
			}
			else if (typeCode3 <= TypeCode.UInt64)
			{
				result = ((LanguagePrimitives.IsSignedInteger(typeCode) || LanguagePrimitives.IsSignedInteger(typeCode2)) ? typeof(double) : typeof(ulong));
			}
			else if (typeCode3 == TypeCode.Decimal)
			{
				result = typeof(decimal);
			}
			else if (typeCode3 == TypeCode.Single || typeCode3 == TypeCode.Double)
			{
				result = typeof(double);
			}
			return result;
		}

		// Token: 0x040018A5 RID: 6309
		private object minRange;

		// Token: 0x040018A6 RID: 6310
		private IComparable minComparable;

		// Token: 0x040018A7 RID: 6311
		private object maxRange;

		// Token: 0x040018A8 RID: 6312
		private IComparable maxComparable;

		// Token: 0x040018A9 RID: 6313
		private Type promotedType;
	}
}
