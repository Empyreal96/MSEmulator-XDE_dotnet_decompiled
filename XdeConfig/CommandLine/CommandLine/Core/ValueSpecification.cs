using System;
using System.Collections.Generic;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000089 RID: 137
	internal sealed class ValueSpecification : Specification
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		public ValueSpecification(int index, string metaName, bool required, Maybe<int> min, Maybe<int> max, Maybe<object> defaultValue, string helpText, string metaValue, IEnumerable<string> enumValues, Type conversionType, TargetType targetType, bool hidden = false) : base(SpecificationType.Value, required, min, max, defaultValue, helpText, metaValue, enumValues, conversionType, targetType, hidden)
		{
			this.index = index;
			this.metaName = metaName;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000CA1C File Offset: 0x0000AC1C
		public static ValueSpecification FromAttribute(ValueAttribute attribute, Type conversionType, IEnumerable<string> enumValues)
		{
			return new ValueSpecification(attribute.Index, attribute.MetaName, attribute.Required, (attribute.Min == -1) ? Maybe.Nothing<int>() : Maybe.Just<int>(attribute.Min), (attribute.Max == -1) ? Maybe.Nothing<int>() : Maybe.Just<int>(attribute.Max), attribute.Default.ToMaybe<object>(), attribute.HelpText, attribute.MetaValue, enumValues, conversionType, conversionType.ToTargetType(), attribute.Hidden);
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000CA9B File Offset: 0x0000AC9B
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000CAA3 File Offset: 0x0000ACA3
		public string MetaName
		{
			get
			{
				return this.metaName;
			}
		}

		// Token: 0x040000EB RID: 235
		private readonly int index;

		// Token: 0x040000EC RID: 236
		private readonly string metaName;
	}
}
