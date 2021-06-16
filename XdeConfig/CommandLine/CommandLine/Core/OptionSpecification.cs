using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x0200006F RID: 111
	internal sealed class OptionSpecification : Specification
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000B03C File Offset: 0x0000923C
		public OptionSpecification(string shortName, string longName, bool required, string setName, Maybe<int> min, Maybe<int> max, char separator, Maybe<object> defaultValue, string helpText, string metaValue, IEnumerable<string> enumValues, Type conversionType, TargetType targetType, bool hidden = false) : base(SpecificationType.Option, required, min, max, defaultValue, helpText, metaValue, enumValues, conversionType, targetType, hidden)
		{
			this.shortName = shortName;
			this.longName = longName;
			this.separator = separator;
			this.setName = setName;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B084 File Offset: 0x00009284
		public static OptionSpecification FromAttribute(OptionAttribute attribute, Type conversionType, IEnumerable<string> enumValues)
		{
			return new OptionSpecification(attribute.ShortName, attribute.LongName, attribute.Required, attribute.SetName, (attribute.Min == -1) ? Maybe.Nothing<int>() : Maybe.Just<int>(attribute.Min), (attribute.Max == -1) ? Maybe.Nothing<int>() : Maybe.Just<int>(attribute.Max), attribute.Separator, attribute.Default.ToMaybe<object>(), attribute.HelpText, attribute.MetaValue, enumValues, conversionType, conversionType.ToTargetType(), attribute.Hidden);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000B110 File Offset: 0x00009310
		public static OptionSpecification NewSwitch(string shortName, string longName, bool required, string helpText, string metaValue, bool hidden = false)
		{
			return new OptionSpecification(shortName, longName, required, string.Empty, Maybe.Nothing<int>(), Maybe.Nothing<int>(), '\0', Maybe.Nothing<object>(), helpText, metaValue, Enumerable.Empty<string>(), typeof(bool), TargetType.Switch, hidden);
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000B14F File Offset: 0x0000934F
		public string ShortName
		{
			get
			{
				return this.shortName;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000B157 File Offset: 0x00009357
		public string LongName
		{
			get
			{
				return this.longName;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000B15F File Offset: 0x0000935F
		public char Separator
		{
			get
			{
				return this.separator;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000B167 File Offset: 0x00009367
		public string SetName
		{
			get
			{
				return this.setName;
			}
		}

		// Token: 0x040000C8 RID: 200
		private readonly string shortName;

		// Token: 0x040000C9 RID: 201
		private readonly string longName;

		// Token: 0x040000CA RID: 202
		private readonly char separator;

		// Token: 0x040000CB RID: 203
		private readonly string setName;
	}
}
