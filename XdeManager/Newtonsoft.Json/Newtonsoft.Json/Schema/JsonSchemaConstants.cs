using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A0 RID: 160
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal static class JsonSchemaConstants
	{
		// Token: 0x040002F6 RID: 758
		public const string TypePropertyName = "type";

		// Token: 0x040002F7 RID: 759
		public const string PropertiesPropertyName = "properties";

		// Token: 0x040002F8 RID: 760
		public const string ItemsPropertyName = "items";

		// Token: 0x040002F9 RID: 761
		public const string AdditionalItemsPropertyName = "additionalItems";

		// Token: 0x040002FA RID: 762
		public const string RequiredPropertyName = "required";

		// Token: 0x040002FB RID: 763
		public const string PatternPropertiesPropertyName = "patternProperties";

		// Token: 0x040002FC RID: 764
		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		// Token: 0x040002FD RID: 765
		public const string RequiresPropertyName = "requires";

		// Token: 0x040002FE RID: 766
		public const string MinimumPropertyName = "minimum";

		// Token: 0x040002FF RID: 767
		public const string MaximumPropertyName = "maximum";

		// Token: 0x04000300 RID: 768
		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		// Token: 0x04000301 RID: 769
		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		// Token: 0x04000302 RID: 770
		public const string MinimumItemsPropertyName = "minItems";

		// Token: 0x04000303 RID: 771
		public const string MaximumItemsPropertyName = "maxItems";

		// Token: 0x04000304 RID: 772
		public const string PatternPropertyName = "pattern";

		// Token: 0x04000305 RID: 773
		public const string MaximumLengthPropertyName = "maxLength";

		// Token: 0x04000306 RID: 774
		public const string MinimumLengthPropertyName = "minLength";

		// Token: 0x04000307 RID: 775
		public const string EnumPropertyName = "enum";

		// Token: 0x04000308 RID: 776
		public const string ReadOnlyPropertyName = "readonly";

		// Token: 0x04000309 RID: 777
		public const string TitlePropertyName = "title";

		// Token: 0x0400030A RID: 778
		public const string DescriptionPropertyName = "description";

		// Token: 0x0400030B RID: 779
		public const string FormatPropertyName = "format";

		// Token: 0x0400030C RID: 780
		public const string DefaultPropertyName = "default";

		// Token: 0x0400030D RID: 781
		public const string TransientPropertyName = "transient";

		// Token: 0x0400030E RID: 782
		public const string DivisibleByPropertyName = "divisibleBy";

		// Token: 0x0400030F RID: 783
		public const string HiddenPropertyName = "hidden";

		// Token: 0x04000310 RID: 784
		public const string DisallowPropertyName = "disallow";

		// Token: 0x04000311 RID: 785
		public const string ExtendsPropertyName = "extends";

		// Token: 0x04000312 RID: 786
		public const string IdPropertyName = "id";

		// Token: 0x04000313 RID: 787
		public const string UniqueItemsPropertyName = "uniqueItems";

		// Token: 0x04000314 RID: 788
		public const string OptionValuePropertyName = "value";

		// Token: 0x04000315 RID: 789
		public const string OptionLabelPropertyName = "label";

		// Token: 0x04000316 RID: 790
		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping = new Dictionary<string, JsonSchemaType>
		{
			{
				"string",
				JsonSchemaType.String
			},
			{
				"object",
				JsonSchemaType.Object
			},
			{
				"integer",
				JsonSchemaType.Integer
			},
			{
				"number",
				JsonSchemaType.Float
			},
			{
				"null",
				JsonSchemaType.Null
			},
			{
				"boolean",
				JsonSchemaType.Boolean
			},
			{
				"array",
				JsonSchemaType.Array
			},
			{
				"any",
				JsonSchemaType.Any
			}
		};
	}
}
