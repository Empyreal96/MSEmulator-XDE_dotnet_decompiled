using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000076 RID: 118
	internal abstract class Specification
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x0000B940 File Offset: 0x00009B40
		protected Specification(SpecificationType tag, bool required, Maybe<int> min, Maybe<int> max, Maybe<object> defaultValue, string helpText, string metaValue, IEnumerable<string> enumValues, Type conversionType, TargetType targetType, bool hidden = false)
		{
			this.tag = tag;
			this.required = required;
			this.min = min;
			this.max = max;
			this.defaultValue = defaultValue;
			this.conversionType = conversionType;
			this.targetType = targetType;
			this.helpText = helpText;
			this.metaValue = metaValue;
			this.enumValues = enumValues;
			this.hidden = hidden;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		public SpecificationType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		public bool Required
		{
			get
			{
				return this.required;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		public Maybe<int> Min
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		public Maybe<int> Max
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public Maybe<object> DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000B9D0 File Offset: 0x00009BD0
		public string HelpText
		{
			get
			{
				return this.helpText;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000B9D8 File Offset: 0x00009BD8
		public string MetaValue
		{
			get
			{
				return this.metaValue;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000B9E0 File Offset: 0x00009BE0
		public IEnumerable<string> EnumValues
		{
			get
			{
				return this.enumValues;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000B9E8 File Offset: 0x00009BE8
		public Type ConversionType
		{
			get
			{
				return this.conversionType;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000B9F0 File Offset: 0x00009BF0
		public TargetType TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000BA00 File Offset: 0x00009C00
		public static Specification FromProperty(PropertyInfo property)
		{
			object[] customAttributes = property.GetCustomAttributes(true);
			IEnumerable<OptionAttribute> source = customAttributes.OfType<OptionAttribute>();
			if (source.Count<OptionAttribute>() == 1)
			{
				OptionSpecification optionSpecification = OptionSpecification.FromAttribute(source.Single<OptionAttribute>(), property.PropertyType, ReflectionHelper.GetNamesOfEnum(property.PropertyType));
				if (optionSpecification.ShortName.Length == 0 && optionSpecification.LongName.Length == 0)
				{
					return optionSpecification.WithLongName(property.Name.ToLowerInvariant());
				}
				return optionSpecification;
			}
			else
			{
				IEnumerable<ValueAttribute> source2 = customAttributes.OfType<ValueAttribute>();
				if (source2.Count<ValueAttribute>() == 1)
				{
					ValueAttribute attribute = source2.Single<ValueAttribute>();
					Type propertyType = property.PropertyType;
					IEnumerable<string> enumerable;
					if (!property.PropertyType.GetTypeInfo().IsEnum)
					{
						enumerable = Enumerable.Empty<string>();
					}
					else
					{
						IEnumerable<string> names = Enum.GetNames(property.PropertyType);
						enumerable = names;
					}
					return ValueSpecification.FromAttribute(attribute, propertyType, enumerable);
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x040000D3 RID: 211
		private readonly SpecificationType tag;

		// Token: 0x040000D4 RID: 212
		private readonly bool required;

		// Token: 0x040000D5 RID: 213
		private readonly bool hidden;

		// Token: 0x040000D6 RID: 214
		private readonly Maybe<int> min;

		// Token: 0x040000D7 RID: 215
		private readonly Maybe<int> max;

		// Token: 0x040000D8 RID: 216
		private readonly Maybe<object> defaultValue;

		// Token: 0x040000D9 RID: 217
		private readonly string helpText;

		// Token: 0x040000DA RID: 218
		private readonly string metaValue;

		// Token: 0x040000DB RID: 219
		private readonly IEnumerable<string> enumValues;

		// Token: 0x040000DC RID: 220
		private readonly Type conversionType;

		// Token: 0x040000DD RID: 221
		private readonly TargetType targetType;
	}
}
